using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.SystemView;

namespace Source {
    namespace SystemView {
        enum DockingAutopilotStage {
            DISENGAGED                  = 0,
            CIRCULARIZING               = 1,
            PLANNING_TRAJECTORY         = 2,
            TRANSITIONING               = 3,
            IN_TRANSIT                  = 4,
            MATCHING_ORBIT              = 5,
            DOCKING                     = 6,

            CANCELLING_VELOCITY         = 1,
            ACCELERATING                = 2,
            DECCELERATING               = 3
        };

        enum OrbitalAutopilotStage {
            DISENGAGED                  = 0,
            CIRCULARIZING               = 1,
            SETTING_APOAPSIS            = 2,
            SECOND_CIRCULARIZATION      = 3,
            WAITING_ON_ROTATION         = 4,
            SETTING_PERIAPSIS           = 5
        };

        public enum ShipClass {
            FIGHTER,
            FREIGHTER,
            DESTROYER,
            CAPITAL_SHIP
        };

        public class SystemShip {
            public  OrbitingObjectDescriptor descriptor;
            public  OrbitingObjectDescriptor start, destination;

            public  bool path_planned = false;

            public  int health, max_health;
            public  int shield, max_shield;

            public  int shield_regeneration_rate;

            public  SpaceObject self;

            public  float acceleration;
            public  float horizontal_acceleration;

            public  float rotation;

            public  float torque = 50.0f;

            public  List<ShipWeapon> weapons;

            private float[] autopilot_time_required;
            private float[] autopilot_delta_v;

            public  bool destroyed = false;
            public  bool docked;
            public  SpaceStation docked_at;
            public  ShipClass ship_class;

            private DockingAutopilotStage docking_stage;
            private OrbitalAutopilotStage orbital_stage;
            private SpaceStation docking_target;
            private bool circularize_at_periapsis;
            private float distance;

            public  bool ignore_gravity;

            public SystemShip() {
                self              = new SpaceObject();
                descriptor        = new OrbitingObjectDescriptor(self);
                weapons           = new List<ShipWeapon>();
                autopilot_delta_v = new float[2];
                self.mass         = 100;
            }

            public void destroy() {
                foreach(ShipWeapon weapon in weapons)
                    weapon.cleanup();

                destroyed = true;
            }

            // Gradually rotates the ship towards the given angle
            // Must be called each tick until the angle is reached

            public void rotate_to(float angle, float current_time) {
                while(rotation > Tools.twopi) rotation -= Tools.twopi;
                while(rotation < 0.0f)        rotation  = Tools.twopi + rotation;

                if(rotation == angle) return;

                float diff1 = angle - rotation;
                float diff2 = rotation - angle;

                if(diff1 < 0.0f) diff1 = Tools.twopi + diff1;
                if(diff2 < 0.0f) diff2 = Tools.twopi + diff2;

                float acc              = (float)Math.Sqrt(torque / self.angular_inertia);
                float time_to_angle;
                float time_to_brake;

                if(diff2 < diff1) {
                    //      1                        v - √ (2 a d + v²)
                    // d = --- a t² - v t   =>   t = ------------------
                    //      2                                a

                    //                                 d
                    // d = v t               =>   t = ---
                    //                                 v

                    time_to_angle      = diff2 / self.angular_vel;
                    time_to_brake      = 0.25f * (self.angular_vel - (float)Math.Sqrt(2.0f * acc * diff2 + self.angular_vel * self.angular_vel)) / acc;
                } else {
                    //      1                        v + √ (2 a d + v²)
                    // d = --- a t² - v t   =>   t = ------------------
                    //      2                                a

                    //                                 d
                    // d = v t               =>   t = ---
                    //                                 v

                    time_to_angle      = diff1 / self.angular_vel;
                    time_to_brake      = 0.25f * (self.angular_vel + (float)Math.Sqrt(2.0f * acc * diff1 + self.angular_vel * self.angular_vel)) / acc;
                }

                if(time_to_angle < 0.0f) time_to_angle *= -1.0f;
                if(time_to_brake < 0.0f) time_to_brake *= -1.0f;

                if((diff2 < diff1 && time_to_angle >  time_to_brake)
                || (diff1 < diff2 && time_to_angle <= time_to_brake)) {
                    rotation              += self.angular_vel * current_time - 0.5f * acc * current_time * current_time;
                    self.angular_vel      -= acc * current_time;
                } else {
                    rotation              += self.angular_vel * current_time + 0.5f * acc * current_time * current_time;
                    self.angular_vel      += acc * current_time;
                }

                if(rotation - 0.005f < angle && rotation + 0.005f > angle) { rotation = angle; self.angular_vel = 0.0f; }
            }

            // Accelerates the ship in the current direction
            // Must be constantly called each tick while accelerating

            public void accelerate(float current_time) {
                float accx = (float)Math.Cos(rotation) * acceleration;
                float accy = (float)Math.Sin(rotation) * acceleration;

                accx *= current_time;
                accy *= current_time;

                self.posx += self.velx * current_time + accx / 2.0f * current_time;
                self.posy += self.vely * current_time + accy / 2.0f * current_time;

                self.velx += accx;
                self.vely += accy;

                descriptor.change_frame_of_reference(descriptor.central_body);
            }

            // Keeps the ship moving linearly along it's current velocity
            // Note: When this function is called the ship is unaffected by gravity

            public void transition(float current_time) {
                self.posx += self.velx * current_time;
                self.posy += self.vely * current_time;

                descriptor.change_frame_of_reference(descriptor.central_body);
            }

            public bool circularize(float current_time, bool circularize_at_apoapsis = true) {
                if(descriptor.central_body == null) return true;

                float true_anomaly;

                if(circularize_at_apoapsis)
                    true_anomaly         = Tools.pi;
                else
                    true_anomaly         = 0.0f;

                float[] vel              = descriptor.get_velocity_at(descriptor.get_distance_from_center_at(true_anomaly), true_anomaly);
                float targetrotation     = Tools.get_angle(vel[0], vel[1]);
                float velocity_direction = Tools.get_angle(self.velx, self.vely);

                if(!circularize_at_apoapsis) 
                    targetrotation       = Tools.normalize_angle(targetrotation + Tools.pi);

                if(rotation == targetrotation) {
                    float diff = true_anomaly - descriptor.true_anomaly;
                    if(diff > -0.4f && diff < 0.4f) accelerate(current_time);
                    else descriptor.update_position(current_time);
                } else { rotate_to(targetrotation, current_time); descriptor.update_position(current_time); }

                return descriptor.eccentricity < 0.02f;
            }

            public bool set_apoapsis(float target, float current_time) {
                if(descriptor.central_body == null || descriptor.apoapsis == target) return true;

                float   mean_anomaly       = target > descriptor.apoapsis ? 0.0f : Tools.pi;
                float[] vel                = descriptor.get_velocity_at(descriptor.get_distance_from_center_at(mean_anomaly), mean_anomaly);
                float   targetrotation     = Tools.get_angle(vel[0], vel[1]);
                float   velocity_direction = Tools.get_angle(self.velx, self.vely);

                float   d                  = descriptor.apoapsis - target;

                if(rotation == targetrotation) {
                    float diff1 = targetrotation - velocity_direction;
                    float diff2 = diff1 + Tools.pi;

                    while(diff1 > Tools.twopi) diff1 -= Tools.twopi;
                    while(diff2 > Tools.twopi) diff2 -= Tools.twopi;

                    if((diff1 > -0.4f && diff1 < 0.4f && target > descriptor.apoapsis)
                    || (diff2 > -0.4f && diff2 < 0.4f && target < descriptor.apoapsis))
                        accelerate(current_time);
                    else
                        descriptor.update_position(current_time);
                } else { rotate_to(targetrotation, current_time); descriptor.update_position(current_time); }

                float   d2                 = descriptor.apoapsis - target;

                return (d > 0.0f && d2 <= 0.0f) || (d < 0.0f && d2 >= 0.0f);
            }

            public bool set_periapsis(float target, float current_time) {
                if(descriptor.central_body == null || descriptor.periapsis == target) return true;

                float   mean_anomaly       = target > descriptor.periapsis ? Tools.pi : 0.0f;
                float[] vel                = descriptor.get_velocity_at(descriptor.get_distance_from_center_at(mean_anomaly), mean_anomaly);
                float   targetrotation     = Tools.get_angle(vel[0], vel[1]);
                float   velocity_direction = Tools.get_angle(self.velx, self.vely);

                float   d                  = descriptor.periapsis - target;

                if(rotation == targetrotation) {
                    float diff1 = targetrotation - velocity_direction;
                    float diff2 = diff1 + Tools.pi;

                    while(diff1 > Tools.twopi) diff1 -= Tools.twopi;
                    while(diff2 > Tools.twopi) diff2 -= Tools.twopi;

                    if((diff1 > -0.4f && diff1 < 0.4f && target > descriptor.periapsis)
                    || (diff2 > -0.4f && diff2 < 0.4f && target < descriptor.periapsis))
                        accelerate(current_time);
                    else
                        descriptor.update_position(current_time);
                } else { rotate_to(targetrotation, current_time); descriptor.update_position(current_time); }

                float   d2                 = descriptor.periapsis - target;

                return (d > 0.0f && d2 <= 0.0f) || (d < 0.0f && d2 >= 0.0f);
            }

            public bool orbital_autopilot_tick(float periapsis, float apoapsis, float rot, float current_time) {
                if(descriptor.central_body == null || (
                    descriptor.periapsis == periapsis &&
                    descriptor.apoapsis  ==  apoapsis &&
                    descriptor.rotation  ==       rot)) return false;

                if(orbital_stage != OrbitalAutopilotStage.DISENGAGED) Debug.Log(orbital_stage);

                switch(orbital_stage) {
                    case OrbitalAutopilotStage.DISENGAGED:
                        return false;

                    case OrbitalAutopilotStage.CIRCULARIZING:
                        if(circularize(current_time)) {
                            orbital_stage = OrbitalAutopilotStage.SETTING_APOAPSIS;
                            circularize_at_periapsis = descriptor.apoapsis > apoapsis;
                        }
                        break;
                        
                    case OrbitalAutopilotStage.SETTING_APOAPSIS:
                        if(descriptor.apoapsis > apoapsis) {
                            if(set_periapsis(apoapsis, current_time))
                                orbital_stage = OrbitalAutopilotStage.SECOND_CIRCULARIZATION;
                        } else {
                            if(set_apoapsis(apoapsis, current_time))
                                orbital_stage = OrbitalAutopilotStage.SECOND_CIRCULARIZATION;
                        }
                        break;

                    case OrbitalAutopilotStage.SECOND_CIRCULARIZATION:
                        if(circularize(current_time, !circularize_at_periapsis))
                            orbital_stage = OrbitalAutopilotStage.WAITING_ON_ROTATION;
                        break;

                    case OrbitalAutopilotStage.WAITING_ON_ROTATION:
                        descriptor.update_position(current_time);

                        float target_rotation      = rot;
                        if(periapsis > descriptor.periapsis) target_rotation += Tools.pi;

                        float target_ship_rotation = target_rotation;
                        if(periapsis > descriptor.periapsis) target_ship_rotation += Tools.halfpi;
                        else                                 target_ship_rotation -= Tools.halfpi;

                        while(target_rotation > Tools.twopi) target_rotation      -= Tools.twopi;
                        if   (target_rotation <        0.0f) target_rotation      += Tools.twopi;

                        rotate_to(target_ship_rotation, current_time);

                        float current_rotation = descriptor.true_anomaly + descriptor.rotation;
                        while(current_rotation > Tools.twopi) current_rotation    -= Tools.twopi;
                        if   (current_rotation <        0.0f) current_rotation    += Tools.twopi;

                        if(current_rotation >= target_rotation - 0.02f
                        && current_rotation <= target_rotation + 0.02f) {
                            accelerate(current_time * 25);
                            orbital_stage   = OrbitalAutopilotStage.SETTING_PERIAPSIS;
                        }
                        break;
                    
                    case OrbitalAutopilotStage.SETTING_PERIAPSIS:
                        if(set_periapsis(periapsis, current_time))
                            orbital_stage = OrbitalAutopilotStage.DISENGAGED;
                        break;
                }

                return orbital_stage != OrbitalAutopilotStage.DISENGAGED;
            }

            public void engage_orbital_autopilot() {
                orbital_stage = OrbitalAutopilotStage.CIRCULARIZING;
            }

            public void engage_docking_autopilot(SpaceStation station) {
                docking_stage = DockingAutopilotStage.CIRCULARIZING;
                docking_target = station;
            }

            public void disengage_docking_autopilot() {
                docking_stage = DockingAutopilotStage.DISENGAGED;
                docking_target = null;
                if(docked_at != null) docked_at.undock(this);
            }

            public bool docking_autopilot_tick(float current_time, float AcceptedDeviation, bool station_orbiting) {
                if(docking_stage != DockingAutopilotStage.DISENGAGED) Debug.Log(docking_stage);

                if(station_orbiting) {
                    switch(docking_stage) {
                        case DockingAutopilotStage.DISENGAGED:
                            return false;

                        case DockingAutopilotStage.CIRCULARIZING:
                            if(circularize(current_time)) docking_stage = DockingAutopilotStage.PLANNING_TRAJECTORY;
                            break;

                        case DockingAutopilotStage.PLANNING_TRAJECTORY:
                            descriptor.update_position(current_time);
                            autopilot_time_required = descriptor.calculate_required_deltav(docking_target.descriptor, acceleration, 20.0f);
                            if(autopilot_time_required != null) docking_stage = DockingAutopilotStage.TRANSITIONING;
                            break;

                        case DockingAutopilotStage.TRANSITIONING: {
                            float tx = autopilot_time_required[0];
                            float ty = autopilot_time_required[1];

                            float target_rotation = Tools.get_angle(tx, ty);

                            if(rotation != target_rotation) rotate_to(target_rotation, current_time);
                            else {
                                accelerate(current_time);
                                autopilot_time_required[0] -= (float)Math.Cos(target_rotation) * current_time;
                                autopilot_time_required[1] -= (float)Math.Sin(target_rotation) * current_time;

                                if((tx > 0.0f && autopilot_time_required[0] < 0.0f)
                                || (tx < 0.0f && autopilot_time_required[0] > 0.0f)
                                || (ty > 0.0f && autopilot_time_required[1] < 0.0f)
                                || (ty < 0.0f && autopilot_time_required[1] > 0.0f)) docking_stage = DockingAutopilotStage.IN_TRANSIT;
                            }
                            break;
                        }

                        case DockingAutopilotStage.IN_TRANSIT:
                            descriptor.update_position(current_time);

                            float dx = docking_target.self.posx - self.posx;
                            float dy = docking_target.self.posy - self.posy;
                            float d  = (float)Math.Sqrt(dx * dx + dy * dy);

                            float targetMeanAnomaly;
                            if(descriptor.get_distance_from_center() > docking_target.descriptor.get_distance_from_center()) targetMeanAnomaly =       0.0f;
                            else targetMeanAnomaly = Tools.pi;

                            float eta = descriptor.orbital_period * (targetMeanAnomaly - descriptor.mean_anomaly) * 0.5f;
                            if(eta < 0.0f) eta *= -1;

                            float[] vel = descriptor.get_velocity_at(descriptor.get_distance_from_center_at(targetMeanAnomaly), targetMeanAnomaly);

                            autopilot_delta_v[0] = vel[0] - self.velx;
                            autopilot_delta_v[1] = vel[1] - self.vely;

                            float timeToSlowDown = Tools.magnitude(autopilot_delta_v) / acceleration;

                            float dt = eta - timeToSlowDown;

                            if(dt < 5.0f) docking_stage = DockingAutopilotStage.MATCHING_ORBIT;
                            break;

                        case DockingAutopilotStage.MATCHING_ORBIT: {
                            float dvx = autopilot_delta_v[0];
                            float dvy = autopilot_delta_v[1];

                            float target_rotation = Tools.get_angle(dvx, dvy);

                            if(rotation != target_rotation) rotate_to(target_rotation, current_time);
                            else {
                                accelerate(current_time);
                                autopilot_delta_v[0] -= (float)Math.Cos(target_rotation) * current_time * acceleration;
                                autopilot_delta_v[1] -= (float)Math.Sin(target_rotation) * current_time * acceleration;

                                if((dvx > 0.0f && autopilot_delta_v[0] < 0.0f)
                                || (dvx < 0.0f && autopilot_delta_v[0] > 0.0f)
                                || (dvy > 0.0f && autopilot_delta_v[1] < 0.0f)
                                || (dvy < 0.0f && autopilot_delta_v[1] > 0.0f)) docking_stage = DockingAutopilotStage.DOCKING;
                            }
                            break;
                        }

                        case DockingAutopilotStage.DOCKING:
                            // todo
                            break;
                    }
                } else
                    switch(docking_stage) {
                        case DockingAutopilotStage.DISENGAGED:
                            return false;

                        case DockingAutopilotStage.CANCELLING_VELOCITY: {
                            ignore_gravity = true;

                            float angle = Tools.normalize_angle(Tools.pi + Tools.get_angle(self.velx, self.vely));
                            rotate_to(angle, current_time);
                            float velx = self.velx;
                            float vely = self.vely;
                            if(rotation == angle) {
                                accelerate(current_time);
                                if((velx > 0.0f && self.velx < 0.0f)
                                || (vely > 0.0f && self.vely < 0.0f)
                                || (velx < 0.0f && self.velx > 0.0f)
                                || (vely < 0.0f && self.vely > 0.0f)) {
                                    self.velx = 0.0f;
                                    self.vely = 0.0f;
                                    docking_stage = DockingAutopilotStage.ACCELERATING;
                                    distance = Tools.get_distance(self.posx, self.posy, docking_target.self.posx, docking_target.self.posy);
                                }
                            } else descriptor.update_position(current_time);
                            break;
                        }

                        case DockingAutopilotStage.ACCELERATING: {
                            float angle = Tools.get_angle(docking_target.self.posx - self.posx, docking_target.self.posy - self.posy);
                            rotate_to(angle, current_time);
                            if(rotation == angle) {
                                accelerate(current_time);
                                if(Tools.get_distance(self.posx, self.posy, docking_target.self.posx, docking_target.self.posy) <= distance * 0.75f)
                                    docking_stage = DockingAutopilotStage.DECCELERATING;
                            }
                            break;
                        }

                        case DockingAutopilotStage.DECCELERATING: {
                            float angle = Tools.normalize_angle(
                                Tools.pi +
                                Tools.get_angle(docking_target.self.posx - self.posx, docking_target.self.posy - self.posy)
                            );

                            rotate_to(angle, current_time);

                            if(rotation == angle) {
                                if(Tools.get_distance(self.posx, self.posy, docking_target.self.posx, docking_target.self.posy) <= distance * 0.25f) {
                                    accelerate(current_time);
                                    if(docking_target.dock(this)) {
                                        docking_stage = DockingAutopilotStage.DISENGAGED;
                                        docking_target = null;
                                        ignore_gravity = false;
                                    }
                                } else transition(current_time);
                            } else transition(current_time);
                            break;
                        }
                    }
                
                return true;
            }
        }
    }
}
