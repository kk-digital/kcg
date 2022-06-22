using Enums.Tile;
using KMath;
using UnityEngine;

namespace Planet.Unity
{
    class TileVariantTest : MonoBehaviour
    {
        [SerializeField] Material Material;
        
        Planet.TileMap TileMap;

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
                TileMap.RemoveTile(x, y, Enums.Tile.MapLayerType.Front);
                //TileMap.Layers.BuildLayerTexture(TileMap, Enums.Tile.MapLayerType.Front);
                
            }

            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            TileMap.Layers.DrawLayer(TileMap, Enums.Tile.MapLayerType.Front, Instantiate(Material), transform, 10);
        }

        // create the sprite atlas for testing purposes
        public void Initialize()
        {
            int tilesMoon = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png", 16, 16);
            int oreTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);
            
            GameState.TileCreationApi.CreateTile(8);
            GameState.TileCreationApi.SetTileName("ore_1");
            GameState.TileCreationApi.SetTileTexture16(oreTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(9);
            GameState.TileCreationApi.SetTileName("glass");
            GameState.TileCreationApi.SetTileSpriteSheet16(tilesMoon, 11, 10);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(10);
            GameState.TileCreationApi.SetTileName("moon");
            GameState.TileCreationApi.SetTileSpriteSheet16(tilesMoon, 0, 0);
            GameState.TileCreationApi.EndTile();



            // Generating the map
            var mapSize = new Vec2i(16, 16);

            TileMap = new TileMap(mapSize);

            for(int j = TileMap.Borders.IntBottom; j < TileMap.Borders.IntTop; j++)
            {
                for(int i = TileMap.Borders.IntLeft; i < TileMap.Borders.IntRight; i++)
                {
                    var frontTile = new Tile.Tile(new Vec2f(i, j));
                    var oreTile = new Tile.Tile(new Vec2f(i, j));

                    if (i >= mapSize.X / 2)
                    {
                        if (j % 2 == 0 && i == mapSize.X / 2)
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
                        if (j % 3 == 0 && i == mapSize.X / 2 + 1)
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

                    if (j is > 1 and < 6 || (j > (8 + i)))
                    {
                       frontTile.Type = -1; 
                       oreTile.Type = -1;
                    }


                    TileMap.SetTile(ref frontTile, MapLayerType.Front);
                }
            }

            TileMap.UpdateTileMapPositions(Enums.Tile.MapLayerType.Front);

            //TileMap.Layers.BuildLayerTexture(TileMap, Enums.Tile.MapLayerType.Front);
            //TileMap.Layers.BuildLayerTexture(TileMap, Enums.Tile.MapLayerType.Ore);
        }
        
    }
}


