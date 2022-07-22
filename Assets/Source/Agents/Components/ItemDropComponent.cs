using Entitas;
using Item;
using Enums;
using Inventory;
using System.Collections.Generic;

namespace Agent
{
    [Agent]
    public class ItemDropComponent : IComponent
    {
        public List<ItemDrop> itemDrop;
    }

    public class ItemDrop
    {
        public int itemCount;
        public ItemType itemType;
    }
}