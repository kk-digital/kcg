using System;
using UnityEngine;
using System.Collections.Generic;
using Entity;
using Components;
using Agent;
using Entitas;


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

    class ParticlesTest : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;

        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> verticies = new List<Vector3>();

        ParticleEmitterEntity Emitter = new ParticleEmitterEntity();
        ParticleEmitterEntity Emitter2 = new ParticleEmitterEntity();
        Texture2D PipeSprite;
        Texture2D OreSprite;

        Contexts EntitasContext = Contexts.sharedInstance;
        ParticleUpdateSystem ParticleUpdateSystem;

        GameObject prefab;
        GameObject orePrefab;

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
            //remove all children MeshRenderer
            /*foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);*/

            Emitter.Update(EntitasContext, prefab);
            Emitter2.Update(EntitasContext, orePrefab);
            ParticleUpdateSystem.Execute();
           // DrawParticles();
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            
            ParticleUpdateSystem = new ParticleUpdateSystem(EntitasContext);

            Emitter = new ParticleEmitterEntity(new Vector2(-4.0f, 0),
             1.0f, new Vector2(0, -20.0f), 1.7f, 0.0f, new int[]{0}, new Vector2(1.0f, 10.0f),
            0.0f, 1.0f, new Color(255.0f, 255.0f, 255.0f, 255.0f),
            0.2f, 3.0f, true, 1, 0.05f);

            Emitter2 = new ParticleEmitterEntity(new Vector2(2.0f, 0),
             1.0f, new Vector2(0, -20.0f), 3.5f, 0.0f, new int[]{0}, new Vector2(1.0f, 10.0f),
            0.0f, 1.0f, new Color(255.0f, 255.0f, 255.0f, 255.0f),
            0.2f, 3.0f, true, 20, 0.5f);

            // we load the sprite sheets here
            int SomeObjectTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Objects\\algaeTank1.png",
                         32, 64);
            int PlayerTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png",
                         32, 48);
            int PipeTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\sprite\\item\\admin_icon_pipesim.png",
            16, 16);

            int OreTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png",
            16, 16);


            // bit the sprites into the sprite atlas
            // we can blit the same sprite
            // but its only for testing purpose
            // we should remove that in the future
            GameState.SpriteAtlasManager.Blit(SomeObjectTileSheet, 0, 0);
            GameState.SpriteAtlasManager.Blit(PlayerTileSheet, 0, 0);;
            GameState.SpriteAtlasManager.Blit(PlayerTileSheet, 0, 0);
            GameState.SpriteAtlasManager.Blit(SomeObjectTileSheet, 0, 0);
            GameState.SpriteAtlasManager.Blit(PlayerTileSheet, 0, 0);
            GameState.SpriteAtlasManager.Blit(PlayerTileSheet, 0, 0);

            int PipeSpriteIndex = GameState.SpriteAtlasManager.Blit(PipeTileSheet, 0, 0);
            byte[] pipeBytes = new byte[16 * 16 * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(PipeSpriteIndex, pipeBytes);

            int OreSpriteIndex = GameState.SpriteAtlasManager.Blit(OreTileSheet, 0, 0);
            byte[] oreBytes = new byte[16 * 16 * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(OreSpriteIndex, oreBytes);

            PipeSprite = CreateTextureFromRGBA(pipeBytes, 16, 16);
            OreSprite = CreateTextureFromRGBA(oreBytes, 16, 16);

            prefab = CreateParticlePrefab(0, 0, 0.5f, 0.5f, PipeSprite);
            orePrefab = CreateParticlePrefab(0, 0, 0.5f, 0.5f, OreSprite);
            
        }

        void DrawParticles()
        {
            IGroup<GameEntity> entities = 
            EntitasContext.game.GetGroup(GameMatcher.Particle2dPosition);
            foreach (var gameEntity in entities)
            {
                var pos = gameEntity.particle2dPosition;
                DrawSprite(pos.Position.x, pos.Position.y, 0.5f, 0.5f, PipeSprite);
            }
        }


         // draws 1 sprite into the screen
        // Note(Mahdi): this code is for testing purpose
        void DrawSprite(float x, float y, float w, float h, Texture2D tex)
        {
            var mat = Instantiate(Material);
            mat.SetTexture("_MainTex", tex);
            var go = CreateObject(transform, "abc", 0, mat);
            var mf = go.GetComponent<MeshFilter>();
            var mesh =  mf.sharedMesh;

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

        private GameObject CreateParticlePrefab(float x, float y, float w, float h, Texture2D tex)
        {
            var mat = Instantiate(Material);
            mat.SetTexture("_MainTex", tex);
            var go = CreateObject(transform, "abc", 0, mat);
            var mf = go.GetComponent<MeshFilter>();
            var mesh =  mf.sharedMesh;

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

            go.transform.position = new Vector3(99999, 99999, 99999);
            return go;
        }


        private GameObject CreateObject(Transform parent, string name, int sortingOrder, Material material)
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

            return go;
        }

      /*   void CreateSlots(int InventoryID)
        {
            // To do: Change size of grid to match width and height of inventory.
            GameEntity entity = context.game.GetEntityWithAgent2dInventory(InventoryID);

            int size = entity.agent2dInventory.Width * entity.agent2dInventory.Height;
            for (int i = 0; i < size; i++)
            {
                GameObject obj = new GameObject("slot" + i.ToString(), typeof(RectTransform));
                obj.transform.parent = ParentObject.transform;
            }
        }

        void CreateInventoryEntity(int InventoryID)
        {
            GameEntity entity = context.game.CreateEntity();
            const int height = 8;
            const int width = 8;
            const int selectedSlot = 0;

            BitArray slots = new BitArray(height * width, false);
            entity.AddAgent2dInventory(InventoryID, width, height, selectedSlot, slots);
        }
*/
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

