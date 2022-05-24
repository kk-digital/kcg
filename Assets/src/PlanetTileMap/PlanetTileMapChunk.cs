using TileProperties;

namespace PlanetTileMap
{
    public class PlanetTileMapChunk
    {
        public PlanetTile[,] Tiles;

        public int Position; // Makes sorting function much easier and faster

        public int Usage; // Used for sorting chunks by usage
        public int Seq;

        public PlanetTileMapChunk()
        {
            Tiles = new PlanetTile[16, 16];
            Seq = 0;
        }
    }
}
