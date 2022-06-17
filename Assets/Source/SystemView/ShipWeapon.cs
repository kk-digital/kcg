using System;
using System.Collections.Generic;
using UnityEngine; // For color

namespace SystemView
{
    public enum WeaponFlags               // Useful for storing many of a weapon's properties on one single byte
    {
        WEAPON_PROJECTILE = 1 << 0,
        WEAPON_LASER      = 1 << 1,
        WEAPON_BROADSIDE  = 1 << 2,
        WEAPON_TURRET     = 1 << 3,
        WEAPON_POSX       = 1 << 4,       // Left  = flags & WEAPON_POSX, right = ~flags & WEAPON_POSX
        WEAPON_POSY       = 1 << 5        // front = flags & WEAPON_POSY, back  = ~flags & WEAPON_POSY
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

        // Can't be WeaponFlags as type as C# doesn't let you bitwise OR enum values unless every single possible combination
        // you might want to OR is defined as a value... Microsoft why??
        public int flags;

        // todo update this later
        public bool TryFiringAt(SystemShip Target, int CurrentTime)
        {
            Cooldown -= CurrentTime;
            if (Cooldown < 0) Cooldown = 0;

            float dx = Target.self.posx - Self.self.posx;
            float dy = Target.self.posy - Self.self.posy;

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

                Projectile.Body.posx = Self.self.posx;
                Projectile.Body.posy = Self.self.posy;

                Projectile.Body.velx = (Target.self.posx - Self.self.posx) / d * ProjectileVelocity;
                Projectile.Body.vely = (Target.self.posy - Self.self.posy) / d * ProjectileVelocity;
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

            float dx = x - Self.self.posx;
            float dy = y - Self.self.posy;
            float d  = Tools.magnitude(dx, dy);

            if(d > Range) return;

            if((flags & (int)WeaponFlags.WEAPON_BROADSIDE) != 0) {
                float angle = Self.rotation + (((flags & (int)WeaponFlags.WEAPON_POSX) != 0) ? Tools.halfpi : -Tools.halfpi);

                if(angle < 0.0f) angle = Tools.twopi + angle;
                while(angle > Tools.twopi) angle -= Tools.twopi;

                float firing_angle = (float)Math.Acos(dx / d);
                if(dy < 0.0f) firing_angle = 2.0f * 3.1415926f - firing_angle;

                if(firing_angle < angle - Tools.quarterpi || firing_angle > angle + Tools.quarterpi) return;
            }
            
            Cooldown = AttackSpeed;

            if((flags & (int)WeaponFlags.WEAPON_PROJECTILE) != 0) {
                ShipWeaponProjectile Projectile = new ShipWeaponProjectile();

                Projectile.Self = Self;
                Projectile.Weapon = this;

                Projectile.Body.posx = Self.self.posx;
                Projectile.Body.posy = Self.self.posy;

                float angle = (float)Math.Acos(dx / d);

                if (dy < 0.0f) angle = 2.0f * 3.1415926f - angle;

                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);

                Projectile.Body.velx = cos * (float)Math.Sqrt(ProjectileVelocity * ProjectileVelocity - sin * sin) + Self.self.velx;
                Projectile.Body.vely = sin * (float)Math.Sqrt(ProjectileVelocity * ProjectileVelocity - cos * cos) + Self.self.vely;

                Projectile.TimeElapsed = 0.0f;
                Projectile.LifeSpan = Range / ProjectileVelocity;

                Projectile.ProjectileColor = ProjectileColor;

                Projectile.ShieldPenetration = ShieldPenetration;

                Projectile.Damage = Damage;

                ProjectilesFired.Add(Projectile);
            }
        }
    }
}
