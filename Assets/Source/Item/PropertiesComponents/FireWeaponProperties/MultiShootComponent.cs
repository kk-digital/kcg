using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.Property.FireWeapon
{
    /// <summary>
    /// SpreadAngle
    /// </summary>
    [ItemProperties]
    // Todo: Rename this.
    public struct MultiShootComponent : IComponent
    {
        public float SpreadAngle;
        public int NumOfBullets;
    }
}
