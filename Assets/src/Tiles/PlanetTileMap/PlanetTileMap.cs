//using Entitas;
using System;
using System.Runtime.CompilerServices;
using TileProperties;
using Enums;
using UnityEngine;

namespace PlanetTileMap
{
    public struct TopTilesMap
    {
        public int[] Data;

        // more attributes if needed

        public int Width
        {
            get
            {
                return Data.Length;
            }
        }

        public TopTilesMap(int width)
        {
            Data = new int[width];
        }
    }

    public struct PlanetTileMap
    {

        public struct ChunkBehaviour
        {
            public Vector2Int Size;

            public PlanetTileMapChunk[] List;
            public int[] IndexList; // 0 = error, 1 = empty, 2 = unexplored (TODO)

            public int Next;

            public PlanetTileMapChunk Error; // todo: fill this with error tiles
            public PlanetTileMapChunk Empty;
        }
        
        // public static const PlanetTile AirTile = new PlanetTile(); - PlanetTile cannot be const in c#?
        public static readonly PlanetTile AirTile = new();

        public Vector2Int Size;
        public ChunkBehaviour Chunk;
        public TopTilesMap TopTilesMap;
        public PlanetWrapBehavior WrapBehavior;

        public Vector2Int NaturalLayerChunkSize;
        public Vector2Int NaturalLayerSize;

        public NaturalLayerChunk[] NaturalLayerChunkList;
        public int[] OreMap;

        public PlanetTileMap(Vector2Int size) : this()
        {
            NaturalLayerChunkSize = new Vector2Int(16, 16);

            Size.x = size.x;
            Size.y = size.y;

            Chunk.Size.x = Size.x >> 4;
            Chunk.Size.y = Size.y >> 4;

            WrapBehavior = PlanetWrapBehavior.NoWrapAround; // Set to WrapAround manually if needed

            Chunk.Next = 0;

            Chunk.IndexList = new int[Chunk.Size.x * Chunk.Size.y];
            Chunk.List = new PlanetTileMapChunk[Chunk.Size.x * Chunk.Size.y];

            TopTilesMap = new TopTilesMap(Size.x);
            OreMap = new int[Size.x * Size.y];
            NaturalLayerSize = new Vector2Int(Size.x / NaturalLayerChunkSize.x + 1, Size.y / NaturalLayerChunkSize.y + 1);
            NaturalLayerChunkList = new NaturalLayerChunk[NaturalLayerSize.x * NaturalLayerSize.y];

            for (int i = 0; i < Chunk.IndexList.Length; i++)
                Chunk.IndexList[i] = 2;
        }
        


        public void UpdateTopTilesMap()
        {
            for(int i = 0; i < Size.x; i++)
            {
                TopTilesMap.Data[i] = 0;
                for(int j = Size.y - 1; j >= 0; j--)
                {
                    ref PlanetTile tile = ref GetTileRef(i, j);
                    if (tile.Initialized || tile.BackTilePropertiesId != -1)
                    {
                        TopTilesMap.Data[i] = j;
                        break;
                    }
                }
            }
        }

        public ref NaturalLayerChunk GetNaturalLayerChunk(int x, int y)
        {
            int index = x / NaturalLayerChunkSize.x + (y / NaturalLayerChunkSize.y) * NaturalLayerSize.x;

            return ref NaturalLayerChunkList[index];
        }

        // Is this really the only way to inline a function in c#?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetChunkIndex(int x, int y)
        {
            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;

            return Chunk.IndexList[(x >> 4) * Chunk.Size.y + (y >> 4)];
        }

        private int AddChunk(PlanetTileMapChunk chunk, int x, int y)
        {
            // I feel like resizing by 1 each time is not very efficient... Change it later?
            Array.Resize(ref Chunk.List, Chunk.Next + 1);

            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;
            chunk.ChunkIndexListID = (x >> 4) * Chunk.Size.y + (y >> 4);

            Chunk.IndexList[chunk.ChunkIndexListID] = Chunk.Next + 3;
            Chunk.List[Chunk.Next] = chunk;
            Chunk.Next++;
            return Chunk.Next + 2;
        }

        public PlanetTileMapChunk GetChunk(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0: return Chunk.Error;
                case 1: return Chunk.Empty;
                case 2: return Chunk.Empty; // UNEXPLORED
            }

            Chunk.List[chunkIndex - 3].Usage++;
            return Chunk.List[chunkIndex - 3];
        }

        public ref PlanetTileMapChunk GetChunkRef(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);

            switch (chunkIndex)
            {
                case 0:
                    throw new IndexOutOfRangeException();
                // We are getting a reference here, most likely to edit the chunk / add a tile, so we can't just return an empty chunk
                // Instead, we will just create a new chunk
                case < 3:
                    chunkIndex = AddChunk(new PlanetTileMapChunk(), x, y);
                    break;
            }

            Chunk.List[chunkIndex - 3].Usage++;
            return ref Chunk.List[chunkIndex - 3];
        }

        public void SetChunk(int x, int y, PlanetTile[,] tiles)
        {
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0:
                    return;
                case < 3:
                    chunkIndex = AddChunk(new PlanetTileMapChunk(), x, y);
                    break;
            }

            Chunk.List[chunkIndex - 3].Seq++;

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    Chunk.List[chunkIndex - 3].Tiles[i, j] = tiles[i, j];
        }
        public ref PlanetTile GetTileRef(int x, int y)
        {
            ref PlanetTileMapChunk chunk = ref GetChunkRef(x, y);

            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        public PlanetTile GetTile(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);
            return chunkIndex == 1 ? AirTile : Chunk.List[chunkIndex - 3].Tiles[x & 0x0F, y & 0x0f];
        }

        public void SetTile(int x, int y, PlanetTile tile)
        {
            ref PlanetTileMapChunk chunk = ref GetChunkRef(x, y);
            chunk.Seq++; // Updating tile, increment seq
            chunk.Tiles[x & 0x0F, y & 0x0F] = tile;
        }

        public ref PlanetTile getTile(int x, int y)
        {
            ref PlanetTileMapChunk chunk = ref GetChunkRef(x, y);
            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        // Sort chunks by most used using quick sort

        private void swap(int index1, int index2)
        {
            // Swap chunks
            (Chunk.List[index1], Chunk.List[index2]) = (Chunk.List[index2], Chunk.List[index1]);

            // Then update chunk index list - This is what storing the Position inside the chunk is most useful for
            Chunk.IndexList[Chunk.List[index1].ChunkIndexListID] = index1 + 3;
            Chunk.IndexList[Chunk.List[index2].ChunkIndexListID] = index2 + 3;
        }

        private int partition(int start, int end)
        {
            // Use negative of the usage to have the list sorted from most used to least used without having to reverse afterwards
            int p = -Chunk.List[start].Usage;

            int count = 0;
            for (int k = start + 1; k <= end; k++)
                if (-Chunk.List[k].Usage <= p)
                    count++;

            int pi = start + count;
            swap(pi, start);

            int i = start, j = end;

            while (i < pi && j > pi)
            {
                while (-Chunk.List[i].Usage <= p) i++;
                while (-Chunk.List[j].Usage > p) j--;

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
            if (Chunk.List == null || Chunk.List.Length == 0) return;

            quickSort(0, Chunk.Next - 1);
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
