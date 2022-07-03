using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Animation
{
    [Agent, Projectile]
    public class StateComponent : IComponent
    {
        public float AnimationSpeed;

        public Animation State;
    }
}