using System;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    enum AutopilotStage
    {
        DISENGAGED = 0,
        CIRCULARIZING,
        PLANNING_TRAJECTORY,
        TRANSITIONING,
        IN_TRANSIT,
        MATCHING_ORBIT,
        DOCKING
    };

    public class SystemShip
    {
        public OrbitingObjectDescriptor Descriptor;
        public OrbitingObjectDescriptor Start, Destination;

        public bool PathPlanned = false;
        public bool Reached     = false;

        public int Health, MaxHealth;
        public int Shield, MaxShield;

        public int ShieldRegenerationRate;

        public SpaceObject self;

        public float Acceleration;

        public float Rotation;

        public float RotationSpeedModifier = 2.0f;

        public List<ShipWeapon> Weapons;

        private float[] AutopilotTimeRequired;
        private float[] AutopilotDeltaV;

        public bool Destroyed = false;

        private AutopilotStage autopilotStage;
        private SpaceStation dockingAutopilotTarget;

        public SystemShip()
        {
            self            = new SpaceObject();
            Descriptor      = new OrbitingObjectDescriptor(self);
            Weapons         = new List<ShipWeapon>();
            AutopilotDeltaV = new float[2];
        }

        public void Destroy()
        {
            Destroyed = true;
        }

        public void RotateTo(float Angle, float CurrentTime)
        {
            while (Rotation < 0.0f) Rotation = Tools.twopi + Rotation;
            while (Rotation > Tools.twopi) Rotation -= Tools.twopi;

            if (Rotation == Angle) return;

            float diff1 = Angle - Rotation;
            float diff2 = Rotation - Angle;

            if (diff1 < 0.0f) diff1 = Tools.twopi + diff1;
            if (diff2 < 0.0f) diff2 = Tools.twopi + diff2;

            if (diff2 < diff1)
            {
                Rotation -= RotationSpeedModifier * CurrentTime;

                diff1 = Angle - Rotation;
                diff2 = Rotation - Angle;

                if (diff1 < 0.0f) diff1 = Tools.twopi + diff1;
                if (diff2 < 0.0f) diff2 = Tools.twopi + diff2;

                if (diff1 < diff2) Rotation = Angle;
            }
            else
            {
                Rotation += RotationSpeedModifier * CurrentTime;

                diff1 = Angle - Rotation;
                diff2 = Rotation - Angle;

                if (diff1 < 0.0f) diff1 = Tools.twopi + diff1;
                if (diff2 < 0.0f) diff2 = Tools.twopi + diff2;

                if (diff2 < diff1) Rotation = Angle;
            }
        }

        public void Accelerate(float CurrentTime) {
            float AccX = (float)Math.Cos(Rotation) * Acceleration;
            float AccY = (float)Math.Sin(Rotation) * Acceleration;

            AccX *= CurrentTime;
            AccY *= CurrentTime;

            self.posx += self.velx * CurrentTime + AccX / 2.0f * CurrentTime;
            self.posy += self.vely * CurrentTime + AccY / 2.0f * CurrentTime;

            self.velx += AccX;
            self.vely += AccY;

            Descriptor.change_frame_of_reference(Descriptor.central_body);
        }

        public void Circularize(float CurrentTime)
        {
            if (Descriptor.central_body == null) return;

            float[] Vel = Descriptor.get_velocity_at(Descriptor.get_distance_from_center_at(Tools.pi), Tools.pi);

            float targetrotation = (float)Math.Acos(Vel[0] / Math.Sqrt(Vel[0] * Vel[0] + Vel[1] * Vel[1]));
            float VelocityDirection = (float)Math.Acos(self.velx / Math.Sqrt(self.velx * self.velx + self.vely * self.vely));
            if (self.vely < 0.0f) VelocityDirection = Tools.twopi - VelocityDirection;

            if (Rotation == targetrotation)
            {
                float diff = targetrotation - VelocityDirection;
                if (diff > -0.4f && diff < 0.4f) Accelerate(CurrentTime);
                else Descriptor.update_position(CurrentTime);
            } else { RotateTo(targetrotation, CurrentTime); Descriptor.update_position(CurrentTime); }
        }

        public void EngageDockingAutopilot(SpaceStation Station)
        {
            autopilotStage = AutopilotStage.CIRCULARIZING;
            dockingAutopilotTarget = Station;
        }

        public void DisengageDockingAutopilot()
        {
            autopilotStage = AutopilotStage.DISENGAGED;
            dockingAutopilotTarget = null;
        }

        public bool DockingAutopilotLoop(float CurrentTime, float AcceptedDeviation)
        {
            if (autopilotStage != AutopilotStage.DISENGAGED) Debug.Log(autopilotStage);

            switch (autopilotStage)
            {
                case AutopilotStage.DISENGAGED:
                    return false;

                case AutopilotStage.CIRCULARIZING:
                    Circularize(CurrentTime);
                    if (Descriptor.eccentricity < 0.02f) autopilotStage = AutopilotStage.PLANNING_TRAJECTORY;
                    break;

                case AutopilotStage.PLANNING_TRAJECTORY:
                    Descriptor.update_position(CurrentTime);
                    AutopilotTimeRequired = Descriptor.calculate_required_deltav(dockingAutopilotTarget.Descriptor, Acceleration, 20.0f);
                    if (AutopilotTimeRequired != null) autopilotStage = AutopilotStage.TRANSITIONING;
                    break;

                case AutopilotStage.TRANSITIONING: {
                    float tx = AutopilotTimeRequired[0];
                    float ty = AutopilotTimeRequired[1];

                    float target_rotation = Tools.get_angle(tx, ty);

                    if(Rotation != target_rotation) RotateTo(target_rotation, CurrentTime);
                    else {
                        Accelerate(CurrentTime);
                        AutopilotTimeRequired[0] -= (float)Math.Cos(target_rotation) * CurrentTime;
                        AutopilotTimeRequired[1] -= (float)Math.Sin(target_rotation) * CurrentTime;

                        if((tx > 0.0f && AutopilotTimeRequired[0] < 0.0f)
                        || (tx < 0.0f && AutopilotTimeRequired[0] > 0.0f)
                        || (ty > 0.0f && AutopilotTimeRequired[1] < 0.0f)
                        || (ty < 0.0f && AutopilotTimeRequired[1] > 0.0f)) autopilotStage = AutopilotStage.IN_TRANSIT;
                    }
                    break;
                }

                case AutopilotStage.IN_TRANSIT:
                    Descriptor.update_position(CurrentTime);

                    float dx = dockingAutopilotTarget.Self.posx - self.posx;
                    float dy = dockingAutopilotTarget.Self.posy - self.posy;
                    float d  = (float)Math.Sqrt(dx * dx + dy * dy);

                    float targetMeanAnomaly;
                    if (Descriptor.get_distance_from_center() > dockingAutopilotTarget.Descriptor.get_distance_from_center()) targetMeanAnomaly =       0.0f;
                    else targetMeanAnomaly = Tools.pi;

                    float eta = Descriptor.orbital_period * (targetMeanAnomaly - Descriptor.mean_anomaly) * 0.5f;
                    if (eta < 0.0f) eta *= -1;

                    float[] vel = Descriptor.get_velocity_at(Descriptor.get_distance_from_center_at(targetMeanAnomaly), targetMeanAnomaly);

                    AutopilotDeltaV[0] = vel[0] - self.velx;
                    AutopilotDeltaV[1] = vel[1] - self.vely;

                    float timeToSlowDown = Tools.magnitude(AutopilotDeltaV) / Acceleration;

                    float dt = eta - timeToSlowDown;

                    if (dt < 5.0f) autopilotStage = AutopilotStage.MATCHING_ORBIT;
                    break;

                case AutopilotStage.MATCHING_ORBIT: {
                    float dvx = AutopilotDeltaV[0];
                    float dvy = AutopilotDeltaV[1];

                    float target_rotation = Tools.get_angle(dvx, dvy);

                    if(Rotation != target_rotation) RotateTo(target_rotation, CurrentTime);
                    else {
                        Accelerate(CurrentTime);
                        AutopilotDeltaV[0] -= (float)Math.Cos(target_rotation) * CurrentTime * Acceleration;
                        AutopilotDeltaV[1] -= (float)Math.Sin(target_rotation) * CurrentTime * Acceleration;

                        if((dvx > 0.0f && AutopilotDeltaV[0] < 0.0f)
                        || (dvx < 0.0f && AutopilotDeltaV[0] > 0.0f)
                        || (dvy > 0.0f && AutopilotDeltaV[1] < 0.0f)
                        || (dvy < 0.0f && AutopilotDeltaV[1] > 0.0f)) autopilotStage = AutopilotStage.DOCKING;
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
