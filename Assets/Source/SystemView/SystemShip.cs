using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.SystemView;

namespace Source {
    namespace SystemView {
        enum AutopilotStage {
            DISENGAGED = 0,
            CIRCULARIZING,
            PLANNING_TRAJECTORY,
            TRANSITIONING,
            IN_TRANSIT,
            MATCHING_ORBIT,
            DOCKING
        };

        public class SystemShip {
            public OrbitingObjectDescriptor descriptor;
            public OrbitingObjectDescriptor start, destination;

            public bool path_planned = false;

            public int health, max_health;
            public int shield, max_shield;

            public int shield_regeneration_rate;

            public SpaceObject self;

            public float acceleration;
            public float horizontal_acceleration;

            public float rotation;

            public float torque = 2.0f;

            public List<ShipWeapon> weapons;

            private float[] autopilot_time_required;
            private float[] autopilot_delta_v;

            public bool destroyed = false;

            private AutopilotStage stage;
            private SpaceStation docking_target;

            public SystemShip() {
                self            = new SpaceObject();
                descriptor      = new OrbitingObjectDescriptor(self);
                weapons         = new List<ShipWeapon>();
                autopilot_delta_v = new float[2];
            }

            public void destroy() {
                foreach(ShipWeapon weapon in weapons)
                    weapon.cleanup();

                destroyed = true;
            }

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

            public void circularize(float current_time) {
                if(descriptor.central_body == null) return;

                float[] vel              = descriptor.get_velocity_at(descriptor.get_distance_from_center_at(Tools.pi), Tools.pi);
                float targetrotation     = Tools.get_angle(vel[0], vel[1]);
                float velocity_direction = Tools.get_angle(self.velx, self.vely);

                if(rotation == targetrotation) {
                    float diff = targetrotation - velocity_direction;
                    if(diff > -0.4f && diff < 0.4f) accelerate(current_time);
                    else descriptor.update_position(current_time);
                } else { rotate_to(targetrotation, current_time); descriptor.update_position(current_time); }
            }

            public bool set_apoapsis(float target, float current_time) {
                if(descriptor.central_body == null || descriptor.apoapsis == target) return true;

                float   mean_anomaly       = target > descriptor.apoapsis ? 0.0f : Tools.pi;
                float[] vel                = descriptor.get_velocity_at(descriptor.get_distance_from_center_at(mean_anomaly), mean_anomaly);
                float   targetrotation     = Tools.get_angle(vel[0], vel[1]);
                float   velocity_direction = Tools.get_angle(self.velx, self.vely);

                float   d                  = descriptor.apoapsis - target;

                if(rotation == targetrotation) {
                    float diff = targetrotation - velocity_direction;
                    if((diff >          - 0.4f && diff <            0.4f && target > descriptor.apoapsis)
                    || (diff > Tools.pi - 0.4f && diff < Tools.pi + 0.4f && target < descriptor.apoapsis))
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
                    float diff = targetrotation - velocity_direction;
                    if((diff >          -0.4f && diff <            0.4f && target > descriptor.periapsis)
                    || (diff > Tools.pi - 0.4f && diff < Tools.pi + 0.4f && target < descriptor.periapsis))
                        accelerate(current_time);
                    else
                        descriptor.update_position(current_time);
                } else { rotate_to(targetrotation, current_time); descriptor.update_position(current_time); }

                float   d2                 = descriptor.periapsis - target;

                return (d > 0.0f && d2 <= 0.0f) || (d < 0.0f && d2 >= 0.0f);
            }

            public void engage_docking_autopilot(SpaceStation station) {
                stage = AutopilotStage.CIRCULARIZING;
                docking_target = station;
            }

            public void disengage_docking_autopilot() {
                stage = AutopilotStage.DISENGAGED;
                docking_target = null;
            }

            public bool DockingAutopilotLoop(float CurrentTime, float AcceptedDeviation) {
                if(stage != AutopilotStage.DISENGAGED) Debug.Log(stage);

                switch(stage) {
                    case AutopilotStage.DISENGAGED:
                        return false;

                    case AutopilotStage.CIRCULARIZING:
                        circularize(CurrentTime);
                        if(descriptor.eccentricity < 0.02f) stage = AutopilotStage.PLANNING_TRAJECTORY;
                        break;

                    case AutopilotStage.PLANNING_TRAJECTORY:
                        descriptor.update_position(CurrentTime);
                        autopilot_time_required = descriptor.calculate_required_deltav(docking_target.descriptor, acceleration, 20.0f);
                        if(autopilot_time_required != null) stage = AutopilotStage.TRANSITIONING;
                        break;

                    case AutopilotStage.TRANSITIONING: {
                        float tx = autopilot_time_required[0];
                        float ty = autopilot_time_required[1];

                        float target_rotation = Tools.get_angle(tx, ty);

                        if(rotation != target_rotation) rotate_to(target_rotation, CurrentTime);
                        else {
                            accelerate(CurrentTime);
                            autopilot_time_required[0] -= (float)Math.Cos(target_rotation) * CurrentTime;
                            autopilot_time_required[1] -= (float)Math.Sin(target_rotation) * CurrentTime;

                            if((tx > 0.0f && autopilot_time_required[0] < 0.0f)
                            || (tx < 0.0f && autopilot_time_required[0] > 0.0f)
                            || (ty > 0.0f && autopilot_time_required[1] < 0.0f)
                            || (ty < 0.0f && autopilot_time_required[1] > 0.0f)) stage = AutopilotStage.IN_TRANSIT;
                        }
                        break;
                    }

                    case AutopilotStage.IN_TRANSIT:
                        descriptor.update_position(CurrentTime);

                        float dx = docking_target.Self.posx - self.posx;
                        float dy = docking_target.Self.posy - self.posy;
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

                        if(dt < 5.0f) stage = AutopilotStage.MATCHING_ORBIT;
                        break;

                    case AutopilotStage.MATCHING_ORBIT: {
                        float dvx = autopilot_delta_v[0];
                        float dvy = autopilot_delta_v[1];

                        float target_rotation = Tools.get_angle(dvx, dvy);

                        if(rotation != target_rotation) rotate_to(target_rotation, CurrentTime);
                        else {
                            accelerate(CurrentTime);
                            autopilot_delta_v[0] -= (float)Math.Cos(target_rotation) * CurrentTime * acceleration;
                            autopilot_delta_v[1] -= (float)Math.Sin(target_rotation) * CurrentTime * acceleration;

                            if((dvx > 0.0f && autopilot_delta_v[0] < 0.0f)
                            || (dvx < 0.0f && autopilot_delta_v[0] > 0.0f)
                            || (dvy > 0.0f && autopilot_delta_v[1] < 0.0f)
                            || (dvy < 0.0f && autopilot_delta_v[1] > 0.0f)) stage = AutopilotStage.DOCKING;
                        }
                        break;
                    }

                    case AutopilotStage.DOCKING:
                        // todo
                        break;
                }

                return true;
            }
        }
    }
}
