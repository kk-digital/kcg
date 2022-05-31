using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Components
{
    public struct Particle2dHealthComponent : IComponent
    {
        public float Health;
        public float DecayRate;

        public Particle2dHealthComponent(float health, float decayRate)
        {
            Health = health;
            DecayRate = decayRate;
        }
    }

    public struct Particle2dPositionComponent : IComponent
    {
        public Vector2 Position;
        public Vector2 Acceleration;
        public Vector2 Velocity;

        public Particle2dPositionComponent(Vector2 position, Vector2 acceleration,
                    Vector2 velocity)
        {
            Position = position;
            Acceleration = acceleration;
            Velocity = velocity;
        }
    }

    public struct Particle2dRotationComponent : IComponent
    {
        public float Rotation;
        public float DeltaRotation;

        public Particle2dRotationComponent(float rotation, float deltaRotation)
        {
            Rotation = rotation;
            DeltaRotation = deltaRotation;
        }
    }

    public struct Particle2dScaleComponent : IComponent
    {
        public float Scale;
        public float DeltaScale;

        public Particle2dScaleComponent(float scale, float deltaScale)
        {
            Scale = scale;
            DeltaScale = deltaScale;
        }
    }

    public struct Particle2dSpriteComponent : IComponent
    {
        public int SpriteId;
        public Color Color;

        public Particle2dSpriteComponent(int spriteId, Color color)
        {
            SpriteId = spriteId;
            Color = color;
        }
    }

    public struct Particle2dAnimationComponent : IComponent
    {
        public float CurrentFrame;
        public float AnimationSpeed;

        public Particle2dAnimationComponent(float currentFrame, float animationSpeed)
        {
            CurrentFrame = currentFrame;
            AnimationSpeed = animationSpeed;
        }
    }

}