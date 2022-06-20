using Entitas;
using KMath;

namespace Physics
{
    public struct Sphere2DColliderComponent : IComponent
    {
        public float Radius;
        public Vec2f Size;
    }
}