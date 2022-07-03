using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Projectile
{
    [Projectile]
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;
    }
}