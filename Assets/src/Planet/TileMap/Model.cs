using System;
using Enums;
using UnityEngine;

namespace Planet.TileMap
{
    public class Model
    {
        // public static const PlanetTile AirTile = new PlanetTile(); - PlanetTile cannot be const in c#?
        public static readonly Tile.Model AirTile = new();

        public Tile.Model[][] Tiles;
        public Vector2Int MapSize;
        public ChunkList Chunks;
        public Layers Layers;
        public HeightMap HeightMap;

        public Model(Vector2Int size)
        {
            var layersCount = Enum.GetNames(typeof(PlanetLayer)).Length;

            MapSize.x = size.x;
            MapSize.y = size.y;

            Chunks.Size.x = MapSize.x >> 4;
            Chunks.Size.y = MapSize.y >> 4;
            Chunks.Data = new Chunk[Chunks.Size.x * Chunks.Size.y];

            Tiles = new Tile.Model[layersCount][];
            for(int layerIndex = 0; layerIndex < layersCount; layerIndex++)
            {
                Tiles[layerIndex] = new Tile.Model[MapSize.x * MapSize.y];
            }
            
            HeightMap = new HeightMap(MapSize);
            Layers = new Layers
            {
                LayerTextures = new Texture2D[layersCount],
                MapSize = size
            };
        }

        public void UpdateTilesOnPosition(int x, int y, PlanetLayer planetLayer)
        {
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] TilePositionToTileSet = new int[]
            {
                 15, 12, 14, 13, 3, 0, 2, 1, 11, 8, 10, 9, 7, 4, 6, 5
            };
            

            if (x < 0 || y < 0 | x >= MapSize.x || y >= MapSize.y)
            {
                return;
            }

            ref Tile.Model tile = ref GetTileRef(x, y, planetLayer);
             
            if (tile.TileType >= 0)
            {
                Tile.TileType properties = 
                                GameState.CreationApi.GetTileProperties(tile.TileType);
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
                        neighbors[(int)Enums.Tile.Neighbor.Right] = neighborTile.TileType;
                    }

                    if (x - 1 >= 0)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x - 1, y, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Left] = neighborTile.TileType;
                    }

                    if (y + 1 < MapSize.y)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x, y + 1, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Up] = neighborTile.TileType;
                    }

                    if (y - 1 >= 0)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x, y - 1, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Down] = neighborTile.TileType;
                    }


                    Enums.Tile.Position tilePosition = tile.GetTilePosition(neighbors, tile.TileType);

                    // the sprite ids are next to each other in the sprite atlas
                    // we jus thave to know which one to draw based on the offset
                    tile.SpriteId = properties.BaseSpriteId + TilePositionToTileSet[(int)tilePosition];
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
        public void UpdateTileMapPositions(PlanetLayer planetLayer)
        {
            for(int y = 0; y < MapSize.y; y++)
            {
                for(int x = 0; x < MapSize.x; x++)
                {
                    UpdateTilesOnPosition(x, y, planetLayer);
                }
            }
        }
        

        public void RemoveTile(int x, int y, PlanetLayer planetLayer)
        {
            if (x < 0 || y < 0 | x >= MapSize.x || y >= MapSize.y)
            {
                return;
            }

            ref Tile.Model tile = ref GetTileRef(x, y, planetLayer);

            tile.TileType = -1;

            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
                    UpdateTilesOnPosition(i, j, planetLayer);
                }
            }
        }
        public ref Tile.Model GetTileRef(int x, int y, PlanetLayer planetLayer)
        {
            ref Chunk chunk = ref Chunks.GetChunkRef(x, y);

            return ref Tiles[(int)planetLayer][x + y * MapSize.x];
        }

        public Tile.Model GetTile(int x, int y, PlanetLayer planetLayer)
        {
            return GetTileRef(x, y, planetLayer);
        }

        public void SetTile(int x, int y, Tile.Model tile, PlanetLayer planetLayer)
        {
            ref Chunk chunk = ref Chunks.GetChunkRef(x, y);
            chunk.Seq++; // Updating tile, increment seq
            Tiles[(int)planetLayer][x + y * MapSize.x] = tile;
        }
    }
}
