using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action.Property
{
    [ActionProperties]
    public struct NameComponent : IComponent
    {
        [PrimaryEntityIndex]
        public string TypeName;
    }
}
