using Enums;
using UnityEngine;

namespace Planet.TileMap
{
    public struct HeightMap
    {
        public Vector2Int MapSize;
        public int[] Data;

        public HeightMap(Vector2Int mapSize)
        {
            MapSize = mapSize;
            Data = new int[mapSize.x];
        }
        
        public void UpdateTopTilesMap(ref Model tileMap)
        {
            for(int i = 0; i < MapSize.x; i++)
            {
                Data[i] = 0;
                for(int j = MapSize.y - 1; j >= 0; j--)
                {
                    ref var tile = ref tileMap.GetTileRef(i, j, PlanetLayer.Front);
                    if (tile.PropertiesId != -1)
                    {
                        Data[i] = j;
                        break;
                    }
                }
            }
        }
    }
}

