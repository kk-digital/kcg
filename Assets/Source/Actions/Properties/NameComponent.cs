using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action.Property
{
    [ActionProperties]
    public class NameComponent : IComponent
    {
        [PrimaryEntityIndex]
        public string TypeName;
    }
}
