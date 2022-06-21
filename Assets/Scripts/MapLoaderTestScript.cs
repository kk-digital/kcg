using System;
using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;
using Physics;

#if UNITY_EDITOR
using KMath;
using UnityEditor;
#endif

namespace Planet.Unity
{
    //Note: TileMap should be mostly controlled by GameManager


    //Note(Mahdi): we are just testing and making sure everything is working
    // before we move things out of here
    // there will be things like rendering, collision, TileMap
    // that are not supposed to be here.

    class MapLoaderTestScript : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;

        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();
        
        public Planet.TileMap TileMap;

        int SortingOrder = 0;

        int PlayerSpriteID;
        int PlayerSprite2ID;
        const float TileSize = 1.0f;

        readonly Vector2 MapOffset = new(-3.0f, 4.0f);

        static bool InitTiles;


        ECSInput.InputProcessSystem InputProcessSystems;
        Agent.AgentSpawnerSystem AgentSpawnerSystem;
        Physics.PhysicsMovableSystem PhysicsMovableSystem;
        Agent.AgentDrawSystem AgentDrawSystem;
        Physics.PhysicsProcessCollisionSystem AgentProcessCollisionSystem;

        public void Start()
        {
            if (!InitTiles)
            {
                InitializeSystems();
                CreateDefaultTiles();

                InitTiles = true;
            }
        }   

        void InitializeSystems()
        {
            int CharacterSpriteSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png", 32, 48);

            int CharacterSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(CharacterSpriteSheet, 0, 0, Enums.AtlasType.Agent);


            InputProcessSystems = new ECSInput.InputProcessSystem();
            AgentSpawnerSystem = new Agent.AgentSpawnerSystem();
            PhysicsMovableSystem = new Physics.PhysicsMovableSystem();
            AgentDrawSystem = new Agent.AgentDrawSystem();
            AgentProcessCollisionSystem = new Physics.PhysicsProcessCollisionSystem();

            AgentSpawnerSystem.SpawnPlayer(Material, CharacterSpriteId, 32, 48, new Vec2f(3.0f, 2.0f), 0, 0);
        }

        public void Update()
        {
            // check if the sprite atlas textures needs to be updated
            for(int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
            {
                GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
            }

            // check if the tile sprite atlas textures needs to be updated
            for(int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
            {
                GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                
                var chunkIndex = TileMap.Chunks.GetChunkIndex(x, y);
                var tileIndex = Tile.Tile.GetTileIndex(x, y);
                
                Debug.Log($"{x} {y} ChunkIndex: {chunkIndex} TileIndex: {tileIndex}");
            }
        
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                Debug.Log(x + " " + y);
                TileMap.RemoveTile(x, y, Enums.Tile.MapLayerType.Front);
                //TileMap.Layers.BuildLayerTexture(TileMap, Enums.Tile.MapLayerType.Front);
                
            }

            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            InputProcessSystems.Update();
            PhysicsMovableSystem.Update();
            AgentProcessCollisionSystem.Update(TileMap);
            TileMap.Layers.DrawLayer(TileMap, Enums.Tile.MapLayerType.Front, Instantiate(Material), transform, 10);
            TileMap.Layers.DrawLayer(TileMap, Enums.Tile.MapLayerType.Ore, Instantiate(Material), transform, 11);
            AgentDrawSystem.Draw(Instantiate(Material), transform, 12);
        }


        public void CreateDefaultTiles()
        {
            int metalSlabsTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Tiles_metal_slabs\\Tiles_metal_slabs.png", 16, 16);
            int stoneBulkheads = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tile_wallbase\\Tiles_stone_bulkheads.png", 16, 16);
            int tilesMoon = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png", 16, 16);
            int oreTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);


            GameState.TileCreationApi.CreateTile(8);
            GameState.TileCreationApi.SetTileName("ore_1");
            GameState.TileCreationApi.SetTileTexture16(oreTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(9);
            GameState.TileCreationApi.SetTileName("glass");
            GameState.TileCreationApi.SetTileSpriteSheet16(tilesMoon, 11, 10);
            GameState.TileCreationApi.EndTile();



            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);

            TileMap = new TileMap(mapSize);

            for(int j = TileMap.Borders.IntBottom; j < TileMap.Borders.IntTop; j++)
            {
                for(int i = TileMap.Borders.IntLeft; i < TileMap.Borders.IntRight; i++)
                {
                    var frontTile = new Tile.Tile(new Vec2f(i, j));
                    var oreTile = new Tile.Tile(new Vec2f(i, j));

                    frontTile.Type = 9;


                    if (i % 10 == 0)
                    {
                        oreTile.Type = 8;
                    }

                    if (j is > 1 and < 6 || (j > (8 + i)))
                    {
                       frontTile.Type = -1; 
                       oreTile.Type = -1;
                    }

                    TileMap.SetTile(ref frontTile, MapLayerType.Front);
                    TileMap.SetTile(ref oreTile, MapLayerType.Ore);
                }
            }
            
            TileMap.UpdateTileMapPositions(MapLayerType.Front);
            TileMap.UpdateTileMapPositions(MapLayerType.Ore);
            //TileMap.Layers.BuildLayerTexture(TileMap, Enums.Tile.MapLayerType.Front);
            //TileMap.Layers.BuildLayerTexture(TileMap, Enums.Tile.MapLayerType.Ore);
        }
    }
}
