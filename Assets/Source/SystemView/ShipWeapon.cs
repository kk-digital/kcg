using System;
using System.Collections.Generic;
using UnityEngine; // For color

namespace SystemView
{
    public enum WeaponFlags               // Useful for storing many of a weapon's properties on one single byte
    {
        WEAPON_PROJECTILE = 2 >> 0,
        WEAPON_LASER      = 2 >> 1,
        WEAPON_BROADSIDE  = 2 >> 2,
        WEAPON_TURRET     = 2 >> 3,
        WEAPON_POSX       = 2 >> 4,       // Left  = flags & WEAPON_POSX, right = ~flags & WEAPON_POSX
        WEAPON_POSY       = 2 >> 5        // front = flags & WEAPON_POSY, back  = ~flags & WEAPON_POSY
    }

    public class ShipWeapon
    {
        public SystemShip Self;

        public Color ProjectileColor;

        public float Range;

        public float ShieldPenetration;

        public float ShieldDamageMultiplier;
        public float HullDamageMultiplier;

        public float ProjectileVelocity;

        public int Damage;

        public int AttackSpeed; // in milliseconds
        public int Cooldown;    // in milliseconds

        public List<ShipWeaponProjectile> ProjectilesFired = new List<ShipWeaponProjectile>();

        public WeaponFlags flags;

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

            Projectile.TimeElapsed            = 0.0f;
            Projectile.LifeSpan               = Range / ProjectileVelocity;

            Projectile.ProjectileColor        = ProjectileColor;

            Projectile.ShieldPenetration      = ShieldPenetration;

            Projectile.ShieldDamageMultiplier = ShieldDamageMultiplier;
            Projectile.HullDamageMultiplier   = HullDamageMultiplier;

            Projectile.Damage = Damage;

            ProjectilesFired.Add(Projectile);

            return true;
        }

        public void Fire(float x, float y)
        {
            if (Cooldown > 0) return;

            Cooldown = AttackSpeed;

            // if (flags & WeaponFlags.WEAPON_PROJECTILE)     // don't you hate it when languages don't accept ints or enums in ifs?...
            if ((flags & WeaponFlags.WEAPON_PROJECTILE) != 0) // like do I really need that != 0 and those brackets?...
            {
                ShipWeaponProjectile Projectile = new ShipWeaponProjectile();

                Projectile.Self = Self;
                Projectile.Weapon = this;

                Projectile.Body.PosX = Self.Self.PosX;
                Projectile.Body.PosY = Self.Self.PosY;

                float dx = x - Self.Self.PosX;
                float dy = y - Self.Self.PosY;

                float d = (float)Math.Sqrt(dx * dx + dy * dy);

                float angle = (float)Math.Acos(dx / d);

                if (dy < 0.0f) angle = 2.0f * 3.1415926f - angle;

                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);

                Projectile.Body.VelX = cos * (float)Math.Sqrt(ProjectileVelocity * ProjectileVelocity - sin * sin) + Self.Self.VelX;
                Projectile.Body.VelY = sin * (float)Math.Sqrt(ProjectileVelocity * ProjectileVelocity - cos * cos) + Self.Self.VelY;

                Projectile.TimeElapsed = 0.0f;
                Projectile.LifeSpan = Range / ProjectileVelocity;

                Projectile.ProjectileColor = ProjectileColor;

                Projectile.ShieldPenetration = ShieldPenetration;

                Projectile.Damage = Damage;

                ProjectilesFired.Add(Projectile);
            }
            
            if ((flags & WeaponFlags.WEAPON_LASER) != 0)
            {

            }
        }
    }
}
