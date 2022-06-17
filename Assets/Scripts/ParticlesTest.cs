using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Planet.Unity
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

        Texture2D PipeSprite;
        Texture2D OreSprite;

        Texture2D VentSprite;
        
        Particle.UpdateSystem ParticleUpdateSystem;
        Particle.EmitterUpdateSystem ParticleEmitterUpdateSystem;

        GameObject PipePrefab;
        GameObject OrePrefab;
        GameObject VentPrefab;

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
            ParticleUpdateSystem.Execute();
            ParticleEmitterUpdateSystem.Execute();
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            
            ParticleUpdateSystem = new Particle.UpdateSystem();
            ParticleEmitterUpdateSystem = new Particle.EmitterUpdateSystem();

            // we load the sprite sheets here
            int pipeTileSheet = 
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\sprite\\item\\admin_icon_pipesim.png", 16, 16);

            int oreTileSheet = 
            Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);

            int ventTileSheet = Game.State.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Objects\\vent1.png", 16, 16);


            int pipeSpriteIndex = Game.State.SpriteAtlasManager.CopySpriteToAtlas(pipeTileSheet, 0, 0, Enums.AtlasType.Particle);
            byte[] pipeBytes = new byte[16 * 16 * 4];
            Game.State.SpriteAtlasManager.GetSpriteBytes(pipeSpriteIndex, pipeBytes, Enums.AtlasType.Particle);

            int oreSpriteIndex = Game.State.SpriteAtlasManager.CopySpriteToAtlas(oreTileSheet, 0, 0, Enums.AtlasType.Particle);
            byte[] oreBytes = new byte[16 * 16 * 4];
            Game.State.SpriteAtlasManager.GetSpriteBytes(oreSpriteIndex, oreBytes, Enums.AtlasType.Particle);

            int ventSpriteIndex = Game.State.SpriteAtlasManager.CopySpriteToAtlas(ventTileSheet, 0, 0, Enums.AtlasType.Particle);
            byte[] ventBytes = new byte[16 * 16 * 4];
            Game.State.SpriteAtlasManager.GetSpriteBytes(ventSpriteIndex, ventBytes, Enums.AtlasType.Particle);

            PipeSprite = CreateTextureFromRGBA(pipeBytes, 16, 16);
            OreSprite = CreateTextureFromRGBA(oreBytes, 16, 16);
            VentSprite = CreateTextureFromRGBA(ventBytes, 16, 16);

            PipePrefab = CreateParticlePrefab(0, 0, 0.5f, 0.5f, PipeSprite);
            OrePrefab = CreateParticlePrefab(0, 0, 0.5f, 0.5f, OreSprite);
            VentPrefab = CreateParticlePrefab(0, 0, 0.5f, 0.5f, VentSprite);


            CreateParticleEmitterEntity(VentPrefab, new Vector2(-4.0f, 0),
             1.0f, new Vector2(0, -20.0f), 1.7f, 0.0f, new int[]{0}, new Vector2(1.0f, 10.0f),
            0.0f, 1.0f, new Color(255.0f, 255.0f, 255.0f, 255.0f),
            0.2f, 3.0f, true, 1, 0.05f, PipePrefab);

            CreateParticleEmitterEntity(VentPrefab, new Vector2(2.0f, 0),
             1.0f, new Vector2(0, -20.0f), 3.5f, 0.0f, new int[]{0}, new Vector2(1.0f, 10.0f),
            0.0f, 1.0f, new Color(255.0f, 255.0f, 255.0f, 255.0f),
            0.2f, 3.0f, true, 20, 0.5f, OrePrefab);
            
        }

        private GameObject CreateParticlePrefab(float x, float y, float w, float h, Texture2D tex)
        {
            var mat = Instantiate(Material);
            mat.SetTexture("_MainTex", tex);
            var go = CreateObject(transform, "particle", 0, mat);
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

        private void CreateParticleEmitterEntity(GameObject emitterPrefab, Vector2 position, float decayRate,
            Vector2 acceleration, float deltaRotation, float deltaScale,
            int[] spriteIds, Vector2 startingVelocity,
            float startingRotation, float startingScale, Color startingColor,
            float animationSpeed, float duration, bool loop, int particleCount, 
            float timeBetweenEmissions, GameObject prefab)
        {
            var e = Contexts.sharedInstance.game.CreateEntity();
            var gameObject = UnityEngine.Object.Instantiate(emitterPrefab);
            gameObject.transform.position = new Vector3(position.x, position.y, 0.0f);
                
            e.AddParticleEmitter2dPosition(position, new Vector2(), new Vector2());
            e.AddParticleEmitterState(gameObject, prefab, decayRate, acceleration, deltaRotation,
            deltaScale, spriteIds, startingVelocity, startingRotation, startingScale, startingColor,
            animationSpeed, duration, loop, particleCount, timeBetweenEmissions, 0.0f);
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

