using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class DrawSystem
    {

        Contexts EntitasContext;

        public DrawSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public void Draw(Material material, Transform transform)
        {
            var openInventories = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.InventoryDrawable, GameMatcher.InventoryID));
            // If empty Draw ToolBar.

            foreach (GameEntity inventoryEntity in openInventories)
            {
                DrawInventory(material, transform, inventoryEntity);
            }
        }

        private void DrawInventory(Material material, Transform transform, GameEntity inventoryEntity)
        {
            const float tileSize = 1.0f;
            const float slotSize = 0.9f;

            // Get Inventory Info.
            int width = inventoryEntity.inventorySize.Width;
            int height = inventoryEntity.inventorySize.Height;

            float h = height * tileSize;
            float w = width * tileSize;

            // Get Initial Positon.
            float x = -w / 2;
            float y = -h / 2;

            // If is tool bar draw at the botton of the screen.
            if (inventoryEntity.isInventoryToolBar)
                y = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).y + tileSize / 2f;

            DrawBackGround(x, y, w, h, material, transform);

            DrawCells(x, y, width, height, tileSize, slotSize, material, transform, inventoryEntity);

            var itemInInventory = EntitasContext.game.GetEntitiesWithItemAttachedInventory(inventoryEntity.inventoryID.ID);
            DrawIcons(x, y, width, height, tileSize, slotSize, material, transform, itemInInventory);
        }

        void DrawBackGround(float x, float y, float w, float h, Material material, Transform transform)
        {
            Color backGround = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            Utility.Render.DrawQuadColor(x, y, w, h, backGround, Object.Instantiate(material), transform, 0);
        }

        void DrawCells(float x, float y, int width, int height, float tileSize, float slotSize, Material material, Transform transform, GameEntity inventoryEntity)
        {
            Color borderColor = Color.grey;
            Color selectedBorderColor = Color.yellow;

            int selectedSlotPos = inventoryEntity.inventorySlots.Selected;
            int selectedSlotPosX = selectedSlotPos % width;
            int selectedSlotPosY = (height - 1) - (selectedSlotPos - selectedSlotPosX) / width;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // Assign Border Color.
                    Color quadColor = borderColor;
                    if (selectedSlotPosX == i && selectedSlotPosY == j)
                        quadColor = selectedBorderColor;

                    // Get Quad Position
                    float slotX = x + i * tileSize;
                    float slotY = y + j * tileSize;

                    Utility.Render.DrawQuadColor(slotX + (1.0f - slotSize) / 2.0f, slotY + (1.0f - slotSize) / 2.0f, slotSize, slotSize, quadColor, Object.Instantiate(material), transform, 10);
                }
            }
        }

        void DrawIcons(float x, float y, int width, int height, float tileSize, float slotSize, Material material, Transform transform, HashSet<GameEntity> itemInInventory)
        {
            foreach (GameEntity itemEntity in itemInInventory)
            {
                int slotNumber = itemEntity.itemAttachedInventory.SlotNumber;
                int i = slotNumber % width;
                int j = (height - 1) - (slotNumber - i) / width;

                // Calculate Slot Border positon.
                float slotX = x + i;
                float slotY = y + j;

                // Draw sprites.
                GameEntity itemAttributeEntity = EntitasContext.game.GetEntityWithItemAttributesBasic(itemEntity.itemID.ItemType);
                int SpriteID = itemAttributeEntity.itemAttributeInventorySprite.ID;

                Sprites.Model sprite = GameState.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle);

                float spriteSize = slotSize * 0.9f;

                Utility.Render.DrawSprite(slotX + (1.0f - spriteSize) / 2.0f, slotY + (1.0f - spriteSize) / 2.0f, spriteSize, spriteSize, sprite, Object.Instantiate(material), transform, 11);
            }
        }
    }
}
