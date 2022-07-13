using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Item.FireWeapon
{
    [ItemParticle, ItemInventory]
    public class SpreadComponent : IComponent
    {
        [Range(-1f, 1f)]
        public float SpreadAngle;
    }
}

