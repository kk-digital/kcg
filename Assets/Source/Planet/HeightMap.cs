using KMath;
using UnityEngine;

namespace Planet
{
    public struct HeightMap
    {
        public Vec2i MapSize;
        public int[] Data;

        public HeightMap(Vec2i mapSize)
        {
            MapSize = mapSize;
            Data = new int[mapSize.X];
        }
        
        public void UpdateTopTilesMap(ref TileMap tileMap)
        {
            for(int i = 0; i < MapSize.X; i++)
            {
                Data[i] = 0;
                for(int j = MapSize.Y - 1; j >= 0; j--)
                {
                    ref var tile = ref tileMap.GetTileRef(i, j, Enums.Tile.MapLayerType.Front);
                    if (tile.Type != -1)
                    {
                        Data[i] = j;
                        break;
                    }
                }
            }
        }
    }
}

