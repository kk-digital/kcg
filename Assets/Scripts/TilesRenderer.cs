using UnityEngine;
using System.Collections.Generic;
using Physics;
using TileProperties;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PlanetTileMap.Unity
{
    //Note: TileMap should be mostly controlled by GameManager
    class TilesRenderer : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;
       // MeshBuilder mb;
      //  Mesh mesh;

        // TmxImporter holds the temporary data for loading
        // and is used to make a 3 stage loading process
        //TmxMapFileLoader.TmxImporter FileLoader;

        //List<MeshBuilder> meshBuildersByLayers = new List<MeshBuilder>();

        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> verticies = new List<Vector3>();

        PlanetTileMap TileMap;
        RectangleBoundingBoxCollision Player;


        static bool InitTiles = false;
        

        public void Start()
        {
            if (!InitTiles)
            {
                CreateDefaultTiles();
                CreateTestPlayer();
                InitTiles = true;
            }
            // TODO(Mahdi): does not make sense to put them here
            // move them out ! 
            InitStage1();
            InitStage2();
        }

        //All memory allocations/setups go here
        //File loading should not occur at this stage
        public void InitStage1()
        {
            // todo: commented out the tmx stuff for now
            /*FileLoader = new TmxImporter(Path.Combine(BaseDir, TileMap));
            FileLoader.LoadStage1();*/
        }

        //Load settings from files and other init, that requires systems to be intialized
        public void InitStage2()
        {
            //remove all children MeshRenderer
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);
        

            DrawMapTest();
            PlayerCollidersTest();
            //TestDrawTiles();
            LateUpdate();
        }      

        //NOTE(Mahdi): this is used to create some test tiles
        // to make sure the system is working
        public void CreateDefaultTiles()
        {
            int MetalSlabsTileSheet = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Tiles_metal_slabs\\Tiles_metal_slabs.png");
            int StoneBulkheads = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tile_wallbase\\Tiles_stone_bulkheads.png");
            int TilesMoon = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png");


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

          /*  GameState.TileCreationApi.CreateTile(8);
            GameState.TileCreationApi.SetTileName("tile_moon_2");
            GameState.TileCreationApi.SetTileTexture16(TilesMoon, 7, 1);
            GameState.TileCreationApi.EndTile()*/;

            int mapWidth = 128;
            int mapHeight = 128;

            TileMap = new PlanetTileMap(mapWidth, mapHeight);

            for(int j = 0; j < mapHeight; j++)
            {
                for(int i = 0; i < mapWidth; i++)
                {
                    PlanetTile tile = PlanetTile.EmptyTile();
                    tile.TileIdPerLayer[0] = 0;
                    if (i % 10 == 0)
                    {
                        tile.TileIdPerLayer[0] = 7;
                    }
                    if (j % 2 == 0)
                    {
                        tile.TileIdPerLayer[0] = 2;
                    }
                    if (j % 3 == 0)
                    {
                        tile.TileIdPerLayer[0] = 1;
                    }

                    if ((j > 1 && j < 6) || j > 10)
                    {
                       tile.TileIdPerLayer[0] = -1; 
                    }

                    
                    TileMap.SetTile(i, j, tile);
                }
            }

        }

        void DrawTile(float x, float y, float w, float h, byte[] spriteBytes)
        {
            var tex = CreateTextureFromRGBA(spriteBytes, 32, 32);
            var mat = Instantiate(Material);
            mat.SetTexture("_MainTex", tex);
            var mesh = CreateMesh(transform, "abc", 0, mat);

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


        void CreateTestPlayer()
        {
            var pos = new Vector2(2, 2);

            // 1f - considered to be 32 pixel
            Player = new RectangleBoundingBoxCollision(pos, new Vector2(1f, 1f));
        }
        void PlayerCollidersTest()
        {
            Debug.Log($"Player Bottom Collided: {Player.IsCollidingBottom(ref TileMap, Player.Pos)}");
        }

        void DrawMapTest()
        {
            float BeginX = -3.0f;
            float BeginY = 4.0f;

            float TileSize = 0.2f;

            byte[] bytes = new byte[32 * 32* 4];

            for(int layerIndex = 0; layerIndex < 1; layerIndex++)
            {
                for(int j = 0; j < TileMap.Ysize; j++)
                {
                    for(int i = 0; i < TileMap.Xsize; i++)
                    {
                        ref PlanetTile tile = ref TileMap.getTile(i, j);
                        int tilePropertiesIndex = tile.TileIdPerLayer[layerIndex];

                        if (tilePropertiesIndex >= 0)
                        {
                            TilePropertiesData tileProperties =
                                GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);

                            GameState.SpriteAtlasManager.GetSpriteBytes(tileProperties.SpriteId, bytes);

                            var x = BeginX + (i * TileSize);
                            var y = BeginY + (j * TileSize);

                            DrawTile(x, y, TileSize, TileSize, bytes);
                        }

                    }
                }
            }
        }

        void TestDrawTiles()
        {
            float BeginX = -3.0f;
            float BeginY = 4.0f;
           // int id = TileSpriteLoader.TileSpriteLoader.Instance.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Tiles_metal_slabs\\Tiles_metal_slabs.png");
           // GameState.SpriteAtlasManager.Blit16(id, 0, 0);

           // byte[] spriteBytes = GameState.SpriteAtlasManager.GetSpriteBytes(0);


            // getting the tile properties
            TilePropertiesData slab1 =
                GameState.TileCreationApi.GetTileProperties(0);

            TilePropertiesData slab2 =
                GameState.TileCreationApi.GetTileProperties(1);

            TilePropertiesData slab3 =
                GameState.TileCreationApi.GetTileProperties(2);

            TilePropertiesData slab4 =
                GameState.TileCreationApi.GetTileProperties(3);

             TilePropertiesData tile5 =
                GameState.TileCreationApi.GetTileProperties(4);
            TilePropertiesData tile6 =
                GameState.TileCreationApi.GetTileProperties(5);
            TilePropertiesData tile7 =
                GameState.TileCreationApi.GetTileProperties(6);

            TilePropertiesData tileMoon1 =
             GameState.TileCreationApi.GetTileProperties(7);

            byte[] slab1Bytes = new byte[32 * 32 * 4];
            byte[] slab2Bytes = new byte[32 * 32 * 4];
            byte[] slab3Bytes = new byte[32 * 32 * 4];
            byte[] slab4Bytes = new byte[32 * 32 * 4];
            byte[] tile5Bytes = new byte[32 * 32 * 4];
            byte[] tile6Bytes = new byte[32 * 32 * 4];
            byte[] tile7Bytes = new byte[32 * 32 * 4];
            byte[] tileMoonBytes = new byte[32 * 32 * 4];

            GameState.SpriteAtlasManager.GetSpriteBytes(slab1.SpriteId, slab1Bytes);
            GameState.SpriteAtlasManager.GetSpriteBytes(slab2.SpriteId, slab2Bytes);
            GameState.SpriteAtlasManager.GetSpriteBytes(slab3.SpriteId, slab3Bytes);
            GameState.SpriteAtlasManager.GetSpriteBytes(slab4.SpriteId, slab4Bytes);
            GameState.SpriteAtlasManager.GetSpriteBytes(tile5.SpriteId, tile5Bytes);
            GameState.SpriteAtlasManager.GetSpriteBytes(tile6.SpriteId, tile6Bytes);
            GameState.SpriteAtlasManager.GetSpriteBytes(tile7.SpriteId, tile7Bytes);
            GameState.SpriteAtlasManager.GetSpriteBytes(tileMoon1.SpriteId, tileMoonBytes);
            
            float TileSize = 0.2f;


    

            DrawTile(BeginX + ((0 - 15) * TileSize), BeginY, TileSize, TileSize, slab2Bytes);
            for (int i = 1; i < 50; i++)
            {
                DrawTile(BeginX + ((i - 15) * TileSize), BeginY, TileSize, TileSize, slab3Bytes);
            }
            DrawTile(BeginX + ((50 - 15) * TileSize), BeginY, TileSize, TileSize, slab4Bytes);



            
            DrawTile(BeginX, BeginY + TileSize, TileSize, TileSize, tile6Bytes);
            for (int i = 1; i < 30; i++)
            {
                DrawTile(BeginX + i * TileSize, BeginY + TileSize, TileSize, TileSize, tile5Bytes);
            }
            DrawTile(BeginX + 30 * TileSize, BeginY + TileSize, TileSize, TileSize, tile7Bytes);



            for(int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    DrawTile((i - 50) * TileSize, (-j + 19) * TileSize, TileSize, TileSize, tileMoonBytes);
                }
            }


            for(int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    DrawTile((i - 50) * TileSize, (j + 29) * TileSize, TileSize, TileSize, tileMoonBytes);
                }
            }
        }

        public void LoadMap()
        {
            InitStage1();
            InitStage2();
        }


        private Mesh CreateMesh(Transform parent, string name, int sortingOrder, Material material)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent);

            var mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            var mf = go.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.sortingOrder = sortingOrder;

            return mesh;
        }

        private void LateUpdate()
        {
         /*   var visibleRect = CalcVisibleRect();

            //rebuild all layers for visible rect
            foreach (var mb in meshBuildersByLayers)
                mb.BuildMesh(visibleRect);*/
        }

        private static Rect CalcVisibleRect()
        {
            var cam = Camera.main;
            var pos = cam.transform.position;
            var height = 2f * cam.orthographicSize;
            var width = height * cam.aspect;
            var visibleRect = new Rect(pos.x - width / 2, pos.y - height / 2, width, height);
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

#if UNITY_EDITOR
    [CustomEditor(typeof(TilesRenderer))]
    public class TerrainGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (TilesRenderer)target;

            // Show default inspector property editor
            DrawDefaultInspector();

            if (GUILayout.Button("Load Map"))
            {
                myTarget.LoadMap();
            }

        }
    }
#endif
}
