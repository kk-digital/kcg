using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.FireWeapon
{
    [Item]
    public class ChargeComponent : IComponent
    {
        public float ChargeRate;
        public float ChargeMin;
        public float ChargeMax;
    }
}

