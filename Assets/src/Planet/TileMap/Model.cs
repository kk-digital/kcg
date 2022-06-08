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
        public Vector2Int Size;
        public ChunkList Chunks;
        public HeightMap HeightMap;

        public Texture2D[] LayerTextures;

        public Model(Vector2Int size)
        {
            var layersCount = Enum.GetNames(typeof(PlanetLayer)).Length;

            Size.x = size.x;
            Size.y = size.y;

            Chunks.Size.x = Size.x >> 4;
            Chunks.Size.y = Size.y >> 4;
            Chunks.Data = new Chunk[Chunks.Size.x * Chunks.Size.y];

            Tiles = new Tile.Model[layersCount][];
            for(int layerIndex = 0; layerIndex < layersCount; layerIndex++)
            {
                Tiles[layerIndex] = new Tile.Model[Size.x * Size.y];
            }
            
            HeightMap = new HeightMap(Size.x);
            LayerTextures = new Texture2D[layersCount];
        }
        
        //TODO: Implement
        public void UpdateTopTilesMap()
        {
            for(int i = 0; i < Chunks.Size.x; i++)
            {
                HeightMap.Data[i] = 0;
                for(int j = Chunks.Size.y - 1; j >= 0; j--)
                {
                    ref Tile.Model tile = ref GetTileRef(i, j, PlanetLayer.Front);
                    if (tile.PropertiesId != -1)
                    {
                        HeightMap.Data[i] = j;
                        break;
                    }
                }
            }
        }

        public void BuildLayerTexture(PlanetLayer planetLayer)
        {
            byte[] Bytes = new byte[32 * 32 * 4];
            byte[] Data = new byte[Size.x * Size.y * 32 * 32 * 4];

            for(int y = 0; y < Size.y; y++)
            {
                for(int x = 0; x < Size.x; x++)
                {
                    ref Tile.Model tile = ref GetTileRef(x, y, planetLayer);

                    int spriteId = tile.SpriteId;

                    if (spriteId >= 0)
                    {
                        
                        GameState.TileSpriteAtlasManager.GetSpriteBytes(spriteId, Bytes);

                        int tileX = (x * 32);
                        int tileY = (y * 32);

                        for (int j = 0; j <  32; j++)
                        {
                            for(int i = 0; i < 32; i++)
                            {
                                int index = 4 * (((i + tileX)) + ((j + tileY)) * (Size.x * 32));
                                int bytesIndex = 4 * (i + (32 - j - 1) * 32);
                                Data[index] = 
                                     Bytes[bytesIndex];
                                Data[index + 1] = 
                                     Bytes[bytesIndex + 1];
                                Data[index + 2] = 
                                     Bytes[bytesIndex + 2];
                                Data[index + 3] = 
                                     Bytes[bytesIndex + 3];
                            }
                        }
                    }
                }
            }
            
            LayerTextures[(int)planetLayer] = Utility.TextureUtils.CreateTextureFromRGBA(Data, Size.x * 32, Size.y * 32);


        }

        public void UpdateTilePositions(int x, int y, PlanetLayer planetLayer)
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
            

            if (x < 0 || y < 0 | x >= Size.x || y >= Size.y)
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

                    if (x + 1 < Size.x)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x + 1, y, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Right] = neighborTile.TileType;
                    }

                    if (x - 1 >= 0)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x - 1, y, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Left] = neighborTile.TileType;
                    }

                    if (y + 1 < Size.y)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x, y + 1, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Up] = neighborTile.TileType;
                    }

                    if (y - 1 >= 0)
                    {
                        ref Tile.Model neighborTile = ref GetTileRef(x, y - 1, planetLayer);
                        neighbors[(int)Enums.Tile.Neighbor.Down] = neighborTile.TileType;
                    }


                    Enums.Tile.Position tilePosition = GetTilePosition(neighbors, tile.TileType);

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

        public void UpdateAllTilePositions(PlanetLayer planetLayer)
        {
            for(int y = 0; y < Size.y; y++)
            {
                for(int x = 0; x < Size.x; x++)
                {
                    UpdateTilePositions(x, y, planetLayer);
                }
            }
        }

        public void DrawLayer(PlanetLayer planetLayer, Material material, Transform transform, int DrawOrder)
        {
            Render.Sprite sprite = new Render.Sprite();
            sprite.Texture = LayerTextures[(int)planetLayer];
            sprite.TextureCoords = new Vector4(0, 0, 1, -1);

            Utility.RenderUtils.DrawSprite(0, 0, 1.0f * Size.x, 1.0f * Size.y, sprite, material, transform, DrawOrder);
            
        }

        public void RemoveTile(int x, int y, PlanetLayer planetLayer)
        {
            if (x < 0 || y < 0 | x >= Size.x || y >= Size.y)
            {
                return;
            }


            ref Tile.Model tile = ref GetTileRef(x, y, planetLayer);

            tile.TileType = -1;

            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
                    UpdateTilePositions(i, j, planetLayer);
                }
            }
        }
        public ref Tile.Model GetTileRef(int x, int y, PlanetLayer planetLayer)
        {
            ref Chunk chunk = ref Chunks.GetChunkRef(x, y);

            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref Tiles[(int)planetLayer][x + y * Size.x];
        }

        public Tile.Model GetTile(int x, int y, PlanetLayer planetLayer)
        {
            return GetTileRef(x, y, planetLayer);
        }

        public void SetTile(int x, int y, Tile.Model tile, PlanetLayer planetLayer)
        {
            ref Chunk chunk = ref Chunks.GetChunkRef(x, y);
            chunk.Seq++; // Updating tile, increment seq
            Tiles[(int)planetLayer][x + y * Size.x] = tile;
        }
        
        public static int CheckTile(int[] neighbors, int rules, int tileId)
         {
             // 16 different values can be stored
             // using only 4 bits for the
             // adjacent tiles 

            int[] NeighborBit = new int[] {
                0x1, 0x2, 0x4, 0x8
            };

            int match = 0;
            // number of total neighbors is 4 right/left/down/up
            for(int i = 0; i < neighbors.Length; i++)
            {
                // check if we have to have the same tileId
                // in this particular neighbor                      
                if ((rules & NeighborBit[i]) == NeighborBit[i])
                {
                    // if this neighbor does not match return -1 immediately
                    if (neighbors[i] != tileId)
                    {
                        return -1;
                    }
                    else 
                    {
                        match++;
                    }
                }
            }


            return match;
        }

        public static Enums.Tile.Position GetTilePosition(int[] neighbors, int tileId)
        {
            int biggestMatch = 0;
            Enums.Tile.Position tilePosition = 0;

            // we have 16 different values for the spriteId
            for(int i = 1; i < 16; i++)
            {
                int match = CheckTile(neighbors, i, tileId);

                // pick only tiles with the biggest match count
                if (match > biggestMatch)
                {
                    biggestMatch = match;
                    tilePosition = (Enums.Tile.Position)i;
                }
            }

            return tilePosition;
        } 
    }
}
