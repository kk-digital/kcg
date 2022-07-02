using Entitas;
using KMath;

namespace Physics
{
    [Agent, Item, Vehicle, Projectile]
    public struct Box2DColliderComponent : IComponent
    {
        public Vec2f Size;
        public Vec2f Offset;
    }
} 
