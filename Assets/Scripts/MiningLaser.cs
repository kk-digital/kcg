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

    // Item Spawner System
    Item.SpawnerSystem SpawnerSystem;
    
    // Item Draw System
    Item.DrawSystem DrawSystem;

    // Initialize
    private bool Init;
    private Vector2 laserPosition;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        laserPosition = new Vector2(2.0f, 2.5f);

        tileMap = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;

        // Assign Contexts
        contexts = Contexts.sharedInstance;

        // Assign Spawner System
        SpawnerSystem = new Item.SpawnerSystem(contexts);

        // Assign Draw System
        DrawSystem = new Item.DrawSystem(contexts);

        // Initialize the mining laser
        Initialize();
    }

    private void Initialize()
    {
        // Get Sheet ID
        int laserSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\lasergun-temp.png", 195, 79);

        // Create Item Entity
        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "Laser");

        // Create Texture
        Item.CreationApi.Instance.SetTexture(laserSpriteSheet);

        // Create Inventory Texture
        Item.CreationApi.Instance.SetInventoryTexture(laserSpriteSheet);

        // Create Size Component
        Item.CreationApi.Instance.SetSize(new Vector2(1.0f, 0.5f));

        // End
        Item.CreationApi.Instance.EndItem();

        // Spawn the created item
        SpawnerSystem.SpawnItem(Enums.ItemType.Gun, laserPosition);

        // Initializon done
        Init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    void Update()
    {
        if (Init)
        {
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
            DrawSystem.Draw(Instantiate(Material), transform, 14);

            if (Input.GetKey(KeyCode.Mouse0))
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
                        tileMap.RemoveTile(cell.x, cell.y, Enums.Tile.MapLayerType.Front);
                        tileMap.BuildLayerTexture(Enums.Tile.MapLayerType.Front);
                    }

                    Debug.DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(pointerPos.x, pointerPos.y, 0.0f), Color.red);
                }
            }
        }
    }
}
