using Entitas;
using Item;
using Enums;
using Inventory;
using System.Collections.Generic;

namespace Agend
{
    [Agent]
    public class ItemDropComponent : IComponent
    {
        public List<ItemDrop> itemDrop;
    }

    public class ItemDrop
    {
        private int itemCount;
        ItemType itemType;
    }
}