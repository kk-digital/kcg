using TileProperties;

namespace PlanetTileMap
{
    public class PlanetTileMapChunk
    {
        public PlanetTile[,] Tiles;

        // Makes sorting function much easier and faster
        // This is the index of this chunk in the ChunkIndexList
        public int ChunkIndexListID; 

        public int Usage; // Used for sorting chunks by usage
        public int Seq;

        public PlanetTileMapChunk()
        {
            Tiles = new PlanetTile[16, 16];
            
           /* for(int i = 0; i < 16; i++)
            {
                for(int j = 0; j < 16; j++)
                {
                    Tiles[j, i] = PlanetTile.EmptyTile();
                }
            }*/

            Seq = 0;
        }
    }
}
