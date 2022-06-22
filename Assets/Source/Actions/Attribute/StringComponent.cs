using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action.Attribute
{
    public struct NameComponent : IComponent
    {
        [PrimaryEntityIndex]
        public string TypeName;
    }
}
