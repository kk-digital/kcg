using UnityEngine;
using System.Collections.Generic;
using KMath;
using Entitas;

namespace Inventory
{
    public class DrawSystem
    {

        public void Draw(Contexts contexts, Material material, Transform transform)
        {
            var openInventories = contexts.inventory.GetGroup(InventoryMatcher.AllOf(InventoryMatcher.InventoryDrawable, InventoryMatcher.InventoryID));
            // If empty Draw ToolBar.

            foreach (InventoryEntity inventoryEntity in openInventories)
            {
                DrawInventory(contexts, material, transform, inventoryEntity);
            }
        }

        private void DrawInventory(Contexts entitasContext, Material material, Transform transform, InventoryEntity inventoryEntity)
        {
            // Todo: Add scrool bar.
            // Todo: allow user to move inventory position?

            // Calculate Positions and Tile Sizes relative to sceen.

            Vec2f tileSize = new Vec2f(1f / 16f, 1f / 16f * Screen.width / Screen.height);
            Vec2f slotSize = tileSize * 0.9f;

            // Get Inventory Info.
            int width = inventoryEntity.inventorySize.Width;
            int height = inventoryEntity.inventorySize.Height;

            float h = height * tileSize.Y;
            float w = width * tileSize.X;

            // Get Initial Positon.
            float x = 0.5f;
            float y = 0.5f;

            x -= w / 2f;
            y -= h / 2f;

            // If is tool bar draw at the botton of the screen.
            if (inventoryEntity.isInventoryToolBar)
                y = tileSize.Y / 2f;

            DrawBackGround(x, y, w, h, material);

            DrawCells(x, y, width, height, tileSize, slotSize, material, inventoryEntity);

            var itemInInventory = entitasContext.item.GetEntitiesWithItemAttachedInventory(inventoryEntity.inventoryID.ID);
            DrawIcons(entitasContext, x, y, width, height, tileSize, slotSize, material, transform, itemInInventory);
        }

        void DrawBackGround(float x, float y, float w, float h, Material material)
        {
            Color backGround = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            Utility.Render.DrawQuadColorGui(x, y, w, h, backGround, material);
        }

        void DrawCells(float x, float y, int width, int height, Vec2f tileSize, Vec2f slotSize, Material material, InventoryEntity inventoryEntity)
        {
            Color borderColor = Color.grey;
            Color selectedBorderColor = Color.yellow;

            int selectedSlotPos = inventoryEntity.inventorySlots.Selected;
            int selectedSlotPosX = selectedSlotPos % width;
            int selectedSlotPosY = (height - 1) - (selectedSlotPos - selectedSlotPosX) / width;

            for (int i = 0; i < width; i++)
            {
                if (inventoryEntity.isInventoryToolBar)
                {
                    // Get Quad Position
                    float slotX = x + i * tileSize.X + slotSize.X * 0.5f - 0.125f;
                    float slotY = y - slotSize.Y * 0.5f;

                    //Utility.Render.DrawString(slotX + (tileSize.X - slotSize.X) / 2.0f, 
                    //           slotY + (tileSize.Y - slotSize.Y) / 2.0f, 0.25f, "" + (i + 1), 16, new Color(255, 255, 255, 255));
                }

                for (int j = 0; j < height; j++)
                {
                    // Assign Border Color.
                    Color quadColor = borderColor;
                    if (selectedSlotPosX == i && selectedSlotPosY == j)
                        quadColor = selectedBorderColor;

                    // Get Quad Position
                    float slotX = x + i * tileSize.X;
                    float slotY = y + j * tileSize.Y;

                    Utility.Render.DrawQuadColorGui(slotX + (tileSize.X - slotSize.X) / 2.0f, slotY + (tileSize.Y - slotSize.Y) / 2.0f, slotSize.X, slotSize.Y, quadColor, material);
                    Vec2f spriteSize = slotSize * 0.8f;
                    Utility.Render.DrawQuadColorGui(slotX + (tileSize.X - spriteSize.X) / 2.0f, slotY + (tileSize.Y - spriteSize.Y) / 2.0f, spriteSize.X, spriteSize.Y, borderColor, material);

                }
            }
        }

        void DrawIcons(Contexts entitasContext, float x, float y, int width, int height, Vec2f tileSize, Vec2f slotSize, Material material, Transform transform, HashSet<ItemEntity> itemInInventory)
        {
            foreach (ItemEntity itemEntity in itemInInventory)
            {
                int slotNumber = itemEntity.itemAttachedInventory.SlotNumber;
                int i = slotNumber % width;
                int j = (height - 1) - (slotNumber - i) / width;

                // Calculate Slot Border positon.
                float slotX = x + i * tileSize.X;
                float slotY = y + j * tileSize.Y;

                // Draw Count if stackable.
                if (itemEntity.hasItemStack)
                {
                    int fontSize = 50;
                    
                    // these Change with Camera size. Find better soluiton. AutoSize? MeshPro?
                    float characterSize = 0.05f * Camera.main.pixelWidth / 1024.0f;
                    float posOffset = 0.04f;

                    var rect = new Rect(600 * Random.value, 450 * Random.value, 200, 150);

                    // Todo: Implement or import librart to draw string on screen immediately. 
                    //Utility.Render.DrawString(slotX + posOffset, slotY + posOffset, characterSize, itemEntity.itemStack.Count.ToString(), fontSize, Color.white, transform, drawOrder + 4);
                }

                // Draw sprites.
                ItemPropertiesEntity itemPropertyEntity = entitasContext.itemProperties.GetEntityWithItemProperty(itemEntity.itemType.Type);
                int SpriteID = itemPropertyEntity.itemPropertyInventorySprite.ID;

                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle);

                Vec2f spriteSize = slotSize * 0.8f;
                slotX = slotX + (tileSize.X - spriteSize.X) / 2.0f;
                slotY = slotY + (tileSize.Y - spriteSize.Y) / 2.0f;
                Utility.Render.DrawSpriteColorGui(slotX, slotY, spriteSize.X, spriteSize.Y, sprite, material);
            }
        }
    }
}
