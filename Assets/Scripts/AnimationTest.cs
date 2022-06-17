using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;
using KMath;

namespace Planet.Unity
{
    class AnimationTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        Planet.PlanetState Planet;

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

            Planet.Update(Time.deltaTime, Material, transform);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            EntitasContext = Contexts.sharedInstance;



            int TilesMoon = 
                        Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png", 16, 16);
            int OreTileSheet = 
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);
            int GunSpriteSheet =
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\gun-temp.png", 44, 25);

            int RockSpriteSheet =
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png", 16, 16);
            int RockDustSpriteSheet =
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png", 16, 16);

            int DustSpriteSheet = 
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\particles\\dust1.png", 16, 16);
            int SlimeSpriteSheet = 
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\slime.png", 32, 32);

    
            int DustBaseSpriteId = Game.State.SpriteAtlasManager.CopySpriteToAtlas(DustSpriteSheet, 0, 0, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(DustSpriteSheet, 1, 0, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(DustSpriteSheet, 2, 0, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(DustSpriteSheet, 3, 0, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(DustSpriteSheet, 4, 0, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(DustSpriteSheet, 5, 0, Enums.AtlasType.Agent);

            int SlimeIdleBaseSpriteId = Game.State.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 0, 0, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 1, 0, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 2, 0, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 3, 0, Enums.AtlasType.Agent);

            int SlimeJumpBaseSpriteId = Game.State.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 0, 2, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 1, 2, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 0, 3, Enums.AtlasType.Agent);
            Game.State.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 1, 3, Enums.AtlasType.Agent);


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

            int particleAnimation = 0;
            int slimeIdle = 1;
            int slimeJump = 2;

            Game.State.AnimationManager.CreateAnimation(particleAnimation);
            Game.State.AnimationManager.SetName("particle");
            Game.State.AnimationManager.SetTimePerFrame(0.15f);
            Game.State.AnimationManager.SetBaseSpriteID(DustBaseSpriteId);
            Game.State.AnimationManager.SetFrameCount(6);
            Game.State.AnimationManager.EndAnimation();

            Game.State.AnimationManager.CreateAnimation(slimeIdle);
            Game.State.AnimationManager.SetName("slime-idle");
            Game.State.AnimationManager.SetTimePerFrame(0.35f);
            Game.State.AnimationManager.SetBaseSpriteID(SlimeIdleBaseSpriteId);
            Game.State.AnimationManager.SetFrameCount(4);
            Game.State.AnimationManager.EndAnimation();

            Game.State.AnimationManager.CreateAnimation(slimeJump);
            Game.State.AnimationManager.SetName("slime-jump");
            Game.State.AnimationManager.SetTimePerFrame(0.35f);
            Game.State.AnimationManager.SetBaseSpriteID(SlimeJumpBaseSpriteId);
            Game.State.AnimationManager.SetFrameCount(4);
            Game.State.AnimationManager.EndAnimation();



            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);
            Planet = new Planet.PlanetState(mapSize);
            GenerateMap();

            Planet.AddAgent(Instantiate(Material), 0, 16, 16, new Vec2f(6.0f, 3.0f), particleAnimation);
            Planet.AddAgent(Instantiate(Material), 0, 32, 32, new Vec2f(2.0f, 3.0f), slimeIdle);
            Planet.AddAgent(Instantiate(Material), 0, 32, 32, new Vec2f(4.0f, 3.0f), slimeJump);
        }




        void GenerateMap()
        {
            Planet.TileMap TileMap = Planet.TileMap;

            Vec2i mapSize = TileMap.MapSize;

           for(int j = 0; j < mapSize.Y; j++)
            {
                for(int i = 0; i < mapSize.X; i++)
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


