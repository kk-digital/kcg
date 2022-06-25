using Entitas;

namespace Projectile
{

    public struct ProjectileEntity
    {
        public int ProjectileId;
        public bool IsInitialized;
        public GameEntity Entity;
    }
}