using System;
using System.Collections.Generic;
using UnityEngine; // For color

namespace SystemView
{
    public class ShipWeapon
    {
        public SystemShip Self;

        public Color ProjectileColor;

        public float Range;

        public float ShieldPenetration;

        public float ProjectileVelocity;

        public int Damage;

        public int AttackSpeed; // in milliseconds
        public int Cooldown;    // in milliseconds

        public List<ShipWeaponProjectile> ProjectilesFired = new List<ShipWeaponProjectile>();

        public bool TryFiringAt(SystemShip Target, int CurrentTime)
        {
            Cooldown -= CurrentTime;
            if (Cooldown < 0) Cooldown = 0;

            if (Cooldown > 0 || Self == Target || Target == null || Math.Sqrt((Self.PosX - Target.PosX) * (Self.PosX - Target.PosX) + (Self.PosY - Target.PosY) * (Self.PosY - Target.PosY)) > Range) return false;

            Cooldown = AttackSpeed;

            ShipWeaponProjectile Projectile = new ShipWeaponProjectile();

            // todo: Projectile orbit

            Projectile.Self = Self;
            Projectile.Weapon = this;

            Projectile.PosX = Self.PosX;
            Projectile.PosY = Self.PosY;

            // todo: Math doesn't like vertical lines.
            Projectile.Slope = (Target.PosY - Self.PosY) / (Target.PosX - Self.PosX);
            Projectile.NegativeDirection = (Target.PosX - Self.PosX) < 0.0f;

            Projectile.DistanceTravelled = 0.0f;
            Projectile.Range = Range;

            Projectile.ProjectileColor = ProjectileColor;

            Projectile.ShieldPenetration = ShieldPenetration;

            Projectile.ProjectileVelocity = ProjectileVelocity;

            Projectile.Damage = Damage;

            ProjectilesFired.Add(Projectile);

            return true;
        }
    }
}
