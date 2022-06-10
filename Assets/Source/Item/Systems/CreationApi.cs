using Entitas;
using UnityEngine;

namespace Item
{
    public class CreationApi
    {
        private static CreationApi instance;
        public static CreationApi Instance => instance ??= new CreationApi();

        public Contexts EntitasContext = Contexts.sharedInstance;

        public GameEntity ItemType = null;

        public void CreateItem(Enums.ItemType itemType, string name)
        {
            ItemType = EntitasContext.game.CreateEntity();
            ItemType.AddItemAttributesBasic(itemType, name);
        }

        public void SetName(string name)
        {
            if (ItemType == null)
                return;
        }

        public void SetTexture(int spriteSheetId, int row, int column)
        {
            if (ItemType == null)
                return;
        }

        public void SetInventoryTexture(int spriteSheetId, int row, int column)
        {
            if (ItemType == null)
                return;

        }

        public void MakeStackable(int maxStackCount)
        {
            if (ItemType == null)
                return;

            ItemType.AddItemAttributeStackable(maxStackCount);
        }

        public void MakePlaceable()
        {
            if (ItemType == null)
                return;

            ItemType.isItemAttributePlaceable = true;
        }

        public void MakeEquipament()
        {
            if (ItemType == null)
                return;

            ItemType.isItemAttributePlaceable = true;
        }

        public void EndItem()
        {
            // Todo: Check if ItemType is valid.
            ItemType = null;
        }
    }
}
