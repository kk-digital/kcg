using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;
using KMath;
using Inventory;

namespace Planet.Unity
{
    class ItemTest : MonoBehaviour
    {
        [SerializeField] Material   Material;

        Contexts                    EntitasContext;
        Planet.PlanetState          Planet;
        Agent.AgentEntity           Player;

        static bool Init = false;

        public void Start()
        {
            EntitasContext = Contexts.sharedInstance;

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

            // unity rendering stuff
            // will be removed later
            foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            {
                if (Application.isPlaying)
                {
                    Destroy(mr.gameObject);
                }
                else
                {
                    DestroyImmediate(mr.gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                GameState.ActionSchedulerSystem.ScheduleAction(Player.Entity, 
                    GameState.ActionCreationSystem.CreateAction(Player.AgentId, Player.Entity.agentID.ID));
            }

            int toolBarID = Player.Entity.agentToolBar.ToolBarID;
            InventoryEntity Inventory = EntitasContext.inventory.GetEntityWithInventoryID(toolBarID);
            int selectedSlot = Inventory.inventorySlots.Selected;
       
            GameEntity item = GameState.InventoryManager.GetItemInSlot(toolBarID, selectedSlot);
            if (item != null)
            {
                ItemPropertiesEntity itemProperty = EntitasContext.itemProperties.GetEntityWithItemProperty(item.itemID.ItemType);
                if (itemProperty.hasItemPropertyAction)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        GameState.ActionSchedulerSystem.ScheduleAction(Player.Entity,
                            GameState.ActionCreationSystem.CreateAction(itemProperty.itemPropertyAction.ActionTypeID, Player.AgentId));
                    }
                }
            }

            GameState.InventoryDrawSystem.Draw(Material, transform, 14);
            Planet.Update(Time.deltaTime, Material, transform);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            GameResources.Initialize();
            GameState.ActionInitializeSystem.Initialize(Material);

            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);
            Planet = new Planet.PlanetState(mapSize);
            GenerateMap();

            Player = Planet.AddPlayer(GameResources.CharacterSpriteId, 32, 48, new Vec2f(3.0f, 3.0f), 0, 100, 100, 100, 100, 100);
            int toolBarID = Player.Entity.agentToolBar.ToolBarID;

            // Create Action            
            GameState.ItemSpawnSystem.SpawnItem(EntitasContext, Enums.ItemType.Gun, new Vec2f(3.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnItem(EntitasContext, Enums.ItemType.Ore, new Vec2f(6.0f, 3.0f));
            var SpawnEnemyTool = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.SpawnEnemySlimeTool);
            GameState.InventoryManager.AddItem(SpawnEnemyTool, toolBarID);
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


                    tileMap.SetTile(i, j, frontTile, MapLayerType.Front);
                }
            }



            tileMap.UpdateTileMapPositions(MapLayerType.Front);
            //TileMap.BuildLayerTexture(MapLayerType.Front);
        }
    }
}
