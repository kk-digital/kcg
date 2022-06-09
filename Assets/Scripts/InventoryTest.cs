using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTest : MonoBehaviour
{
    GameObject parentObject;
    Contexts context;
    SpriteLoaderTest SpriteLoader = new();
    List<Sprite> Sprites = new();
    Inventory.ManagerSystem inventoryManagerSystem;

    [SerializeField] Material material;

    List<int> triangles = new();
    List<Vector2> uvs = new();
    List<Vector3> verticies = new();

    public void Start()
    {
        context = Contexts.sharedInstance;
        inventoryManagerSystem = new Inventory.ManagerSystem(context);
        parentObject = GameObject.Find("Canvas/InventoryView/Viewport/Content");
        LoadSprites();

        // We load the sprite sheets here
        //int RockSpriteID =
        //            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png",
        //             30, 30);
        //int RockDustSpriteID = 
        //    GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png",
        //             30, 30);
        //int GunSpriteID =
        //    GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png",
        //             30, 30);

        LoadSprites();
        int GunSpriteID = 2;
        int RockSpriteID = 0;
        int RockDustSpriteID = 1;


        // Create inventory.
        const int inventoryID = 0;
        CreateInventoryEntity(inventoryID);
        CreateSlots(inventoryID);

        // Test not stackable items.
        for (uint i = 0; i < 10; i++)
        {
            CreateItemsEntity("Gun", inventoryID, Enums.ItemType.Gun, GunSpriteID);
        }

        // Testing stackable items.
        for (uint i = 0; i < 1000; i++)
        {
            CreateItemsEntity("Rock", inventoryID, Enums.ItemType.Rock, RockSpriteID);
            CreateItemsEntity("RockDust", inventoryID, Enums.ItemType.RockDust, RockDustSpriteID);
        }

        CreateObjects(inventoryID);
    }

    public void Update()
    {
        // in here we will draw everything to the screen


        // remove all children MeshRenderer
        //foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        //    if (Application.isPlaying)
        //        Destroy(mr.gameObject);
        //    else
        //        DestroyImmediate(mr.gameObject);
        //
        //
        //DrawItemsIcons();
    }

    // TODO: Use GetSpriteSheetID instead.
    void LoadSprites()
    {
        // We load the sprite sheets here.
        // Load Sprite.
        Sprites.Add(SpriteLoader.LoadNewSprite("Assets\\StreamingAssets\\assets\\item\\rock1.png"));
        Sprites.Add(SpriteLoader.LoadNewSprite("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png"));
        Sprites.Add(SpriteLoader.LoadNewSprite("Assets\\StreamingAssets\\assets\\item\\lasergun-temp.png"));
    }
    
    void CreateSlots(int inventoryID)
    {
        // To do: Change size of grid to match width and height of inventory.
        var entity = context.game.GetEntityWithInventoryID(inventoryID);

        int size = entity.inventorySize.Width * entity.inventorySize.Height;
        for (int i = 0; i < size; i++)
        {
            var obj = new GameObject("slot" + i, typeof(RectTransform));
            obj.transform.parent = parentObject.transform;
        }
    }

    void CreateInventoryEntity(int inventoryID)
    {
        var entity = context.game.CreateEntity();
        const int height = 8;
        const int width = 8;
        const int selectedSlot = 0;

        BitArray slots = new BitArray(height * width, false);

        entity.isInventory = true;
        entity.AddInventoryID(inventoryID);
        entity.AddInventorySize(width, height);
        entity.AddInventorySlots(slots, selectedSlot);
    }

    void CreateItemsEntity(string label, int inventoryID, Enums.ItemType itemType, int spriteID)
    {
        GameEntity entity = context.game.CreateEntity();
        const int maxStackCount = 99;
        switch (itemType)
        {
            case Enums.ItemType.Gun:
                entity.AddItem(label, spriteID, itemType);
                break;
            case Enums.ItemType.Rock:
                entity.AddItem(label, spriteID, itemType);
                entity.AddItemStack(1, maxStackCount);
                break;
            case Enums.ItemType.RockDust:
                entity.AddItem(label, spriteID, itemType);
                entity.AddItemStack(1, maxStackCount);
                break;
            default:
                Debug.Log("Invalid Item Type");
                break;
        }
        inventoryManagerSystem.AddItem(entity, inventoryID);

    }

    // TODO: use DrawSprite and PixelPerfectGrid?.
    void CreateObjects(int inventoryID)
    {
        var group = context.game.GetEntitiesWithItemAttachedInventory(inventoryID);
        foreach (GameEntity entity in group)
        {
        
            GameObject obj = new GameObject(entity.item.Label, typeof(RectTransform), typeof(Image));
            obj.transform.parent = parentObject.transform.GetChild(entity.itemAttachedInventory.SlotNumber).gameObject.transform;
            obj.GetComponent<RectTransform>().sizeDelta = parentObject.GetComponent<GridLayoutGroup>().cellSize;
            obj.GetComponent<Image>().sprite = Sprites[entity.item.SpriteID];
            if (entity.hasItemStack)
            {
                Debug.Log("Slot" + entity.itemAttachedInventory.SlotNumber + ": " + entity.itemStack.StackCount + "items");
            }
            // TODO: Add item count to icon.
        }
    }

    void DrawItemsIcons()
    {
        var group = context.game.GetEntitiesWithItemAttachedInventory(0);
        foreach (var item in group)
        { 
            
        }
    }

    // draws 1 tile into the screen
    // Note(Mahdi): this code is for testing purpose
    void DrawSprite(float x, float y, float w, float h, byte[] spriteBytes,
         int spriteW, int spriteH)
    {
        var tex = CreateTextureFromRGBA(spriteBytes, spriteW, spriteH);
        var mat = Instantiate(material);
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

