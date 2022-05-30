using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TileProperties;
using Enums;
using UnityEngine;

namespace Tiles.PlanetMap
{
    public class TilesPlanetMap
    {
        public struct ChunkList
        {
            public Vector2Int Size;

            public TPMChunk[] List;
            public int[] IndexList; // 0 = error, 1 = empty, 2 = unexplored (TODO)

            public int Next;

            public TPMChunk Error; // todo: fill this with error tiles
            public TPMChunk Empty;
        }

        public static readonly PlanetTile AirTile = new();

        public Vector2Int Size;
        public ChunkList Chunks;
        public readonly float TileSize = 1.0f;

        public PlanetWrapBehavior WrapBehavior;

        public TilesPlanetMap(Vector2Int size)
        {
            Size.x = size.x;
            Size.y = size.y;

            Chunks.Size.x = Size.x >> 4;
            Chunks.Size.y = Size.y >> 4;

            WrapBehavior = PlanetWrapBehavior.NoWrapAround; // Set to WrapAround manually if needed

            Chunks.Next = 0;

            Chunks.IndexList = new int[Chunks.Size.x * Chunks.Size.y];
            Chunks.List = new TPMChunk[Chunks.Size.x * Chunks.Size.y];

            for (int i = 0; i < Chunks.IndexList.Length; i++)
                Chunks.IndexList[i] = 2;
        }

        #region Updater
        
        public void UpdateMap()
        {

        }
        
        // TODO: Move out from here

        #endregion

        // Is this really the only way to inline a function in c#?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetChunkIndex(int x, int y)
        {
            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;

            return Chunks.IndexList[(x >> 4) * Chunks.Size.y + (y >> 4)];
        }

        private int AddChunk(TPMChunk chunk, int x, int y)
        {
            // I feel like resizing by 1 each time is not very efficient... Change it later?
            Array.Resize(ref Chunks.List, Chunks.Next + 1);

            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;
            chunk.ChunkIndexListID = (x >> 4) * Chunks.Size.y + (y >> 4);

            Chunks.IndexList[chunk.ChunkIndexListID] = Chunks.Next + 3;
            Chunks.List[Chunks.Next] = chunk;
            Chunks.Next++;
            return Chunks.Next + 2;
        }

        public TPMChunk GetChunk(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0: return Chunks.Error;
                case 1: return Chunks.Empty;
                case 2: return Chunks.Empty; // UNEXPLORED
            }

            Chunks.List[chunkIndex - 3].Usage++;
            return Chunks.List[chunkIndex - 3];
        }

        public ref TPMChunk GetChunkRef(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);

            switch (chunkIndex)
            {
                case 0:
                    throw new IndexOutOfRangeException();
                // We are getting a reference here, most likely to edit the chunk / add a tile, so we can't just return an empty chunk
                // Instead, we will just create a new chunk
                case < 3:
                    chunkIndex = AddChunk(new TPMChunk(), x, y);
                    break;
            }

            Chunks.List[chunkIndex - 3].Usage++;
            return ref Chunks.List[chunkIndex - 3];
        }

        public void SetChunk(int x, int y, PlanetTile[,] tiles)
        {
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0:
                    return;
                case < 3:
                    chunkIndex = AddChunk(new TPMChunk(), x, y);
                    break;
            }

            Chunks.List[chunkIndex - 3].Seq++;

            for (int i = 0; i < 16; i++)
            for (int j = 0; j < 16; j++)
                Chunks.List[chunkIndex - 3].Tiles[i, j] = tiles[i, j];
        }

        public ref PlanetTile GetTileRef(int x, int y)
        {
            ref TPMChunk chunk = ref GetChunkRef(x, y);

            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        public PlanetTile GetTile(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);
            return chunkIndex == 1 ? AirTile : Chunks.List[chunkIndex - 3].Tiles[x & 0x0F, y & 0x0f];
        }

        public void SetTile(int x, int y, PlanetTile tile)
        {
            ref TPMChunk chunk = ref GetChunkRef(x, y);
            chunk.Seq++; // Updating tile, increment seq
            chunk.Tiles[x & 0x0F, y & 0x0F] = tile;
        }

        public ref PlanetTile getTile(int x, int y)
        {
            ref TPMChunk chunk = ref GetChunkRef(x, y);
            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        // Sort chunks by most used using quick sort
        private void swap(int index1, int index2)
        {
            // Swap chunks
            (Chunks.List[index1], Chunks.List[index2]) = (Chunks.List[index2], Chunks.List[index1]);

            // Then update chunk index list - This is what storing the Position inside the chunk is most useful for
            Chunks.IndexList[Chunks.List[index1].ChunkIndexListID] = index1 + 3;
            Chunks.IndexList[Chunks.List[index2].ChunkIndexListID] = index2 + 3;
        }

        private int partition(int start, int end)
        {
            // Use negative of the usage to have the list sorted from most used to least used without having to reverse afterwards
            int p = -Chunks.List[start].Usage;

            int count = 0;
            for (int k = start + 1; k <= end; k++)
                if (-Chunks.List[k].Usage <= p)
                    count++;

            int pi = start + count;
            swap(pi, start);

            int i = start, j = end;

            while (i < pi && j > pi)
            {
                while (-Chunks.List[i].Usage <= p) i++;
                while (-Chunks.List[j].Usage > p) j--;

                if (i < pi && j > pi)
                    swap(i++, j--);
            }

            return pi;
        }

        private void quickSort(int start, int end)
        {
            if (start >= end) return;

            int p = partition(start, end);
            quickSort(start, p - 1);
            quickSort(p + 1, end);
        }

        public void SortChunks()
        {
            // Sort chunks from most used to least used
            if (Chunks.List == null || Chunks.List.Length == 0) return;

            quickSort(0, Chunks.Next - 1);
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
