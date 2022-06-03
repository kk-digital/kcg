using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Vehicle
{
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;
    }
}