using Entitas;

namespace Action
{
    public struct TimeComponent : IComponent
    {
        public float StartTime;
        /// <summary>
        /// How long it takes to execute the action in miliseconds
        /// </summary>
        public float Duration;
    }
}
