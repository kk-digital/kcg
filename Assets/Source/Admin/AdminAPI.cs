using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Admin
{
    // Admin API
    public static class AdminAPI
    {
        // Spawn Item Function
        public static ItemEntity SpawnItem(Enums.ItemType itemID, Contexts contexts)
        {
            if(contexts == null)
                return null;

            // Spawn Item
            ItemEntity item = GameState.ItemSpawnSystem.SpawnInventoryItem(contexts, itemID);

            // Return Item
            return item;
        }

        // Give Item to Active Agent Function
        public static void AddItem(Inventory.InventoryManager manager, int inventoryID, Enums.ItemType itemID, Contexts contexts)
        {
            if (contexts == null)
                return;

            manager.AddItem(contexts, SpawnItem(itemID, contexts), inventoryID);
        }

        // Give Item to Agent Function
        public static void AddItem(Inventory.InventoryManager manager, AgentEntity agentID, Enums.ItemType itemID, Contexts contexts)
        {
            if (contexts == null)
                return;

            manager.AddItem(contexts, SpawnItem(itemID, contexts), agentID.agentInventory.InventoryID);
        }

        // Chunk Visualizer X Offset
        public static float chunkVisualizerXOffset = 0.5f;

        // Chunk Visualizer Y Offset
        public static float chunkVisualizerYOffset = 0.0f;

        // Display Chunk Visualizer
        public static void DrawChunkVisualizer(PlanetTileMap.TileMap tileMap)
        {
            // Draw square to every tile
            for(int y = 0; y < tileMap.MapSize.Y; y++)
            {
                for (int x = 0; x < tileMap.MapSize.X; x++)
                {
                    // Get Chunks
                    for (int i = 0; i < tileMap.ChunkArrayLength; i++)
                    {
                        // If chunks is empty make gizmos color black, if it's not make it green.
                        if (tileMap.ChunkArray[i].Type == Enums.Tile.MapChunkType.Empty)
                            Gizmos.color = Color.black;
                        else
                            Gizmos.color = Color.green;

                        // Draw colored cubes to the editor display (Debug)
                        Gizmos.DrawCube(new Vector3(x + chunkVisualizerXOffset, y + chunkVisualizerYOffset), new Vector3(1, 1));
                    }
                }
            }
        }
    }
}
