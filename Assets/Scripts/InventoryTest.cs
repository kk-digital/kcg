using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTest : MonoBehaviour
{
    Contexts context;

    Inventory.InventoryManager inventoryManager;
    Inventory.DrawSystem    inventoryDrawSystem;
    Item.SpawnerSystem      itemSpawnSystem;
    ECSInput.InputProcessSystem  inputProcessSystem;

    [SerializeField] Material material;

    public void Start()
    {
        Initialize();

        context = Contexts.sharedInstance;
        inventoryManager = new Inventory.InventoryManager();
        itemSpawnSystem = new Item.SpawnerSystem();
        inventoryDrawSystem = new Inventory.DrawSystem();
        inputProcessSystem = new ECSInput.InputProcessSystem();
        var inventoryAttacher = Inventory.InventoryAttacher.Instance;

        // Create Agent and inventory.
        int agnetID = 0;
        int inventoryWidth = 6;
        int inventoryHeight = 5;
        int toolBarSize = 8;

        GameEntity playerEntity = context.game.CreateEntity();
        playerEntity.AddAgentID(agnetID);
        playerEntity.isAgentPlayer = true;
        inventoryAttacher.AttachInventoryToAgent(Contexts.sharedInstance.game, inventoryWidth, inventoryHeight, agnetID);
        inventoryAttacher.AttachToolBarToPlayer(Contexts.sharedInstance.game, toolBarSize, agnetID);

        int inventoryID = playerEntity.agentInventory.InventoryID;
        int toolBarID = playerEntity.agentToolBar.ToolBarID;

        // Add item to tool bar.
        {
            GameEntity entity = itemSpawnSystem.SpawnInventoryItem(context.game, Enums.ItemType.Gun);
            inventoryManager.AddItem(entity, toolBarID);
        }

        // Test not stackable items.
        for (uint i = 0; i < 10; i++)
        {
            GameEntity entity = itemSpawnSystem.SpawnInventoryItem(context.game, Enums.ItemType.Gun);
            inventoryManager.AddItem(entity, inventoryID);
        }

        // Testing stackable items.
        for (uint i = 0; i < 10; i++)
        {
            GameEntity entity = itemSpawnSystem.SpawnInventoryItem(context.game, Enums.ItemType.Rock);
            inventoryManager.AddItem(entity, inventoryID);
            entity = itemSpawnSystem.SpawnInventoryItem(context.game, Enums.ItemType.RockDust);
            inventoryManager.AddItem(entity, inventoryID);
        }
    }

    public void Update()
    {
        // check if the sprite atlas textures needs to be updated
        for(int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
        {
            GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
        }

        // check if the tile sprite atlas textures needs to be updated
        for(int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
        {
            GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
        }

        //remove all children MeshRenderer
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);

        inputProcessSystem.Update(Contexts.sharedInstance.game);
        inventoryDrawSystem.Draw(material, transform, 0);

    }

    private void Initialize()
    {
        int GunSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\gun-temp.png", 44, 25);
        int RockSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png", 16, 16);
        int RockDustSpriteSheet =
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png", 16, 16);

        int GunIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(GunSpriteSheet, 0, 0, Enums.AtlasType.Particle);
        int RockIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(RockSpriteSheet, 0, 0, Enums.AtlasType.Particle);
        int RockDustIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(RockDustSpriteSheet, 0, 0, Enums.AtlasType.Particle);

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "Gun");
        Item.CreationApi.Instance.SetTexture(GunIcon);
        Item.CreationApi.Instance.SetInventoryTexture(GunIcon);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Rock, "Rock");
        Item.CreationApi.Instance.SetTexture(RockIcon);
        Item.CreationApi.Instance.SetInventoryTexture(RockIcon);
        Item.CreationApi.Instance.SetStackable(99);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.RockDust, "RockDust");
        Item.CreationApi.Instance.SetTexture(RockDustIcon);
        Item.CreationApi.Instance.SetInventoryTexture(RockDustIcon);
        Item.CreationApi.Instance.SetStackable(99);
        Item.CreationApi.Instance.EndItem();
    }
}
