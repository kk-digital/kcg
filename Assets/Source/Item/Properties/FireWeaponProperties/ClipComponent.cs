using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.Property.FireWeapon
{
    [ItemProperties]
    public class ClipComponent : IComponent
    {
        public int ClipSize;
        public float ReloadTime;
    }
}
