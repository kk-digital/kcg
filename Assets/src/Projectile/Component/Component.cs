using Entitas;
using Enums;

namespace Projectile
{
    public class Component : IComponent
    {
        public ProjectileType projectileType;
        public ProjectileDrawType projectileDrawType;
    }
}
