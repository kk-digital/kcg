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

        readonly Vector2 MapOffset = new(-3.0f, 4.0f);

        static bool InitTiles;

        public void Start()
        {
            if (!InitTiles)
            {
                CreateDefaultTiles();
                
                InitTiles = true;
            }
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

            TileMap.DrawLayer(Layer.Front, Instantiate(Material), transform, 10);
            TileMap.DrawLayer(Layer.Ore, Instantiate(Material), transform, 11);
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

            // creating the tiles
            GameState.TileCreationApi.CreateTile(0);
            GameState.TileCreationApi.SetTileName("slab1");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(1);
            GameState.TileCreationApi.SetTileName("slab2");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 1, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(2);
            GameState.TileCreationApi.SetTileName("slab3");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 4, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(3);
            GameState.TileCreationApi.SetTileName("slab4");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 5, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(4);
            GameState.TileCreationApi.SetTileName("tile5");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 5, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(5);
            GameState.TileCreationApi.SetTileName("tile6");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 4, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(6);
            GameState.TileCreationApi.SetTileName("tile7");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 7, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(7);
            GameState.TileCreationApi.SetTileName("tile_moon_1");
            GameState.TileCreationApi.SetTileTexture(TilesMoon, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(8);
            GameState.TileCreationApi.SetTileName("ore_1");
            for(int i = 0; i < Enum.GetNames(typeof(TileVariant.Variant)).Length; i++)
            {
                GameState.TileCreationApi.SetTileVariant16(OreTileSheet, 0, 0, (TileVariant.Variant)i);
            }
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(9);
            GameState.TileCreationApi.SetTileName("glass");
            GameState.TileCreationApi.SetTileTexture16(TilesMoon, 12, 11);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 12, 11, TileVariant.Variant.Middle);

            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 13, 11, TileVariant.Variant.Right);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 11, 11, TileVariant.Variant.Left);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 12, 10, TileVariant.Variant.Top);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 12, 12, TileVariant.Variant.Bottom);
            
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 16, 12, TileVariant.Variant.InnerTopRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 17, 12, TileVariant.Variant.InnerTopLeft);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 16, 11, TileVariant.Variant.InnerBottomRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 17, 11, TileVariant.Variant.InnerBottomLeft);


            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 13, 10, TileVariant.Variant.OuterTopRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 11, 10, TileVariant.Variant.OuterTopLeft);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 13, 12, TileVariant.Variant.OuterBottomRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 11, 12, TileVariant.Variant.OuterBottomLeft);

            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 13, 13, TileVariant.Variant.TipRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 11, 13, TileVariant.Variant.TipLeft);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 14, 10, TileVariant.Variant.TipTop);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 14, 12, TileVariant.Variant.TipBottom);

            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 14, 11, TileVariant.Variant.TipVertical);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 12, 13, TileVariant.Variant.TipHorizontal);

            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 14, 13, TileVariant.Variant.Default);

            GameState.TileCreationApi.EndTile();



            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);

            TileMap = new PlanetTileMap(mapSize);

            for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    PlanetTile tile = PlanetTile.EmptyTile();
                    tile.FrontTilePropertiesId = 9;


                    if (i % 10 == 0)
                    {
                        //tile.FrontTilePropertiesId = 7;
                        tile.OreTilePropertiesId = 8;
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
                       tile.FrontTilePropertiesId = -1; 
                       tile.OreTilePropertiesId = -1;
                    }

                    
                    TileMap.SetTile(i, j, tile);
                }
            }

            TileMap.UpdateTopTilesMap();
            TileMap.UpdateAllTileVariants(Layer.Front);
            TileMap.UpdateAllTileVariants(Layer.Ore);
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