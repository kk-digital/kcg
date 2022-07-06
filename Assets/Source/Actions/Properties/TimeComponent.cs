using Entitas;

namespace Action.Property
{
    [ActionProperties]
    public class TimeComponent : IComponent
    {
        /// <summary>
        /// How long it takes to execute the action in miliseconds
        /// </summary>
        public float Duration;
    }
}
