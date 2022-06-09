using System;
using System.Collections.Generic;

namespace SystemView
{
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
    }
}
