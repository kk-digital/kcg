
using KMath;
using UnityEngine;

namespace Projectile
{

    public struct ProjectileProperties
    {
        public int PropertiesId;
        public string Name;
        
        public int SpriteId;
        public Vec2f Size;
        public bool HasAnimation;
        public Animation.AnimationType AnimationType;

        public float Speed;
        public Vec2f Acceleration;
        public float DeltaRotation;

        public bool canRamp;
        public float StartVelocity;
        public float MaxVelocity;
        public float rampTime;
    }
}

