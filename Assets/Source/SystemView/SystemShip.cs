using System;
using System.Collections.Generic;

namespace SystemView
{
    enum AutopilotStage
    {
        DISENGAGED = 0,
        PLANNING_TRAJECTORY,
        IN_TRANSIT,
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

        public void EngageDockingAutopilot(SpaceStation Station)
        {
            autopilotStage = AutopilotStage.PLANNING_TRAJECTORY;
            dockingAutopilotTarget = Station;
        }

        public void DisengageDockingAutopilot()
        {
            autopilotStage = AutopilotStage.DISENGAGED;
            dockingAutopilotTarget = null;
        }

        public bool DockingAutopilotLoop(float CurrentTime)
        {
            switch (autopilotStage)
            {
                case AutopilotStage.DISENGAGED:
                    return false;

                case AutopilotStage.PLANNING_TRAJECTORY:
                    if (Descriptor.PlanPath(dockingAutopilotTarget.Descriptor, 0.1f)) autopilotStage = AutopilotStage.IN_TRANSIT;
                    break;

                case AutopilotStage.IN_TRANSIT:
                    Descriptor.UpdatePosition(CurrentTime);

                    float dx = dockingAutopilotTarget.Self.PosX - Self.PosX;
                    float dy = dockingAutopilotTarget.Self.PosY - Self.PosY;
                    float d2 = dx * dx + dy * dy;
                    if (d2 < 0.5f) autopilotStage = AutopilotStage.DOCKING;
                    break;

                case AutopilotStage.DOCKING:
                    // todo
                    break;
            }

            return true;
        }
    }
}
