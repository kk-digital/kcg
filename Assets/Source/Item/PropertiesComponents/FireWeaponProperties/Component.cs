using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.Property.FireWeapon
{
    /// <summary>
    /// CoolDown - How Long it takes to shoot again in seconds.
    /// </summary>
    [ItemProperties]
    public struct Component : IComponent
    {
        public float BulletSpeed;
        public float CoolDown;
        public float Range;
        public int   BulletSpriteID;
    }
}
