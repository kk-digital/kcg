using Entitas;
using UnityEngine;
using System;

/*
    How To use it:
        Item.CreationApi.Instance.CreateItem(Item Type, Item Type Name);
        Item.CreationApi.Instance.SetTexture(SpriteSheetID);
        Item.CreationApi.Instance.SetInventoryTexture(SpriteSheetID);
        Item.CreationApi.Instance.MakeStackable(Max number of items in a stack.);
        Item.CreationApi.Instance.EndItem();
*/

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
            ItemType.AddItemAttributes(itemType, name);
        }

        public void SetName(string name)
        {
            if (ItemType == null)
                return;
            var Attributes = ItemType.itemAttributes;
            ItemType.ReplaceItemAttributes(Attributes.ItemType, name);
        }

        public void SetSize(Vector2 size)
        {
            ItemType.AddItemAttributeSize(size);
        }

        public void SetTexture(int spriteSheetID)
        {
            if (ItemType == null)
                return;

            int spriteAtlasID = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 0, Enums.AtlasType.Particle);
            ItemType.AddItemAttributeSprite(spriteAtlasID);
        }


        public void SetInventoryTexture(int spriteSheetID)
        {
            if (ItemType == null)
                return;

            int spriteAtlasID = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 0, Enums.AtlasType.Particle);
            ItemType.AddItemAttributeInventorySprite(spriteAtlasID);

        }

        public void SetAction(int actionID)
        {
            ItemType.AddItemAttributeAction(actionID);
        }

        public void SetStackable(int maxStackCount)
        {
            if (ItemType == null)
                return;

            ItemType.AddItemAttributeStackable(maxStackCount);
        }

        public void SetPlaceable()
        {
            if (ItemType == null)
                return;

            ItemType.isItemAttributePlaceable = true;
        }

        public void SetEquipament()
        {
            if (ItemType == null)
                return;

            ItemType.isItemAttributePlaceable = true;
        }

        public void EndItem()
        {
            // Todo: Check if ItemType is valid in debug mode.
            ItemType = null;
        }
    }
}
