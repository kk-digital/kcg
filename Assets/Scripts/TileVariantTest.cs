using UnityEngine;
using TileProperties;

using System.Collections.Generic;

namespace PlanetTileMap.Unity
{
    class TileVariantTest : MonoBehaviour
    {
        [SerializeField] Material Material;


        Contexts EntitasContext = Contexts.sharedInstance;
        PlanetTileMap TileMap;

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
                TileMap.RemoveTile(x, y, Layer.Front);
                TileMap.BuildLayerTexture(Layer.Front);
                
            }

            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            TileMap.DrawLayer(Layer.Front, Instantiate(Material), transform, 10);
            TileMap.DrawLayer(Layer.Ore, Instantiate(Material), transform, 11);
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

            TileMap = new PlanetTileMap(mapSize);

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
                            frontTile.PropertiesId = 10;
                        }
                        else
                        {
                            frontTile.PropertiesId = 9;
                        }
                    }
                    else
                    {
                        if (j % 3 == 0 && i == mapSize.x / 2 + 1)
                        {
                            frontTile.PropertiesId = 9;
                        }
                        else
                        {
                            frontTile.PropertiesId = 10;
                        }
                    }


                    if (i % 10 == 0)
                    {
                        //tile.FrontTilePropertiesId = 7;
                        oreTile.PropertiesId = 8;
                    }
                    if (j % 2 == 0)
                    {
                       // tile.FrontTilePropertiesId = 2;
                    }
                    if (j % 3 == 0)
                    {
                       // tile.FrontTilePropertiesId = 9;

                    }

                    if ((j > 1 && j < 6) || (j > (8 + i)))
                    {
                       frontTile.PropertiesId = -1; 
                       oreTile.PropertiesId = -1;
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


