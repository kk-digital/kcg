using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Agent
{
    public class AgentIDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;
    }
}