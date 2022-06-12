using Entitas;
using UnityEngine;

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
            ItemType.AddItemAttributesBasic(itemType, name);
        }

        public void SetName(string name)
        {
            if (ItemType == null)
                return;
        }

        public void SetTexture(string spritePath)
        {
            if (ItemType == null)
                return;

            int spriteSheetID = GameState.SpriteLoader.GetSpriteSheetID(spritePath);
            int spriteAtlasID = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 0, Enums.AtlasType.Particle);
        }

        public void SetTexture(int spriteSheetID)
        {
            if (ItemType == null)
                return;

            int spriteAtlasID = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 0, Enums.AtlasType.Particle);
        }

        public void SetInventoryTexture(string spritePath)
        {
            if (ItemType == null)
                return;

            int spriteSheetID = GameState.SpriteLoader.GetSpriteSheetID(spritePath);
            int spriteAtlasID = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 0, Enums.AtlasType.Particle);

            ItemType.AddItemAttributeInventorySprite(spriteAtlasID);
        }

        public void SetInventoryTexture(int spriteSheetID)
        {
            if (ItemType == null)
                return;

            int spriteAtlasID = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 0, Enums.AtlasType.Particle);
            ItemType.AddItemAttributeInventorySprite(spriteAtlasID);

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
            // Todo: Check if ItemType is valid in debug mode.
            ItemType = null;
        }
    }
}
