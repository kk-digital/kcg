using UnityEngine;
using Enums.Tile;
using KMath;
using Item;
using Animancer;
using PlanetTileMap;

namespace Planet.Unity
{
    class MovementSceneScript : MonoBehaviour
    {
        [SerializeField] Material Material;

        public PlanetState Planet;

        AgentEntity Player;
        int PlayerID;

        int CharacterSpriteId;
        int inventoryID;
        int toolBarID;

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
            
            Planet.Update(Time.deltaTime, Material, transform);
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


            GameResources.Initialize();

            // Generating the map
            Vec2i mapSize = new Vec2i(128, 24);
            Planet = new Planet.PlanetState();
            Planet.Init(mapSize);

            Planet.InitializeSystems(Material, transform);
            GenerateMap();
            var camera = Camera.main;
            Vector3 lookAtPosition = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));

            Planet.TileMap.UpdateBackTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
            Planet.TileMap.UpdateMidTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
            Planet.TileMap.UpdateFrontTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);

            Player = Planet.AddPlayer(new Vec2f(3.0f, 20));
            PlayerID = Player.agentID.ID;

            //TileMapManager.Save(Planet.TileMap, "movement-map.kmap");
        }



        void GenerateMap()
        {
            KMath.Random.Mt19937.init_genrand((ulong) System.DateTime.Now.Ticks);
            
            ref var tileMap = ref Planet.TileMap;


            for(int j = 0; j < tileMap.MapSize.Y / 2; j++)
            {
                for(int i = 0; i < tileMap.MapSize.X; i++)
                {
                    tileMap.GetFrontTile(i, j).ID = TileID.Moon;
                    tileMap.GetBackTile(i, j).ID = TileID.Background;
                }
            }

            

            for(int i = 0; i < tileMap.MapSize.X; i++)
            {
                tileMap.GetFrontTile(i, 0).ID = TileID.Bedrock;
                tileMap.GetFrontTile(i, tileMap.MapSize.Y - 1).ID = TileID.Bedrock;
            }

            for(int j = 0; j < tileMap.MapSize.Y; j++)
            {
                tileMap.GetFrontTile(0, j).ID = TileID.Bedrock;
                tileMap.GetFrontTile(tileMap.MapSize.X - 1, j).ID = TileID.Bedrock;
            }

            var camera = Camera.main;
            Vector3 lookAtPosition = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));

            tileMap.UpdateBackTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
            tileMap.UpdateMidTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
            tileMap.UpdateFrontTileMapPositions((int)lookAtPosition.x, (int)lookAtPosition.y);
        }

    }
}
