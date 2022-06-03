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

            /*if (Self.Descriptor != null) // orbit
            {
                // todo
                // is this even needed? a straight line approximation might be fine either way as ships are fighting within very short range, right?
            }
            else // straight line
            {*/
                Projectile.Self = Self;
                Projectile.Weapon = this;

                Projectile.PosX = Self.PosX;
                Projectile.PosY = Self.PosY;

                // todo: Math doesn't like vertical lines.
                Projectile.Slope = (Target.PosY - Self.PosY) / (Target.PosX - Self.PosX);
            //}

            Projectile.DistanceTravelled = 0.0f;
            Projectile.Range = Range;

            Projectile.ProjectileColor = ProjectileColor;

            Projectile.ShieldPenetration = ShieldPenetration;

            Projectile.ProjectileVelocity = ProjectileVelocity * (((Target.PosX - Self.PosX) < 0.0f) ? -1 : 1);

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

            // todo: Math doesn't like vertical lines.
            Projectile.Slope = (float)(Math.Sin(Self.Rotation) / Math.Cos(Self.Rotation));

            bool Reverse = Math.Cos(Self.Rotation) < 0.0f && Self.VelX > 0.0f
                        || Math.Cos(Self.Rotation) > 0.0f && Self.VelX < 0.0f
                        || Math.Sin(Self.Rotation) < 0.0f && Self.VelY > 0.0f
                        || Math.Sin(Self.Rotation) > 0.0f && Self.VelY < 0.0f;

            Projectile.DistanceTravelled = 0.0f;
            Projectile.Range = Range + (float)Math.Sqrt(Self.VelX * Self.VelX + Self.VelY * Self.VelY) * (Reverse ? -1 : 1) * Range / ProjectileVelocity;

            Projectile.ProjectileColor = ProjectileColor;

            Projectile.ShieldPenetration = ShieldPenetration;

            Projectile.ProjectileVelocity = (ProjectileVelocity + (float)Math.Sqrt(Self.VelX * Self.VelX + Self.VelY * Self.VelY) * (Reverse ? -1 : 1)) * ((Math.Cos(Self.Rotation) < 0.0) ? -1 : 1);

            Projectile.Damage = Damage;

            ProjectilesFired.Add(Projectile);
        }
    }
}
