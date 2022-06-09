using UnityEngine;

namespace Planet
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
        
        public void UpdateTopTilesMap(ref TileMap tileMap)
        {
            for(int i = 0; i < MapSize.x; i++)
            {
                Data[i] = 0;
                for(int j = MapSize.y - 1; j >= 0; j--)
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

