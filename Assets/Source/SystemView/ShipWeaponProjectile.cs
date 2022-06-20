using System;
using UnityEngine; // For color
using Scripts.SystemView;

namespace Source {
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

            public void DoDamage(SystemShip Target) {
                Target.shield -= (int)(Damage * (1.0 - ShieldPenetration));
                Target.health -= (int)(Damage * ShieldPenetration);

                if(Target.shield < 0) {
                    Target.health += Target.shield;
                    Target.shield = 0;
                }

                if(Target.health <= 0) {
                    Target.destroy();
                }

                Weapon.projectiles_fired.Remove(this);
            }
        }
    }
}
