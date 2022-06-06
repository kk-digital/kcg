using Enums;

namespace TileMap
{
    public class ManagerSystem
    {
        public static readonly ManagerSystem Instance;

        static ManagerSystem()
        {
            Instance = new ManagerSystem();
        }

        public ref Tile.Component GetTile(ref Component tileMap, int x, int y)
        {
            var chunk = tileMap.Chunks.GetChunk(x, y);
            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        public void SetTile(ref Component tileMap, int x, int y, Tile.Component tile)
        {
            var chunk = tileMap.Chunks.GetChunk(x, y);
            chunk.Seq++; // Updating tile, increment seq
            chunk.Tiles[x & 0x0F, y & 0x0F] = tile;
        }
        
        public void RemoveTile(ref TileMap.Component tileMap, int x, int y, PlanetLayer layer)
        {
            if (x < 0 || y < 0 | x >= tileMap.Chunks.Size.x || y >= tileMap.Chunks.Size.y)
            {
                return;
            }

            if (layer == PlanetLayer.Back)
            {
                ref var tile = ref ManagerSystem.Instance.GetTile(ref tileMap, x, y);

                tile.BackTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Mid)
            {
                ref var tile = ref ManagerSystem.Instance.GetTile(ref tileMap, x, y);

                tile.MidTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Front)
            {
                ref var tile = ref ManagerSystem.Instance.GetTile(ref tileMap, x, y);

                tile.FrontTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Ore)
            {
                ref var tile = ref ManagerSystem.Instance.GetTile(ref tileMap, x, y);

                tile.OreTilePropertiesId = -1;
            }

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    GenerateSystem.Instance.UpdateTileVariants(ref tileMap, i, j, layer);
                }
            }
        }
    }
}

