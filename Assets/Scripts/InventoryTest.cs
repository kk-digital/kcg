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
    ECSInput.ProcessSystem  inputProcessSystem;

    [SerializeField] Material material;

    public void Start()
    {
        Initialize();

        context = Contexts.sharedInstance;
        inventoryManagerSystem = new Inventory.ManagerSystem(context);
        itemSpawnSystem = new Item.SpawnerSystem(context);
        inventoryDrawSystem = new Inventory.DrawSystem(context);
        inputProcessSystem = new ECSInput.ProcessSystem();
        var inventoryAttacher = Inventory.InventoryAttacher.Instance;

        // Create Agent and inventory.
        int agnetID = 0;
        int inventoryWidth = 6;
        int inventoryHeight = 5;
        int toolBarSize = 8;

        GameEntity playerEntity = context.game.CreateEntity();
        playerEntity.AddAgentID(agnetID);
        playerEntity.isAgentPlayer = true;
        inventoryAttacher.AttachInventoryToAgent(inventoryWidth, inventoryHeight, agnetID);
        inventoryAttacher.AttachToolBarToPlayer(toolBarSize, agnetID);

        int inventoryID = playerEntity.agentInventory.InventoryID;
        int toolBarID = playerEntity.agentToolBar.ToolBarID;
        
        // Add item to tool bar.
        {
            GameEntity entity = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.Gun);
            inventoryManagerSystem.AddItem(entity, toolBarID);
        }

        // Test not stackable items.
        for (uint i = 0; i < 10; i++)
        {
            GameEntity entity = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.Gun);
            inventoryManagerSystem.AddItem(entity, inventoryID);
        }

        // Testing stackable items.
        for (uint i = 0; i < 10; i++)
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

        inputProcessSystem.Update();
        inventoryDrawSystem.Draw(material, transform, 0);
    }

    private void Initialize()
    {
        int GunSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\gun-temp.png");
        int RockSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png");
        int RockDustSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png");

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "Gun");
        Item.CreationApi.Instance.SetTexture(GunSpriteSheet);
        Item.CreationApi.Instance.SetInventoryTexture(GunSpriteSheet);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Rock, "Rock");
        Item.CreationApi.Instance.SetTexture(RockSpriteSheet);
        Item.CreationApi.Instance.SetInventoryTexture(RockSpriteSheet);
        Item.CreationApi.Instance.MakeStackable(99);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.RockDust, "RockDust");
        Item.CreationApi.Instance.SetTexture(RockDustSpriteSheet);
        Item.CreationApi.Instance.SetInventoryTexture(RockDustSpriteSheet);
        Item.CreationApi.Instance.MakeStackable(99);
        Item.CreationApi.Instance.EndItem();
    }
}
