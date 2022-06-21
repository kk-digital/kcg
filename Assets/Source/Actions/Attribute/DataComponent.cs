using Entitas;

namespace Action.Attribute
{
    /// <summary>
    /// Should be avoided. Don't use this if data is available in planet/agent or inside tools.
    /// </summary>
    public class DataComponent : IComponent
    {
        public object Data;
    }
}
