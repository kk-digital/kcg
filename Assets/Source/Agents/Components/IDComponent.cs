using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Agent
{
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;
    }
}