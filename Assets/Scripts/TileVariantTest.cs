using Enums;
using UnityEngine;

namespace Planet.Unity
{
    class TileVariantTest : MonoBehaviour
    {
        [SerializeField] Material Material;


        Contexts EntitasContext = Contexts.sharedInstance;
        TileMap.Model TileMap;

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
                Debug.Log(x + " " + y);
                TileMap.RemoveTile(x, y, PlanetLayer.Front);
                TileMap.Layers.BuildLayerTexture(ref TileMap, PlanetLayer.Front);
                
            }

            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            TileMap.Layers.DrawLayer(PlanetLayer.Front, Instantiate(Material), transform, 10);
            TileMap.Layers.DrawLayer(PlanetLayer.Ore, Instantiate(Material), transform, 11);
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

            TileMap = new TileMap.Model(mapSize);

            for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    Tile.Model frontTile = Tile.Model.EmptyTile();
                    Tile.Model oreTile = Tile.Model.EmptyTile();

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

                    
                    TileMap.SetTile(i, j, frontTile, PlanetLayer.Front);
                    TileMap.SetTile(i, j, oreTile, PlanetLayer.Ore);
                }
            }

            TileMap.HeightMap.UpdateTopTilesMap(ref TileMap);

            TileMap.UpdateTileMapPositions(PlanetLayer.Front);
            TileMap.UpdateTileMapPositions(PlanetLayer.Ore);

            TileMap.Layers.BuildLayerTexture(ref TileMap, PlanetLayer.Front);
            TileMap.Layers.BuildLayerTexture(ref TileMap, PlanetLayer.Ore);
        }
        
    }
}


