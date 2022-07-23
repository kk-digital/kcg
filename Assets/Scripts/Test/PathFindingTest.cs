using Enums.Tile;
using Item;
using KMath;
using UnityEngine;
using PlanetTileMap;

namespace Planet.Unity
{
    class PathFindingTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        Planet.PlanetState Planet;
        AgentEntity Agent;

        static bool Init = false;

        public void Start()
        {
            Debug.Log("Click somewhere to set slime target gol.");
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

  
         if (Input.GetKeyDown(KeyCode.Mouse0))
         {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vec2f goalPos = new Vec2f(worldPosition.x, worldPosition.y);
                GameState.ActionCreationSystem.CreateMovementAction(Planet.EntitasContext, Enums.ActionType.MoveAction,
                   Agent.agentID.ID, goalPos);
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

            Agent = Planet.AddEnemy(new Vec2f(1.0f, 3.0f));
        }

        void GenerateMap()
        {
            ref var tileMap = ref Planet.TileMap;

            for (int j = 0; j < tileMap.MapSize.Y; j++)
            {
                for (int i = 0; i < tileMap.MapSize.X; i++)
                {
                    TileMaterialType frontTile;

                    if (i >= tileMap.MapSize.X / 2)
                    {
                        if (j % 2 == 0 && i == tileMap.MapSize.X / 2)
                        {
                            frontTile = TileMaterialType.Moon;
                        }
                        else
                        {
                            frontTile = TileMaterialType.Glass;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == tileMap.MapSize.X / 2 + 1)
                        {
                            frontTile = TileMaterialType.Glass;
                        }
                        else
                        {
                            frontTile = TileMaterialType.Moon;
                        }
                    }

                    if (j is > 1 and < 6 || (j > 8 + i))
                    {
                        frontTile = TileMaterialType.Air;
                    }


                    tileMap.SetFrontTile(i, j, frontTile);
                }
            }
        }
    }
}