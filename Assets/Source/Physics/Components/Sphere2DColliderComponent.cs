using Entitas;
using KMath;

namespace Physics
{
    [Agent, Item, Vehicle, Projectile]
    public struct Sphere2DColliderComponent : IComponent
    {
        public float Radius;
        public Vec2f Size;
    }
}