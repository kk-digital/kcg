using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class SpawnEnemy : MonoBehaviour
{
    // ATLAS
    [SerializeField] Material Material;

    // Input Process System
    ECSInput.ProcessSystem InputProcessSystems;

    // Agent Spawner System
    Agent.SpawnerSystem AgentSpawnerSystem;

    // Physics Movable System
    Physics.MovableSystem PhysicsMovableSystem;

    // Agent Draw System
    Agent.DrawSystem AgentDrawSystem;

    // Agent Enemy System
    Agent.EnemyAiSystem AgentEnemySystem;

    // Agent Process Collision System
    Physics.ProcessCollisionSystem AgentProcessCollisionSystem;

    // Inventory Manager System
    Inventory.ManagerSystem inventoryManagerSystem;

    // Inventory Draw System
    Inventory.DrawSystem inventoryDrawSystem;

    // Item Spawner System
    Item.SpawnerSystem itemSpawnSystem;

    // Entitas Contexts
    Contexts contexts;

    // Tile Map
    private Planet.TileMap tileMap;

    // Initializon bool
    private bool Init = false;

    // Slime Sprite ID
    private int SlimeMoveLeftBaseSpriteId;

    // Agent ID
    private int agentID = 1;

    // Is held or not
    private bool isHeld;

    void Start()
    {
        // Initialize All Items
        InitializeItems();

        // Find Tile Map
        tileMap = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;

        // Enemy Sprite Sheet ID
        int EnemySpriteSheetID = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\slime.png", 32, 32);

        // Slime Animation Slice Tiles
        SlimeMoveLeftBaseSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(EnemySpriteSheetID, 0, 0, Enums.AtlasType.Agent);
        GameState.SpriteAtlasManager.CopySpriteToAtlas(EnemySpriteSheetID, 1, 0, Enums.AtlasType.Agent);
        GameState.SpriteAtlasManager.CopySpriteToAtlas(EnemySpriteSheetID, 2, 0, Enums.AtlasType.Agent);
        GameState.SpriteAtlasManager.CopySpriteToAtlas(EnemySpriteSheetID, 3, 0, Enums.AtlasType.Agent);

        // Create Input Process System
        InputProcessSystems = new ECSInput.ProcessSystem();

        // Create Spawner System
        AgentSpawnerSystem = new Agent.SpawnerSystem();

        // Physics Movable System
        PhysicsMovableSystem = new Physics.MovableSystem();

        // Agent Draw System
        AgentDrawSystem = new Agent.DrawSystem();

        // Agent Process Collision System
        AgentProcessCollisionSystem = new Physics.ProcessCollisionSystem();

        // Agent Enemy System
        AgentEnemySystem = new Agent.EnemyAiSystem();

        // Assign Contexts
        contexts = Contexts.sharedInstance;

        // Create Inventory Manager System
        inventoryManagerSystem = new Inventory.ManagerSystem(contexts);

        // Create Item Spawner System
        itemSpawnSystem = new Item.SpawnerSystem(contexts);

        // Create Draw System
        inventoryDrawSystem = new Inventory.DrawSystem(contexts);

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
        inventoryAttacher.AttachInventoryToAgent(inventoryWidth, inventoryHeight, agentID);
        inventoryAttacher.AttachToolBarToPlayer(toolBarSize, agentID);

        int inventoryID = playerEntity.agentInventory.InventoryID;
        int toolBarID = playerEntity.agentToolBar.ToolBarID;

        // Add item to tool bar.
        {
            GameEntity entity = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.PlacementTool);
            inventoryManagerSystem.AddItem(entity, toolBarID);
        }

        // Test not stackable items.
        for (uint i = 0; i < 10; i++)
        {
            GameEntity entity = itemSpawnSystem.SpawnIventoryItem(Enums.ItemType.PlacementTool);
            inventoryManagerSystem.AddItem(entity, inventoryID);
        }

        // Init finished
        Init = true;
    }

    private void SpawnEnemySlime(Vector2 pos)
    {
        AgentSpawnerSystem.SpawnEnemy(Material, SlimeMoveLeftBaseSpriteId, 32, 32, pos, agentID++);
    }

    private void InitializeItems()
    {
        // Get Sheet ID
        int slimeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\slime.png", 32, 32);

        // Create Item
        Item.CreationApi.Instance.CreateItem(Enums.ItemType.PlacementTool, "Slime");

        // Set texture of item
        Item.CreationApi.Instance.SetTexture(slimeSpriteSheet);

        // Create Inventory texture
        Item.CreationApi.Instance.SetInventoryTexture(slimeSpriteSheet);

        // End of the item
        Item.CreationApi.Instance.EndItem();
    }

    void Update()
    {
        if (Init)
        {
            // Get Slot Entites
            IGroup<GameEntity> entities =
            contexts.game.GetGroup(GameMatcher.InventorySlots);
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
                SpawnEnemySlime(new Vector2(worldPosition.x, worldPosition.y));
            }

            // Delete the old one
            foreach (var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            // Upate Input Process System
            InputProcessSystems.Update();

            // Update Physics Movable Systems
            PhysicsMovableSystem.Update();

            // Update Process Collision System
            AgentProcessCollisionSystem.Update(tileMap);

            // Update Draw System
            AgentDrawSystem.Draw(Instantiate(Material), transform, 17);

            // Update Inventory Draw System
            inventoryDrawSystem.Draw(Instantiate(Material), transform, 100);
        }

    }
}
