using System;
using UnityEngine;

namespace Planet.TileMap
{
    public class Model
    {
        // public static const PlanetTile AirTile = new PlanetTile(); - PlanetTile cannot be const in c#?
        public static readonly Tile.Model AirTile = new();
        
        public Vector2Int MapSize;
        public ChunkList Chunks;
        public Layers Layers;
        public HeightMap HeightMap;

        public Model(Vector2Int mapSize)
        {
            MapSize = mapSize;

            Chunks = new ChunkList(mapSize);

            HeightMap = new HeightMap(MapSize);
            Layers = new Layers
            {
                LayerTextures = new Texture2D[Layers.Count],
                MapSize = mapSize
            };
        }

        #region TileApi

        public int GetTileIndex(int x, int y)
        {
            var chunkX = x & 0x0f;
            var chunkY = (y & 0x0f) << 4;

            return chunkX + chunkY;
        }

        public ref Tile.Model GetTileRef(int x, int y, Enums.Tile.MapLayerType planetLayer)
        {
            ref var chunk = ref Chunks.GetChunkRef(x, y);
            if (chunk.Type == Enums.Tile.MapChunkType.Error)
            {
                throw new IndexOutOfRangeException();
            }
            
            var tileIndex = GetTileIndex(x, y);
            return ref chunk.Tiles[(int)planetLayer][tileIndex];
        }
        public void SetTile(int x, int y, Tile.Model tile, Enums.Tile.MapLayerType planetLayer)
        {
            var chunk = Chunks.GetChunkRef(x, y);
            if (chunk.Type == Enums.Tile.MapChunkType.Error) return;
            
            chunk.Seq++; // Updating tile, increment seq
            var tileIndex = GetTileIndex(x, y);
            chunk.Tiles[(int)planetLayer][tileIndex] = tile;
            chunk.Type = Enums.Tile.MapChunkType.Explored;
        }
        public void RemoveTile(int x, int y, Enums.Tile.MapLayerType planetLayer)
        {
            ref Tile.Model tile = ref GetTileRef(x, y, planetLayer);

            tile.Type = -1;

            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
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
            
            ref Tile.Model tile = ref GetTileRef(x, y, planetLayer);
             
            if (tile.Type >= 0)
            {
                Tile.Type properties = 
                                GameState.CreationApi.GetTileProperties(tile.Type);
                if (properties.AutoMapping)
                {
                    // we have 4 neighbors per tile
                    // could be more but its 4 for now
                    // right/left/down/up
                    int[] neighbors = new int[4];

                    for(int i = 0; i < neighbors.Length; i++)
                    {
                        neighbors[i] = -1;
                    }

                    if (x + 1 < MapSize.x)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x + 1, y, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Right] = neighborTile.Type;
                    }

                    if (x - 1 >= 0)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x - 1, y, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Left] = neighborTile.Type;
                    }

                    if (y + 1 < MapSize.y)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x, y + 1, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Up] = neighborTile.Type;
                    }

                    if (y - 1 >= 0)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x, y - 1, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Down] = neighborTile.Type;
                    }


                    Enums.Tile.Position tilePosition = tile.GetTilePosition(neighbors, tile.Type);

                    // the sprite ids are next to each other in the sprite atlas
                    // we jus thave to know which one to draw based on the offset
                    tile.SpriteId = properties.BaseSpriteId + tilePositionToTileSet[(int)tilePosition];
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
        }
        public void UpdateTileMapPositions(Enums.Tile.MapLayerType planetLayer)
        {
            for(int y = 0; y < MapSize.y; y++)
            {
                for(int x = 0; x < MapSize.x; x++)
                {
                    UpdateTilesOnPosition(x, y, planetLayer);
                }
            }
        }

        #endregion
    }
}
