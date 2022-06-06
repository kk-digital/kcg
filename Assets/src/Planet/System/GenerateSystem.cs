using TileProperties;
using UnityEngine;

namespace Planet
{
    public class GenerateSystem
    {
        public static readonly GenerateSystem Instance;

        static GenerateSystem()
        {
            Instance = new GenerateSystem();
        }

        public PlanetTileMap.PlanetTileMap GenerateTileMap()
        {
            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);

            var tileMap = new PlanetTileMap.PlanetTileMap(mapSize);

            for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    PlanetTile tile = PlanetTile.EmptyTile();
                    tile.FrontTilePropertiesId = 9;


                    if (i % 10 == 0)
                    {
                        //tile.FrontTilePropertiesId = 7;
                        tile.OreTilePropertiesId = 8;
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
                        tile.FrontTilePropertiesId = -1; 
                        tile.OreTilePropertiesId = -1;
                    }

                    
                    tileMap.SetTile(i, j, tile);
                }
            }

            tileMap.UpdateTopTilesMap();
            tileMap.UpdateAllTileVariants(PlanetTileMap.Layer.Front);
            tileMap.UpdateAllTileVariants(PlanetTileMap.Layer.Ore);
            tileMap.BuildLayerTexture(PlanetTileMap.Layer.Front);
            tileMap.BuildLayerTexture(PlanetTileMap.Layer.Ore);

            return tileMap;
        }
    }
}

