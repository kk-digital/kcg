using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Enums.Tile;
using KMath;

namespace PlanetTileMap
{
    public struct Chunk
    {
        public static readonly bool DebugChunkReadCount = true;
        
        public MapChunkType Type;
        public Tile[][] Tiles;
        
        public int ReadCount;
        /// <summary>
        /// <para>Sequence Number is incremented every write to a chunk</para>
        /// <para>Only increment sequence number if a write occurs</para>
        /// </summary>
        public int Sequence;

        public Chunk(MapChunkType type) : this()
        {
            Init(type);
            ReadCount = 0;
            Sequence = 0;
        }

        public void Init(MapChunkType type)
        {
            Type = type;
            
            if (type == MapChunkType.Error)
            {
                Tiles = null;
                return;
            }

            Tiles = new Tile[TileMap.LayerCount][];

            for (int planetLayer = 0; planetLayer < TileMap.LayerCount; planetLayer++)
            {
                // 256 == 0001 0000 0000 == 16 * 16
                Tiles[planetLayer] = Enumerable.Repeat(new Tile(), 256).ToArray();
            }
        }
    }
}
