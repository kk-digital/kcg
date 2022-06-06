//using Entitas;
using System;
using Enums;
using UnityEngine;

namespace TileMap
{
    public struct TopMap
    {
        public int[] Data;

        // more attributes if needed
        public int Width => Data.Length;

        public TopMap(int width)
        {
            Data = new int[width];
        }
        
        public void UpdateTopTilesMap(ref TileMap.Component tileMap)
        {
            for (int i = 0; i < tileMap.Chunks.Size.x; i++)
            {
                tileMap.TopMap.Data[i] = 0;
                for (int j = tileMap.Chunks.Size.y - 1; j >= 0; j--)
                {
                    ref var tile = ref ManagerSystem.Instance.GetTile(ref tileMap, i, j);
                    if (tile.BackTilePropertiesId != -1)
                    {
                        tileMap.TopMap.Data[i] = j;
                        break;
                    }
                }
            }
        }
    }

    public struct Component
    {
        public ChunkList Chunks;
        public NaturalLayer NaturalLayer;

        public TopMap TopMap;

        public Texture2D[] LayerTextures;

        public Component(Vector2Int chunkSize) : this()
        {
            int layersCount = Enum.GetNames(typeof(PlanetLayer)).Length;

            Chunks = new ChunkList(chunkSize, PlanetWrapBehavior.NoWrapAround);
            Chunks.MakeAllChunksExplored();
            NaturalLayer = new NaturalLayer(chunkSize, new Vector2Int(16, 16));
            
            TopMap = new TopMap(chunkSize.x);

            LayerTextures = new Texture2D[layersCount];
        }
    }
}
