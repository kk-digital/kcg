using Entitas;
using Enums;

namespace Projectile
{
    public class TypeComponent : IComponent
    {
        public ProjectileType Type;
        public ProjectileDrawType DrawType;
    }
}
