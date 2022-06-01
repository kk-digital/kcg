using System.Collections.Generic;
using UnityEngine;
using Systems;
using Entitas;

public class VehicleTest : MonoBehaviour
{
    // Vehilce Draw System
    VehicleDrawSystem vehicleDrawSystem;

    // Entitas's Contexts
    Contexts contexts;

    // Rendering elements
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
    List<Vector3> verticies = new List<Vector3>();
    [SerializeField] Material Material;

    // Sprite to Render
    Texture2D vehicleSprite;
    GameObject prefab;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        // Assign Contexts
        contexts = Contexts.sharedInstance;

        // Initialize Vehicle Draw System
        vehicleDrawSystem = new VehicleDrawSystem(contexts, "Assets\\StreamingAssets\\assets\\luis\\vehicles\\Jet_chassis.png", 144, 96);

        // Loading Image
        vehicleDrawSystem.Initialize();

        // Loading Image
        InitImage(vehicleDrawSystem.GetSpriteID());
    }

    // Loading Vehicle Image

    private void InitImage(int spriteID)
    {
        int imageSpriteIndex = GameState.SpriteAtlasManager.Blit(spriteID, 0, 0);
        byte[] imageBytes = new byte[144 * 96 * 32];

        GameState.SpriteAtlasManager.GetSpriteBytes(imageSpriteIndex, imageBytes);

        vehicleSprite = CreateTextureFromRGBA(imageBytes, 144, 96);

        prefab = CreateParticlePrefab(0, 0, 0.5f, 0.5f, vehicleSprite);
    }


    // Rendering Vehicle Image

    private void Update()
    {
        IGroup<GameEntity> entities =
        contexts.game.GetGroup(GameMatcher.Particle2dPosition);
        foreach (var gameEntity in entities)
        {
            var pos = gameEntity.particle2dPosition;
            DrawSprite(pos.Position.x, pos.Position.y, 0.5f, 0.5f, vehicleSprite);
        }
    }
    private GameObject CreateParticlePrefab(float x, float y, float w, float h, Texture2D tex)
    {
        var mat = Instantiate(Material);
        mat.SetTexture("_MainTex", tex);
        var go = CreateObject(transform, "vehicle", 0, mat);
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
        var mat = Instantiate(Material);
        mat.SetTexture("_MainTex", tex);
        var go = CreateObject(transform, "vehicle", 0, mat);
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
