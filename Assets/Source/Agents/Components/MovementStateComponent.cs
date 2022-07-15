using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Agent
{
    [Agent]
    public class MovementStateComponent : IComponent
    {
        public int JumpCounter;
        public MovementState MovementState;
        public bool Running;

        public float DashCooldown;
    }
}