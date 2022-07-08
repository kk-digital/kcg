using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.FireWeapon
{
    [Item]
    public class ClipComponent : IComponent
    {
        public int NumOfBullets;
    }
}

