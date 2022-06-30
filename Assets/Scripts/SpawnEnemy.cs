using UnityEngine;
using Entitas;
using KMath;

public class SpawnEnemy : MonoBehaviour
{
    // ATLAS
    [SerializeField] Material Material;

    // Inventory Manager System
    Inventory.InventoryManager InventoryManager;

    // Inventory Draw System
    Inventory.InventoryDrawSystem inventoryDrawSystem;

    // Item Spawner System
    Item.SpawnerSystem itemSpawnSystem;

    // Entitas Contexts
    Contexts contexts;

    // Initializon bool
    private bool Init = false;

    // Slime Sprite ID
    private int SlimeMoveLeftBaseSpriteId;

    // Is held or not
    private bool isHeld;

    // Planet State
    Planet.PlanetState planetState;

    void Start()
    {
        // Initialize All Items
        InitializeItems();
        
        Contexts entitasContext = Contexts.sharedInstance;

        // Generating the map
        var mapSize = new Vec2i(16, 16);
        planetState = new Planet.PlanetState();
        planetState.Init(mapSize);
        planetState.InitializeSystems(Material, transform);

        // Enemy Sprite Sheet ID
        int EnemySpriteSheetID = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Enemies\\Slime\\slime.png", 32, 32);

        // Slime Animation Slice Tiles
        SlimeMoveLeftBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(EnemySpriteSheetID, 0, 0, 3, 0, Enums.AtlasType.Agent);

        GameState.AnimationManager.CreateAnimation(1);
        GameState.AnimationManager.SetName("slime-move-left");
        GameState.AnimationManager.SetTimePerFrame(0.35f);
        GameState.AnimationManager.SetBaseSpriteID(SlimeMoveLeftBaseSpriteId);
        GameState.AnimationManager.SetFrameCount(4);
        GameState.AnimationManager.EndAnimation();

        // Assign Contexts
        contexts = Contexts.sharedInstance;

        // Create Inventory Manager
        InventoryManager = new Inventory.InventoryManager();

        // Create Item Spawner System
        itemSpawnSystem = new Item.SpawnerSystem();

        // Create Draw System
        inventoryDrawSystem = new Inventory.InventoryDrawSystem();

        // Create Inventory Attacher
        var inventoryAttacher = Inventory.InventoryAttacher.Instance;

        // Create Agent and inventory.
        int agentID = 3;
        int inventoryWidth = 6;
        int inventoryHeight = 5;
        int toolBarSize = 8;

        GameEntity playerEntity = contexts.game.CreateEntity();
        playerEntity.ReplaceAgentID(agentID);
        playerEntity.isAgentPlayer = true;
        inventoryAttacher.AttachInventoryToAgent(Contexts.sharedInstance, inventoryWidth, inventoryHeight, playerEntity);
        inventoryAttacher.AttachToolBarToPlayer(Contexts.sharedInstance, toolBarSize, playerEntity);

        int inventoryID = playerEntity.agentInventory.InventoryID;
        int toolBarID = playerEntity.agentToolBar.ToolBarID;

        // Add item to tool bar.
        {
            GameEntity entity = itemSpawnSystem.SpawnInventoryItem(contexts.game, Enums.ItemType.PlacementTool);
            InventoryManager.AddItem(contexts, entity, toolBarID);
        }

        // Test not stackable items.
        for (uint i = 0; i < 10; i++)
        {
            GameEntity entity = itemSpawnSystem.SpawnInventoryItem(contexts.game, Enums.ItemType.PlacementTool);
            InventoryManager.AddItem(contexts, entity, inventoryID);
        }

        // Init finished
        Init = true;
    }

    // Spawn Enemy
    private void SpawnEnemySlime(Vec2f pos)
    {
        // Add Enemy to Enemy list
        planetState.AddEnemy(SlimeMoveLeftBaseSpriteId, 32, 32, pos, 1);
    }

    private void InitializeItems()
    {
        // Get Sheet ID
        int slimeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Enemies\\Slime\\slime.png", 32, 32);
        int slimeIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(slimeSpriteSheet, 0, 0, Enums.AtlasType.Particle);
        // Create Item
        Item.CreationApi.Instance.CreateItem(contexts, Enums.ItemType.PlacementTool, "Slime");

        // Set texture of item
        Item.CreationApi.Instance.SetTexture(slimeIcon);

        // Create Inventory texture
        Item.CreationApi.Instance.SetInventoryTexture(slimeIcon);

        // End of the item
        Item.CreationApi.Instance.EndItem();
    }

    void Update()
    {
        if (Init)
        {
            // Get Slot Entites
            IGroup<InventoryEntity> entities =
            contexts.inventory.GetGroup(InventoryMatcher.InventorySlots);
            // Detect if spawner helded or not
            foreach (var slots in entities)
            {
                if (slots.inventorySlots.Selected == 1)
                {
                    isHeld = true;
                }
                else
                {
                    isHeld = false;
                }
            }

            // Spawn Enemy when key event
            if (Input.GetKeyDown(KeyCode.Mouse0) && isHeld)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
                SpawnEnemySlime(new Vec2f(worldPosition.x, worldPosition.y));
            }

            // Update Inventory Draw System
            planetState.Update(Time.deltaTime, Material, transform);
        }
    }
    private void OnRenderObject()
    {
        inventoryDrawSystem.Draw(Instantiate(Material), transform);
    }
}
