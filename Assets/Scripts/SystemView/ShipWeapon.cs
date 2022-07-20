using System;
using System.Collections.Generic;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView
    {
        public enum WeaponFlags {             // Useful for storing many of a weapon's properties on one single byte
            WEAPON_PROJECTILE = 1 << 0,
            WEAPON_LASER      = 1 << 1,
            WEAPON_BROADSIDE  = 1 << 2,
            WEAPON_TURRET     = 1 << 3,
            WEAPON_ROCKET     = 1 << 4,
            WEAPON_SEEKING    = 1 << 5,       // Projectiles that seek nearby enemies
            WEAPON_POSX       = 1 << 6,       // Left  = flags & WEAPON_POSX, right = ~flags & WEAPON_POSX
            WEAPON_POSY       = 1 << 7,       // front = flags & WEAPON_POSY, back  = ~flags & WEAPON_POSY
        }

        public class ShipWeapon {
            public SystemShip self;

            public Color color;

            public float range;

            public float shield_penetration;

            public float shield_damage_multiplier;
            public float hull_damage_multiplier;

            public float projectile_velocity;

            public float last_x;
            public float last_y;
            public float acc;

            public int damage;

            public float rotation_rate;
            public float rotation;
            public float FOV;
            public Vector3[] vertices = new Vector3[2];

            public int attack_speed; // in milliseconds
            public int cooldown;     // in milliseconds

            public int   projectiles_per_burst = 1;
            public float projectile_spread;
            public float projectile_mass = 0.01f;

            public float max_velocity;
            public float detection_angle;

            public int   penetration = 1; // Amount of enemies beams and projectiles can hit

            public List<ShipWeaponProjectile> projectiles_fired = new List<ShipWeaponProjectile>();

            public CameraController camera;
            public GameObject       laser_object;
            public LineRenderer     laser_renderer;
            public Material         mat;
            public SystemState      state;

            public int flags;

            private class target_info {
                public SystemShip ship;
                public float      distance;
            };

            // Pre defined weapon types
            // Regular single shot cannon
            public static ShipWeapon add_cannon(SystemShip self, SystemState state, int flags) {
                ShipWeapon cannon = new ShipWeapon();

                cannon.color                 = Color.white;
                cannon.range                 = 30.0f;
                cannon.shield_penetration    = 0.2f;
                cannon.projectile_velocity   = 5.0f;
                cannon.damage                = 3000;
                cannon.attack_speed          = 800;
                cannon.cooldown              = 0;
                cannon.FOV                   = Tools.quarterpi;
                cannon.self                  = self;
                cannon.state                 = state;
                cannon.projectiles_per_burst = 1;
                cannon.flags                 = (int)WeaponFlags.WEAPON_PROJECTILE | flags;

                self.weapons.Add(cannon);

                return cannon;
            }

            // Autocannon shoots 2x as fast as cannon, but 2/3 damage
            public static ShipWeapon add_auto_cannon(SystemShip self, SystemState state, int flags) {
                ShipWeapon cannon = new ShipWeapon();

                cannon.color                 = Color.white;
                cannon.range                 = 30.0f;
                cannon.shield_penetration    = 0.2f;
                cannon.projectile_velocity   = 5.0f;
                cannon.damage                = 2000;
                cannon.attack_speed          = 400;
                cannon.cooldown              = 0;
                cannon.FOV                   = Tools.quarterpi;
                cannon.self                  = self;
                cannon.state                 = state;
                cannon.projectiles_per_burst = 1;
                cannon.flags                 = (int)WeaponFlags.WEAPON_PROJECTILE | flags;

                self.weapons.Add(cannon);

                return cannon;
            }

            // Tri cannon fires 3 shots at once, each shot does same damage as auto cannon, but shoots a little over 3x slower
            public static ShipWeapon add_tri_cannon(SystemShip self, SystemState state, int flags) {
                ShipWeapon cannon = new ShipWeapon();

                cannon.color                 = Color.white;
                cannon.range                 = 30.0f;
                cannon.shield_penetration    = 0.2f;
                cannon.projectile_velocity   = 5.0f;
                cannon.damage                = 2000;
                cannon.attack_speed          = 1250;
                cannon.cooldown              = 0;
                cannon.FOV                   = Tools.sixthpi;
                cannon.self                  = self;
                cannon.state                 = state;
                cannon.projectiles_per_burst = 3;
                cannon.projectile_spread     = Tools.sixthpi;
                cannon.flags                 = (int)WeaponFlags.WEAPON_PROJECTILE | flags;

                self.weapons.Add(cannon);

                return cannon;
            }

            // Very high damage low fire rate weapon
            public static ShipWeapon add_railgun(SystemShip self, SystemState state, int flags) {
                ShipWeapon railgun = new ShipWeapon();

                railgun.color                 = new Color(0.8f, 0.7f, 0.3f, 1.0f);
                railgun.range                 = 100.0f;
                railgun.shield_penetration    = 0.3f;
                railgun.projectile_velocity   = 80.0f;
                railgun.damage                = 4000;
                railgun.attack_speed          = 2500;
                railgun.cooldown              = 0;
                railgun.FOV                   = Tools.sixthpi;
                railgun.self                  = self;
                railgun.state                 = state;
                railgun.projectiles_per_burst = 1;
                railgun.flags                 = (int)WeaponFlags.WEAPON_PROJECTILE | flags;

                self.weapons.Add(railgun);

                return railgun;
            }

            // Slow missile that tracks projectiles in a 45 degree cone
            public static ShipWeapon add_torpedo(SystemShip self, SystemState state, int flags) {
                ShipWeapon torpedo = new ShipWeapon();

                torpedo.color                 = new Color(1.0f, 0.4f, 0.3f, 1.0f);
                torpedo.range                 = 75.0f;
                torpedo.shield_penetration    = 0.05f;
                torpedo.projectile_velocity   =  5.0f;
                torpedo.acc                   = 12.5f;
                torpedo.max_velocity          =  7.5f;
                torpedo.damage                =  8000;
                torpedo.attack_speed          =   750;
                torpedo.cooldown              =     0;
                torpedo.FOV                   = Tools.quarterpi;
                torpedo.self                  = self;
                torpedo.state                 = state;
                torpedo.projectiles_per_burst = 1;
                torpedo.flags                 = (int)WeaponFlags.WEAPON_PROJECTILE
                                              | (int)WeaponFlags.WEAPON_SEEKING
                                              | (int)WeaponFlags.WEAPON_ROCKET
                                              | flags;

                self.weapons.Add(torpedo);

                return torpedo;
            }

            public void cleanup() {
                if(laser_renderer != null) GameObject.Destroy(laser_renderer);
                if(laser_object   != null) GameObject.Destroy(laser_object);
            }

            // todo remove
            public bool TryFiringAt(SystemShip target, int current_millis) {
                cooldown -= current_millis;
                if (cooldown < 0) cooldown = 0;

                float dx = target.self.posx - self.self.posx;
                float dy = target.self.posy - self.self.posy;

                float d = (float)Math.Sqrt(dx * dx + dy * dy);

                if (cooldown > 0 || self == target || target == null || d > range) return false;

                cooldown = attack_speed;

                ShipWeaponProjectile projectile   = new ShipWeaponProjectile();

                projectile.Self                   = self;
                projectile.Weapon                 = this;

                projectile.Body.posx              = self.self.posx;
                projectile.Body.posy              = self.self.posy;

                projectile.Body.velx              = (target.self.posx - self.self.posx) / d * projectile_velocity;
                projectile.Body.vely              = (target.self.posy - self.self.posy) / d * projectile_velocity;

                projectile.TimeElapsed            = 0.0f;
                projectile.LifeSpan               = range / projectile_velocity;

                projectile.ProjectileColor        = color;

                projectile.ShieldPenetration      = shield_penetration;

                projectile.ShieldDamageMultiplier = shield_damage_multiplier;
                projectile.HullDamageMultiplier   = hull_damage_multiplier;

                projectile.Damage                 = damage;

                projectile.seeking                = (flags & (int)WeaponFlags.WEAPON_SEEKING) != 0;
                projectile.rocket                 = (flags & (int)WeaponFlags.WEAPON_ROCKET)  != 0;
                projectile.acc                    = acc;
                projectile.state                  = state;
                projectile.penetration            = penetration;

                projectiles_fired.Add(projectile);

                return true;
            }

            public void update() {
                if(rotation_rate == 0.0f) {
                    rotation = self.rotation;

                    if((flags & (int)WeaponFlags.WEAPON_BROADSIDE) != 0)
                        rotation += ((flags & (int)WeaponFlags.WEAPON_POSX) != 0) ? Tools.halfpi : -Tools.halfpi;
                }

                if((flags & (int)WeaponFlags.WEAPON_LASER) != 0) {
                    int charging_time           = (int)(attack_speed * 0.05f);
                    int laser_duration_time     = (int)(attack_speed * 0.45f);
                    int remaining_charging_time = cooldown - (attack_speed - charging_time);
                    int remaining_time          = cooldown - (attack_speed - laser_duration_time);

                    if(remaining_time > 0) {
                        vertices[0]                  = new Vector3(self.self.posx, self.self.posy, 0.0f);
                        vertices[1]                  = new Vector3(last_x, last_y, 0.0f);

                        laser_renderer.SetPositions(vertices);
                        laser_renderer.positionCount = 2;

                        if(remaining_charging_time > 0) {
                            float RemainingTimeAsPercentage = 1.0f - (float)remaining_charging_time / (float)laser_duration_time;
                            laser_renderer.startWidth = laser_renderer.endWidth = 0.1f * RemainingTimeAsPercentage / camera.scale;
                            laser_renderer.startColor = new Color(color.r, color.g, color.b, color.a * RemainingTimeAsPercentage + 0.10f);
                            laser_renderer.endColor   = new Color(color.r, color.g, color.b, color.a * RemainingTimeAsPercentage + 0.02f);
                        } else {
                            float RemainingTimeAsPercentage = (float)remaining_time / (float)laser_duration_time;
                            laser_renderer.startWidth = laser_renderer.endWidth = RemainingTimeAsPercentage * 0.1f / camera.scale;
                            laser_renderer.startColor = new Color(color.r, color.g, color.b, color.a * RemainingTimeAsPercentage + 0.10f);
                            laser_renderer.endColor   = new Color(color.r, color.g, color.b, color.a * RemainingTimeAsPercentage + 0.02f);
                        }
                    } else {
                        GameObject.Destroy(laser_renderer);
                        GameObject.Destroy(laser_object);
                    }
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

            public void fire(float x, float y) {
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
                    float half_spread           = projectile_spread * 0.5f;
                    float spread_per_projectile = projectile_spread / projectiles_per_burst;

                    for(int i = 0; i < projectiles_per_burst; i++) {
                        ShipWeaponProjectile projectile = new ShipWeaponProjectile();

                        projectile.Self = self;
                        projectile.Weapon = this;

                        projectile.Body.posx = self.self.posx;
                        projectile.Body.posy = self.self.posy;

                        float angle = Tools.get_angle(dx, dy);

                        angle -= half_spread;
                        angle += spread_per_projectile * (i + 0.5f);
                        angle  = Tools.normalize_angle(angle);

                        float rel_velx = (float)Math.Cos(angle) * projectile_velocity;
                        float rel_vely = (float)Math.Sin(angle) * projectile_velocity;

                        projectile.Body.velx         = self.self.velx + rel_velx;
                        projectile.Body.vely         = self.self.vely + rel_vely;

                        projectile.TimeElapsed       = 0.0f;
                        projectile.LifeSpan          = range / projectile_velocity;

                        projectile.ProjectileColor   = color;

                        projectile.ShieldPenetration = shield_penetration;

                        projectile.Damage            = damage;

                        projectile.seeking           = (flags & (int)WeaponFlags.WEAPON_SEEKING)  != 0;
                        projectile.detection_angle   = detection_angle;
                        projectile.rocket            = (flags & (int)WeaponFlags.WEAPON_ROCKET)   != 0;
                        projectile.acc_angle         = angle;
                        projectile.acc               = acc;
                        projectile.state             = state;
                        projectile.penetration       = penetration;
                        projectile.max_velocity      = max_velocity;

                        /*
                         * (1) E  = m * v²
                         * 
                         * Split into x and y component
                         * 
                         * (2) Ex = m * vx²
                         *      ⤷ Energy imparted along x-axis
                         *      
                         * (3) Ey = m * vy²
                         *      ⤷ Energy imparted along y-axis
                         *      
                         * Solve for velocity change applied to ship
                         * 
                         * (4) m₀ * vx₀² = m₁ * vx₁²
                         *        ⤹          ⤷ Energy imparted on the projectile
                         *         Energy imparted on the ship
                         * 
                         *                m₁ * vx₁²
                         * (4) vx₀ = ± √ ——————————
                         *           ⤹       m₀
                         *            Sign is opposite of vx₁'s sign
                         * 
                         * (5) m₀ * vy₀² = m₁ * vy₁²
                         *        ⤹          ⤷ Energy imparted on the projectile
                         *         Energy imparted on the ship
                         * 
                         *                m₁ * vy₁²
                         * (5) vy₀ = ± √ ——————————
                         *           ⤹       m₀
                         *            Sign is opposite of vy₁'s sign
                         * 
                         */

                        float vx = (float)Math.Sqrt(projectile_mass / self.self.mass * rel_velx * rel_velx);
                        float vy = (float)Math.Sqrt(projectile_mass / self.self.mass * rel_vely * rel_vely);

                        if(rel_velx >= 0.0f) self.self.velx -= vx;
                        else                 self.self.velx += vx;

                        if(rel_vely >= 0.0f) self.self.vely -= vy;
                        else                 self.self.vely += vy;

                        projectiles_fired.Add(projectile);
                    }
                }

                if((flags & (int)WeaponFlags.WEAPON_LASER) != 0) {
                    if(mat == null) {
                        Shader shader = Shader.Find("Hidden/Internal-Colored");
                        mat = new Material(shader);
                        mat.hideFlags = HideFlags.HideAndDontSave;

                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

                        // Turn off backface culling, depth writes, depth test.
                        mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                        mat.SetInt("_ZWrite", 0);
                        mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                    }

                    laser_object                 = new GameObject();
                    laser_object.name            = "Laser";

                    laser_renderer               = laser_object.AddComponent<LineRenderer>();
                    laser_renderer.material      = mat;
                    laser_renderer.useWorldSpace = true;

                    vertices[0]                  = new Vector3(self.self.posx, self.self.posy, 0.0f);
                    vertices[1]                  = new Vector3(x, y, 0.0f);

                    laser_renderer.SetPositions(vertices);
                    laser_renderer.positionCount = 2;
                    laser_renderer.material      = mat;

                    last_x = x;
                    last_y = y;

                    // todo: should also be able to target stuff other than ships, like satellites, stations, etc.

                    target_info[] targets = new target_info[penetration];

                    foreach(var s in state.ships) {
                        SystemShip ship = s.obj;

                        if(ship == self) continue;

                        // (1) y = mx + b   =>   mx - y + b = 0
                        //                  =>   b = y - mx

                        //          |mx - y + b|
                        // (2) d = --------------
                        //           √ (m² + 1)

                        float m = dy / dx;
                        float b = y - m * x;

                        float distance_from_beam = Math.Abs(m * ship.self.posx - ship.self.posy + b) / (float)Math.Sqrt(m * m + 1);
                         
                        // Probably should not be affected by camera scale, but it makes it easier at least for testing
                        if(distance_from_beam < 2.5f / camera.scale) {

                            float distance = Tools.get_distance(self.self.posx, self.self.posy, ship.self.posx, ship.self.posy);

                            for(int i = 0; i < penetration; i++)
                                if(targets[i] == null) {

                                    targets[i]          = new target_info();
                                    targets[i].ship     = ship;
                                    targets[i].distance = distance;

                                } else if(targets[i].distance > distance) {

                                    for(int j = penetration - 1; j > i; j--) targets[j] = targets[j - 1];

                                    targets[i].ship     = ship;
                                    targets[i].distance = distance;

                                }

                        }

                        foreach(target_info target in targets)

                            if(target != null) {

                                float shield_damage          = damage * shield_damage_multiplier * 1.0f - shield_penetration;
                                float hull_damage            = damage *   hull_damage_multiplier *        shield_penetration;

                                target.ship.shield          -= (int)shield_damage;

                                if(target.ship.shield < 0) {
                                    hull_damage             -= (float)target.ship.shield / shield_damage_multiplier * hull_damage_multiplier;
                                    target.ship.shield       = 0;
                                }

                                target.ship.health          -= (int)hull_damage;

                                if(target.ship.health <= 0) target.ship.destroy();

                            }
                    }
                }
            }
        }
    }
}
