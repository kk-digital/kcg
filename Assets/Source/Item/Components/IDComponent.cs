using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using Enums;

namespace Item
{
    [ItemInventory, ItemParticle]
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int              ID;
    }
}
