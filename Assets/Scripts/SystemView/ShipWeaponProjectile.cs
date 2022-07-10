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
            public bool  overshoot;
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

                    foreach(SystemShip ship in state.ships)
                        if(ship != Self) {
                            float distance = Tools.get_distance(Body.posx, Body.posy, ship.self.posx, ship.self.posy);

                            if(target == null || target_distance > distance) {
                                target_distance = distance;
                                target = ship;
                            }
                        }

                    if(target != null) {

                        if(!overshoot) {

                            float dx        = target.self.posx - Body.posx;
                            float dy        = target.self.posy - Body.posy;

                            float angle     = Tools.get_angle(dx, dy);
                            float magnitude = Tools.magnitude(Body.velx, Body.vely) + acc * dt;

                            Body.velx       = (float)Math.Cos(angle) * magnitude;
                            Body.vely       = (float)Math.Sin(angle) * magnitude;

                        } else {

                            /*
                             * In 1D-Space:
                             * 
                             * For fixed velocity
                             * 
                             * (1) d = v * t
                             *
                             * With acceleration
                             *
                             * (2) d = v₀ * t + ½at²
                             *          ⤷ start velocity
                             * 
                             * In 2D-Space:
                             * 
                             * (3) dx = vx₀ * t + ½axt²
                             * (4) dy = vy₀ * t + ½ayt²
                             *
                             */

                            /*
                             * (5) dx = x₁ - x₀
                             *           ⤹    ⤷ x position of projectile
                             *            x position of target
                             * 
                             * (6) dy = y₁ - y₀
                             *           ⤹    ⤷ y position of projectile
                             *            y position of target
                             */

                            /*
                             * Insert (5) and (6) in (3) and (4)
                             * 
                             * (3) x₁ - x₀ = vx₀ * t + ½axt²
                             * (4) y₁ - y₀ = vy₀ * t + ½ayt²
                             * 
                             * Solve (3) and (4) for t
                             * 
                             *           ______________________
                             *          √ 2ax * (x₁ - x₀) + vx₀² + vx₀
                             * (3) t = ——————————————————————————————
                             *                       ax
                             *                       
                             *           ______________________
                             *          √ 2ay * (y₁ - y₀) + vy₀² + vy₀
                             * (4) t = ——————————————————————————————
                             *                       ay
                             * 
                             * Insert (4) into (3)
                             * 
                             *       ______________________           _____________________
                             *      √ 2ax * (x₁ - x₀) + vx₀² + vx₀     √ 2ay * (y₁ - y₀) + vy₀² + vy₀
                             * (7) —————————————————————————————— = ——————————————————————————————
                             *                   ax                              ay
                             *
                             */

                            /*
                             *          __________              _________
                             * (8) a = √ ax² + ay²   =>   ay = √ a² - ax²
                             *
                             */


                            /*
                             * Insert (8) into (7)
                             * 
                             *                                          ____________________________
                             *       ______________________            /  _________                 |
                             *      √ 2ax * (x₁ - x₀) + vx₀² + vx₀     \/ 2√ a² - ax² * (y₁ - y₀) + vy₀² + vy₀
                             * (7) —————————————————————————————— = ———————————————————————————————————————
                             *                   ax                                _________
                             *                                                    √ a² - ax²
                             * 
                             * Simplify as much as realistically possible
                             * 
                             *                                          ____________________________
                             *       ______________________            /  _________                 |
                             *      √ 2ax * (x₁ - x₀) + vx₀² + vx₀     \/ 2√ a² - ax² * (y₁ - y₀) + vy₀² + vy₀
                             * (7) —————————————————————————————— - ——————————————————————————————————————— = 0
                             *                   ax                                _________
                             *                                                    √ a² - ax²
                             * 
                             *                                                            ____________________________
                             *      _________     ______________________                 /  _________                 |
                             *     √ a² - ax² * (√ 2ax * (x₁ - x₀) + vx₀² + vx₀) - ax * (\/ 2√ a² - ax² * (y₁ - y₀) + vy₀² + vy₀)
                             * (7) ——————————————————————————————————————————————————————————————————————————————————————————— = 0
                             *                                                 _________
                             *                                           ax * √ a² - ax²
                             *                                           
                             *                                                            ____________________________
                             *      _________     ______________________                 /  _________                 |
                             * (7) √ a² - ax² * (√ 2ax * (x₁ - x₀) + vx₀² + vx₀) - ax * (\/ 2√ a² - ax² * (y₁ - y₀) + vy₀² + vy₀) = 0
                             * 
                             * Build derivative of (7) for ax to approximate using Newton's method
                             * Using dx instead of (x₁ - x₀) and dy instead of (y₁ - y₀) for the sake of brevity
                             * 
                             *                                                              _________   
                             *      d(7)                     dy ax²                      dx√ a² - ax²   
                             * (9) ------ = ——————————————————————————————————————— + ——————————————————
                             *      d ax                     _____________________      _______________
                             *                  _________   /    __________       |    √ 2 dx ax + vx₀² 
                             *               2 √2a² - ax² \/ dy √ 2a² - ax² + vy₀²
                             * 
                             *                      _______________             _____________________
                             *               ax * (√ 2 dx ax + vx₀² + vx₀)      /    __________       |
                             *            - —————————————————————————————— - \/ dy √ 2a² - ax² + vy₀²  - vy₀
                             *                          _________
                             *                         √ a² - ax²
                             * 
                             * Approximate ax using Newton's method
                             * 
                             *             f(xₙ)
                             * xₙ₊₁ = xₙ - ———————
                             *            f'(xₙ)
                             * 
                             *                   (7)
                             * (10) axₙ₊₁ = axₙ - ————
                             *                   (9)
                             */

                            // TODO: This should predict the ETA, and use that ETA to calculate the position of the enemy on impact,
                            //       and use that predicted position for the actual target seeking. However, this math is already
                            //       a huge mess, and I will have to do it at a later date.

                            float dx = target.self.posx - Body.posx;
                            float dy = target.self.posy - Body.posy;

                            Func<float, float> formula7 = ax => {
                                float ay = (float)Math.Sqrt(acc * acc - ax * ax);

                                float result = (float)(ay * (Math.Sqrt(2.0 * ax * dx + Body.velx * Body.velx) + Body.velx)
                                             -         ax * (Math.Sqrt(2.0 * ay * dy + Body.vely * Body.vely) + Body.vely));

                                if(float.IsNaN(result)) {
                                    ay *= -1.0f;

                                    result = (float)(ay * (Math.Sqrt(2.0 * ax * dx + Body.velx * Body.velx) + Body.velx)
                                           -         ax * (Math.Sqrt(2.0 * ay * dy + Body.vely * Body.vely) + Body.vely));
                                }

                                return result;
                            };

                            Func<float, float> formula9 = ax => {
                                float ay  = (float)Math.Sqrt(      acc * acc - ax * ax);
                                float ay2 = (float)Math.Sqrt(2.0 * acc * acc - ax * ax);
                                float vx  = Body.velx;
                                float vy  = Body.vely;
                                float vx2 = Body.velx * Body.velx;
                                float vy2 = Body.vely * Body.vely;

                                if(dy < 0) { ay *= -1; ay2 *= -1; }

                                float result = (float)(dy * ax * ax / (2.0 * ay2 * Math.Sqrt(dy * ay2 + vy2))
                                             + dx * ay * ay / Math.Sqrt(2.0        * dx * ax  + vx2)
                                             - ax * (vx +     Math.Sqrt(2.0        * dx * ax  + vx2)) / ay
                                             - Math.Sqrt(dy * ay2 + vy2) - vy);

                                return result;
                            };

                                  float estimate = acc * dx / Tools.magnitude(dx, dy);
                                  float result   = 0.0f;
                            const float delta    = 1E-0f;

                            do {

                                estimate -= formula7(estimate) / formula9(estimate);
                                result    = formula7(estimate);

                            } while(result > delta || result < -delta || float.IsNaN(result));

                            Body.velx += dt * estimate;
                            Body.vely += dt * (float)Math.Sqrt(acc * acc - estimate * estimate);

                        }

                        accelerated     = true;

                    }
                }

                if(rocket && !accelerated) {
                    float vel   = Tools.magnitude(Body.velx, Body.vely);

                    Body.velx  += acc * dt * Body.velx / vel;
                    Body.vely  += acc * dt * Body.vely / vel;

                    accelerated = true;
                }

                if(accelerated && max_velocity != 0.0f) {
                    float vel = Tools.magnitude(Body.velx, Body.vely);
                    if(vel > max_velocity) {
                        Body.velx *= max_velocity / vel;
                        Body.vely *= max_velocity / vel;
                    }
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
