using Entitas;
using KMath;

namespace Physics
{
    [Agent, ItemParticle, Vehicle, Projectile]
    public class Box2DColliderComponent : IComponent
    {
        public Vec2f Size;
        public Vec2f Offset;
    }
} 
