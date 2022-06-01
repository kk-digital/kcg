using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Agent
{
    [Agent]
    public class PlayerComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;
    }
}

