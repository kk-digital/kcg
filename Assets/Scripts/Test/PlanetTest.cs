using UnityEngine;
using Enums.Tile;
using KMath;

namespace Planet.Unity
{
    class PlanetTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        public PlanetState Planet;
        Inventory.InventoryManager inventoryManager;
        Inventory.DrawSystem inventoryDrawSystem;

        // Health Bar
        KGUI.HealthBarUI healthBarUI;

        // Food Bar
        KGUI.FoodBarUI foodBarUI;

        // Water Bar
        KGUI.WaterBarUI waterBarUI;

        // Oxygen Bar
        KGUI.OxygenBarUI oxygenBarUI;

        // Fuel Bar
        KGUI.FuelBarUI fuelBarUI;

        AgentEntity Player;
        int PlayerID;

        int CharacterSpriteId;
        int inventoryID;
        int toolBarID;

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
            int toolBarID = Player.agentToolBar.ToolBarID;
            InventoryEntity Inventory = Planet.EntitasContext.inventory.GetEntityWithInventoryID(toolBarID);
            int selectedSlot = Inventory.inventorySlots.Selected;

            ItemEntity item = GameState.InventoryManager.GetItemInSlot(Planet.EntitasContext.item, toolBarID, selectedSlot);
            ItemPropertiesEntity itemProperty = Planet.EntitasContext.itemProperties.GetEntityWithItemProperty(item.itemType.Type);
            if (itemProperty.hasItemPropertyAction)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameState.ActionSchedulerSystem.ScheduleAction(Player,
                        GameState.ActionCreationSystem.CreateAction(Planet.EntitasContext, itemProperty.itemPropertyAction.ActionTypeID));
                }
            }

            Planet.Update(Time.deltaTime, Material, transform);
            //   Vector2 playerPosition = Player.Entity.physicsPosition2D.Value;

            // transform.position = new Vector3(playerPosition.x - 6.0f, playerPosition.y - 6.0f, -10.0f);
        }
        
        private void OnRenderObject()
        {
            inventoryDrawSystem.Draw(Planet.EntitasContext, Material, transform);
        }

        private void OnGUI()
        {
            if (Init)
            {
                //Health Bar Draw
                healthBarUI.Draw(Planet.EntitasContext);

                // Food Bar Update
                foodBarUI.Update();

                // Water Bar Update
                waterBarUI.Update();

                // Fuel Bar Update
                fuelBarUI.Update();

                // Oxygen Bar Update
                oxygenBarUI.Update();
            }
        }

        private void OnDrawGizmos()
        {
            // Set the color of gizmos
            Gizmos.color = Color.green;
            
            // Draw a cube around the map
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(Planet.TileMap.MapSize.X, Planet.TileMap.MapSize.Y, 0.0f));

            // Draw lines around player if out of bounds
            if (Player != null)
                if(Player.physicsPosition2D.Value.X -10.0f >= Planet.TileMap.MapSize.X)
                {
                    // Out of bounds
                
                    // X+
                    Gizmos.DrawLine(new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y, 0.0f), new Vector3(Player.physicsPosition2D.Value.X + 10.0f, Player.physicsPosition2D.Value.Y));

                    // X-
                    Gizmos.DrawLine(new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y, 0.0f), new Vector3(Player.physicsPosition2D.Value.X - 10.0f, Player.physicsPosition2D.Value.Y));

                    // Y+
                    Gizmos.DrawLine(new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y, 0.0f), new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y + 10.0f));

                    // Y-
                    Gizmos.DrawLine(new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y, 0.0f), new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y - 10.0f));
                }

            // Draw Chunk Visualizer
            Admin.AdminAPI.DrawChunkVisualizer(Planet.TileMap);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            inventoryManager = new Inventory.InventoryManager();
            inventoryDrawSystem = new Inventory.DrawSystem();

            GameResources.Initialize();

            // Generating the map
            Vec2i mapSize = new Vec2i(32, 24);
            Planet = new Planet.PlanetState();
            Planet.Init(mapSize);
            Planet.InitializeSystems(Material, transform);

            GameResources.CreateItems(Planet.EntitasContext);

            GenerateMap();
            SpawnStuff();

            inventoryID = Player.agentInventory.InventoryID;
            toolBarID = Player.agentToolBar.ToolBarID;

            // Admin API Spawn Items
            Admin.AdminAPI.SpawnItem(Enums.ItemType.Gun, Planet.EntitasContext);
            Admin.AdminAPI.SpawnItem(Enums.ItemType.Ore, Planet.EntitasContext);

            // Admin API Add Items
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.PlacementTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.RemoveTileTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.SpawnEnemySlimeTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.MiningLaserTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.PipePlacementTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.ParticleEmitterPlacementTool, Planet.EntitasContext);

            // Health Bar Initialize
            healthBarUI = new KGUI.HealthBarUI();
            healthBarUI.Initialize();

            // Food Bar Initialize
            foodBarUI = new KGUI.FoodBarUI();
            foodBarUI.Initialize(Planet.EntitasContext);

            // Water Bar Initialize
            waterBarUI = new KGUI.WaterBarUI();
            waterBarUI.Initialize(Planet.EntitasContext);

            // Oxygen Bar Initialize
            oxygenBarUI = new KGUI.OxygenBarUI();
            oxygenBarUI.Initialize(Planet.EntitasContext);

            // Oxygen Bar Initialize
            fuelBarUI = new KGUI.FuelBarUI();
            fuelBarUI.Initialize(Planet.EntitasContext);
        }

        void GenerateMap()
        {
            KMath.Random.Mt19937.init_genrand((ulong) System.DateTime.Now.Ticks);
            
            ref var tileMap = ref Planet.TileMap;

            for (int j = 0; j < tileMap.MapSize.Y; j++)
            {
                for (int i = 0; i < tileMap.MapSize.X; i++)
                {
                    var frontTileID = TileID.Air;

                    if (i >= tileMap.MapSize.X / 2)
                    {
                        if (j % 2 == 0 && i == tileMap.MapSize.X / 2)
                        {
                            frontTileID = TileID.Moon;
                        }
                        else
                        {
                            frontTileID = TileID.Glass;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == tileMap.MapSize.X / 2 + 1)
                        {
                            frontTileID = TileID.Glass;
                        }
                        else
                        {
                            frontTileID = TileID.Moon;
                            /*if ((int) KMath.Random.Mt19937.genrand_int32() % 10 == 0)
                            {
                                int oreRandom = (int) KMath.Random.Mt19937.genrand_int32() % 3;
                                if (oreRandom == 0)
                                {
                                    frontTile.SpriteId2 = GameResources.OreSprite;
                                }
                                else if (oreRandom == 1)
                                {
                                    frontTile.SpriteId2 = GameResources.Ore2Sprite;
                                }
                                else
                                {
                                    frontTile.SpriteId2 = GameResources.Ore3Sprite;
                                }

                                frontTile.DrawType = TileDrawType.Composited;
                            }*/
                        }
                    }


                    tileMap.SetFrontTile(i, j, frontTileID);
                }
            }

            for (int i = 0; i < tileMap.MapSize.X; i++)
            {
                for (int j = tileMap.MapSize.Y - 10; j < tileMap.MapSize.Y; j++)
                {
                    tileMap.SetFrontTile(i, j, TileID.Air);
                }
            }

            int carveHeight = tileMap.MapSize.Y - 10;

            for (int i = 0; i < tileMap.MapSize.X; i++)
            {
                int move = ((int) KMath.Random.Mt19937.genrand_int32() % 3) - 1;
                if (((int) KMath.Random.Mt19937.genrand_int32() % 5) <= 3)
                {
                    move = 0;
                }

                carveHeight += move;
                if (carveHeight >= tileMap.MapSize.Y)
                {
                    carveHeight = tileMap.MapSize.Y - 1;
                }

                if (carveHeight < 0)
                {
                    carveHeight = 0;
                }

                for (int j = carveHeight; j < tileMap.MapSize.Y && j < carveHeight + 4; j++)
                {
                    tileMap.SetFrontTile(i, j, TileID.Air);
                }
            }

            carveHeight = 5;

            for (int i = tileMap.MapSize.X - 1; i >= 0; i--)
            {
                int move = ((int) KMath.Random.Mt19937.genrand_int32() % 3) - 1;
                if (((int) KMath.Random.Mt19937.genrand_int32() % 10) <= 3)
                {
                    move = 1;
                }

                carveHeight += move;
                if (carveHeight >= tileMap.MapSize.Y)
                {
                    carveHeight = tileMap.MapSize.Y - 1;
                }

                if (carveHeight < 0)
                {
                    carveHeight = 0;
                }

                for (int j = carveHeight; j < tileMap.MapSize.Y && j < carveHeight + 4; j++)
                {
                    tileMap.SetFrontTile(i, j, TileID.Air);
                }
            }

            //tileMap.UpdateTileMapPositions(MapLayerType.Front);
        }

        void SpawnStuff()
        {
            ref var tileMap = ref Planet.TileMap;
            System.Random random = new System.Random((int)System.DateTime.Now.Ticks);

            float spawnHeight = tileMap.MapSize.Y - 2;

            Player = Planet.AddPlayer(new Vec2f(3.0f, spawnHeight));
            PlayerID = Player.agentID.ID;

            Planet.AddAgent(new Vec2f(6.0f, spawnHeight));
            Planet.AddAgent(new Vec2f(1.0f, spawnHeight));

            for(int i = 0; i < tileMap.MapSize.X; i++)
            {
                if (random.Next() % 5 == 0)
                {
                    Planet.AddEnemy(new Vec2f((float)i, spawnHeight));    
                }
            }
            
            GameState.ItemSpawnSystem.SpawnItem(Planet.EntitasContext, Enums.ItemType.Gun, new Vec2f(6.0f, spawnHeight));
            GameState.ItemSpawnSystem.SpawnItem(Planet.EntitasContext, Enums.ItemType.Ore, new Vec2f(10.0f, spawnHeight));
        }
    }
}
