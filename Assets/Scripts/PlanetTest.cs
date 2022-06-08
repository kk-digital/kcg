using UnityEngine;
using TileProperties;
using PlanetTileMap;
using System.Collections.Generic;

namespace Planet.Unity
{
    class PlanetTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        Planet Planet;

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
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                Planet.TileMap.RemoveTile(x, y, Layer.Front);
                Planet.TileMap.BuildLayerTexture(Layer.Front);
                
            }

            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            GameState.ProcessSystem.Update();
            GameState.MovableSystem.Update();
            GameState.CollisionSystem.Update(Planet.TileMap);
            Planet.TileMap.DrawLayer(Layer.Front, Instantiate(Material), transform, 10);
            Planet.TileMap.DrawLayer(Layer.Ore, Instantiate(Material), transform, 11);
            GameState.DrawSystem.Draw(Instantiate(Material), transform, 12);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            int TilesMoon = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png");
            int OreTileSheet = 
            GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png");
            
            GameState.TileCreationApi.CreateTile(8);
            GameState.TileCreationApi.SetTileName("ore_1");
            GameState.TileCreationApi.SetTileTexture16(OreTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(9);
            GameState.TileCreationApi.SetTileName("glass");
            GameState.TileCreationApi.SetTileSpriteSheet16(TilesMoon, 11, 10);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(10);
            GameState.TileCreationApi.SetTileName("moon");
            GameState.TileCreationApi.SetTileSpriteSheet16(TilesMoon, 0, 0);
            GameState.TileCreationApi.EndTile();



            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);
            Planet = new Planet(mapSize);
            GenerateMap();


            GameState.SpawnerSystem.SpawnPlayer(Material);
        }




        void GenerateMap()
        {
            PlanetTileMap.PlanetTileMap TileMap = Planet.TileMap;

           Vector2Int mapSize = TileMap.Size;

           for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    PlanetTile frontTile = PlanetTile.EmptyTile();
                    PlanetTile oreTile = PlanetTile.EmptyTile();

                    if (i >= mapSize.x / 2)
                    {
                        if (j % 2 == 0 && i == mapSize.x / 2)
                        {
                            frontTile.TileType = 10;
                        }
                        else
                        {
                            frontTile.TileType = 9;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == mapSize.x / 2 + 1)
                        {
                            frontTile.TileType = 9;
                        }
                        else
                        {
                            frontTile.TileType = 10;
                        }
                    }


                    if (i % 10 == 0)
                    {
                        oreTile.TileType = 8;
                    }

                    if ((j > 1 && j < 6) || (j > (8 + i)))
                    {
                       frontTile.TileType = -1; 
                       oreTile.TileType = -1;
                    }

                    
                    TileMap.SetTile(i, j, frontTile, Layer.Front);
                    TileMap.SetTile(i, j, oreTile, Layer.Ore);
                }
            }

            TileMap.UpdateTopTilesMap();

            TileMap.UpdateAllTilePositions(Layer.Front);
            TileMap.UpdateAllTilePositions(Layer.Ore);

            TileMap.BuildLayerTexture(Layer.Front);
            TileMap.BuildLayerTexture(Layer.Ore);
        
        }
        
    }
}


