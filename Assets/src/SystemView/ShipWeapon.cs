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

        // todo update this later
        public bool TryFiringAt(SystemShip Target, int CurrentTime)
        {
            Cooldown -= CurrentTime;
            if (Cooldown < 0) Cooldown = 0;

            if (Cooldown > 0 || Self == Target || Target == null || Math.Sqrt((Self.PosX - Target.PosX) * (Self.PosX - Target.PosX) + (Self.PosY - Target.PosY) * (Self.PosY - Target.PosY)) > Range) return false;

            Cooldown = AttackSpeed;

            ShipWeaponProjectile Projectile = new ShipWeaponProjectile();

            /*if (Self.Descriptor != null) // orbit
            {
                // todo
                // is this even needed? a straight line approximation might be fine either way as ships are fighting within very short LifeSpan, right?
            }
            else // straight line
            {*/
                Projectile.Self = Self;
                Projectile.Weapon = this;

                Projectile.PosX = Self.PosX;
                Projectile.PosY = Self.PosY;

                float d = (float)Math.Sqrt((Target.PosX - Self.PosX) * (Target.PosX - Self.PosX) + (Target.PosY - Self.PosY) * (Target.PosY - Self.PosY));

                Projectile.VelX = (Target.PosX - Self.PosX) / d * ProjectileVelocity;
                Projectile.VelY = (Target.PosY - Self.PosY) / d * ProjectileVelocity;
            //}

            Projectile.TimeElapsed = 0.0f;
            Projectile.LifeSpan = Range / ProjectileVelocity;

            Projectile.ProjectileColor = ProjectileColor;

            Projectile.ShieldPenetration = ShieldPenetration;

            Projectile.Damage = Damage;

            ProjectilesFired.Add(Projectile);

            return true;
        }

        public void Fire()
        {
            if (Cooldown > 0) return;

            Cooldown = AttackSpeed;

            ShipWeaponProjectile Projectile = new ShipWeaponProjectile();

            Projectile.Self = Self;
            Projectile.Weapon = this;

            Projectile.PosX = Self.PosX;
            Projectile.PosY = Self.PosY;

            float cos = (float)Math.Cos(Self.Rotation);
            float sin = (float)Math.Sin(Self.Rotation);

            Projectile.VelX = cos * (float)Math.Sqrt(ProjectileVelocity * ProjectileVelocity - sin * sin) + Self.VelX;
            Projectile.VelY = sin * (float)Math.Sqrt(ProjectileVelocity * ProjectileVelocity - cos * cos) + Self.VelY;

            Projectile.TimeElapsed = 0.0f;
            Projectile.LifeSpan = Range / ProjectileVelocity;

            Projectile.ProjectileColor = ProjectileColor;

            Projectile.ShieldPenetration = ShieldPenetration;

            Projectile.Damage = Damage;

            ProjectilesFired.Add(Projectile);
        }
    }
}
