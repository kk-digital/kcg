using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action.CoolDown
{
    /// <summary>
    /// This component is gonna be part of an entity which exist only while entity is in coolDown.
    /// </summary>
    [ActionCoolDown]
    public struct Component : IComponent
    {
        [EntityIndex]
        public Enums.ActionType TypeID;
        [EntityIndex]
        public int AgentID;
    }
}
