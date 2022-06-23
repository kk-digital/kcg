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
                //TileMap.BuildLayerTexture(MapLayerType.Front);
                
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                TileMap.RemoveTile(x, y, MapLayerType.Front);
                //TileMap.BuildLayerTexture(MapLayerType.Front);
                
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
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png", 16, 16);
            int OreTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);
            int GunSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\gun-temp.png", 44, 25);

            int RockSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png", 16, 16);
            int RockDustSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png", 16, 16);

            int DustSpriteSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\particles\\dust1.png", 16, 16);
            int SlimeSpriteSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\slime.png", 32, 32);

    
            int DustBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(DustSpriteSheet, 0, 0, 5, 0, Enums.AtlasType.Agent);

            int SlimeIdleBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(SlimeSpriteSheet, 0, 0, 3, 0, Enums.AtlasType.Agent);

            int SlimeJumpBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(SlimeSpriteSheet, 0, 2, 1, 3, Enums.AtlasType.Agent);


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

            int particleAnimation = 0;
            int slimeIdle = 1;
            int slimeJump = 2;

            GameState.AnimationManager.CreateAnimation(particleAnimation);
            GameState.AnimationManager.SetName("particle");
            GameState.AnimationManager.SetTimePerFrame(0.15f);
            GameState.AnimationManager.SetBaseSpriteID(DustBaseSpriteId);
            GameState.AnimationManager.SetFrameCount(6);
            GameState.AnimationManager.EndAnimation();

            GameState.AnimationManager.CreateAnimation(slimeIdle);
            GameState.AnimationManager.SetName("slime-idle");
            GameState.AnimationManager.SetTimePerFrame(0.35f);
            GameState.AnimationManager.SetBaseSpriteID(SlimeIdleBaseSpriteId);
            GameState.AnimationManager.SetFrameCount(4);
            GameState.AnimationManager.EndAnimation();

            GameState.AnimationManager.CreateAnimation(slimeJump);
            GameState.AnimationManager.SetName("slime-jump");
            GameState.AnimationManager.SetTimePerFrame(0.35f);
            GameState.AnimationManager.SetBaseSpriteID(SlimeJumpBaseSpriteId);
            GameState.AnimationManager.SetFrameCount(4);
            GameState.AnimationManager.EndAnimation();



            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);
            Planet = new Planet.PlanetState(mapSize, EntitasContext.game, EntitasContext.particle);
            GenerateMap();

            Planet.AddAgent(Instantiate(Material), 0, 16, 16, new Vec2f(6.0f, 3.0f), particleAnimation);
            Planet.AddAgent(Instantiate(Material), 0, 32, 32, new Vec2f(2.0f, 3.0f), slimeIdle);
            Planet.AddAgent(Instantiate(Material), 0, 32, 32, new Vec2f(4.0f, 3.0f), slimeJump);
            
        }




        void GenerateMap()
        {
            TileMap tileMap = Planet.TileMap;

            for (int j = tileMap.Borders.IntLeft; j < tileMap.Borders.IntTop; j++)
            {
                for (int i = tileMap.Borders.IntRight; i < tileMap.Borders.IntRight; i++)
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

        }

    }
}


