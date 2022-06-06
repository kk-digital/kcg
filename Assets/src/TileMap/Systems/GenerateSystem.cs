using Enums;
using UnityEngine;

namespace TileMap
{
    public class GenerateSystem
    {
        public static readonly GenerateSystem Instance;

        static GenerateSystem()
        {
            Instance = new GenerateSystem();
        }

        public Component GenerateTileMap()
        {
            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);

            var tileMap = new Component(mapSize);

            for (int j = 0; j < mapSize.y; j++)
            {
                for (int i = 0; i < mapSize.x; i++)
                {
                    var tile = Tile.Component.EmptyTile();
                    tile.FrontTilePropertiesId = 9;


                    if (i % 10 == 0)
                    {
                        //tile.FrontTilePropertiesId = 7;
                        tile.OreTilePropertiesId = 8;
                    }

                    if (j % 2 == 0)
                    {
                        // tile.FrontTilePropertiesId = 2;
                    }

                    if (j % 3 == 0)
                    {
                        // tile.FrontTilePropertiesId = 9;

                    }

                    if ((j > 1 && j < 6) || (j > (8 + i)))
                    {
                        tile.FrontTilePropertiesId = -1;
                        tile.OreTilePropertiesId = -1;
                    }


                    ManagerSystem.Instance.SetTile(ref tileMap, i, j, tile);
                }
            }

            tileMap.TopMap.UpdateTopTilesMap(ref tileMap);
            UpdateAllTileVariants(ref tileMap, PlanetLayer.Front);
            UpdateAllTileVariants(ref tileMap, PlanetLayer.Ore);
            BuildLayerTexture(ref tileMap, PlanetLayer.Front);
            BuildLayerTexture(ref tileMap, PlanetLayer.Ore);

            return tileMap;
        }

        public void BuildLayerTexture(ref TileMap.Component tileMap, PlanetLayer layer)
        {
            var bytes = new byte[32 * 32 * 4];
            var data = new byte[tileMap.Chunks.Size.x * tileMap.Chunks.Size.y * 32 * 32 * 4];

            for (int y = 0; y < tileMap.Chunks.Size.y; y++)
            {
                for (int x = 0; x < tileMap.Chunks.Size.x; x++)
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
                                int index = 4 * (((i + tileX)) + ((j + tileY)) * (tileMap.Chunks.Size.x * 32));
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

            tileMap.LayerTextures[(int) layer] = Utility.TextureUtils.CreateTextureFromRGBA(data, tileMap.Chunks.Size.x * 32, tileMap.Chunks.Size.y * 32);
        }
        public void UpdateTileVariants(ref TileMap.Component tileMap, int x, int y, PlanetLayer planetLayer)
        {
            if (x < 0 || y < 0 | x >= tileMap.Chunks.Size.x || y >= tileMap.Chunks.Size.y) return;
            if (planetLayer != PlanetLayer.Front) return;

            ref var tile = ref ManagerSystem.Instance.GetTile(ref tileMap, x, y);

            if (tile.FrontTilePropertiesId >= 0)
            {
                var neighbors = new int[8];
                for (int i = 0; i < neighbors.Length; i++)
                {
                    neighbors[i] = -1;
                }

                if (x + 1 < tileMap.Chunks.Size.x)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref tileMap, x + 1, y);
                    neighbors[(int) TileNeighbor.Right] = neighborTile.FrontTilePropertiesId;
                }

                if (x - 1 >= 0)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref tileMap, x - 1, y);
                    neighbors[(int) TileNeighbor.Left] = neighborTile.FrontTilePropertiesId;
                }

                if (y + 1 < tileMap.Chunks.Size.y)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref tileMap, x, y + 1);
                    neighbors[(int) TileNeighbor.Top] = neighborTile.FrontTilePropertiesId;
                }

                if (y - 1 >= 0)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref tileMap, x, y - 1);
                    neighbors[(int) TileNeighbor.Bottom] = neighborTile.FrontTilePropertiesId;
                }

                if (x + 1 < tileMap.Chunks.Size.x && y + 1 < tileMap.Chunks.Size.y)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref tileMap, x + 1, y + 1);
                    neighbors[(int) TileNeighbor.TopRight] = neighborTile.FrontTilePropertiesId;
                }

                if (x - 1 >= 0 && y + 1 < tileMap.Chunks.Size.y)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref tileMap, x - 1, y + 1);
                    neighbors[(int) TileNeighbor.TopLeft] = neighborTile.FrontTilePropertiesId;
                }

                if (x + 1 < tileMap.Chunks.Size.x && y - 1 >= 0)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref tileMap, x + 1, y - 1);
                    neighbors[(int) TileNeighbor.BottomRight] = neighborTile.FrontTilePropertiesId;
                }

                if (x - 1 >= 0 && y - 1 >= 0)
                {
                    ref var neighborTile = ref ManagerSystem.Instance.GetTile(ref tileMap, x - 1, y - 1);
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
        public void UpdateAllTileVariants(ref TileMap.Component tileMap, PlanetLayer layer)
        {
            for (int y = 0; y < tileMap.Chunks.Size.y; y++)
            {
                for (int x = 0; x < tileMap.Chunks.Size.x; x++)
                {
                    UpdateTileVariants(ref tileMap, x, y, layer);
                }
            }
        }
    }
}

