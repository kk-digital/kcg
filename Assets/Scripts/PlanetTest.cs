using UnityEngine;
using System.Collections.Generic;
using Enums.Tile;

namespace Planet.Unity
{
    class PlanetTest : MonoBehaviour
    {
        [SerializeField] Material Material;

        Planet.PlanetState Planet;

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
            TileMap.Model TileMap = Planet.TileMap;

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                TileMap.RemoveTile(x, y, MapLayerType.Front);
                TileMap.BuildLayerTexture(MapLayerType.Front);
                
            }

            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            GameState.ProcessSystem.Update();
            GameState.MovableSystem.Update();
            GameState.CollisionSystem.Update(TileMap);
            
            TileMap.Layers.DrawLayer(MapLayerType.Front, Instantiate(Material), transform, 10);
            TileMap.Layers.DrawLayer(MapLayerType.Ore, Instantiate(Material), transform, 11);
            GameState.DrawSystem.Draw(Instantiate(Material), transform, 12);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            int TilesMoon = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png");
            int OreTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png");
            
            GameState.CreationApi.CreateTile(8);
            GameState.CreationApi.SetTileName("ore_1");
            GameState.CreationApi.SetTileTexture16(OreTileSheet, 0, 0);
            GameState.CreationApi.EndTile();

            GameState.CreationApi.CreateTile(9);
            GameState.CreationApi.SetTileName("glass");
            GameState.CreationApi.SetTileSpriteSheet16(TilesMoon, 11, 10);
            GameState.CreationApi.EndTile();

            GameState.CreationApi.CreateTile(10);
            GameState.CreationApi.SetTileName("moon");
            GameState.CreationApi.SetTileSpriteSheet16(TilesMoon, 0, 0);
            GameState.CreationApi.EndTile();



            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);
            Planet = new Planet.PlanetState(mapSize);
            GenerateMap();


            Planet.AddPlayer(Instantiate(Material), new Vector2(3.0f, 3.0f));
            Planet.AddAgent(Instantiate(Material), new Vector2(6.0f, 2.0f));
        }




        void GenerateMap()
        {
            TileMap.Model TileMap = Planet.TileMap;

           Vector2Int mapSize = TileMap.MapSize;

           for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    Tile.Model frontTile = Tile.Model.EmptyTile;
                    Tile.Model oreTile = Tile.Model.EmptyTile;

                    if (i >= mapSize.x / 2)
                    {
                        if (j % 2 == 0 && i == mapSize.x / 2)
                        {
                            frontTile.Type = 10;
                        }
                        else
                        {
                            frontTile.Type = 9;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == mapSize.x / 2 + 1)
                        {
                            frontTile.Type = 9;
                        }
                        else
                        {
                            frontTile.Type = 10;
                        }
                    }


                    if (i % 10 == 0)
                    {
                        oreTile.Type = 8;
                    }

                    if ((j > 1 && j < 6) || (j > (8 + i)))
                    {
                       frontTile.Type = -1; 
                       oreTile.Type = -1;
                    }

                    
                    TileMap.SetTile(i, j, frontTile, MapLayerType.Front);
                    TileMap.SetTile(i, j, oreTile, MapLayerType.Ore);
                }
            }

            TileMap.HeightMap.UpdateTopTilesMap(ref TileMap);

            TileMap.UpdateTileMapPositions(MapLayerType.Front);
            TileMap.UpdateTileMapPositions(MapLayerType.Ore);
            TileMap.BuildLayerTexture(MapLayerType.Front);
            TileMap.BuildLayerTexture(MapLayerType.Ore);
        
        }
        
    }
}


