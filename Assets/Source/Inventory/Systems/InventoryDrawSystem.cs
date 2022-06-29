﻿using UnityEngine;
using System.Collections.Generic;
using Entitas;

namespace Inventory
{
    public class InventoryDrawSystem
    {

        /// <summary>
        /// DrawCall will use form drawOrder to drawOrder + 3.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="transform"></param>
        /// <param name="drawOrder"></param>
        public void Draw(Contexts contexts, Material material, Transform transform, int drawOrder)
        {
            var openInventories = contexts.inventory.GetGroup(InventoryMatcher.AllOf(InventoryMatcher.InventoryDrawable, InventoryMatcher.InventoryID));
            // If empty Draw ToolBar.

            foreach (InventoryEntity inventoryEntity in openInventories)
            {
                DrawInventory(contexts, material, transform, inventoryEntity, drawOrder);
            }
        }

        private void DrawInventory(Contexts entitasContext, Material material, Transform transform, InventoryEntity inventoryEntity, int drawOrder)
        {
            // Todo: Add scrool bar.
            // Todo: allow user to move inventory position?

            // Calculate Positions and Tile Sizes relative to sceen.
            float tileSize = 
                Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/16, 0f, 0f)).x
                - Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x;

            float slotSize = tileSize * 0.9f;

            // Get Inventory Info.
            int width = inventoryEntity.inventorySize.Width;
            int height = inventoryEntity.inventorySize.Height;

            float h = height * tileSize;
            float w = width * tileSize;

            // Get Initial Positon.
            float x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0f, 0f)).x;
            float y = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height / 2f, 0f)).y;

            x -= w / 2f;
            y -= h / 2f;

            // If is tool bar draw at the botton of the screen.
            if (inventoryEntity.isInventoryToolBar)
            {
                y = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).y + tileSize / 2f;
            }

            DrawBackGround(x, y, w, h, material, transform, drawOrder);

            DrawCells(x, y, width, height, tileSize, slotSize, material, transform, inventoryEntity, drawOrder);

            var itemInInventory = entitasContext.game.GetEntitiesWithItemAttachedInventory(inventoryEntity.inventoryID.ID);
            DrawIcons(entitasContext, x, y, width, height, tileSize, slotSize, material, transform, itemInInventory, drawOrder);
        }

        void DrawBackGround(float x, float y, float w, float h, Material material, Transform transform, int drawOrder)
        {
            Color backGround = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            Utility.Render.DrawQuadColor(x, y, w, h, backGround, material);
        }

        void DrawCells(float x, float y, int width, int height, float tileSize, float slotSize, Material material, Transform transform, InventoryEntity inventoryEntity, int drawOrder)
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
                    float slotX = x + i * tileSize + slotSize * 0.5f - 0.125f;
                    float slotY = y - slotSize * 0.5f;

                    Utility.Render.DrawString(slotX + (tileSize - slotSize) / 2.0f, 
                                slotY + (tileSize - slotSize) / 2.0f, 0.25f, "" + (i + 1), 16, new Color(255, 255, 255, 255),
                                             transform, drawOrder + 2);
                }

                for (int j = 0; j < height; j++)
                {
                    // Assign Border Color.
                    Color quadColor = borderColor;
                    if (selectedSlotPosX == i && selectedSlotPosY == j)
                        quadColor = selectedBorderColor;

                    // Get Quad Position
                    float slotX = x + i * tileSize;
                    float slotY = y + j * tileSize;

                    Utility.Render.DrawQuadColor(slotX + (tileSize - slotSize) / 2.0f, slotY + (tileSize - slotSize) / 2.0f, slotSize, slotSize, quadColor, material);
                    float spriteSize = slotSize * 0.8f;
                    Utility.Render.DrawQuadColor(slotX + (tileSize - spriteSize) / 2.0f, slotY + (tileSize - spriteSize) / 2.0f, spriteSize, spriteSize, borderColor, material);

                }
            }
        }

        void DrawIcons(Contexts entitasContext, float x, float y, int width, int height, float tileSize, float slotSize, Material material, Transform transform, HashSet<GameEntity> itemInInventory, int drawOrder)
        {
            foreach (GameEntity itemEntity in itemInInventory)
            {
                int slotNumber = itemEntity.itemAttachedInventory.SlotNumber;
                int i = slotNumber % width;
                int j = (height - 1) - (slotNumber - i) / width;

                // Calculate Slot Border positon.
                float slotX = x + i * tileSize;
                float slotY = y + j * tileSize;

                // Draw Count if stackable.
                if (itemEntity.hasItemStack)
                {
                    int fontSize = 50;
                    
                    // these Change with Camera size. Find better soluiton. AutoSize? MeshPro?
                    float characterSize = 0.05f * Camera.main.pixelWidth / 1024.0f;
                    float posOffset = 0.04f;
                    
                    Utility.Render.DrawString(slotX + posOffset, slotY + posOffset, characterSize, itemEntity.itemStack.Count.ToString(), fontSize, Color.white, transform, drawOrder + 4);
                }

                // Draw sprites.
                ItemPropertiesEntity itemPropertyEntity = entitasContext.itemProperties.GetEntityWithItemProperty(itemEntity.itemID.ItemType);
                int SpriteID = itemPropertyEntity.itemPropertyInventorySprite.ID;

                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle);

                float spriteSize = slotSize * 0.8f;
                slotX = slotX + (tileSize - spriteSize) / 2.0f;
                slotY = slotY + (tileSize - spriteSize) / 2.0f;
                Utility.Render.DrawSprite(slotX, slotY, spriteSize, spriteSize, sprite, material);
            }
        }
    }
}
