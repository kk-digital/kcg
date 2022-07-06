﻿using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;
using KMath;
using Inventory;

namespace Planet.Unity
{
    class ItemTest : MonoBehaviour
    {
        [SerializeField] Material   Material;

        Planet.PlanetState          Planet;
        AgentEntity           Player;

        static bool Init = false;

        public void Start()
        {

            if (!Init)
            {
                Initialize();
                Init = true;
            }
        }

        public void Update()
        {
            ref var tileMap = ref Planet.TileMap;
            Material material = Material;

            if (Input.GetKeyDown(KeyCode.T))
            {
                GameState.ActionSchedulerSystem.ScheduleAction(Player, 
                    GameState.ActionCreationSystem.CreateAction(Planet.EntitasContext, Enums.ActionType.DropAction));
            }

            int toolBarID = Player.agentToolBar.ToolBarID;
            InventoryEntity Inventory = Planet.EntitasContext.inventory.GetEntityWithInventoryID(toolBarID);
            int selectedSlot = Inventory.inventorySlots.Selected;
       
            ItemEntity item = GameState.InventoryManager.GetItemInSlot(Planet.EntitasContext.item, toolBarID, selectedSlot);
            if (item != null)
            {
                ItemPropertiesEntity itemProperty = Planet.EntitasContext.itemProperties.GetEntityWithItemProperty(item.itemType.Type);
                if (itemProperty.hasItemPropertyAction)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        GameState.ActionSchedulerSystem.ScheduleAction(Player,
                            GameState.ActionCreationSystem.CreateAction(Planet.EntitasContext, itemProperty.itemPropertyAction.ActionTypeID)); 
                    }
                }
            }
            Planet.Update(Time.deltaTime, Material, transform);
        }

        private void OnRenderObject()
        {
            GameState.InventoryDrawSystem.Draw(Planet.EntitasContext, Material, transform);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            GameResources.Initialize();


            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);
            Planet = new Planet.PlanetState();
            Planet.Init(mapSize);
            Planet.InitializeSystems(Material, transform);

            GameResources.CreateItems(Planet.EntitasContext);

            GenerateMap();

            Player = Planet.AddPlayer(GameResources.CharacterSpriteId, 32, 48, new Vec2f(3.0f, 3.0f), 0, 100, 100, 100, 100, 100);
            int toolBarID = Player.agentToolBar.ToolBarID;

            // Create Action            
            GameState.ItemSpawnSystem.SpawnItem(Planet.EntitasContext, Enums.ItemType.Gun, new Vec2f(3.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnItem(Planet.EntitasContext, Enums.ItemType.Ore, new Vec2f(6.0f, 3.0f));
            var SpawnEnemyTool = GameState.ItemSpawnSystem.SpawnInventoryItem(Planet.EntitasContext.item, Enums.ItemType.SpawnEnemySlimeTool);
            GameState.InventoryManager.AddItem(Planet.EntitasContext, SpawnEnemyTool, toolBarID);
        }

        void GenerateMap()
        {
            ref var tileMap = ref Planet.TileMap;

            for (int j = 0; j < tileMap.MapSize.Y; j++)
            {
                for (int i = 0; i < tileMap.MapSize.X; i++)
                {
                    TileID frontTile;

                    if (i >= tileMap.MapSize.X / 2)
                    {
                        if (j % 2 == 0 && i == tileMap.MapSize.X / 2)
                        {
                            frontTile = TileID.Moon;
                        }
                        else
                        {
                            frontTile = TileID.Glass;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == tileMap.MapSize.X / 2 + 1)
                        {
                            frontTile = TileID.Glass;
                        }
                        else
                        {
                            frontTile = TileID.Moon;
                        }
                    }

                    if (j is > 1 and < 6 || (j > 8 + i))
                    {
                        frontTile = TileID.Air;
                    }


                    tileMap.SetFrontTile(i, j, frontTile);
                }
            }
            //TileMap.BuildLayerTexture(MapLayerType.Front);
        }
    }
}
