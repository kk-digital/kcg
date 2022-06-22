using UnityEngine;
using System.Collections.Generic;
using Entitas;
using Enums.Tile;
using KMath;
using Enums;

namespace Planet.Unity
{
    class PlanetTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        Planet.PlanetState Planet;
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameState.ActionSchedulerSystem.ScheduleAction(Player.Entity,
                    GameState.ActionCreationSystem.CreateAction(itemAttribute.itemAttributeAction.ActionTypeID, Player.AgentId));
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
            Planet = new Planet.PlanetState(mapSize, EntitasContext.game);
            GenerateMap();
            SpawnStuff();

            GameState.ActionInitializeSystem.Initialize(Planet, Material);

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


            inventoryManager.AddItem(placementTool, toolBarID);
            inventoryManager.AddItem(removeTileTool, toolBarID);
            inventoryManager.AddItem(spawnEnemySlimeTool, toolBarID);
            inventoryManager.AddItem(miningLaserTool, toolBarID);
            inventoryManager.AddItem(pipePlacementTool, toolBarID);
        }
        
        void GenerateOre()
        {
            var tileMap = Planet.TileMap;
            var tileMapBorders = tileMap.Borders;
            
            for(int j = tileMapBorders.IntBottom; j < tileMapBorders.IntTop; j++)
            {
                for(int i = tileMapBorders.IntLeft; i < tileMapBorders.IntRight; i++)
                {
                    ref var tile = ref tileMap.GetTileRef(i, j, MapLayerType.Front);

                    if (tile.Type == 10 && (int)KMath.Random.Mt19937.genrand_int32() % 30 == 0)
                    {
                        var oreTile = new Tile.Tile(new Vec2f(i, j));
                        
                        int type = (int)KMath.Random.Mt19937.genrand_int32() % 6;
                        if (type == 0)
                        {
                            oreTile.Type = (int)Tile.TileEnum.Ore1;
                        }
                        else if (type is 1 or 2)
                        {
                            oreTile.Type = (int)Tile.TileEnum.Ore2;
                        }
                        else if (type is 3 or 4)
                        {
                            oreTile.Type = (int)Tile.TileEnum.Ore3;
                        }
                        
                        tileMap.SetTile(ref oreTile, MapLayerType.Ore);
                    }
                }
            }
        }

        void GenerateMap()
        {
            KMath.Random.Mt19937.init_genrand((ulong) System.DateTime.Now.Ticks);
            
            TileMap tileMap = Planet.TileMap;

            var borders = tileMap.Borders;

            for (int j = borders.IntBottom; j < borders.IntTop; j++)
            {
                for (int i = borders.IntLeft; i < borders.IntRight; i++)
                {
                    var frontTile = new Tile.Tile(new Vec2f(i, j));

                    if (i >= borders.IntRight / 2)
                    {
                        if (j % 2 == 0 && i == borders.IntRight / 2)
                        {
                            frontTile.Type = (int)Tile.TileEnum.Moon;
                        }
                        else
                        {
                            frontTile.Type = (int)Tile.TileEnum.Glass;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == borders.IntRight / 2 + 1)
                        {
                            frontTile.Type = (int)Tile.TileEnum.Glass;
                        }
                        else
                        {
                            frontTile.Type = (int)Tile.TileEnum.Moon;
                        }
                    }


                    tileMap.SetTile(ref frontTile, MapLayerType.Front);
                }
            }

            for (int i = 0; i < borders.IntRight; i++)
            {
                for (int j = borders.IntTop - 10; j < borders.IntTop; j++)
                {
                    var tile = new Tile.Tile(new Vec2f(i, j));
                    tileMap.SetTile(ref tile, MapLayerType.Front);
                }
            }

            int carveHeight = borders.IntTop - 10;

            for (int i = borders.IntLeft; i < borders.IntRight; i++)
            {
                int move = ((int) KMath.Random.Mt19937.genrand_int32() % 3) - 1;
                if (((int) KMath.Random.Mt19937.genrand_int32() % 5) <= 3)
                {
                    move = 0;
                }

                carveHeight += move;
                if (carveHeight >= borders.IntTop)
                {
                    carveHeight = borders.IntTop - 1;
                }

                for (int j = carveHeight; j < borders.IntTop && j < carveHeight + 4; j++)
                {
                    var tile = new Tile.Tile(new Vec2f(i, j));
                    tileMap.SetTile(ref tile, MapLayerType.Front);
                }
            }

            carveHeight = 5;

            for (int i = borders.IntRight - 1; i >= borders.IntLeft; i--)
            {
                int move = ((int) KMath.Random.Mt19937.genrand_int32() % 3) - 1;
                if (((int) KMath.Random.Mt19937.genrand_int32() % 10) <= 3)
                {
                    move = 1;
                }

                carveHeight += move;
                if (carveHeight >= borders.IntTop)
                {
                    carveHeight = borders.IntTop - 1;
                }

                for (int j = carveHeight; j < borders.IntTop && j < carveHeight + 4; j++)
                {
                    var tile = new Tile.Tile(new Vec2f(i, j));
                    tileMap.SetTile(ref tile, MapLayerType.Front);
                }
            }


            GenerateOre();

            tileMap.UpdateTileMapPositions(MapLayerType.Front);
            tileMap.UpdateTileMapPositions(MapLayerType.Ore);
            //TileMap.BuildLayerTexture(MapLayerType.Front);
            //TileMap.BuildLayerTexture(MapLayerType.Ore);

        }

        void SpawnStuff()
        {
            TileMap tileMap = Planet.TileMap;
            System.Random random = new System.Random((int)System.DateTime.Now.Ticks);

            var borders = tileMap.Borders;

            float spawnHeight = borders.Top - 2f;

            Player = Planet.AddPlayer(Instantiate(Material), CharacterSpriteId, 32, 48, 
                    new Vec2f(3.0f, spawnHeight), 0);
            PlayerID = Player.Entity.agentID.ID;

            Planet.AddAgent(Instantiate(Material), CharacterSpriteId, 32, 48, new Vec2f(6.0f, spawnHeight), 0);
            Planet.AddAgent(Instantiate(Material), CharacterSpriteId, 32, 48, new Vec2f(1.0f, spawnHeight), 0);

            for(int i = borders.IntLeft; i < borders.IntRight; i++)
            {
                if (random.Next() % 5 == 0)
                {
                    Planet.AddEnemy(Instantiate(Material), CharacterSpriteId, 32, 32, new Vec2f((float)i, spawnHeight), 2);    
                }
            }


            
            GameState.ItemSpawnSystem.SpawnItem(EntitasContext.game, Enums.ItemType.Gun, new Vec2f(6.0f, spawnHeight));
            GameState.ItemSpawnSystem.SpawnItem(EntitasContext.game, Enums.ItemType.Ore, new Vec2f(3.0f, spawnHeight));
        }
        
    }
}
