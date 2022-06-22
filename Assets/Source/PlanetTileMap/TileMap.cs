using Enums.Tile;
using KMath;
using UnityEngine;

namespace PlanetTileMap
{
    public struct TileMap
    {
        public Vec2i MapSize;
        public ChunkList ChunkList;
        public Layer Layer;

        public TileMap(Vec2i mapSize)
        {
            ChunkList = new ChunkList(4096);

            MapSize = mapSize;
            
            Layer = new Layer
            {
                LayerTextures = new Texture2D[Layer.Count],
                NeedsUpdate = new bool[Layer.Count],
                MapSize = mapSize
            };

            for(int layerIndex = 0; layerIndex < Layers.Count; layerIndex++)
            {
                Layers.NeedsUpdate[layerIndex] = true;
            }
        }

        private void BuildLayerTexture(MapLayerType planetLayer)
        {
            //Layers.BuildLayerTexture(this, planetLayer);
        }

        #region TileApi

        /// <summary>
        /// Checks if position is inside Map Size
        /// </summary>
        /// <param name="x">TileMap coordinates</param>
        /// <param name="y">TileMap coordinates</param>
        public bool IsValid(int x, int y)
        {
            return x >= 0 && x < MapSize.X &&
                   y >= 0 && y < MapSize.Y;
        }

        public void SetTile(int x, int y, TileID tileID, MapLayerType planetLayer)
        {
            if (!IsValid(x, y)) return;

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkList.chunkList[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                chunk.Init(MapChunkType.Explored);
            }
            
            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.Tiles[(int) planetLayer][tileIndex] = tile;
            chunk.Sequence++;
            UpdateTile(x, y, planetLayer);
        }

        public ref Tile GetTileRef(int x, int y, MapLayerType planetLayer)
        {
            if (!IsValid(x, y))
            {
                return ref Tile.Tile.Air;
            }
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkList.chunkList[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                return ref Tile.Tile.Air;
            }
            
            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;
            
            return ref chunk.Tiles[(int) planetLayer][tileIndex];
        }

        public void RemoveTile(int x, int y, MapLayerType planetLayer)
        {
            ref var tile = ref GetTileRef(x, y, planetLayer);
            tile = Tile.Tile.Empty;
            UpdateTile(x, y, planetLayer);
        }

        private void UpdateTile(int x, int y, MapLayerType planetLayer)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!IsValid(i, 0)) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!IsValid(i, j)) continue;
                    UpdateTilesOnPosition(i, j, planetLayer);
                }
            }
        }

        #endregion

        #region TilePositionUpdater

        public void UpdateTilesOnPosition(int x, int y, MapLayerType planetLayer)
        {
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {15, 12, 14, 13, 3, 0, 2, 1, 11, 8, 10, 9, 7, 4, 6, 5};

            ref Tile.Tile tile = ref GetTileRef(x, y, planetLayer);

            if (tile.Property >= 0)
            {
                Tile.TileProperty property = GameState.TilePropertyManager.GetTileProperty(tile.Property);
                if (property.AutoMapping)
                {
                    // we have 4 neighbors per tile
                    // could be more but its 4 for now
                    // right/left/down/up
                    int[] neighbors = new int[4];

                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        neighbors[i] = -1;
                    }

                    if (x + 1 < MapSize.X)
                    {
                        ref Tile.Tile neighborTile = ref GetTileRef(x + 1, y, planetLayer);
                        neighbors[(int) Neighbor.Right] = neighborTile.Property;
                    }

                    if (x - 1 >= 0)
                    {
                        ref Tile.Tile neighborTile = ref GetTileRef(x - 1, y, planetLayer);
                        neighbors[(int) Neighbor.Left] = neighborTile.Property;
                    }

                    if (y + 1 < MapSize.Y)
                    {
                        ref Tile.Tile neighborTile = ref GetTileRef(x, y + 1, planetLayer);
                        neighbors[(int) Neighbor.Up] = neighborTile.Property;
                    }

                    if (y - 1 >= 0)
                    {
                        ref Tile.Tile neighborTile = ref GetTileRef(x, y - 1, planetLayer);
                        neighbors[(int) Neighbor.Down] = neighborTile.Property;
                    }


                    var tilePosition = tile.GetTilePosition(neighbors, tile.Property);

                    // the sprite ids are next to each other in the sprite atlas
                    // we jus thave to know which one to draw based on the offset
                    tile.SpriteId = property.BaseSpriteId + tilePositionToTileSet[(int) tilePosition];
                }

                else
                {
                    tile.SpriteId = property.BaseSpriteId;
                }
            }
            else
            {
                tile.SpriteId = -1;
            }

            Layer.NeedsUpdate[(int) planetLayer] = true;
        }

        public void UpdateTileMapPositions(MapLayerType planetLayer)
        {
            for(int y = 0; y < MapSize.Y; y++)
            {
                for(int x = 0; x < MapSize.X; x++)
                {
                    UpdateTilesOnPosition(x, y, planetLayer);
                }
            }
        }

        #endregion
    }
}
