using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Agent
{
    [Agent]
    public class MovementStateComponent : IComponent
    {
        public bool Jumping;
        public bool DoubleJumping;
        public bool Dashing;
        public bool Flying;

        public float DashCooldown;
    }
}