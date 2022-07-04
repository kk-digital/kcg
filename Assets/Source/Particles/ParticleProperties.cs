
using KMath;
using UnityEngine;

namespace Particle
{

    public struct ParticleProperties
    {
        public int PropertiesId;
        public string Name;
        
        public float DecayRate;
        public Vector2 Acceleration;
        public float DeltaRotation;
        public float DeltaScale;

        // we can use a mix of sprites for the particles
        public int SpriteId;

        public bool HasAnimation;
        public Animation.AnimationType AnimationType;

        // the starting properties of the particles
        public Vec2f Size;
        public Vector2 StartingVelocity;
        public float StartingRotation;
        public float StartingScale;
        public Color StartingColor;
        public float AnimationSpeed;
    }
}

