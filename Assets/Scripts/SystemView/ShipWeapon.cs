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
        public SystemShip self;

        public Color color;

        public float range;

        public float shield_penetration;

        public float shield_damage_multiplier;
        public float hull_damage_multiplier;

        public float projectile_velocity;

        public int damage;

        public float rotation_rate;
        public float rotation;
        public float FOV;

        public int attack_speed; // in milliseconds
        public int cooldown;     // in milliseconds

        public List<ShipWeaponProjectile> projectiles_fired = new List<ShipWeaponProjectile>();

        // Can't be WeaponFlags as type as C# doesn't let you bitwise OR enum values unless every single possible combination
        // you might want to OR is defined as a value... Microsoft why??
        public int flags;

        // todo update this later
        public bool TryFiringAt(SystemShip target, int current_millis)
        {
            cooldown -= current_millis;
            if (cooldown < 0) cooldown = 0;

            float dx = target.self.posx - self.self.posx;
            float dy = target.self.posy - self.self.posy;

            float d = (float)Math.Sqrt(dx * dx + dy * dy);

            if (cooldown > 0 || self == target || target == null || d > range) return false;

            cooldown = attack_speed;

            ShipWeaponProjectile projectile = new ShipWeaponProjectile();

            /*if (Self.Descriptor != null) // orbit
            {
                // todo
                // is this even needed? a straight line approximation might be fine either way as ships are fighting within very short range, right?
            }
            else // straight line
            {*/
                projectile.Self = self;
                projectile.Weapon = this;

                projectile.Body.posx = self.self.posx;
                projectile.Body.posy = self.self.posy;

                projectile.Body.velx = (target.self.posx - self.self.posx) / d * projectile_velocity;
                projectile.Body.vely = (target.self.posy - self.self.posy) / d * projectile_velocity;
            //}

            projectile.TimeElapsed            = 0.0f;
            projectile.LifeSpan               = range / projectile_velocity;

            projectile.ProjectileColor        = color;

            projectile.ShieldPenetration      = shield_penetration;

            projectile.ShieldDamageMultiplier = shield_damage_multiplier;
            projectile.HullDamageMultiplier   = hull_damage_multiplier;

            projectile.Damage = damage;

            projectiles_fired.Add(projectile);

            return true;
        }

        public void update() {
            if(rotation_rate == 0.0f) {
                rotation = self.rotation;

                if((flags & (int)WeaponFlags.WEAPON_BROADSIDE) != 0)
                    rotation += (((flags & (int)WeaponFlags.WEAPON_POSX) != 0) ? Tools.halfpi : -Tools.halfpi);
            }
        }

        public bool try_targeting(float x, float y, float current_time) {
            float dx = x - self.self.posx;
            float dy = y - self.self.posy;
            float d  = Tools.magnitude(dx, dy);

            if (rotation_rate == 0.0f)
                return d <= range;

            while(rotation <        0.0f) rotation  = Tools.twopi + rotation;
            while(rotation > Tools.twopi) rotation -= Tools.twopi;

            if(d > range) return false;

            float angle = Tools.get_angle(dx, dy);

            if(rotation == angle) return true;

            float diff1 = angle - rotation;
            float diff2 = rotation - angle;

            if(diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
            if(diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

            if(diff2 < diff1) {
                rotation -= rotation_rate * current_time;

                diff1 = angle - rotation;
                diff2 = rotation - angle;

                if(diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
                if(diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

                if(diff1 < diff2) rotation = angle;
            } else {
                rotation += rotation_rate * current_time;

                diff1 = angle - rotation;
                diff2 = rotation - angle;

                if(diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
                if(diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

                if(diff2 < diff1) rotation = angle;
            }

            return true;
        }

        public void fire(float x, float y)
        {
            if (cooldown > 0) return;

            float dx = x - self.self.posx;
            float dy = y - self.self.posy;
            float d  = Tools.magnitude(dx, dy);

            if(d > range) return;

            if(FOV != 0.0f) {
                float angle = rotation;

                if((flags & (int)WeaponFlags.WEAPON_BROADSIDE) != 0)
                    angle = self.rotation + (((flags & (int)WeaponFlags.WEAPON_POSX) != 0) ? Tools.halfpi : -Tools.halfpi);


                if(angle < 0.0f) angle = Tools.twopi + angle;
                while(angle > Tools.twopi) angle -= Tools.twopi;

                float firing_angle = (float)Math.Acos(dx / d);
                if(dy < 0.0f) firing_angle = 2.0f * 3.1415926f - firing_angle;

                if(firing_angle < angle - FOV * 0.5f || firing_angle > angle + FOV * 0.5f) return;
            }

            cooldown = attack_speed;

            if((flags & (int)WeaponFlags.WEAPON_PROJECTILE) != 0) {
                ShipWeaponProjectile projectile = new ShipWeaponProjectile();

                projectile.Self = self;
                projectile.Weapon = this;

                projectile.Body.posx = self.self.posx;
                projectile.Body.posy = self.self.posy;

                float angle = (float)Math.Acos(dx / d);

                if (dy < 0.0f) angle = 2.0f * 3.1415926f - angle;

                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);

                projectile.Body.velx = cos * (float)Math.Sqrt(projectile_velocity * projectile_velocity - sin * sin) + self.self.velx;
                projectile.Body.vely = sin * (float)Math.Sqrt(projectile_velocity * projectile_velocity - cos * cos) + self.self.vely;

                projectile.TimeElapsed = 0.0f;
                projectile.LifeSpan = range / projectile_velocity;

                projectile.ProjectileColor = color;

                projectile.ShieldPenetration = shield_penetration;

                projectile.Damage = damage;

                projectiles_fired.Add(projectile);
            }
        }
    }
}
