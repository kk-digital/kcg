using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Enums;
using SpriteAtlas;
using TileProperties;
using TmxMapFileLoader;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PlanetTileMap.Unity
{
    //Note: TileMap should be mostly controlled by GameManager
    class TilesRenderer : MonoBehaviour
    {
        public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;
        MeshBuilder mb;
        Mesh mesh;

        // TmxImporter holds the temporary data for loading
        // and is used to make a 3 stage loading process
        //TmxMapFileLoader.TmxImporter FileLoader;

        List<MeshBuilder> meshBuildersByLayers = new List<MeshBuilder>();
        

        public void Start()
        {
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
            /*FileLoader.LoadStage2();
            FileLoader.LoadStage3();*/


            meshBuildersByLayers.Clear();
            var planetLayers = new [] { PlanetTileLayer.TileLayerBack, PlanetTileLayer.TileLayerMiddle, PlanetTileLayer.TileLayerFurniture, PlanetTileLayer.TileLayerFront };

            //remove all children MeshRenderer
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);
            
            var sortingOrder = 0;
            foreach (var layer in planetLayers)
            {
                //create material for atlas
                /*var atlas = FileLoader.GetAtlas(layer);

                if (atlas.Length == 0)
                    continue;
                var tex = TextureBuilder.Build(atlas);
                var mat = Instantiate(Material);
                mat.SetTexture("_MainTex", tex);
                var mesh = CreateMesh(transform, layer.ToString(), sortingOrder++, mat);
                var quads = new QuadsBuilder().BuildQuads(FileLoader, layer, 0f);
                mb = new MeshBuilder(quads, mesh);
                meshBuildersByLayers.Add(mb);*/
            }

            TestDrawTiles();
            LateUpdate();
        }        


        void DrawTile(float x, float y, byte[] spriteBytes)
        {
            var tex = CreateTextureFromRGBA(spriteBytes, 32, 32);
            var mat = Instantiate(Material);
            mat.SetTexture("_MainTex", tex);
            var mesh = CreateMesh(transform, "abc", 0, mat);

            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> verticies = new List<Vector3>();


            var p0 = new Vector3(x, y, 0);
            var p1 = new Vector3(x + 1, y + 1, 0);
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
        void TestDrawTiles()
        {
            float BeginX = -6.0f;
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

            //TODO(Mahdi): make these use the same statis array
            // the size should be 32 * 32 * 4 = 4096
            byte[] slab1Bytes = GameState.SpriteAtlasManager.GetSpriteBytes(slab1.SpriteId);
            byte[] slab2Bytes = GameState.SpriteAtlasManager.GetSpriteBytes(slab2.SpriteId);
            byte[] slab3Bytes = GameState.SpriteAtlasManager.GetSpriteBytes(slab3.SpriteId);
            byte[] slab4Bytes = GameState.SpriteAtlasManager.GetSpriteBytes(slab4.SpriteId);
            byte[] tile5Bytes = GameState.SpriteAtlasManager.GetSpriteBytes(tile5.SpriteId);
            byte[] tile6Bytes = GameState.SpriteAtlasManager.GetSpriteBytes(tile6.SpriteId);
            byte[] tile7Bytes = GameState.SpriteAtlasManager.GetSpriteBytes(tile7.SpriteId);
            

            DrawTile(BeginX, BeginY, slab2Bytes);
            for (int i = 1; i < 10; i++)
            {
                DrawTile(BeginX + i, BeginY, slab3Bytes);
            }
            DrawTile(BeginX + 10, BeginY, slab4Bytes);



            
            DrawTile(BeginX, BeginY + 1, tile6Bytes);
            for (int i = 1; i < 15; i++)
            {
                DrawTile(BeginX + i, BeginY + 1, tile5Bytes);
            }
            DrawTile(BeginX + 15, BeginY + 1, tile7Bytes);
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
            var visibleRect = CalcVisibleRect();

            //rebuild all layers for visible rect
            foreach (var mb in meshBuildersByLayers)
                mb.BuildMesh(visibleRect);
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
                var g = rgba[index];
                var b = rgba[index];
                var a = rgba[index];

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
