namespace TileMap
{
    public struct Top
    {
        public int[] Data;

        // more attributes if needed
        public int Width => Data.Length;

        public Top(int width)
        {
            Data = new int[width];
        }
        
        public void UpdateTopTilesMap(ref GameEntity tileMap)
        {
            for (int i = 0; i < tileMap.tileMapData.Chunks.Size.x; i++)
            {
                tileMap.tileMapData.Top.Data[i] = 0;
                for (int j = tileMap.tileMapData.Chunks.Size.y - 1; j >= 0; j--)
                {
                    ref var tile = ref ManagerSystem.Instance.GetTile(ref tileMap, i, j);
                    if (tile.BackTilePropertiesId != -1)
                    {
                        tileMap.tileMapData.Top.Data[i] = j;
                        break;
                    }
                }
            }
        }
    }
}

