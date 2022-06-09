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

            float dx = Target.Self.PosX - Self.Self.PosX;
            float dy = Target.Self.PosY - Self.Self.PosY;

            float d = (float)Math.Sqrt(dx * dx + dy * dy);

            if (Cooldown > 0 || Self == Target || Target == null || d > Range) return false;

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

                Projectile.Body.PosX = Self.Self.PosX;
                Projectile.Body.PosY = Self.Self.PosY;

                Projectile.Body.VelX = (Target.Self.PosX - Self.Self.PosX) / d * ProjectileVelocity;
                Projectile.Body.VelY = (Target.Self.PosY - Self.Self.PosY) / d * ProjectileVelocity;
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

            Projectile.Body.PosX = Self.Self.PosX;
            Projectile.Body.PosY = Self.Self.PosY;

            float cos = (float)Math.Cos(Self.Rotation);
            float sin = (float)Math.Sin(Self.Rotation);

            Projectile.Body.VelX = cos * (float)Math.Sqrt(ProjectileVelocity * ProjectileVelocity - sin * sin) + Self.Self.VelX;
            Projectile.Body.VelY = sin * (float)Math.Sqrt(ProjectileVelocity * ProjectileVelocity - cos * cos) + Self.Self.VelY;

            Projectile.TimeElapsed = 0.0f;
            Projectile.LifeSpan = Range / ProjectileVelocity;

            Projectile.ProjectileColor = ProjectileColor;

            Projectile.ShieldPenetration = ShieldPenetration;

            Projectile.Damage = Damage;

            ProjectilesFired.Add(Projectile);
        }
    }
}
