using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.FireWeapon
{
    [ItemParticle, ItemInventory]
    public class ClipComponent : IComponent
    {
        public int NumOfBullets;
    }
}

