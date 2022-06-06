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

        private ManagerSystem()
        {
            
        }

        public ref Tile.Component GetTile(ref GameEntity entity, int x, int y)
        {
            var chunk = entity.tileMapData.Chunks.GetChunk(x, y);
            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        public void SetTile(ref GameEntity entity, int x, int y, Tile.Component tile)
        {
            var chunk = entity.tileMapData.Chunks.GetChunk(x, y);
            chunk.Seq++; // Updating tile, increment seq
            chunk.Tiles[x & 0x0F, y & 0x0F] = tile;
        }

        public void RemoveTile(ref GameEntity entity, int x, int y, PlanetLayer layer)
        {
            if (x < 0 || y < 0 | x >= entity.tileMapData.Chunks.Size.x || y >= entity.tileMapData.Chunks.Size.y)
            {
                return;
            }

            if (layer == PlanetLayer.Back)
            {
                ref var tile = ref GetTile(ref entity, x, y);

                tile.BackTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Mid)
            {
                ref var tile = ref GetTile(ref entity, x, y);

                tile.MidTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Front)
            {
                ref var tile = ref GetTile(ref entity, x, y);

                tile.FrontTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Ore)
            {
                ref var tile = ref GetTile(ref entity, x, y);

                tile.OreTilePropertiesId = -1;
            }

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    UpdateTileVariants(ref entity, i, j, layer);
                }
            }
        }

        public void BuildLayerTexture(ref GameEntity tileMap, PlanetLayer layer)
        {
            var bytes = new byte[32 * 32 * 4];
            var data = new byte[tileMap.tileMapData.Chunks.Size.x * tileMap.tileMapData.Chunks.Size.y * 32 * 32 * 4];

            for (int y = 0; y < tileMap.tileMapData.Chunks.Size.y; y++)
            {
                for (int x = 0; x < tileMap.tileMapData.Chunks.Size.x; x++)
                {
                    ref var tile = ref ManagerSystem.Instance.GetTile(ref tileMap, x, y);

                    int spriteId = -1;

                    if (layer == PlanetLayer.Back)
                    {
                        spriteId = tile.BackSpriteId;
                    }
                    else if (layer == PlanetLayer.Mid)
                    {
                        spriteId = tile.MidSpriteId;
                    }
                    else if (layer == PlanetLayer.Front)
                    {
                        spriteId = tile.FrontSpriteId;
                    }
                    else if (layer == PlanetLayer.Ore)
                    {
                        spriteId = tile.OreSpriteId;
                    }

                    if (spriteId >= 0)
                    {

                        GameState.TileSpriteAtlasManager.GetSpriteBytes(spriteId, bytes);

                        int tileX = (x * 32);
                        int tileY = (y * 32);

                        for (int j = 0; j < 32; j++)
                        {
                            for (int i = 0; i < 32; i++)
                            {
                                int index = 4 * (((i + tileX)) + ((j + tileY)) * (tileMap.tileMapData.Chunks.Size.x * 32));
                                int bytesIndex = 4 * (i + (32 - j - 1) * 32);
                                data[index] =
                                    bytes[bytesIndex];
                                data[index + 1] =
                                    bytes[bytesIndex + 1];
                                data[index + 2] =
                                    bytes[bytesIndex + 2];
                                data[index + 3] =
                                    bytes[bytesIndex + 3];
                            }
                        }
                    }
                }
            }

            tileMap.tileMapData.LayerTextures[(int) layer] =
                Utility.TextureUtils.CreateTextureFromRGBA(data, tileMap.tileMapData.Chunks.Size.x * 32,
                    tileMap.tileMapData.Chunks.Size.y * 32);
        }

        public void UpdateTileVariants(ref GameEntity entity, int x, int y, PlanetLayer planetLayer)
        {
            if (x < 0 || y < 0 | x >= entity.tileMapData.Chunks.Size.x || y >= entity.tileMapData.Chunks.Size.y) return;
            if (planetLayer != PlanetLayer.Front) return;

            ref var tile = ref ManagerSystem.Instance.GetTile(ref entity, x, y);

            if (tile.FrontTilePropertiesId >= 0)
            {
                var neighbors = new int[8];
                for (int i = 0; i < neighbors.Length; i++)
                {
                    neighbors[i] = -1;
                }

                if (x + 1 < entity.tileMapData.Chunks.Size.x)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref entity, x + 1, y);
                    neighbors[(int) TileNeighbor.Right] = neighborTile.FrontTilePropertiesId;
                }

                if (x - 1 >= 0)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref entity, x - 1, y);
                    neighbors[(int) TileNeighbor.Left] = neighborTile.FrontTilePropertiesId;
                }

                if (y + 1 < entity.tileMapData.Chunks.Size.y)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref entity, x, y + 1);
                    neighbors[(int) TileNeighbor.Top] = neighborTile.FrontTilePropertiesId;
                }

                if (y - 1 >= 0)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref entity, x, y - 1);
                    neighbors[(int) TileNeighbor.Bottom] = neighborTile.FrontTilePropertiesId;
                }

                if (x + 1 < entity.tileMapData.Chunks.Size.x && y + 1 < entity.tileMapData.Chunks.Size.y)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref entity, x + 1, y + 1);
                    neighbors[(int) TileNeighbor.TopRight] = neighborTile.FrontTilePropertiesId;
                }

                if (x - 1 >= 0 && y + 1 < entity.tileMapData.Chunks.Size.y)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref entity, x - 1, y + 1);
                    neighbors[(int) TileNeighbor.TopLeft] = neighborTile.FrontTilePropertiesId;
                }

                if (x + 1 < entity.tileMapData.Chunks.Size.x && y - 1 >= 0)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref entity, x + 1, y - 1);
                    neighbors[(int) TileNeighbor.BottomRight] = neighborTile.FrontTilePropertiesId;
                }

                if (x - 1 >= 0 && y - 1 >= 0)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref entity, x - 1, y - 1);
                    neighbors[(int) TileNeighbor.BottomLeft] = neighborTile.FrontTilePropertiesId;
                }

                var tileVariants = TileNeighborExt.GetVariant(neighbors, tile.FrontTilePropertiesId);
                var properties = GameState.CreationAPISystem.GetTileProperties(tile.FrontTilePropertiesId);
                tile.FrontSpriteId = properties.Variants[(int) tileVariants];
            }
            else
            {
                tile.FrontSpriteId = -1;
            }
        }

        public void UpdateAllTileVariants(ref GameEntity tileMap, PlanetLayer layer)
        {
            for (int y = 0; y < tileMap.tileMapData.Chunks.Size.y; y++)
            {
                for (int x = 0; x < tileMap.tileMapData.Chunks.Size.x; x++)
                {
                    UpdateTileVariants(ref tileMap, x, y, layer);
                }
            }
        }
    }
}

