using Entitas;

namespace Action.CoolDown
{
    /// <summary>
    /// This should exist only while action is in cooldown.
    /// </summary>
    [ActionCoolDown]
    public struct TimeComponent : IComponent
    {
        /// <summary>
        /// CoolDown End Time.
        /// </summary>
        public float EndTime;
    }
}
