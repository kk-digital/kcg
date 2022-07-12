using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action
{
    [Action]
    public class InterruptComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;
    }
}
