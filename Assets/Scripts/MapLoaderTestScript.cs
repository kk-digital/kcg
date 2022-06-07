using UnityEngine;
using System.Collections.Generic;
using TileProperties;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PlanetTileMap.Unity
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
        
        public PlanetTileMap TileMap;

        int SortingOrder = 0;

        int PlayerSpriteID;
        int PlayerSprite2ID;
        const float TileSize = 1.0f;

        Contexts EntitasContext = Contexts.sharedInstance;

        readonly Vector2 MapOffset = new(-3.0f, 4.0f);

        static bool InitTiles;


        ECSInput.ProcessSystem ProcessSystems;
        Agent.SpawnerSystem SpawnerSystem;
        Agent.MovableSystem MovableSystem;
        Agent.DrawSystem DrawSystem;
        Agent.CollisionSystem CollisionSystem;

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
            ProcessSystems = new ECSInput.ProcessSystem(EntitasContext);
            SpawnerSystem = new Agent.SpawnerSystem(EntitasContext);
            MovableSystem = new Agent.MovableSystem(EntitasContext);
            DrawSystem = new Agent.DrawSystem(EntitasContext);
            CollisionSystem = new Agent.CollisionSystem(EntitasContext);

            SpawnerSystem.SpawnPlayer(Material);
        }

        public void Update()
        {
            
        
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                Debug.Log(x + " " + y);
                TileMap.RemoveTile(x, y, Layer.Front);
                TileMap.BuildLayerTexture(Layer.Front);
                
            }

            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            ProcessSystems.Update();
            MovableSystem.Update();
            CollisionSystem.Update(TileMap);
            TileMap.DrawLayer(Layer.Front, Instantiate(Material), transform, 10);
            TileMap.DrawLayer(Layer.Ore, Instantiate(Material), transform, 11);
            DrawSystem.Draw(Instantiate(Material), transform, 12);
        }


        public void CreateDefaultTiles()
        {
            int MetalSlabsTileSheet = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Tiles_metal_slabs\\Tiles_metal_slabs.png");
            int StoneBulkheads = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tile_wallbase\\Tiles_stone_bulkheads.png");
            int TilesMoon = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png");
            int OreTileSheet = 
            GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png");


            GameState.TileCreationApi.CreateTile(8);
            GameState.TileCreationApi.SetTileName("ore_1");
            GameState.TileCreationApi.SetTileTexture16(OreTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(9);
            GameState.TileCreationApi.SetTileName("glass");
            GameState.TileCreationApi.SetTileSpriteSheet16(TilesMoon, 11, 10);
            GameState.TileCreationApi.EndTile();



            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);

            TileMap = new PlanetTileMap(mapSize);

            for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    PlanetTile frontTile = PlanetTile.EmptyTile();
                    PlanetTile oreTile = PlanetTile.EmptyTile();

                    frontTile.PropertiesId = 9;


                    if (i % 10 == 0)
                    {
                        //tile.FrontTilePropertiesId = 7;
                        oreTile.PropertiesId = 8;
                    }
                    if (j % 2 == 0)
                    {
                       // tile.FrontTilePropertiesId = 2;
                    }
                    if (j % 3 == 0)
                    {
                       // tile.FrontTilePropertiesId = 9;

                    }

                    if ((j > 1 && j < 6) || (j > (8 + i)))
                    {
                       frontTile.PropertiesId = -1; 
                       oreTile.PropertiesId = -1;
                    }

                    
                    TileMap.SetTile(i, j, frontTile, Layer.Front);
                    TileMap.SetTile(i, j, oreTile, Layer.Ore);
                }
            }

            TileMap.UpdateTopTilesMap();
            TileMap.UpdateAllTilePositions(Layer.Front);
            TileMap.UpdateAllTilePositions(Layer.Ore);
            TileMap.BuildLayerTexture(Layer.Front);
            TileMap.BuildLayerTexture(Layer.Ore);
        }
        
        

        public struct R
        {
            public float X;
            public float Y;
            public float W;
            public float H;

            public R(float x, float y, float w, float h)
            {
                X = x;
                Y = y;
                W = w;
                H = h;
            }
        }
        private static R CalcVisibleRect()
        {
            var cam = Camera.main;
            var pos = cam.transform.position;
            var height = 2f * cam.orthographicSize;
            var width = height * cam.aspect;
            var visibleRect = new R(pos.x - width / 2, pos.y - height / 2, width, height);
            return visibleRect;
        }

        private Texture2D CreateTextureFromRGBA(byte[] rgba, int w, int h)
        {

            var res = new Texture2D(w, h, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };

            var pixels = new Color32[w * h];
            for (int x = 0 ; x < w; x++)
            for (int y = 0 ; y < h; y++)
            { 
                int index = (x + y * w) * 4;
                var r = rgba[index];
                var g = rgba[index + 1];
                var b = rgba[index + 2];
                var a = rgba[index + 3];

                pixels[x + y * w] = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
            }
            res.SetPixels32(pixels);
            res.Apply();

            return res;
        }

    }
}