namespace TileMap
{
    public class Chunk
    {
        public Tile.Component[,] Tiles;

        // Makes sorting function much easier and faster
        // This is the index of this chunk in the ChunkIndexList
        public int ChunkIndexListID; 

        public int Usage; // Used for sorting chunks by usage
        public int Seq;

        public Chunk()
        {
            Tiles = new Tile.Component[16, 16];
            Seq = 0;
        }
        
        public struct NaturalLayerChunk
        {
            int Property;
            // chunk properties
        }
    }
}
