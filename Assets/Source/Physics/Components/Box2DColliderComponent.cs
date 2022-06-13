using Entitas;
using KMath;

namespace Physics
{
    public struct Box2DColliderComponent : IComponent
    {
        public Vec2f Size;
        public Vec2f Offset
    }
} 
