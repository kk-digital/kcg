using System;
using UnityEngine; // For color

namespace SystemView
{
    public class ShipWeaponProjectile
    {
        // todo: is this even needed?        v
        public OrbitingObjectDescriptor Descriptor;

        public SystemShip Self;
        public ShipWeapon Weapon;

        public SystemViewBody Body;

        public float TimeElapsed;
        public float LifeSpan;

        public Color ProjectileColor;

        public float ShieldPenetration;

        public int Damage;

        public ShipWeaponProjectile()
        {
            Body = new();
            Body.Mass = 1.0f;
        }

        public bool UpdatePosition(float dt)
        {
            if((TimeElapsed += dt) > LifeSpan)
            {
                Weapon.ProjectilesFired.Remove(this);
                return false;
            }

            if (Descriptor == null) // Linear trajectory
            {
                Body.PosX += dt * Body.VelX;
                Body.PosY += dt * Body.VelY;
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

        public bool InRangeOf(SystemShip Target, float AcceptableRange)
        {
            float dx = Body.PosX - Target.Self.PosX;
            float dy = Body.PosY - Target.Self.PosY;

            return Target != null && Math.Sqrt(dx * dx + dy * dy) < AcceptableRange;
        }

        public void DoDamage(SystemShip Target)
        {
            Target.Shield -= (int)(Damage * (1.0 - ShieldPenetration));
            Target.Health -= (int)(Damage * ShieldPenetration);

            if (Target.Shield < 0)
            {
                Target.Health += Target.Shield;
                Target.Shield = 0;
            }

            if (Target.Health <= 0)
            {
                Target.Destroy();
            }

            Weapon.ProjectilesFired.Remove(this);
        }
    }
}
