using System;
using Enums;
using UnityEngine;

namespace Planet.TileMap
{
    public struct ChunkList
    {
        public Vector2Int Size;

        public Chunk[] Data;
        // TODO: 0 = error, 1 = empty, 2 = unexplored
        public PlanetWrapBehavior WrapBehavior;

        public Chunk Error; // todo: fill this with error tiles
        public Chunk Empty;
        
        private int AddChunk(Chunk chunk, int x, int y)
        {
            var chunkCount = Data.Length;
            
            // I feel like resizing by 1 each time is not very efficient... Change it later?
            Array.Resize(ref Data, chunkCount + 1);

            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;
            chunk.ChunkIndexListID = (x >> 4) * Size.y + (y >> 4);
            
            Data[chunkCount] = chunk;
            return chunkCount;
        }
        
        // Is this really the only way to inline a function in c#?
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetChunkIndex(int x, int y)
        {
            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;

            return (x >> 4) * Size.y + (y >> 4);
        }
        
        public Chunk GetChunk(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0: return Error;
                case 1: return Empty;
                case 2: return Empty; // UNEXPLORED
            }

            Data[chunkIndex - 3].Usage++;
            return Data[chunkIndex - 3];
        }
        
        public ref Chunk GetChunkRef(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);

            switch (chunkIndex)
            {
                case 0:
                    throw new IndexOutOfRangeException();
                // We are getting a reference here, most likely to edit the chunk / add a tile, so we can't just return an empty chunk
                // Instead, we will just create a new chunk
                case < 3:
                    chunkIndex = AddChunk(new Chunk(), x, y);
                    break;
            }

            Data[chunkIndex - 3].Usage++;
            return ref Data[chunkIndex - 3];
        }
        
        //TODO: Implement
        public void SetChunk(int x, int y, Tile.Model[][,] tiles)
        {
            var layersCount = Enum.GetNames(typeof(PlanetLayer)).Length;
            
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0:
                    return;
                case < 3:
                    chunkIndex = AddChunk(new Chunk(), x, y);
                    break;
            }

            Data[chunkIndex].Seq++;

            int beginX = (int)(x / Size.x) * Size.x;
            int beginY = (int)(y / Size.y) * Size.y;

        
            for(int layerIndex = 0; layerIndex < layersCount; layerIndex++)
            {
                //for (int i = 0; i < 16; i++)
                //for (int j = 0; j < 16; j++)
                    //Data[layerIndex].Tiles[(i + beginX) + (j + beginY) * Size.x] = tiles[layerIndex][i, j];
            }
        }
    }
}
