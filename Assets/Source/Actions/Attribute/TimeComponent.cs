using Entitas;

namespace Action.Attribute
{
    public struct TimeComponent : IComponent
    {
        /// <summary>
        /// How long it takes to execute the action in miliseconds
        /// </summary>
        public float Duration;
    }
}
