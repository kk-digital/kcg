using Entitas;

namespace Action.Property
{
    /// <summary>
    /// Should be avoided. Don't use this if data is available in planet/agent or inside tools.
    /// </summary>
    [ActionProperties]
    public class DataComponent : IComponent
    {
        public object Data;
    }
}
