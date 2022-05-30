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

        public List<ShipWeaponProjectile> ProjectilesFired;

        public void TryFiring(SystemShip Target)
        {
            if (Cooldown > 0) return;

            if (Self == Target) return;

            if (Math.Sqrt(Self.PosX - Target.PosX) * (Self.PosX - Target.PosX) + (Self.PosY - Target.PosY) * (Self.PosY - Target.PosY) > Range) return;

            Cooldown = AttackSpeed;

            ShipWeaponProjectile Projectile = new ShipWeaponProjectile();

            // todo: Projectile orbit

            Projectile.Self = Self;

            Projectile.PosX = Self.PosX;
            Projectile.PosY = Self.PosY;

            Projectile.Slope = (Target.PosX - Self.PosX) / (Target.PosY - Self.PosY);
            Projectile.NegativeDirection = (Target.PosX - Self.PosX) < 0.0f;

            Projectile.DistanceTravelled = 0.0f;
            Projectile.Range = Range;

            Projectile.ProjectileColor = ProjectileColor;

            Projectile.ShieldPenetration = ShieldPenetration;

            Projectile.ProjectileVelocity = ProjectileVelocity;

            Projectile.Damage = Damage;

            ProjectilesFired.Add(Projectile);
        }
    }
}
