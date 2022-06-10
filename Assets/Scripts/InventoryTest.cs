using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTest : MonoBehaviour
{
    Contexts context;

    Inventory.ManagerSystem inventoryManagerSystem;
    Inventory.DrawSystem    inventoryDrawSystem;
    Item.SpawnerSystem      itemSpawnSystem;

    [SerializeField] Material material;

    public void Start()
    {
        Initialize();

        context = Contexts.sharedInstance;
        inventoryManagerSystem = new Inventory.ManagerSystem(context);
        itemSpawnSystem = new Item.SpawnerSystem(context);
        inventoryDrawSystem = new Inventory.DrawSystem(context);

        // Create inventory.
        const int inventoryID = 0;
        CreateInventoryEntity(inventoryID);

        // Test not stackable items.
        for (uint i = 0; i < 10; i++)
        {
            GameEntity entity = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.Gun);
            inventoryManagerSystem.AddItem(entity, inventoryID);
        }

        // Testing stackable items.
        for (uint i = 0; i < 1000; i++)
        {
            GameEntity entity = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.Rock);
            inventoryManagerSystem.AddItem(entity, inventoryID);
            entity = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.RockDust);
            inventoryManagerSystem.AddItem(entity, inventoryID);
        }
    }

    public void Update()
    {
        //remove all children MeshRenderer
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);

        inventoryDrawSystem.Draw(material, transform);
    }

    private void Initialize()
    {
        int GunSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png");
        int RockSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png");
        int RockDustSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png");

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "Gun");
        Item.CreationApi.Instance.SetTexture(GunSpriteSheet, 0, 0);
        Item.CreationApi.Instance.SetInventoryTexture(GunSpriteSheet, 0, 0);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Rock, "Rock");
        Item.CreationApi.Instance.SetTexture(RockSpriteSheet, 0, 0);
        Item.CreationApi.Instance.SetInventoryTexture(RockSpriteSheet, 0, 0);
        Item.CreationApi.Instance.MakeStackable(99);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.RockDust, "RockDust");
        Item.CreationApi.Instance.SetTexture(RockDustSpriteSheet, 0, 0);
        Item.CreationApi.Instance.SetInventoryTexture(RockDustSpriteSheet, 0, 0);
        Item.CreationApi.Instance.MakeStackable(99);
        Item.CreationApi.Instance.EndItem();

    }

    void CreateInventoryEntity(int inventoryID)
    {
        var entity = context.game.CreateEntity();
        const int height = 8;
        const int width = 8;
        const int selectedSlot = 0;

        BitArray slots = new BitArray(height * width, false);

        entity.AddInventoryID(inventoryID);
        entity.AddInventorySize(width, height);
        entity.AddInventorySlots(slots, selectedSlot);
    }
}
