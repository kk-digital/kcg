using Enums;
using UnityEngine;

namespace TileMap
{
    public class GenerateSystem
    {
        public static readonly GenerateSystem Instance;

        static GenerateSystem()
        {
            Instance = new GenerateSystem();
        }

        public TileMap.Component GenerateTileMap()
        {
            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);

            var tileMap = new TileMap.Component(mapSize);

            for(int j = 0; j < mapSize.y; j++)
            {
                for(int i = 0; i < mapSize.x; i++)
                {
                    var tile = Tile.Component.EmptyTile();
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
            tileMap.UpdateAllTileVariants(PlanetLayer.Front);
            tileMap.UpdateAllTileVariants(PlanetLayer.Ore);
            tileMap.BuildLayerTexture(PlanetLayer.Front);
            tileMap.BuildLayerTexture(PlanetLayer.Ore);

            return tileMap;
        }
    }
}

