using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using Enums;

namespace Item
{
    public sealed class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int              ID;
        [EntityIndex]
        public ItemType   ItemType;
    }
}
