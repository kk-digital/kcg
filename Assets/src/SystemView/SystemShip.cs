using System;
using System.Collections.Generic;

namespace SystemView
{
    public class SystemShip
    {
        public OrbitingObjectDescriptor Descriptor;
        public OrbitingObjectDescriptor Start, Destination;
        public bool PathPlanned = false;
        public bool Reached = false;

        public int Health, MaxHealth;
        public int Shield, MaxShield;

        public int ShieldRegenerationRate;

        public float PosX, PosY;

        public List<ShipWeapon> Weapons;

        public SystemShip()
        {
            Descriptor = new OrbitingObjectDescriptor();
            Weapons = new List<ShipWeapon>();
        }

        public bool PlanPath(OrbitingObjectDescriptor Start, OrbitingObjectDescriptor Destination, float AcceptableDeviation)
        {
            return PathPlanned = Descriptor.PlanPath(Start, Destination, AcceptableDeviation);
        }

        public void UpdatePosition(float dt)
        {
            Descriptor.RotationalPosition += dt / Descriptor.GetDistanceFromCenter() / Descriptor.GetDistanceFromCenter();

            float[] Pos = Descriptor.GetPosition();

            PosX = Pos[0];
            PosY = Pos[1];
        }
    }
}
