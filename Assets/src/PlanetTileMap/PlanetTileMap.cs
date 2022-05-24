//using Entitas;
using System;
using TileProperties;

namespace PlanetTileMap
{
    //public struct PlanetMap : IComponent
    public struct PlanetTileMap
    {
        public int Xsize;
        public int Ysize;

        public PlanetTileMapChunk[] ChunkList;
        public int[] ChunkIndexList; // 0 = error, 1 = empty, 2 = unexplored (TODO)

        private int NextChunk;

        private static PlanetTileMapChunk ErrorChunk = new PlanetTileMapChunk(); // todo: fill this with error tiles
        private static PlanetTileMapChunk EmptyChunk = new PlanetTileMapChunk();

        public PlanetTileMap(int xsize, int ysize) : this()
        {
            Xsize = xsize;
            Ysize = ysize;

            NextChunk = 0;

            ChunkIndexList = new int[Xsize / 16 * Ysize / 16];

            for (int i = 0; i < ChunkIndexList.Length; i++)
                ChunkIndexList[i] = 2;
        }

        private int GetChunkIndex(int x, int y)
        {
            // This feels wrong (x * Xsize + y) / 16
            // Shouldn't it be  (x * Ysize + y) / 16?
            return ChunkIndexList[(x * Ysize + y) >> 4];
        }

        private int AddChunk(PlanetTileMapChunk chunk, int x, int y)
        {
            // I feel like resizing by 1 each time is not very efficient... Change it later?
            Array.Resize(ref ChunkList, NextChunk + 1);

            chunk.Position = (x * Ysize + y) >> 4;
            ChunkIndexList[chunk.Position] = NextChunk + 3;
            ChunkList[NextChunk - 3] = chunk;
            NextChunk++;
            return NextChunk - 4;
        }

        public PlanetTileMapChunk GetChunk(int x, int y)
        {
            int ChunkIndex = GetChunkIndex(x, y);
            switch (ChunkIndex)
            {
                case 0: return ErrorChunk;
                case 1: return EmptyChunk;
                case 2: return EmptyChunk; // UNEXPLORED
            }

            ChunkList[ChunkIndex - 3].Usage++;
            return ChunkList[ChunkIndex - 3];
        }

        public ref PlanetTileMapChunk GetChunkRef(int x, int y)
        {
            int ChunkIndex = GetChunkIndex(x, y);
            if (ChunkIndex == 0) return ref ErrorChunk;
            if (ChunkIndex < 3)
                // We are getting a reference here, most likely to edit the chunk / add a tile, so we can't just return an empty chunk
                // Instead, we will just create a new chunk
                ChunkIndex = AddChunk(new PlanetTileMapChunk(), x, y);

            ChunkList[ChunkIndex - 3].Usage++;
            return ref ChunkList[ChunkIndex - 3];
        }

        public void SetChunk(int x, int y, PlanetTile[,] tiles)
        {
            int ChunkIndex = GetChunkIndex(x, y);
            if (ChunkIndex == 0) return;
            if (ChunkIndex < 3) ChunkIndex = AddChunk(new PlanetTileMapChunk(), x, y);

            ChunkList[ChunkIndex - 3].Seq++;

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    ChunkList[ChunkIndex - 3].Tiles[i, j] = tiles[i, j];
        }

        public ref PlanetTile GetTileRef(int x, int y)
        {
            ref PlanetTileMapChunk chunk = ref GetChunkRef(x, y);
            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        public PlanetTile GetTile(int x, int y)
        {
            return GetChunk(x, y).Tiles[x & 0x0F, y & 0x0f];
        }

        public void SetTile(int x, int y, PlanetTile tile)
        {
            ref PlanetTileMapChunk chunk = ref GetChunkRef(x, y);
            chunk.Seq++; // Updating tile, increment seq
            chunk.Tiles[x & 0x0F, y & 0x0F] = tile;
        }
    

        //Take in PlanetTileMap, and map a horizonal line
    public void GenerateFlatPlanet()
        {
            //default size = X...

            //make a single line horizonally across planet
            //from left to right

            //int TileId = GetTileId("default-tile")
           //for x = 0 to x = Planet.Size.X
           //Planet.SetTile(TileId, x, 10)


        }
    }
}