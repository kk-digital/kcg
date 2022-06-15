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

        public SystemViewBody Self;

        public float Acceleration;

        public float Rotation;

        public float RotationSpeedModifier = 2.0f;

        public List<ShipWeapon> Weapons;

        public bool Destroyed = false;

        private AutopilotStage autopilotStage;
        private SpaceStation dockingAutopilotTarget;

        public SystemShip()
        {
            Self       = new SystemViewBody();
            Descriptor = new OrbitingObjectDescriptor(Self);
            Weapons    = new List<ShipWeapon>();
        }

        public void Destroy()
        {
            Destroyed = true;
        }

        public void RotateTo(float Angle, float CurrentTime)
        {
            while (Rotation < 0.0f) Rotation = 2.0f * 3.1415926f + Rotation;
            while (Rotation > 2.0f * 3.1415926f) Rotation -= 2.0f * 3.1415926f;

            if (Rotation == Angle) return;

            float diff1 = Angle - Rotation;
            float diff2 = Rotation - Angle;

            if (diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
            if (diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

            if (diff2 < diff1)
            {
                Rotation -= RotationSpeedModifier * CurrentTime;

                diff1 = Angle - Rotation;
                diff2 = Rotation - Angle;

                if (diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
                if (diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

                if (diff1 < diff2) Rotation = Angle;
            }
            else
            {
                Rotation += RotationSpeedModifier * CurrentTime;

                diff1 = Angle - Rotation;
                diff2 = Rotation - Angle;

                if (diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
                if (diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

                if (diff2 < diff1) Rotation = Angle;
            }
        }

        public void Circularize(float CurrentTime)
        {
            if (Descriptor.CentralBody == null) return;

            const float     pi = 3.1415926f;
            const float halfpi = 3.1415926f * 0.5f;

            float[] Vel = Descriptor.GetVelocityAt(Descriptor.GetDistanceFromCenterAt(3.1415926f), 3.1415926f);

            float targetrotation = (float)Math.Acos(Vel[0] / Math.Sqrt(Vel[0] * Vel[0] + Vel[1] * Vel[1]));
            float VelocityDirection = (float)Math.Acos(Self.VelX / Math.Sqrt(Self.VelX * Self.VelX + Self.VelY * Self.VelY));
            if (Self.VelY < 0.0f) VelocityDirection = 2.0f * 3.1415926f - VelocityDirection;

            if (Rotation == targetrotation)
            {
                float diff = targetrotation - VelocityDirection;
                if (diff > -0.4f && diff < 0.4f)
                {
                    float AccX = (float)Math.Cos(Rotation) * Acceleration;
                    float AccY = (float)Math.Sin(Rotation) * Acceleration;

                    AccX *= CurrentTime;
                    AccY *= CurrentTime;

                    Self.PosX += Self.VelX * CurrentTime + AccX / 2.0f * CurrentTime;
                    Self.PosY += Self.VelY * CurrentTime + AccY / 2.0f * CurrentTime;

                    Self.VelX += AccX;
                    Self.VelY += AccY;

                    Descriptor.ChangeFrameOfReference(Descriptor.CentralBody);
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
                    if (Descriptor.Eccentricity < 0.02f) autopilotStage = AutopilotStage.PLANNING_TRAJECTORY;
                    break;

                case AutopilotStage.PLANNING_TRAJECTORY:
                    Descriptor.UpdatePosition(CurrentTime);
                    if (Descriptor.PlanPath(dockingAutopilotTarget.Descriptor, AcceptedDeviation)) autopilotStage = AutopilotStage.TRANSITIONING;
                    break;

                case AutopilotStage.TRANSITIONING:

                    break;

                case AutopilotStage.IN_TRANSIT:
                    Descriptor.UpdatePosition(CurrentTime);

                    float dx = dockingAutopilotTarget.Self.PosX - Self.PosX;
                    float dy = dockingAutopilotTarget.Self.PosY - Self.PosY;
                    float d  = (float)Math.Sqrt(dx * dx + dy * dy);

                    float targetMeanAnomaly;
                    if (Descriptor.GetDistanceFromCenter() > dockingAutopilotTarget.Descriptor.GetDistanceFromCenter()) targetMeanAnomaly =       0.0f;
                    else                                                                                                targetMeanAnomaly = 3.1415926f;

                    float eta = Descriptor.OrbitalPeriod * (targetMeanAnomaly - Descriptor.MeanAnomaly) * 0.5f;
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
