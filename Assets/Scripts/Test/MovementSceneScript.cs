using UnityEngine;
using Enums.Tile;
using KMath;
using Item;
using Animancer;
using HUD;
using PlanetTileMap;

namespace Planet.Unity
{
    class MovementSceneScript : MonoBehaviour
    {
        [SerializeField] Material Material;

        public PlanetState Planet;
        Inventory.InventoryManager inventoryManager;
        Inventory.DrawSystem inventoryDrawSystem;


        AgentEntity Player;
        int PlayerID;

        int CharacterSpriteId;
        int inventoryID;
        int toolBarID;

        HUDManager hudManager;

        static bool Init = false;

        public void Start()
        {
            if (!Init)
            {
                Initialize();
                Init = true;
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                TileMapManager.Save(Planet.TileMap, "generated-maps/movement-map.kmap");
                Debug.Log("saved!");
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                var camera = Camera.main;
                Vector3 lookAtPosition = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));

                Planet.TileMap = TileMapManager.Load("generated-maps/movement-map.kmap", (int)lookAtPosition.x, (int)lookAtPosition.y);
                Planet.TileMap.UpdateBackTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
                Planet.TileMap.UpdateMidTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
                Planet.TileMap.UpdateFrontTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);

                Debug.Log("loaded!");
            }

             int toolBarID = Player.agentToolBar.ToolBarID;
            InventoryEntity Inventory = Planet.EntitasContext.inventory.GetEntityWithInventoryID(toolBarID);
            int selectedSlot = Inventory.inventorySlots.Selected;

            ItemInventoryEntity item = GameState.InventoryManager.GetItemInSlot(Planet.EntitasContext.itemInventory, toolBarID, selectedSlot);
            ItemProprieties itemProperty = GameState.ItemCreationApi.Get(item.itemType.Type);
            if (itemProperty.IsTool())
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameState.ActionCreationSystem.CreateAction(Planet.EntitasContext, itemProperty.ToolActionType, 
                       Player.agentID.ID, item.itemID.ID);
                }
            }

            Planet.Update(Time.deltaTime, Material, transform);
        }

        private void OnRenderObject()
        {
            inventoryDrawSystem.Draw(Planet.EntitasContext, Material, transform);
        }

        private void OnGUI()
        {
            if (Init)
            {
                // Draw HUD UI
                hudManager.Update(Player);

                // Draw Statistics
                KGUI.Statistics.StatisticsDisplay.DrawStatistics(ref Planet);
            }
        }
        

        private void OnDrawGizmos()
        {
            // Set the color of gizmos
            Gizmos.color = Color.green;
            
            // Draw a cube around the map
            if(Planet.TileMap != null)
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(Planet.TileMap.MapSize.X, Planet.TileMap.MapSize.Y, 0.0f));

            // Draw lines around player if out of bounds
            if (Player != null)
                if(Player.physicsPosition2D.Value.X -10.0f >= Planet.TileMap.MapSize.X)
                {
                    // Out of bounds
                
                    // X+
                    Gizmos.DrawLine(new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y, 0.0f), new Vector3(Player.physicsPosition2D.Value.X + 10.0f, Player.physicsPosition2D.Value.Y));

                    // X-
                    Gizmos.DrawLine(new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y, 0.0f), new Vector3(Player.physicsPosition2D.Value.X - 10.0f, Player.physicsPosition2D.Value.Y));

                    // Y+
                    Gizmos.DrawLine(new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y, 0.0f), new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y + 10.0f));

                    // Y-
                    Gizmos.DrawLine(new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y, 0.0f), new Vector3(Player.physicsPosition2D.Value.X, Player.physicsPosition2D.Value.Y - 10.0f));
                }

            // Draw Chunk Visualizer
            Admin.AdminAPI.DrawChunkVisualizer(Planet.TileMap);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {

            
            Application.targetFrameRate = 60;

            inventoryManager = new Inventory.InventoryManager();
            inventoryDrawSystem = new Inventory.DrawSystem();
            GameResources.Initialize();

            // Generating the map
            Vec2i mapSize = new Vec2i(128, 24);
            Planet = new Planet.PlanetState();
            Planet.Init(mapSize);

            Planet.InitializeSystems(Material, transform);
            //GenerateMap();
            var camera = Camera.main;
            Vector3 lookAtPosition = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));

            /*Planet.TileMap = TileMapManager.Load("generated-maps/movement-map.kmap", (int)lookAtPosition.x, (int)lookAtPosition.y);
                Debug.Log("loaded!");*/

            GenerateMap();

            Planet.TileMap.UpdateBackTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
            Planet.TileMap.UpdateMidTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
            Planet.TileMap.UpdateFrontTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);

            Player = Planet.AddPlayer(new Vec2f(3.0f, 20));
            PlayerID = Player.agentID.ID;

            inventoryID = Player.agentInventory.InventoryID;
            toolBarID = Player.agentToolBar.ToolBarID;

            // Player Status UI Init
            hudManager = new HUDManager(Planet.EntitasContext, Player);

            // Admin API Spawn Items
            Admin.AdminAPI.SpawnItem(Enums.ItemType.Pistol, Planet.EntitasContext);
            Admin.AdminAPI.SpawnItem(Enums.ItemType.Ore, Planet.EntitasContext);

            // Admin API Add Items
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.PlacementTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.RemoveTileTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.SpawnEnemySlimeTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.MiningLaserTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.PipePlacementTool, Planet.EntitasContext);
            Admin.AdminAPI.AddItem(inventoryManager, toolBarID, Enums.ItemType.ParticleEmitterPlacementTool, Planet.EntitasContext);

            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.Pistol, new Vec2f(3.0f, 25.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.PumpShotgun, new Vec2f(4.0f, 25.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.PulseWeapon, new Vec2f(5.0f, 25.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.SniperRifle, new Vec2f(6.0f, 25.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.Sword, new Vec2f(7.0f, 25.0f));

        }



        void GenerateMap()
        {
            KMath.Random.Mt19937.init_genrand((ulong) System.DateTime.Now.Ticks);
            
            ref var tileMap = ref Planet.TileMap;

            
            for (int j = 0; j < tileMap.MapSize.Y / 2; j++)
            {
                for(int i = 0; i < tileMap.MapSize.X; i++)
                {
                    tileMap.GetFrontTile(i, j).MaterialType = TileMaterialType.Moon;
                    tileMap.GetBackTile(i, j).MaterialType = TileMaterialType.Background;
                }
            }

            

            for(int i = 0; i < tileMap.MapSize.X; i++)
            {
                tileMap.GetFrontTile(i, 0).MaterialType = TileMaterialType.Bedrock;
                tileMap.GetFrontTile(i, tileMap.MapSize.Y - 1).MaterialType = TileMaterialType.Bedrock;
            }

            for(int j = 0; j < tileMap.MapSize.Y; j++)
            {
                tileMap.GetFrontTile(0, j).MaterialType = TileMaterialType.Bedrock;
                tileMap.GetFrontTile(tileMap.MapSize.X - 1, j).MaterialType = TileMaterialType.Bedrock;
            }

            tileMap.GetFrontTile(8, 14).MaterialType = TileMaterialType.Platform;
            tileMap.GetFrontTile(9, 14).MaterialType = TileMaterialType.Platform;
            tileMap.GetFrontTile(10, 14).MaterialType = TileMaterialType.Platform;
            tileMap.GetFrontTile(11, 14).MaterialType = TileMaterialType.Platform;
            tileMap.GetFrontTile(12, 14).MaterialType = TileMaterialType.Platform;
            tileMap.GetFrontTile(13, 14).MaterialType = TileMaterialType.Platform;

            var camera = Camera.main;
            Vector3 lookAtPosition = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));

            tileMap.UpdateBackTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
            tileMap.UpdateMidTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
            tileMap.UpdateFrontTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);

           


        }

    }
}
