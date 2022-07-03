using Entitas;

namespace Projectile
{
    [Projectile]
    public class TypeComponent : IComponent
    {
        public Enums.ProjectileType Type;
        public Enums.ProjectileDrawType DrawType;
    }
}
