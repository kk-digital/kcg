using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;
using KMath;

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
            Planet.TileMap TileMap = Planet.TileMap;
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

            GameState.InventoryDrawSystem.Draw(Material, transform, 14);
            Planet.Update(Time.deltaTime, Material, transform);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            int TilesMoon =
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png", 16, 16);
            int OreTileSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);

            int CharacterSpriteSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png", 32, 48);

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

            int GunSpriteSheet =
                GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\gun-temp.png", 44, 25);
            int OreSpriteSheet =
                GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);

            int GunIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(GunSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            int OreIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(OreSpriteSheet, 0, 0, Enums.AtlasType.Particle);

            Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "Gun");
            Item.CreationApi.Instance.SetTexture(GunIcon);
            Item.CreationApi.Instance.SetInventoryTexture(GunIcon);
            Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
            Item.CreationApi.Instance.EndItem();

            Item.CreationApi.Instance.CreateItem(Enums.ItemType.Ore, "Ore");
            Item.CreationApi.Instance.SetTexture(OreIcon);
            Item.CreationApi.Instance.SetInventoryTexture(OreIcon);
            Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
            Item.CreationApi.Instance.SetStackable(99);
            Item.CreationApi.Instance.EndItem();

            GameState.AnimationManager.CreateAnimation(0);
            GameState.AnimationManager.SetName("character-move-left");
            GameState.AnimationManager.SetTimePerFrame(0.15f);
            GameState.AnimationManager.SetBaseSpriteID(CharacterSpriteId);
            GameState.AnimationManager.SetFrameCount(1);
            GameState.AnimationManager.EndAnimation();

            GameState.AnimationManager.CreateAnimation(1);
            GameState.AnimationManager.SetName("character-move-right");
            GameState.AnimationManager.SetTimePerFrame(0.15f);
            GameState.AnimationManager.SetBaseSpriteID(CharacterSpriteId);
            GameState.AnimationManager.SetFrameCount(1);
            GameState.AnimationManager.EndAnimation();
            GameState.ActionInitializeSystem.Initialize(Planet, Material);

            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);
            Planet = new Planet.PlanetState(mapSize, EntitasContext.game);
            GenerateMap();

            Player = Planet.AddPlayer(Instantiate(Material),CharacterSpriteId, 32, 48, new Vec2f(3.0f, 3.0f), 0);

            // Create Action            
            GameState.ItemSpawnSystem.SpawnItem(EntitasContext.game, Enums.ItemType.Gun, new Vec2f(3.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnItem(EntitasContext.game, Enums.ItemType.Ore, new Vec2f(6.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnInventoryItem(EntitasContext.game, Enums.ItemType.Ore);
        }

        void GenerateMap()
        {
            TileMap tileMap = Planet.TileMap;

            for (int j = tileMap.Borders.IntBottom; j < tileMap.Borders.IntTop; j++)
            {
                for (int i = tileMap.Borders.IntLeft; i < tileMap.Borders.IntRight; i++)
                {
                    var frontTile = new Tile.Tile(new Vec2f(i, j));
                    var oreTile = new Tile.Tile(new Vec2f(i, j));

                    if (i >= tileMap.Borders.IntRight / 2)
                    {
                        if (j % 2 == 0 && i == tileMap.Borders.IntRight / 2)
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
                        if (j % 3 == 0 && i == tileMap.Borders.IntRight / 2 + 1)
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

                    if (j is > 1 and < 6 || (j > 8 + i))
                    {
                        frontTile.Type = -1;
                        oreTile.Type = -1;
                    }


                    tileMap.SetTile(ref frontTile, MapLayerType.Front);
                }
            }



            tileMap.UpdateTileMapPositions(MapLayerType.Front);
            //TileMap.BuildLayerTexture(MapLayerType.Front);
        }
    }
}
