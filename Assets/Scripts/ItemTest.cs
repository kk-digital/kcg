using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;
using KMath;

namespace Planet.Unity
{
    class ItemTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        Contexts EntitasContext;
        Planet.PlanetState Planet;
        Item.SpawnerSystem SpawnerSystem;
        Item.DrawSystem DrawSystem; 

        static bool Init = false;

        public void Start()
        {
            EntitasContext = Contexts.sharedInstance;
            SpawnerSystem = new Item.SpawnerSystem(EntitasContext);
            DrawSystem = new Item.DrawSystem(EntitasContext);

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

            //foreach (var mr in GetComponentsInChildren<MeshRenderer>())

            Planet.Update(Time.deltaTime, Material, transform);
            DrawSystem.Draw(Material, transform, 14);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            int TilesMoon =
                        Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png", 16, 16);
            int OreTileSheet =
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);

            int CharacterSpriteSheet = 
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png", 32, 48);

            int CharacterSpriteId = Game.State.SpriteAtlasManager.CopySpriteToAtlas(CharacterSpriteSheet, 0, 0, Enums.AtlasType.Agent);

            Game.State.TileCreationApi.CreateTile(8);
            Game.State.TileCreationApi.SetTileName("ore_1");
            Game.State.TileCreationApi.SetTileTexture16(OreTileSheet, 0, 0);
            Game.State.TileCreationApi.EndTile();

            Game.State.TileCreationApi.CreateTile(9);
            Game.State.TileCreationApi.SetTileName("glass");
            Game.State.TileCreationApi.SetTileSpriteSheet16(TilesMoon, 11, 10);
            Game.State.TileCreationApi.EndTile();

            Game.State.TileCreationApi.CreateTile(10);
            Game.State.TileCreationApi.SetTileName("moon");
            Game.State.TileCreationApi.SetTileSpriteSheet16(TilesMoon, 0, 0);
            Game.State.TileCreationApi.EndTile();

            int GunSpriteSheet =
                Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\gun-temp.png", 44, 25);
            int OreSpriteSheet =
                Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);

            Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "Gun");
            Item.CreationApi.Instance.SetTexture(GunSpriteSheet);
            Item.CreationApi.Instance.SetInventoryTexture(GunSpriteSheet);
            Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
            Item.CreationApi.Instance.EndItem();

            Item.CreationApi.Instance.CreateItem(Enums.ItemType.Ore, "Ore");
            Item.CreationApi.Instance.SetTexture(OreSpriteSheet);
            Item.CreationApi.Instance.SetInventoryTexture(OreSpriteSheet);
            Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
            Item.CreationApi.Instance.SetStackable(99);
            Item.CreationApi.Instance.EndItem();

            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);
            Planet = new Planet.PlanetState(mapSize);
            GenerateMap();

            Planet.AddPlayer(Instantiate(Material),CharacterSpriteId, 32, 48, new Vec2f(3.0f, 3.0f), 0);

            SpawnerSystem.SpawnItem(Enums.ItemType.Gun, new Vec2f(3.0f, 3.0f));
            SpawnerSystem.SpawnItem(Enums.ItemType.Ore, new Vec2f(6.0f, 3.0f));
            SpawnerSystem.SpawnIventoryItem(Enums.ItemType.Ore);
        }
        void GenerateMap()
        {
            Planet.TileMap TileMap = Planet.TileMap;

            var mapSize = TileMap.MapSize;

            for (int j = 0; j < mapSize.Y; j++)
            {
                for (int i = 0; i < mapSize.X; i++)
                {
                    Tile.Tile frontTile = Tile.Tile.EmptyTile;
                    Tile.Tile oreTile = Tile.Tile.EmptyTile;

                    if (i >= mapSize.X / 2)
                    {
                        if (j % 2 == 0 && i == mapSize.X / 2)
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
                        if (j % 3 == 0 && i == mapSize.X / 2 + 1)
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
