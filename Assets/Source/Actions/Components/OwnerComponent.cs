using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action
{
    /// <summary>
    /// Only add this when action is scheduled to be executed by the agent.
    /// </summary>
    [Action]
    public class OwnerComponent : IComponent
    {
        [EntityIndex]
        public int AgentID;
    }
}
