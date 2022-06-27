using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.FireWeapon
{
    public struct ClipComponent : IComponent
    {
        public int numOfBullets;
    }
}

