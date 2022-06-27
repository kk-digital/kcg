using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action
{
    [Action]
    public class Component : IComponent
    {
        [PrimaryEntityIndex]
        public int                  ID;
        [EntityIndex]
        public Enums.ActionType     TypeID;
    }
}
