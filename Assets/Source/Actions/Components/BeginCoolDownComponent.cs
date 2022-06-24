using Entitas;

namespace Action
{
    /// <summary>
    /// This should exist only while action is in cooldown.
    /// </summary>
    [Action]
    public struct BeginCoolDownComponent : IComponent
    {
        /// <summary>
        /// CoolDown Start Time.
        /// </summary>
        public float StartTime;
    }
}
