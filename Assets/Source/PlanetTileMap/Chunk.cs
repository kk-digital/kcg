using Enums.Tile;

namespace PlanetTileMap
{
    public struct Chunk
    {
        public int ChunkIndex;
        public MapChunkType Type;

        public Tile[] TileArray;

        public bool NeedsUpdate; //set to true if setting tile. Flag cleared after mesh reconstructed
        public int ReadCount; //increment if GetTile
        public int Sequence;  //increment if SetTile
    }
}
