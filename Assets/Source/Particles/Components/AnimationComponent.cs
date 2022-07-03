using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Particle
{
    [Particle]
    public class AnimationComponent : IComponent
    {
        public float AnimationSpeed;

        public Animation.Animation State;
    }
}