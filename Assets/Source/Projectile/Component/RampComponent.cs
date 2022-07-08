using Entitas;
using KMath;
using UnityEngine;

namespace Projectile
{
    [Projectile]
    public class RampComponent : IComponent
    {
        public bool canRamp;

        public float startVelocity;

        public float maxVelocity;

        public float rampTime;
    }
}
