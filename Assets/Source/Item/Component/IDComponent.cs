using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using Enums;

namespace Item
{
    public struct IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int              ID;
        [EntityIndex]
        public ItemType   ItemType;
    }
}
