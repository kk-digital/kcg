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
    }

    public struct Component
    {
        public ChunkList Chunks;
        public NaturalLayer NaturalLayer;

        public TopMap TopMap;
        public PlanetWrapBehavior WrapBehavior;

        public Texture2D[] LayerTextures;

        public Component(Vector2Int chunkSize) : this()
        {
            int layersCount = Enum.GetNames(typeof(PlanetLayer)).Length;

            Chunks = new ChunkList(chunkSize);
            Chunks.MakeAllChunksExplored();
            NaturalLayer = new NaturalLayer(chunkSize, new Vector2Int(16, 16));

            WrapBehavior = PlanetWrapBehavior.NoWrapAround; // Set to WrapAround manually if needed
            TopMap = new TopMap(chunkSize.x);

            LayerTextures = new Texture2D[layersCount];
        }

        public void BuildLayerTexture(PlanetLayer layer)
        {
            var bytes = new byte[32 * 32 * 4];
            var data = new byte[Chunks.Size.x * Chunks.Size.y * 32 * 32 * 4];

            for (int y = 0; y < Chunks.Size.y; y++)
            {
                for (int x = 0; x < Chunks.Size.x; x++)
                {
                    ref Tile.Component tile = ref GetTile(x, y);

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
                                int index = 4 * (((i + tileX)) + ((j + tileY)) * (Chunks.Size.x * 32));
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

            LayerTextures[(int) layer] =
                Utility.TextureUtils.CreateTextureFromRGBA(data, Chunks.Size.x * 32, Chunks.Size.y * 32);
        }

        public void UpdateTileVariants(int x, int y, PlanetLayer planetLayer)
        {
            if (x < 0 || y < 0 | x >= Chunks.Size.x || y >= Chunks.Size.y)
            {
                return;
            }

            ref Tile.Component tile = ref GetTile(x, y);

            if (planetLayer == PlanetLayer.Front)
            {
                if (tile.FrontTilePropertiesId >= 0)
                {
                    int[] neighbors = new int[8];
                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        neighbors[i] = -1;
                    }

                    if (x + 1 < Chunks.Size.x)
                    {
                        ref Tile.Component neighborTile = ref GetTile(x + 1, y);
                        neighbors[(int) Enums.TileNeighbor.Right] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x - 1 >= 0)
                    {
                        ref Tile.Component neighborTile = ref GetTile(x - 1, y);
                        neighbors[(int) Enums.TileNeighbor.Left] = neighborTile.FrontTilePropertiesId;
                    }

                    if (y + 1 < Chunks.Size.y)
                    {
                        ref Tile.Component neighborTile = ref GetTile(x, y + 1);
                        neighbors[(int) Enums.TileNeighbor.Top] = neighborTile.FrontTilePropertiesId;
                    }

                    if (y - 1 >= 0)
                    {
                        ref Tile.Component neighborTile = ref GetTile(x, y - 1);
                        neighbors[(int) Enums.TileNeighbor.Bottom] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x + 1 < Chunks.Size.x && y + 1 < Chunks.Size.y)
                    {
                        ref Tile.Component neighborTile = ref GetTile(x + 1, y + 1);
                        neighbors[(int) Enums.TileNeighbor.TopRight] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x - 1 >= 0 && y + 1 < Chunks.Size.y)
                    {
                        ref Tile.Component neighborTile = ref GetTile(x - 1, y + 1);
                        neighbors[(int) Enums.TileNeighbor.TopLeft] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x + 1 < Chunks.Size.x && y - 1 >= 0)
                    {
                        ref Tile.Component neighborTile = ref GetTile(x + 1, y - 1);
                        neighbors[(int) Enums.TileNeighbor.BottomRight] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x - 1 >= 0 && y - 1 >= 0)
                    {
                        ref Tile.Component neighborTile = ref GetTile(x - 1, y - 1);
                        neighbors[(int) Enums.TileNeighbor.BottomLeft] = neighborTile.FrontTilePropertiesId;
                    }

                    TileVariants tileVariants = TileNeighborExt.GetVariant(neighbors, tile.FrontTilePropertiesId);
                    Tile.PropertiesData properties =
                        GameState.CreationAPISystem.GetTileProperties(tile.FrontTilePropertiesId);
                    tile.FrontSpriteId = properties.Variants[(int) tileVariants];
                }
                else
                {
                    tile.FrontSpriteId = -1;
                }
            }



        }

        public void UpdateAllTileVariants(PlanetLayer layer)
        {
            for (int y = 0; y < Chunks.Size.y; y++)
            {
                for (int x = 0; x < Chunks.Size.x; x++)
                {
                    UpdateTileVariants(x, y, layer);
                }
            }
        }

        public void UpdateTopTilesMap()
        {
            for (int i = 0; i < Chunks.Size.x; i++)
            {
                TopMap.Data[i] = 0;
                for (int j = Chunks.Size.y - 1; j >= 0; j--)
                {
                    ref Tile.Component tile = ref GetTile(i, j);
                    if (tile.BackTilePropertiesId != -1)
                    {
                        TopMap.Data[i] = j;
                        break;
                    }
                }
            }
        }

        public void DrawLayer(PlanetLayer layer, Material material, Transform transform, int DrawOrder)
        {
            Render.Sprite sprite = new Render.Sprite();
            sprite.Texture = LayerTextures[(int) layer];
            sprite.TextureCoords = new Vector4(0, 0, 1, -1);

            Utility.RenderUtils.DrawSprite(0, 0, 1.0f * Chunks.Size.x, 1.0f * Chunks.Size.y, sprite, material,
                transform, DrawOrder);

        }

        public void RemoveTile(int x, int y, PlanetLayer layer)
        {
            if (x < 0 || y < 0 | x >= Chunks.Size.x || y >= Chunks.Size.y)
            {
                return;
            }

            if (layer == PlanetLayer.Back)
            {
                ref Tile.Component tile = ref GetTile(x, y);

                tile.BackTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Mid)
            {
                ref Tile.Component tile = ref GetTile(x, y);

                tile.MidTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Front)
            {
                ref Tile.Component tile = ref GetTile(x, y);

                tile.FrontTilePropertiesId = -1;
            }
            else if (layer == PlanetLayer.Ore)
            {
                ref Tile.Component tile = ref GetTile(x, y);

                tile.OreTilePropertiesId = -1;
            }

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    UpdateTileVariants(i, j, layer);
                }
            }
        }

        public ref NaturalLayer GetNaturalLayerChunk(int x, int y)
        {
            int index = x / NaturalLayer.ChunkSize.x + (y / NaturalLayer.ChunkSize.y) * NaturalLayer.Size.x;

            return ref NaturalLayer.List[index];
        }

        private Chunk CreateNewChunk(int x, int y)
        {
            Array.Resize(ref Chunks.List, Chunks.Next + 1);

            var chunk = new Chunk();

            Chunks.List[Chunks.Next] = chunk;
            Chunks.Next++;
            return chunk;
        }

        public int GetChunkIndex(int x, int y)
        {
            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Chunks.Size.x;

            var chunkIndex = (x >> 4) * (Chunks.Size.y >> 4) + (y >> 4);

            return chunkIndex;
        }

        public Chunk GetChunk(int x, int y)
        {
            var chunkIndex = GetChunkIndex(x, y);
            var chunk = Chunks.List[chunkIndex];

            switch (chunk.Behaviour)
            {
                case ChunkBehaviour.Error: return Chunks.Error;
                case ChunkBehaviour.Empty: return Chunks.Empty;
                case ChunkBehaviour.Unexplored: return Chunks.Empty;
                case ChunkBehaviour.Explored:
                {
                    Chunks.List[chunkIndex].Usage++;
                    return Chunks.List[chunkIndex];
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetChunk(int x, int y, Tile.Component[,] tiles)
        {
            var chunk = GetChunk(x, y);
            switch (chunk.Behaviour)
            {
                case ChunkBehaviour.Error:
                    return;
                case not ChunkBehaviour.Explored:
                    chunk = CreateNewChunk(x, y);
                    break;
            }

            chunk.Seq++;

            for (int i = 0; i < 16; i++)
            for (int j = 0; j < 16; j++)
                chunk.Tiles[i, j] = tiles[i, j];
        }

        public ref Tile.Component GetTile(int x, int y)
        {
            var chunk = GetChunk(x, y);
            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        public void SetTile(int x, int y, Tile.Component tile)
        {
            var chunk = GetChunk(x, y);
            chunk.Seq++; // Updating tile, increment seq
            chunk.Tiles[x & 0x0F, y & 0x0F] = tile;
        }

        private void Swap(int index1, int index2)
        {
            // Swap chunks
            (Chunks.List[index1], Chunks.List[index2]) = (Chunks.List[index2], Chunks.List[index1]);
        }

        private int Partition(int start, int end)
        {
            // Use negative of the usage to have the list sorted from most used to least used without having to reverse afterwards
            int p = -Chunks.List[start].Usage;

            int count = 0;
            for (int k = start + 1; k <= end; k++)
                if (-Chunks.List[k].Usage <= p)
                    count++;

            int pi = start + count;
            Swap(pi, start);

            int i = start, j = end;

            while (i < pi && j > pi)
            {
                while (-Chunks.List[i].Usage <= p) i++;
                while (-Chunks.List[j].Usage > p) j--;

                if (i < pi && j > pi)
                    Swap(i++, j--);
            }

            return pi;
        }

        // TODO: Move out from here to Utility
        private void QuickSort(int start, int end)
        {
            if (start >= end) return;

            int p = Partition(start, end);
            QuickSort(start, p - 1);
            QuickSort(p + 1, end);
        }

        public void SortChunks()
        {
            // Sort chunks from most used to least used
            if (Chunks.List == null || Chunks.List.Length == 0) return;

            QuickSort(0, Chunks.Next - 1);
        }
    }
}
