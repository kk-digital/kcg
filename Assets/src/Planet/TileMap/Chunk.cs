namespace Planet.TileMap
{
    public class Chunk
    {
        // Makes sorting function much easier and faster
        // This is the index of this chunk in the ChunkIndexList
        public int ChunkIndexListID;
        public Tile.Model[][] Tiles;

        public int Usage; // Used for sorting chunks by usage
        public int Seq;

        public Chunk()
        {
            Seq = 0;
        }
    }
}
