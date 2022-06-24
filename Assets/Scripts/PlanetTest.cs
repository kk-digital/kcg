using UnityEngine;
using Enums.Tile;
using KMath;

namespace Planet.Unity
{
    class PlanetTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        PlanetState Planet;
        Inventory.InventoryManager inventoryManager;
        Inventory.DrawSystem    inventoryDrawSystem;

        Contexts EntitasContext;


        Agent.AgentEntity Player;
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
            int toolBarID = Player.Entity.agentToolBar.ToolBarID;
            GameEntity Inventory = EntitasContext.game.GetEntityWithInventoryID(toolBarID);
            int selectedSlot = Inventory.inventorySlots.Selected;

            GameEntity item = GameState.InventoryManager.GetItemInSlot(toolBarID, selectedSlot);
            GameEntity itemAttribute = EntitasContext.game.GetEntityWithItemAttributes(item.itemID.ItemType);
            if (itemAttribute.hasItemAttributeAction)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameState.ActionSchedulerSystem.ScheduleAction(Player.Entity,
                        GameState.ActionCreationSystem.CreateAction(itemAttribute.itemAttributeAction.ActionTypeID, Player.AgentId));
                }
            }
                
            // unity rendering stuff
            // will be removed layer
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
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

            inventoryDrawSystem.Draw(Instantiate(Material), transform, 1000);
            Planet.Update(Time.deltaTime, Material, transform);

         //   Vector2 playerPosition = Player.Entity.physicsPosition2D.Value;

           // transform.position = new Vector3(playerPosition.x - 6.0f, playerPosition.y - 6.0f, -10.0f);
        }

        void DrawSpriteAtlas()
        {
            ref Sprites.SpriteAtlas atlas = ref GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Agent);
            Sprites.Sprite sprite = new Sprites.Sprite
            {
                Texture = atlas.Texture,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            Utility.Render.DrawSprite(-3, -1, atlas.Width / 32.0f, atlas.Height / 32.0f, sprite, Instantiate(Material), transform);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            EntitasContext = Contexts.sharedInstance;

            inventoryManager = new Inventory.InventoryManager();
            inventoryDrawSystem = new Inventory.DrawSystem(EntitasContext);

            GameResources.Initialize();

            // Generating the map
            Vec2i mapSize = new Vec2i(32, 24);
            Planet = new Planet.PlanetState(mapSize, EntitasContext.game, EntitasContext.particle);
            GenerateMap();
            SpawnStuff();

            GameState.ActionInitializeSystem.Initialize(Material);

            var inventoryAttacher = Inventory.InventoryAttacher.Instance;

            inventoryID = Player.Entity.agentInventory.InventoryID;
            toolBarID = Player.Entity.agentToolBar.ToolBarID;

            GameEntity gun = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.Gun);
            GameEntity ore = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.Ore);
            GameEntity placementTool = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.PlacementTool);
            GameEntity removeTileTool = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.RemoveTileTool);
            GameEntity spawnEnemySlimeTool = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.SpawnEnemySlimeTool);
            GameEntity miningLaserTool = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.MiningLaserTool);
            GameEntity pipePlacementTool = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.PipePlacementTool);
            GameEntity particleEmitterPlacementTool = GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.ParticleEmitterPlacementTool);


            inventoryManager.AddItem(placementTool, toolBarID);
            inventoryManager.AddItem(removeTileTool, toolBarID);
            inventoryManager.AddItem(spawnEnemySlimeTool, toolBarID);
            inventoryManager.AddItem(miningLaserTool, toolBarID);
            inventoryManager.AddItem(pipePlacementTool, toolBarID);
            inventoryManager.AddItem(particleEmitterPlacementTool, toolBarID);
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


                    tileMap.SetTile(i, j, frontTileID, MapLayerType.Front);
                }
            }

            for (int i = 0; i < tileMap.MapSize.X; i++)
            {
                for (int j = tileMap.MapSize.Y - 10; j < tileMap.MapSize.Y; j++)
                {
                    tileMap.SetTile(i, j, TileID.Air, MapLayerType.Front);
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

                for (int j = carveHeight; j < tileMap.MapSize.Y && j < carveHeight + 4; j++)
                {
                    tileMap.SetTile(i, j, TileID.Air, MapLayerType.Front);
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

                for (int j = carveHeight; j < tileMap.MapSize.Y && j < carveHeight + 4; j++)
                {
                    tileMap.SetTile(i, j, TileID.Air, MapLayerType.Front);
                }
            }


            tileMap.UpdateTileMapPositions(MapLayerType.Front);

        }

        void SpawnStuff()
        {
            ref var tileMap = ref Planet.TileMap;
            System.Random random = new System.Random((int)System.DateTime.Now.Ticks);

            float spawnHeight = tileMap.MapSize.Y - 2;

            Player = Planet.AddPlayer(CharacterSpriteId, 32, 48, 
                    new Vec2f(3.0f, spawnHeight), 0);
            PlayerID = Player.Entity.agentID.ID;

            Planet.AddAgent( CharacterSpriteId, 32, 48, new Vec2f(6.0f, spawnHeight), 0);
            Planet.AddAgent(CharacterSpriteId, 32, 48, new Vec2f(1.0f, spawnHeight), 0);

            for(int i = 0; i < tileMap.MapSize.X; i++)
            {
                if (random.Next() % 5 == 0)
                {
                    Planet.AddEnemy(CharacterSpriteId, 32, 32, new Vec2f((float)i, spawnHeight), 2);    
                }
            }


            
            GameState.ItemSpawnSystem.SpawnItem(EntitasContext.game, Enums.ItemType.Gun, new Vec2f(6.0f, spawnHeight));
            GameState.ItemSpawnSystem.SpawnItem(EntitasContext.game, Enums.ItemType.Ore, new Vec2f(10.0f, spawnHeight));
        }
        
    }
}
