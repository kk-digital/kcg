using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;
using KMath;
using Inventory;
using Item;

namespace Planet.Unity
{
    class ItemTest : MonoBehaviour
    {
        [SerializeField] Material   Material;

        Planet.PlanetState    Planet;
        AgentEntity           Player;

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
            ref var tileMap = ref Planet.TileMap;
            Material material = Material;

            if (Input.GetKeyDown(KeyCode.T))
            {
                GameState.ActionCreationSystem.CreateAction(Planet.EntitasContext, (int)Enums.ActionType.DropAction, Player.agentID.ID);
            }

            int toolBarID = Player.agentToolBar.ToolBarID;
            InventoryEntity Inventory = Planet.EntitasContext.inventory.GetEntityWithInventoryID(toolBarID);
            int selectedSlot = Inventory.inventorySlots.Selected;
       
            ItemInventoryEntity item = GameState.InventoryManager.GetItemInSlot(Planet.EntitasContext.itemInventory, toolBarID, selectedSlot);
            if (item != null)
            {
                ItemProprieties itemProperty = GameState.ItemCreationApi.Get(item.itemType.Type);
                if (itemProperty.IsTool())
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        GameState.ActionCreationSystem.CreateAction(Planet.EntitasContext, itemProperty.ToolActionType, 
                            Player.agentID.ID, item.itemID.ID); 
                    }
                }
            }
            Planet.Update(Time.deltaTime, Material, transform);
        }

        private void OnRenderObject()
        {
            GameState.InventoryDrawSystem.Draw(Planet.EntitasContext, Material, transform);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            GameResources.Initialize();

            // Generating the map
            Vec2i mapSize = new Vec2i(16, 16);
            Planet = new Planet.PlanetState();
            Planet.Init(mapSize);
            Planet.InitializeSystems(Material, transform);

            GenerateMap();

            Player = Planet.AddPlayer(GameResources.CharacterSpriteId, 32, 48, new Vec2f(3.0f, 3.0f), 0, 100, 100, 100, 100, 100);
            int toolBarID = Player.agentToolBar.ToolBarID;

            GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.Pistol, new Vec2f(3.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.PumpShotgun, new Vec2f(3.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.PulseWeapon, new Vec2f(3.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.SniperRifle, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.Shotgun, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.LongRifle, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.RPG, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.SMG, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.GrenadeLauncher, new Vec2f(3.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.Sword, new Vec2f(3.0f, 3.0f));
            GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.RiotShield, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.StunBaton, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.AutoCannon, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.Bow, new Vec2f(3.0f, 3.0f));
            //GameState.ItemSpawnSystem.SpawnItemParticle(Planet.EntitasContext, Enums.ItemType.Ore, new Vec2f(6.0f, 3.0f));

            var SpawnEnemyTool = GameState.ItemSpawnSystem.SpawnInventoryItem(Planet.EntitasContext, Enums.ItemType.SpawnEnemySlimeTool);
            GameState.InventoryManager.AddItem(Planet.EntitasContext, SpawnEnemyTool, toolBarID);
        }

        void GenerateMap()
        {
            ref var tileMap = ref Planet.TileMap;

            for (int j = 0; j < tileMap.MapSize.Y; j++)
            {
                for (int i = 0; i < tileMap.MapSize.X; i++)
                {
                    TileID frontTile;

                    if (i >= tileMap.MapSize.X / 2)
                    {
                        if (j % 2 == 0 && i == tileMap.MapSize.X / 2)
                        {
                            frontTile = TileID.Moon;
                        }
                        else
                        {
                            frontTile = TileID.Glass;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == tileMap.MapSize.X / 2 + 1)
                        {
                            frontTile = TileID.Glass;
                        }
                        else
                        {
                            frontTile = TileID.Moon;
                        }
                    }

                    if (j is > 1 and < 6 || (j > 8 + i))
                    {
                        frontTile = TileID.Air;
                    }


                    tileMap.SetFrontTile(i, j, frontTile);
                }
            }
            //TileMap.BuildLayerTexture(MapLayerType.Front);
        }
    }
}
