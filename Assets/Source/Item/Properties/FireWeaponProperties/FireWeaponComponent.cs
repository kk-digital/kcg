using Entitas;
using KMath;

namespace Item.Property
{
    /// <summary>
    /// Fire weapon basic attributes:
    /// 
    /// BulletSpeed     - Start Speed Position.
    /// CoolDown        - How Long it takes to shoot again in seconds.
    /// Range           - Max projectile range. 
    /// BasicDamage     - Demage on hit without any modifiers.
    /// SpriteSize      - Size of the bullet sprite.
    /// BulletSprideID  - SpriteID.
    /// </summary>
    [ItemProperties]
    public class FireWeaponComponent : IComponent
    {
        public float BulletSpeed;
        public float CoolDown;
        public float Range;
        public float BasicDemage;
        public Vec2f SpriteSize;
        public int   BulletSpriteID;
    }
}
