using Entitas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class VehicleDrawSystem : IInitializeSystem, IExecuteSystem
    {
        // Entitas Context
        public Contexts _contexts;

        // Streaming Asset File Path
        public string _filePath;
        
        // Image width
        public int _width;
        
        // Image Height
        public int _height;

        // Rendering elements
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> verticies = new List<Vector3>();
        Material Material;
        Transform _transform;

        // Sprite to Render
        Texture2D vehicleSprite;
        GameObject prefab;
        bool Init = false;

        // Constructor, variables setup
        public VehicleDrawSystem(Contexts contexts, string filePath, int width, int height, Transform transform, Material mat)
        {
            _contexts = contexts;
            _filePath = filePath;
            _width = width;
            _height = height;
            _transform = transform;
            Material = mat;
        }

        // Initializing image and component
        public void Initialize()
        {
            // Create Entity
            GameEntity vehicleDraw = _contexts.game.CreateEntity();

            // Get Image Sprite ID
            int _spriteID = GameState.SpriteLoader.GetSpriteSheetID(_filePath, _width, _height);

            // Blit
            int imageSpriteIndex = GameState.SpriteAtlasManager.Blit(_spriteID, 0, 0);
            // Calculating Bytes
            byte[] imageBytes = new byte[_width * _height * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(imageSpriteIndex, imageBytes);

            // Creating Texture
            vehicleSprite = CreateTextureFromRGBA(imageBytes, _width, _height);

            // Creating Prefab
            prefab = CreateParticlePrefab(0, 0, 0.5f, 0.5f, vehicleSprite);

            // Add Vehicle Component
            vehicleDraw.AddVehicleComponentDraw(_spriteID, GetWidth(), GetHeight());

            // Initialization done
            Init = true;
        }

        // Drawing in Exectue (Execute runs every frame)
        public void Execute()
        {
            if(Init)
            {
                IGroup<GameEntity> entities =
                _contexts.game.GetGroup(GameMatcher.Particle2dPosition);
                foreach (var gameEntity in entities)
                {
                    var pos = gameEntity.particle2dPosition;
                    DrawSprite(pos.Position.x, pos.Position.y, 0.5f, 0.5f, vehicleSprite);
                }
            }
        }

        // Get Contextx Object
        public Contexts GetContexts()
        {
            return _contexts;
        }

        // Get Width
        public int GetWidth()
        {
            return _width;
        }

        // Get Height
        public int GetHeight()
        {
            return _height;
        }

        private GameObject CreateParticlePrefab(float x, float y, float w, float h, Texture2D tex)
        {
            Material.SetTexture("_MainTex", tex);
            var go = CreateObject(_transform, "vehicle", 0, Material);
            var mf = go.GetComponent<MeshFilter>();
            var mesh = mf.sharedMesh;

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

            go.transform.position = new Vector3(0, 2.2f, 0);
            go.transform.localScale = new Vector3(5.84772444f, 3.20090008f, 3.20090008f);
            return go;
        }
        void DrawSprite(float x, float y, float w, float h, Texture2D tex)
        {
            Material.SetTexture("_MainTex", tex);
            var go = CreateObject(_transform, "vehicle", 0, Material);
            var mf = go.GetComponent<MeshFilter>();
            var mesh = mf.sharedMesh;

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
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
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
