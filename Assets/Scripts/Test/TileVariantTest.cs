using Enums.Tile;
using KMath;
using UnityEngine;

namespace Planet.Unity
{
    class TileVariantTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        static bool Init = false;
        public PlanetState Planet;
        

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
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                Debug.Log(x + " " + y);
                Planet.TileMap.RemoveTile(x, y, Enums.Tile.MapLayerType.Front);                
            }

            Planet.TileMap.UpdateLayerMesh(MapLayerType.Front);
            Planet.TileMap.DrawLayer(MapLayerType.Front);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            GameResources.Initialize();

            // Generating the map
            var mapSize = new Vec2i(16, 16);

            Planet = new PlanetState();
            Planet.Init(mapSize);
            Planet.InitializeSystems(Material, transform);

            ref var tileMap = ref Planet.TileMap;

            for(int j = 0; j < tileMap.MapSize.Y; j++)
            {
                for(int i = 0; i < tileMap.MapSize.X; i++)
                {
                    var frontTile = TileID.Air;
                    var oreTile = TileID.Air;

                    if (i >= mapSize.X / 2)
                    {
                        if (j % 2 == 0 && i == mapSize.X / 2)
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
                        if (j % 3 == 0 && i == mapSize.X / 2 + 1)
                        {
                            frontTile = TileID.Glass;
                        }
                        else
                        {
                            frontTile = TileID.Moon;
                        }
                    }

                    if (i % 10 == 0)
                    {
                        oreTile = TileID.Ore1;
                    }

                    if (j is > 1 and < 6 || (j > (8 + i)))
                    {
                       frontTile = TileID.Air; 
                       oreTile = TileID.Air;
                    }


                    Planet.TileMap.SetTile(i,j, frontTile, MapLayerType.Front);
                }
            }
            Planet.TileMap.UpdateTileMapPositions(Enums.Tile.MapLayerType.Front);
        }
    }
}
