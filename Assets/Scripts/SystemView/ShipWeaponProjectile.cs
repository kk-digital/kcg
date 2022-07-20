using System;
using System.Collections.Generic;
using UnityEngine; // For color
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class ShipWeaponProjectile {
            public SystemShip Self;
            public ShipWeapon Weapon;

            public SpaceObject Body;

            public float TimeElapsed;
            public float LifeSpan;

            public Color ProjectileColor;

            public float ShieldPenetration;

            public float ShieldDamageMultiplier;
            public float HullDamageMultiplier;

            public float acc_angle;
            public float acc;
            public float detection_angle;
            public bool  seeking;
            public bool  rocket;
            public int   penetration;
            public float max_velocity;
            public SystemState state;

            public List<SystemShip> hits = new();
            public int Damage;

            public ShipWeaponProjectile() {
                Body = new();
                Body.mass = 1.0f;
            }

            public bool UpdatePosition(float dt) {
                if((TimeElapsed += dt) > LifeSpan) {
                    Weapon.projectiles_fired.Remove(this);
                    return false;
                }

                bool accelerated = false;

                if(seeking) {
                    SystemShip target          = null;
                    float      target_distance = 0.0f;

                    foreach(var s in state.ships) {
                        SystemShip ship = s.obj;

                        if(ship != Self) {
                            if(detection_angle != 0.0f) {
                                float velocity_angle = Tools.get_angle(                 Body.velx,                  Body.vely);
                                float    enemy_angle = Tools.get_angle(ship.self.posx - Body.posx, ship.self.posy - Body.posy);
                                float          angle = Tools.normalize_angle(enemy_angle - velocity_angle);

                                if(angle < -detection_angle * 0.5f || angle > detection_angle * 0.5f) continue;
                            }

                            float distance = Tools.get_distance(Body.posx, Body.posy, ship.self.posx, ship.self.posy);

                            if(target == null || target_distance > distance) {
                                target_distance = distance;
                                target = ship;
                            }
                        }
                    }

                    if(target != null) {

                        float dx        = target.self.posx - Body.posx;
                        float dy        = target.self.posy - Body.posy;

                        float angle     = Tools.get_angle(dx, dy);
                        float magnitude = Tools.magnitude(Body.velx, Body.vely) + acc * dt;

                        Body.velx       = (float)Math.Cos(angle) * magnitude;
                        Body.vely       = (float)Math.Sin(angle) * magnitude;

                        accelerated     = true;

                    }
                }

                if(rocket && !accelerated) {
                    Body.velx  += acc * dt * (float)Math.Cos(acc_angle);
                    Body.vely  += acc * dt * (float)Math.Sin(acc_angle);

                    accelerated = true;
                }

                if(accelerated && max_velocity != 0.0f) {
                    float vel = Tools.magnitude(Body.velx, Body.vely);
                    if(vel > max_velocity) {
                        Body.velx *= max_velocity / vel;
                        Body.vely *= max_velocity / vel;
                    }
                }

                Body.posx += dt * Body.velx;
                Body.posy += dt * Body.vely;
                
                return true;
            }

            public bool InRangeOf(SystemShip Target, float AcceptableRange) {
                float dx = Body.posx - Target.self.posx;
                float dy = Body.posy - Target.self.posy;

                return Target != null && Math.Sqrt(dx * dx + dy * dy) < AcceptableRange;
            }

            public bool DoDamage(SystemShip Target) {
                if(!hits.Contains(Target)) {
                    Target.shield -= (int)(Damage * (1.0 - ShieldPenetration));
                    Target.health -= (int)(Damage * ShieldPenetration);

                    if(Target.shield < 0) {
                        Target.health += Target.shield;
                        Target.shield = 0;
                    }

                    if(Target.health <= 0) {
                        Target.destroy();
                    }

                    hits.Add(Target);

                    penetration--;
                    if(penetration <= 0) {
                        Weapon.projectiles_fired.Remove(this);
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
