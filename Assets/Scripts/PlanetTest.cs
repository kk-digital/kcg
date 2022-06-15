using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;

namespace Planet.Unity
{
    class PlanetTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        Planet.PlanetState Planet;
        Inventory.ManagerSystem inventoryManagerSystem;
        Item.SpawnerSystem      itemSpawnSystem;
        Inventory.DrawSystem    inventoryDrawSystem;

        Contexts EntitasContext;
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
            Planet.TileMap TileMap = Planet.TileMap;
            Material material = Material;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                Planet.PlaceTile(x, y, 10, MapLayerType.Front);
                TileMap.BuildLayerTexture(MapLayerType.Front);
                
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                TileMap.RemoveTile(x, y, MapLayerType.Front);
                TileMap.BuildLayerTexture(MapLayerType.Front);
                
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

            inventoryDrawSystem.Draw(Material, transform, 1000);
            Planet.Update(Time.deltaTime, Material, transform);
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
        
            inventoryManagerSystem = new Inventory.ManagerSystem(EntitasContext);
            itemSpawnSystem = new Item.SpawnerSystem(EntitasContext);
            inventoryDrawSystem = new Inventory.DrawSystem(EntitasContext);



            int TilesMoon = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png", 16, 16);
            int OreTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);
            int GunSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\gun-temp.png", 44, 25);

            int RockSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png", 16, 16);
            int RockDustSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png", 16, 16);

            int SlimeSpriteSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\slime.png", 32, 32);

            int CharacterSpriteSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png", 32, 48);

            int SlimeSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 0, 0, Enums.AtlasType.Agent);
            int CharacterSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(CharacterSpriteSheet, 0, 0, Enums.AtlasType.Agent);

            GameState.TileCreationApi.CreateTile(8);
            GameState.TileCreationApi.SetTileName("ore_1");
            GameState.TileCreationApi.SetTileTexture16(OreTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(9);
            GameState.TileCreationApi.SetTileName("glass");
            GameState.TileCreationApi.SetTileSpriteSheet16(TilesMoon, 11, 10);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(10);
            GameState.TileCreationApi.SetTileName("moon");
            GameState.TileCreationApi.SetTileSpriteSheet16(TilesMoon, 0, 0);
            GameState.TileCreationApi.EndTile();


            GameState.AnimationManager.CreateAnimation(0);
            GameState.AnimationManager.SetName("character-move-left");
            GameState.AnimationManager.SetTimePerFrame(0.15f);
            GameState.AnimationManager.SetBaseSpriteID(0);
            GameState.AnimationManager.SetFrameCount(1);
            GameState.AnimationManager.EndAnimation();

            GameState.AnimationManager.CreateAnimation(1);
            GameState.AnimationManager.SetName("character-move-right");
            GameState.AnimationManager.SetTimePerFrame(0.15f);
            GameState.AnimationManager.SetBaseSpriteID(0);
            GameState.AnimationManager.SetFrameCount(1);
            GameState.AnimationManager.EndAnimation();

            GameState.AnimationManager.CreateAnimation(2);
            GameState.AnimationManager.SetName("slime-move-left");
            GameState.AnimationManager.SetTimePerFrame(0.15f);
            GameState.AnimationManager.SetBaseSpriteID(0);
            GameState.AnimationManager.SetFrameCount(4);
            GameState.AnimationManager.EndAnimation();



            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);
            Planet = new Planet.PlanetState(mapSize);
            GenerateMap();


            var Player = Planet.AddPlayer(Instantiate(Material), CharacterSpriteId, 32, 48, new Vector2(3.0f, 3.0f), 0);
            int PlayerID = Player.Entity.agentID.ID;

            Planet.AddAgent(Instantiate(Material), CharacterSpriteId, 32, 48, new Vector2(6.0f, 3.0f));
            Planet.AddEnemy(Instantiate(Material), SlimeSpriteId, 32, 32, new Vector2(8.0f, 5.0f));
            Planet.AddAgent(Instantiate(Material), CharacterSpriteId, 32, 48, new Vector2(1.0f, 4.0f));

            var inventoryAttacher = Inventory.InventoryAttacher.Instance;

            Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "Gun");
            Item.CreationApi.Instance.SetTexture(GunSpriteSheet);
            Item.CreationApi.Instance.SetInventoryTexture(GunSpriteSheet);
            Item.CreationApi.Instance.SetSize(new Vector2(0.5f, 0.5f));
            Item.CreationApi.Instance.EndItem();

            Item.CreationApi.Instance.CreateItem(Enums.ItemType.Ore, "Ore");
            Item.CreationApi.Instance.SetTexture(OreTileSheet);
            Item.CreationApi.Instance.SetInventoryTexture(OreTileSheet);
            Item.CreationApi.Instance.SetSize(new Vector2(0.5f, 0.5f));
            Item.CreationApi.Instance.SetStackable(99);
            Item.CreationApi.Instance.EndItem();

            Item.CreationApi.Instance.CreateItem(Enums.ItemType.PlacementTool, "PlacementTool");
            Item.CreationApi.Instance.SetTexture(RockSpriteSheet);
            Item.CreationApi.Instance.SetInventoryTexture(RockSpriteSheet);
            Item.CreationApi.Instance.SetSize(new Vector2(0.5f, 0.5f));
            Item.CreationApi.Instance.EndItem();

            Item.CreationApi.Instance.CreateItem(Enums.ItemType.RemoveTileTool, "RemoveTileTool");
            Item.CreationApi.Instance.SetTexture(RockDustSpriteSheet);
            Item.CreationApi.Instance.SetInventoryTexture(RockDustSpriteSheet);
            Item.CreationApi.Instance.SetSize(new Vector2(0.5f, 0.5f));
            Item.CreationApi.Instance.EndItem();


            int inventoryID = Player.Entity.agentInventory.InventoryID;
            int toolBarID = Player.Entity.agentToolBar.ToolBarID;

            GameEntity gun = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.Gun);
            GameEntity ore = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.Ore);
            GameEntity placementTool = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.PlacementTool);
            GameEntity removeTileTool = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.RemoveTileTool);

            inventoryManagerSystem.AddItem(gun, toolBarID);
            inventoryManagerSystem.AddItem(ore, toolBarID);
            inventoryManagerSystem.AddItem(placementTool, toolBarID);
            inventoryManagerSystem.AddItem(removeTileTool, toolBarID);

            itemSpawnSystem.SpawnItem(Enums.ItemType.Gun, new Vector2(6.0f, 3.0f));
            itemSpawnSystem.SpawnItem(Enums.ItemType.Ore, new Vector2(3.0f, 3.0f));
        }




        void GenerateMap()
        {
            Planet.TileMap TileMap = Planet.TileMap;

           Vector2Int mapSize = TileMap.MapSize;

           for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    Tile.Tile frontTile = Tile.Tile.EmptyTile;
                    Tile.Tile oreTile = Tile.Tile.EmptyTile;

                    if (i >= mapSize.x / 2)
                    {
                        if (j % 2 == 0 && i == mapSize.x / 2)
                        {
                            frontTile.Type = 10;
                        }
                        else
                        {
                            frontTile.Type = 9;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == mapSize.x / 2 + 1)
                        {
                            frontTile.Type = 9;
                        }
                        else
                        {
                            frontTile.Type = 10;
                        }
                    }


                    if (i % 10 == 0)
                    {
                        oreTile.Type = 8;
                    }

                    if ((j > 1 && j < 6) || (j > (8 + i)))
                    {
                       frontTile.Type = -1; 
                       oreTile.Type = -1;
                    }

                    
                    TileMap.SetTile(i, j, frontTile, MapLayerType.Front);
                    TileMap.SetTile(i, j, oreTile, MapLayerType.Ore);
                }
            }

            TileMap.HeightMap.UpdateTopTilesMap(ref TileMap);

            TileMap.UpdateTileMapPositions(MapLayerType.Front);
            TileMap.UpdateTileMapPositions(MapLayerType.Ore);
            TileMap.BuildLayerTexture(MapLayerType.Front);
            TileMap.BuildLayerTexture(MapLayerType.Ore);
        
        }
        
    }
}


