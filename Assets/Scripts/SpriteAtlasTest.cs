using System;
using UnityEngine;
using System.Collections.Generic;
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

    class SpriteAtlasTest : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;

        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> verticies = new List<Vector3>();

        Vector2 MapOffset = new Vector2(-3.0f, 4.0f);

        static bool InitTiles = false;
        

        public void Start()
        {
            if (!InitTiles)
            {
                LoadSprites();
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

            LateUpdate();
        }      

        public void Update()
        {
                       //remove all children MeshRenderer
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            DrawSpriteAtlas();
            DrawSprite(2, 1, 1.0f, 2.0f, 3);
            DrawSprite(2, -1, 1.0f, 1.5f, 2);
        }

        // create the sprite atlas for testing purposes
        public void LoadSprites()
        {
            // we load the sprite sheets here
            int SomeObjectTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Objects\\algaeTank1.png",
                         32, 64);
            int PlayerTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png",
                         32, 48);


            // bit the sprites into the sprite atlas
            // we can blit the same sprite
            // but its only for testing purpose
            // we should remove that in the future
            GameState.SpriteAtlasManager.CopySpriteToAtlas(SomeObjectTileSheet, 0, 0, SpriteAtlas.AtlasType.Generic);
            GameState.SpriteAtlasManager.CopySpriteToAtlas(PlayerTileSheet, 0, 0, SpriteAtlas.AtlasType.Generic);;
            GameState.SpriteAtlasManager.CopySpriteToAtlas(PlayerTileSheet, 0, 0, SpriteAtlas.AtlasType.Generic);
            GameState.SpriteAtlasManager.CopySpriteToAtlas(SomeObjectTileSheet, 0, 0, SpriteAtlas.AtlasType.Generic);
            GameState.SpriteAtlasManager.CopySpriteToAtlas(PlayerTileSheet, 0, 0, SpriteAtlas.AtlasType.Generic);
            GameState.SpriteAtlasManager.CopySpriteToAtlas(PlayerTileSheet, 0, 0, SpriteAtlas.AtlasType.Generic);
        }

        // drawing the sprite atlas
        void DrawSpriteAtlas()
        {
            ref SpriteAtlas.SpriteAtlas atlas = ref GameState.SpriteAtlasManager.GetSpriteAtlas(SpriteAtlas.AtlasType.Generic);
            DrawSprite(-3, -1, 
                  atlas.Width / 32.0f, atlas.Height / 32.0f,
                 atlas.Texture, atlas.Width, atlas.Height);
        }

        void DrawSprite(float x, float y, float w, float h, int spriteId)
        {
            SpriteAtlas.Sprite sprite = 
                GameState.SpriteAtlasManager.GetSprite(spriteId, SpriteAtlas.AtlasType.Generic);

            DrawSprite(x, y, w, h, sprite);
        }


         // draws 1 sprite into the screen
        // Note(Mahdi): this code is for testing purpose
        void DrawSprite(float x, float y, float w, float h, Texture2D texture,
             int spriteW, int spriteH)
        {
            var tex = texture;
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
            var u1 = 1.0f;
            var v1 = -1.0f;
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



        void DrawSprite(float x, float y, float w, float h, SpriteAtlas.Sprite sprite)
        {
            var tex = sprite.Texture;
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

            var uv0 = new Vector2(sprite.TextureCoords.x, sprite.TextureCoords.y + sprite.TextureCoords.w);
            var uv1 = new Vector2(sprite.TextureCoords.x + sprite.TextureCoords.z, sprite.TextureCoords.y);
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

        private void LateUpdate()
        {
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

        // we use this helper function to generate a unity Texture2D
        // from pixels
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

