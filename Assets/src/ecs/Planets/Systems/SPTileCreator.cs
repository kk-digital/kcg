using Tiles.PlanetMap;
using TileProperties;

namespace Systems.Planets
{
    public class SPTileCreator
    {
        private static SPTileCreator instance;
        public static SPTileCreator Instance => instance ??= new SPTileCreator();
        
        public void CreateTiles(ref TilesPlanetMap tilesPlanetMap)
        {
            for(int j = 0; j < tilesPlanetMap.Size.y; j++)
            {
                for(int i = 0; i < tilesPlanetMap.Size.x; i++)
                {
                    var tile = PlanetTile.EmptyTile();
                    tile.TileIdPerLayer[0] = 0;
                    if (i % 10 == 0)
                    {
                        tile.TileIdPerLayer[0] = 7;
                    }
                    if (j % 2 == 0)
                    {
                        tile.TileIdPerLayer[0] = 2;
                    }
                    if (j % 3 == 0)
                    {
                        tile.TileIdPerLayer[0] = 1;
                    }

                    if ((j > 1 && j < 6) || j > 10)
                    {
                        tile.TileIdPerLayer[0] = -1; 
                    }

                    
                    tilesPlanetMap.SetTile(i, j, tile);
                }
            }
        }
    }
}
