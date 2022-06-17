using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Projectile
{
    public class ColliderComponent : IComponent
    {
        public bool isFirstSolid;
        public bool isFired;
    }
}