using System;
using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;
using Physics;
using Agent;

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

        int SortingOrder = 0;

        int PlayerSpriteID;
        int PlayerSprite2ID;

        static bool InitTiles;

        public PlanetState PlanetState;
        
        ECSInput.InputProcessSystem InputProcessSystems;
        Agent.AgentSpawnerSystem AgentSpawnerSystem;
        PhysicsMovableSystem PhysicsMovableSystem;
        Agent.MeshBuilderSystem AgentMeshBuilderSystem;
        PhysicsProcessCollisionSystem AgentProcessCollisionSystem;

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
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Characters\\Player\\character.png", 32, 48);

            int CharacterSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(CharacterSpriteSheet, 0, 0, Enums.AtlasType.Agent);


            InputProcessSystems = new ECSInput.InputProcessSystem();
            PhysicsMovableSystem = new Physics.PhysicsMovableSystem();
            AgentMeshBuilderSystem = new Agent.MeshBuilderSystem();
            AgentProcessCollisionSystem = new Physics.PhysicsProcessCollisionSystem();

            AgentMeshBuilderSystem.Initialize(Material, transform, 12);
            GameState.AgentSpawnerSystem.SpawnPlayer(Contexts.sharedInstance, CharacterSpriteId, 32, 48, new Vec2f(3.0f, 2.0f), 0, 0, 100, 100, 100, 100, 100, 0.2f);
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
                
                var xChunkIndex = x >> 4;
                var yChunkIndex = y * PlanetState.TileMap.MapSize.X;
                var chunkIndex = xChunkIndex + (yChunkIndex >> 4);
                
                var xTileIndex = x & 0x0f;
                var yTileIndex = y & 0x0f;
                var tileIndex = xTileIndex + (yTileIndex << 4);
                
                Debug.Log($"{x} {y} ChunkIndex: {chunkIndex} TileIndex: {tileIndex}");
            }
        
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                Debug.Log(x + " " + y);
                PlanetState.TileMap.RemoveTile(x, y, Enums.Tile.MapLayerType.Front);
                //TileMap.Layers.BuildLayerTexture(TileMap, Enums.Tile.MapLayerType.Front);
                
            }


            InputProcessSystems.Update(Contexts.sharedInstance);
            PhysicsMovableSystem.Update(Contexts.sharedInstance.agent);
            AgentProcessCollisionSystem.Update(Contexts.sharedInstance.agent, ref PlanetState.TileMap);

            AgentMeshBuilderSystem.UpdateMesh(Contexts.sharedInstance.agent);
            PlanetState.TileMap.UpdateLayerMesh(MapLayerType.Front);

            PlanetState.TileMap.DrawLayer(MapLayerType.Front);
            Utility.Render.DrawFrame(ref AgentMeshBuilderSystem.Mesh, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Agent));
        }

        public void CreateDefaultTiles()
        {
            int metalSlabsTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Blocks\\BuildingBlocks\\Metal\\Slabs\\Tiles_metal_slabs.png", 16, 16);
            int stoneBulkheads = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Blocks\\BuildingBlocks\\Stone\\Bulkheads\\Tiles_stone_bulkheads.png", 16, 16);
            int tilesMoon = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Terrains\\Tiles_Moon.png", 16, 16);
            int oreTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Gems\\Hexagon\\gem_hexagon_1.png", 16, 16);


            GameState.TileCreationApi.CreateTile(TileID.Ore1);
            GameState.TileCreationApi.SetTileName("ore_1");
            GameState.TileCreationApi.SetTileTexture16(oreTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(TileID.Glass);
            GameState.TileCreationApi.SetTileName("glass");
            GameState.TileCreationApi.SetTileSpriteSheet16(tilesMoon, 11, 10);
            GameState.TileCreationApi.EndTile();



            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);

            PlanetState = new PlanetState();
            PlanetState.Init(mapSize);
            ref var tileMap = ref PlanetState.TileMap;

            for(int j = 0; j < tileMap.MapSize.Y; j++)
            {
                for(int i = 0; i < tileMap.MapSize.X; i++)
                {
                    var frontTile = TileID.Glass;

                    if (j is > 1 and < 6 || (j > (8 + i)))
                    {
                       frontTile = TileID.Air;
                    }

                    PlanetState.TileMap.SetTile(i, j, frontTile, MapLayerType.Front);
                }
            }
            
            PlanetState.TileMap.UpdateTileMapPositions(MapLayerType.Front);
            PlanetState.TileMap.InitializeLayerMesh(Material, transform, 7);
        }
    }
}
