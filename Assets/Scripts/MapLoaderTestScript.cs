using UnityEngine;
using System.Collections.Generic;
using Agent;
using TileProperties;

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
        
        PlanetTileMap TileMap;

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

            //remove all children MeshRenderer
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            //TODO: Move DrawMapTest to DrawMap()
            DrawMapTest();
        }   

        public void Update()
        {

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



            // Generating the map
            Vector2Int mapSize = new Vector2Int(128, 128);

            TileMap = new PlanetTileMap(mapSize);

            for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    PlanetTile tile = PlanetTile.EmptyTile();
                    tile.BackTilePropertiesId = 0;
                    tile.FrontTilePropertiesId = -1;

                    if (i % 10 == 0)
                    {
                        tile.BackTilePropertiesId = 7;
                    }
                    if (j % 2 == 0)
                    {
                        tile.BackTilePropertiesId = 2;
                    }
                    if (j % 3 == 0)
                    {
                        tile.BackTilePropertiesId = 1;
                    }

                    if ((j > 1 && j < 6) || (j > (8 + i)))
                    {
                       tile.BackTilePropertiesId = -1; 
                    }

                    
                    TileMap.SetTile(i, j, tile);
                }
            }

            TileMap.UpdateTopTilesMap();
        }

        void DrawMapTest()
        {

            int PipeTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\sprite\\item\\admin_icon_pipesim.png",
            16, 16);

            int PipeSpriteIndex = GameState.SpriteAtlasManager.CopySpriteToAtlas(PipeTileSheet, 0, 0, SpriteAtlas.AtlasType.Particle);
            byte[] pipeBytes = new byte[16 * 16 * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(PipeSpriteIndex, pipeBytes, SpriteAtlas.AtlasType.Particle);

            byte[] bytes = new byte[32 * 32* 4];

            for(int layerIndex = 0; layerIndex < 1; layerIndex++)
            {
                for(int j = 0; j < TileMap.Size.y; j++)
                {
                    for(int i = 0; i < TileMap.Size.x; i++)
                    {
                        ref PlanetTile tile = ref TileMap.getTile(i, j);
                        int tilePropertiesIndex = tile.BackTilePropertiesId;

                        if (tilePropertiesIndex >= 0)
                        {
                            TilePropertiesData tileProperties =
                                GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);

                            GameState.TileSpriteAtlasManager.GetSpriteBytes(tileProperties.SpriteId, bytes);

                            var x = (i * TileSize);
                            var y = (j * TileSize);
                            DrawTile(x, y, TileSize, TileSize, bytes);
                        }

                        tilePropertiesIndex = tile.FrontTilePropertiesId;
                        if (tilePropertiesIndex >= 0)
                        {
                            TilePropertiesData tileProperties =
                                GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);

                            GameState.TileSpriteAtlasManager.GetSpriteBytes(tileProperties.SpriteId, bytes);

                            var x = (i * TileSize);
                            var y = (j * TileSize);
                            DrawTile(x, y, TileSize, TileSize, bytes);
                        }
                    }
                }
            }

            for(int i = 0; i < TileMap.Size.x; i++)
            {
                int topTileIndex = TileMap.TopTilesMap.Data[i];

                var x = (i * TileSize);
                var y = (topTileIndex * TileSize);

                DrawSprite(x + 0.25f, y + 0.25f, 0.5f, 0.5f, pipeBytes, 16, 16);
            }
        }
        
        

         // draws 1 tile into the screen
        // Note(Mahdi): this code is for testing purpose
        void DrawSprite(float x, float y, float w, float h, byte[] spriteBytes,
             int spriteW, int spriteH)
        {
            var tex = CreateTextureFromRGBA(spriteBytes, spriteW, spriteH);
            var mat = Instantiate(Material);
            mat.SetTexture("_MainTex", tex);
            var mesh = CreateMesh(transform, "abc", SortingOrder++, mat);

            triangles.Clear();
            uvs.Clear();
            verticies.Clear();


            var p0 = new Vector3(x, y, 0);
            var p1 = new Vector3((x + w), (y + h), 0);
            var p2 = p0; p2.y = p1.y;
            var p3 = p1; p3.y = p0.y;

            verticies.Add(p0);
            verticies.Add(p1);
            verticies.Add(p2);
            verticies.Add(p3);

            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(1);
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(3);

            var u0 = 0;
            var u1 = 1;
            var v1 = -1;
            var v0 = 0;

            var uv0 = new Vector2(u0, v0);
            var uv1 = new Vector2(u1, v1);
            var uv2 = uv0; uv2.y = uv1.y;
            var uv3 = uv1; uv3.y = uv0.y;


            uvs.Add(uv0);
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
    

            mesh.SetVertices(verticies);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
        }
        
        void DrawTile(float x, float y, float w, float h, byte[] spriteBytes)
        {
            DrawSprite(x, y, w, h, spriteBytes, 32, 32);
        }
        
        private Mesh CreateMesh(Transform parent, string name, int sortingOrder, Material material)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent);

            var mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            var mf = go.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.sortingOrder = sortingOrder;

            return mesh;
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