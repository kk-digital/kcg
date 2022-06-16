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

        public bool Destroyed = false;

        private AutopilotStage autopilotStage;
        private SpaceStation dockingAutopilotTarget;

        public SystemShip()
        {
            self       = new SpaceObject();
            Descriptor = new OrbitingObjectDescriptor(self);
            Weapons    = new List<ShipWeapon>();
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

        public void Circularize(float CurrentTime)
        {
            if (Descriptor.central_body == null) return;

            float[] Vel = Descriptor.GetVelocityAt(Descriptor.GetDistanceFromCenterAt(Tools.pi), Tools.pi);

            float targetrotation = (float)Math.Acos(Vel[0] / Math.Sqrt(Vel[0] * Vel[0] + Vel[1] * Vel[1]));
            float VelocityDirection = (float)Math.Acos(self.velx / Math.Sqrt(self.velx * self.velx + self.vely * self.vely));
            if (self.vely < 0.0f) VelocityDirection = Tools.twopi - VelocityDirection;

            if (Rotation == targetrotation)
            {
                float diff = targetrotation - VelocityDirection;
                if (diff > -0.4f && diff < 0.4f)
                {
                    float AccX = (float)Math.Cos(Rotation) * Acceleration;
                    float AccY = (float)Math.Sin(Rotation) * Acceleration;

                    AccX *= CurrentTime;
                    AccY *= CurrentTime;

                    self.posx += self.velx * CurrentTime + AccX / 2.0f * CurrentTime;
                    self.posy += self.vely * CurrentTime + AccY / 2.0f * CurrentTime;

                    self.velx += AccX;
                    self.vely += AccY;

                    Descriptor.ChangeFrameOfReference(Descriptor.central_body);
                } else Descriptor.UpdatePosition(CurrentTime);
            } else { RotateTo(targetrotation, CurrentTime); Descriptor.UpdatePosition(CurrentTime); }
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
                    Descriptor.UpdatePosition(CurrentTime);
                    if (Descriptor.PlanPath(dockingAutopilotTarget.Descriptor, AcceptedDeviation)) autopilotStage = AutopilotStage.TRANSITIONING;
                    break;

                case AutopilotStage.TRANSITIONING:

                    break;

                case AutopilotStage.IN_TRANSIT:
                    Descriptor.UpdatePosition(CurrentTime);

                    float dx = dockingAutopilotTarget.Self.posx - self.posx;
                    float dy = dockingAutopilotTarget.Self.posy - self.posy;
                    float d  = (float)Math.Sqrt(dx * dx + dy * dy);

                    float targetMeanAnomaly;
                    if (Descriptor.GetDistanceFromCenter() > dockingAutopilotTarget.Descriptor.GetDistanceFromCenter()) targetMeanAnomaly =       0.0f;
                    else                                                                                                targetMeanAnomaly = Tools.pi;

                    float eta = Descriptor.orbital_period * (targetMeanAnomaly - Descriptor.mean_anomaly) * 0.5f;
                    if (eta < 0.0f) eta *= -1;

                    float[] vel = Descriptor.GetVelocityAt(Descriptor.GetDistanceFromCenterAt(targetMeanAnomaly), targetMeanAnomaly);

                    float timeToSlowDown = (float)Math.Sqrt(vel[0] * vel[0] + vel[1] * vel[1]) / Acceleration;

                    float dt = eta - timeToSlowDown;

                    if (dt < 5.0f) autopilotStage = AutopilotStage.MATCHING_ORBIT;
                    break;

                case AutopilotStage.MATCHING_ORBIT:

                    break;

                case AutopilotStage.DOCKING:
                    // todo
                    break;
            }

            return true;
        }
    }
}
