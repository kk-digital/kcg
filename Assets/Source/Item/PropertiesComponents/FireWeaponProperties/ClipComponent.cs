using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.Property.FireWeapon
{
    [ItemProperties]
    public struct ClipComponent : IComponent
    {
        public float ClipSize;
        public float ReloadTime;
    }
}
