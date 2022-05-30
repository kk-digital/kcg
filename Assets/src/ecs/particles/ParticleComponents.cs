using System.Numerics;
using System.Drawing;

namespace Components
{
    public enum ParticleState
    {
        Empty = 0,
        Loading = 1,
        Running = 2,
    }

    public struct ParticleStateComponent
    {
        public ParticleState State;

        public ParticleStateComponent(ParticleState state)
        {
            State = state;
        }
    }

    public struct Particle2dHealthComponent
    {
        public float Health;
        public float DecayRate;

        public Particle2dHealthComponent(float health, float decayRate)
        {
            Health = health;
            DecayRate = decayRate;
        }
    }

    public struct Particle2dPositionComponent
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Direction;

        public Particle2dPositionComponent(Vector2 position, Vector2 velocity, Vector2 direction)
        {
            Position = position;
            Velocity = velocity;
            Direction = direction;
        }
    }

    public struct Particle2dRotationComponent
    {
        public float Rotation;
        public float DeltaRotation;

        public Particle2dRotationComponent(float rotation, float deltaRotation)
        {
            Rotation = rotation;
            DeltaRotation = deltaRotation;
        }
    }

    public struct Particle2dScaleComponent
    {
        public float Scale;
        public float DeltaScale;

        public Particle2dScaleComponent(float scale, float deltaScale)
        {
            Scale = scale;
            DeltaScale = deltaScale;
        }
    }

    public struct Particle2dSpriteComponent
    {
        public int SpriteId;
        public Color Color;

        public Particle2dSpriteComponent(int spriteId, Color color)
        {
            SpriteId = spriteId;
            Color = color;
        }
    }

    public struct Particle2dAnimationComponent
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