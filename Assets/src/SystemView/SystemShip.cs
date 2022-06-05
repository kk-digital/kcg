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
        public float VelX, VelY;

        public float Acceleration;

        public float Rotation;

        public List<ShipWeapon> Weapons;

        public bool Destroyed = false;

        public SystemShip()
        {
            Descriptor = new OrbitingObjectDescriptor();
            Weapons = new List<ShipWeapon>();
        }

        public void UpdatePosition(float dt)
        {
            if (Descriptor.SemiMajorAxis != 0.0f)
            {
                Descriptor.RotationalPosition += dt / Descriptor.GetDistanceFromCenter() / Descriptor.GetDistanceFromCenter();
                Rotation = Descriptor.RotationalPosition + 3.1415926f / 2.0f;

                float[] Pos = Descriptor.GetPosition();

                PosX = Pos[0];
                PosY = Pos[1];
            }
        }

        public void Destroy()
        {
            Destroyed = true;
        }
    }
}
