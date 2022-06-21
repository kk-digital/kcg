using System;
using System.Runtime.CompilerServices;
using KMath;
using UnityEngine;

namespace Planet
{
    public class TileMap
    {
        public AABB2D Borders;
        public ChunkList Chunks;
        public Layers Layers;

        public TileMap(Vec2i mapSize)
        {
            Chunks = new ChunkList(mapSize);
            
            Borders = new AABB2D(Vec2f.Zero, (Vec2f)mapSize);
            
            Layers = new Layers
            {
                LayerTextures = new Texture2D[Layers.Count],
                NeedsUpdate = new bool[Layers.Count],
                MapSize = mapSize
            };
        }

        private void BuildLayerTexture(Enums.Tile.MapLayerType planetLayer)
        {
            //Layers.BuildLayerTexture(this, planetLayer);
        }

        #region TileApi

        public ref Tile.Tile GetTileRef(int x, int y, Enums.Tile.MapLayerType planetLayer)
        {
            if (!Borders.Intersects(new Vec2f(x, y))) throw new IndexOutOfRangeException();
            
            ref var chunk = ref Chunks[x, y];
            var tileIndex = Tile.Tile.GetTileIndex(x, y);

            return ref chunk[(int)planetLayer, tileIndex];
        }

        public Tile.Tile[] GetTiles(Vec2i[] positions, Enums.Tile.MapLayerType planetLayer)
        {
            var count = 0;
            var tiles = new Tile.Tile[positions.Length];
            
            foreach (var position in positions)
            {
                try
                {
                    ref var tile = ref GetTileRef(position.X, position.Y, planetLayer);
                    if (tile.Type >= 0)
                    {
                        tiles[count] = tile;
                        count++;
                    }
                }
                catch
                {
                    // Just check next position
                }
            }

            if (count == 0) return null;

            if (positions.Length != count)
            {
                Array.Resize(ref tiles, count);
            }

            return tiles;
        }
        
        public void AddTile(ref Tile.Tile tile, Enums.Tile.MapLayerType planetLayer)
        {
            if (!Borders.Intersects(tile.Borders)) throw new IndexOutOfRangeException();

            int x = tile.Borders.IntLeft, y = tile.Borders.IntBottom;
            ref var chunk = ref Chunks[x, y];

            chunk.SetTile(ref tile, planetLayer);

            UpdateTile(x, y, planetLayer);
        }

        public void RemoveTile(int x, int y, Enums.Tile.MapLayerType planetLayer)
        {
            ref var tile = ref GetTileRef(x, y, planetLayer);
            tile.Type = -1;
            UpdateTile(x, y, planetLayer);
        }

        private void UpdateTile(int x, int y, Enums.Tile.MapLayerType planetLayer)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!Borders.Intersects(new Vec2f(i, 0f))) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!Borders.Intersects(new Vec2f(i, j))) continue;
                    UpdateTilesOnPosition(i, j, planetLayer);
                }
            }
        }

        #endregion

        #region TilePositionUpdater

        public void UpdateTilesOnPosition(int x, int y, Enums.Tile.MapLayerType planetLayer)
        {
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {15, 12, 14, 13, 3, 0, 2, 1, 11, 8, 10, 9, 7, 4, 6, 5};

            ref Tile.Tile tile = ref GetTileRef(x, y, planetLayer);

            if (tile.Type >= 0)
            {
                Tile.Type properties = GameState.TileCreationApi.GetTileProperties(tile.Type);
                if (properties.AutoMapping)
                {
                    // we have 4 neighbors per tile
                    // could be more but its 4 for now
                    // right/left/down/up
                    int[] neighbors = new int[4];

                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        neighbors[i] = -1;
                    }

                    if (x + 1 < Borders.IntRight)
                    {
                        ref Tile.Tile neighborTile = ref GetTileRef(x + 1, y, planetLayer);
                        neighbors[(int) Enums.Tile.Neighbor.Right] = neighborTile.Type;
                    }

                    if (x - 1 >= 0)
                    {
                        ref Tile.Tile neighborTile = ref GetTileRef(x - 1, y, planetLayer);
                        neighbors[(int) Enums.Tile.Neighbor.Left] = neighborTile.Type;
                    }

                    if (y + 1 < Borders.IntTop)
                    {
                        ref Tile.Tile neighborTile = ref GetTileRef(x, y + 1, planetLayer);
                        neighbors[(int) Enums.Tile.Neighbor.Up] = neighborTile.Type;
                    }

                    if (y - 1 >= 0)
                    {
                        ref Tile.Tile neighborTile = ref GetTileRef(x, y - 1, planetLayer);
                        neighbors[(int) Enums.Tile.Neighbor.Down] = neighborTile.Type;
                    }


                    Enums.Tile.Position tilePosition = tile.GetTilePosition(neighbors, tile.Type);

                    // the sprite ids are next to each other in the sprite atlas
                    // we jus thave to know which one to draw based on the offset
                    tile.SpriteId = properties.BaseSpriteId + tilePositionToTileSet[(int) tilePosition];
                }

                else
                {
                    tile.SpriteId = properties.BaseSpriteId;
                }
            }
            else
            {
                tile.SpriteId = -1;
            }

            Layers.NeedsUpdate[(int) planetLayer] = true;
        }

        public void UpdateTileMapPositions(Enums.Tile.MapLayerType planetLayer)
        {
            for(int y = Borders.IntBottom; y < Borders.IntTop; y++)
            {
                for(int x = Borders.IntLeft; x < Borders.IntRight; x++)
                {
                    UpdateTilesOnPosition(x, y, planetLayer);
                }
            }
        }

        #endregion
    }
}
