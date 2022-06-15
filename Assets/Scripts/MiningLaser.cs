using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Planet;
using KMath;
using Entitas;

public class MiningLaser : MonoBehaviour
{
    // ATLAS
    [SerializeField] Material Material;

    // Planet Tile Map
    Planet.TileMap tileMap;

    // Entitas Contexts
    Contexts contexts;

    // Initialize
    private bool Init;
    private Vector2 laserPosition;

    // Laser Properties
    private float destroyDelay = 0.5f;
    private bool isHeld = false;

    // Item Draw System
    Item.DrawSystem DrawSystem;

    // Inventory Manager System
    Inventory.ManagerSystem inventoryManagerSystem;

    // Inventory Draw System
    Inventory.DrawSystem inventoryDrawSystem;

    // Item Spawner System
    Item.SpawnerSystem itemSpawnSystem;

    // Input Process System
    ECSInput.ProcessSystem inputProcessSystem;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Initialize the mining laser item
        Initialize();

        // Laser Position
        laserPosition = new Vector2(2.0f, 2.5f);

        tileMap = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;

        // Assign Contexts
        contexts = Contexts.sharedInstance;

        // Assign Draw System
        DrawSystem = new Item.DrawSystem(contexts);

        // Create Inventory Manager System
        inventoryManagerSystem = new Inventory.ManagerSystem(contexts);

        // Create Item Spawner System
        itemSpawnSystem = new Item.SpawnerSystem(contexts);

        // Create Draw System
        inventoryDrawSystem = new Inventory.DrawSystem(contexts);

        // Create Input Process System
        inputProcessSystem = new ECSInput.ProcessSystem();

        // Create Inventory Attacher
        var inventoryAttacher = Inventory.InventoryAttacher.Instance;

        // Create Agent and inventory.
        int agnetID = 0;
        int inventoryWidth = 6;
        int inventoryHeight = 5;
        int toolBarSize = 8;

        GameEntity playerEntity = contexts.game.CreateEntity();
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

        // Initialize Laser Object
        InitializeLaser();
    }

    private void Initialize()
    {
        // Get Sheet ID
        int laserSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\lasergun-temp.png", 195, 79);

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "LaserItem");
        Item.CreationApi.Instance.SetTexture(laserSpriteSheet);
        Item.CreationApi.Instance.SetInventoryTexture(laserSpriteSheet);
        // Create Size Component
        Item.CreationApi.Instance.SetSize(new Vector2(1.0f, 0.5f));
        Item.CreationApi.Instance.EndItem();

    }

    private void InitializeLaser()
    {
        // Spawn the created item
        itemSpawnSystem.SpawnItem(Enums.ItemType.Gun, laserPosition);

        // Initializon done
        Init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    void Update()
    {
        if (Init)
        {
            // Get Slot Entites
            IGroup<GameEntity> entities =
            contexts.game.GetGroup(GameMatcher.InventorySlots);
            foreach (var slots in entities)
            {
                if(slots.inventorySlots.Selected == 1)
                {
                    isHeld = true;
                }
                else
                {
                    isHeld = false;
                }
            }

            // Get Laser Position
            IGroup<GameEntity> Laserentities =
            contexts.game.GetGroup(GameMatcher.PhysicsPosition2D);
            foreach (var laser in Laserentities)
            {
                laserPosition = laser.physicsPosition2D.Value;
            }

            // Delete the old one
            foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            {
                if (Application.isPlaying)
                {
                    Destroy(mr.gameObject);
                }
                else
                {
                    DestroyImmediate(mr.gameObject);
                }
            }

            // Draw System Update
            inputProcessSystem.Update();
            inventoryDrawSystem.Draw(Instantiate(Material), transform, 100);

            // If laser held, draw it.
            if(isHeld)
                DrawSystem.Draw(Instantiate(Material), transform, 16);

            if (Input.GetKey(KeyCode.Mouse0) && isHeld)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                Vector2 pointerPos = Camera.main.ScreenToWorldPoint(mousePos);

                // Create new start and end position based on mouse position
                Cell start = new Cell
                {
                    x = (int)laserPosition.x,
                    y = (int)laserPosition.y
                };

                Cell end = new Cell
                {
                    x = (int)pointerPos.x,
                    y = (int)pointerPos.y
                };

                // Log places drawed line go through
                foreach (var cell in start.LineTo(end))
                {
                    Debug.Log($"({cell.x},{cell.y})");

                    ref var tile = ref tileMap.GetTileRef(cell.x, cell.y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        StartCoroutine(RemoveTile(cell.x, cell.y));
                        tileMap.BuildLayerTexture(Enums.Tile.MapLayerType.Front);
                    }

                    Debug.DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(pointerPos.x, pointerPos.y, 0.0f), Color.red);
                }
            }
        }
    }

    IEnumerator RemoveTile(int x, int y)
    {
        yield return new WaitForSeconds(destroyDelay);
        tileMap.RemoveTile(x, y, Enums.Tile.MapLayerType.Front);
        tileMap.BuildLayerTexture(Enums.Tile.MapLayerType.Front);
    }
}
