using System;
using System.Collections.Generic;
using UnityEngine; // For color
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class ShipWeaponProjectile {
            // todo: is this even needed?        v
            public OrbitingObjectDescriptor Descriptor;

            public SystemShip Self;
            public ShipWeapon Weapon;

            public SpaceObject Body;

            public float TimeElapsed;
            public float LifeSpan;

            public Color ProjectileColor;

            public float ShieldPenetration;

            public float ShieldDamageMultiplier;
            public float HullDamageMultiplier;

            public float acc;
            public bool  seeking;
            public bool  rocket;
            public int   penetration;
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

                    foreach(SystemShip ship in state.ships)
                        if(ship != Self) {
                            float distance = Tools.get_distance(Body.posx, Body.posy, ship.self.posx, ship.self.posy);

                            if(target == null || target_distance > distance) {
                                target_distance = distance;
                                target = ship;
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
                    float vel   = Tools.magnitude(Body.velx, Body.vely);

                    Body.velx  += acc * dt * Body.velx / vel;
                    Body.vely  += acc * dt * Body.vely / vel;
                }

                if(Descriptor == null) // Linear trajectory
                {
                    Body.posx += dt * Body.velx;
                    Body.posy += dt * Body.vely;
                }
                /*else // Orbital trajectory todo
                {
                    Descriptor.RotationalPosition += dt / Descriptor.GetDistanceFromCenter() / Descriptor.GetDistanceFromCenter();

                    float[] Pos = Descriptor.GetPosition();

                    PosX = Pos[0];
                    PosY = Pos[1];
                }*/

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
