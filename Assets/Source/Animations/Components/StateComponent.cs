using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Animation
{
    public class StateComponent : IComponent
    {
        public float AnimationSpeed;

        public Animation State;
    }
}