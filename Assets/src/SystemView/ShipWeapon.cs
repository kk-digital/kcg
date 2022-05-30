using UnityEngine; // For color

namespace SystemView
{
    public class ShipWeapon
    {
        public Color ProjectileColor;

        public float Range;

        public float ShieldPenetration;

        public int Damage;

        public int AttackSpeed; // in milliseconds
        public int Cooldown;    // in milliseconds
    }
}
