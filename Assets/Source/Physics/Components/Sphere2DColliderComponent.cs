using Entitas;
using KMath;

namespace Physics
{
    [Agent, Item, Vehicle, Projectile]
    public class Sphere2DColliderComponent : IComponent
    {
        public float Radius;
        public Vec2f Size;
    }
}