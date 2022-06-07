//using Entitas;
using System;
using TileProperties;
using Enums;
using UnityEngine;

namespace PlanetTileMap
{
    public struct TopTilesMap
    {
        public int[] Data;

        // more attributes if needed

        public int Width
        {
            get
            {
                return Data.Length;
            }
        }

        public TopTilesMap(int width)
        {
            Data = new int[width];
        }
    }

    public class PlanetTileMap
    {

        public struct ChunkBehaviour
        {
            public Vector2Int Size;

            public PlanetTileMapChunk[] List;
            public int[] IndexList; // 0 = error, 1 = empty, 2 = unexplored (TODO)

            public int Next;

            public PlanetTileMapChunk Error; // todo: fill this with error tiles
            public PlanetTileMapChunk Empty;
        }
        
        // public static const PlanetTile AirTile = new PlanetTile(); - PlanetTile cannot be const in c#?
        public static readonly PlanetTile AirTile = new();

        public PlanetTile[][] Tiles;
        public Vector2Int Size;
        public ChunkBehaviour Chunk;
        public TopTilesMap TopTilesMap;
        public PlanetWrapBehavior WrapBehavior;

        int LayersCount;

        public Vector2Int NaturalLayerChunkSize;
        public Vector2Int NaturalLayerSize;

        public Vector2Int ChunkSize;
        public NaturalLayerChunk[] NaturalLayerChunkList;
        public int[] OreMap;

        public Texture2D[] LayerTextures;

        public PlanetTileMap(Vector2Int size)
        {
            LayersCount = Enum.GetNames(typeof(Layer)).Length;

            NaturalLayerChunkSize = new Vector2Int(16, 16);
            ChunkSize = new Vector2Int(16, 16);

            Size.x = size.x;
            Size.y = size.y;

            Chunk.Size.x = Size.x >> 4;
            Chunk.Size.y = Size.y >> 4;

            Tiles = new PlanetTile[LayersCount][];
            for(int layerIndex = 0; layerIndex < LayersCount; layerIndex++)
            {
                Tiles[layerIndex] = new PlanetTile[Size.x * Size.y];
            }
             

            WrapBehavior = PlanetWrapBehavior.NoWrapAround; // Set to WrapAround manually if needed

            Chunk.Next = 0;

            Chunk.IndexList = new int[Chunk.Size.x * Chunk.Size.y];
            Chunk.List = new PlanetTileMapChunk[Chunk.Size.x * Chunk.Size.y];

            TopTilesMap = new TopTilesMap(Size.x);
            OreMap = new int[Size.x * Size.y];
            NaturalLayerSize = new Vector2Int(Size.x / NaturalLayerChunkSize.x + 1, Size.y / NaturalLayerChunkSize.y + 1);
            NaturalLayerChunkList = new NaturalLayerChunk[NaturalLayerSize.x * NaturalLayerSize.y];
            LayerTextures = new Texture2D[LayersCount];

            for (int i = 0; i < Chunk.IndexList.Length; i++)
                Chunk.IndexList[i] = 2;
        }

        public void BuildLayerTexture(Layer layer)
        {
            byte[] Bytes = new byte[32 * 32 * 4];
            byte[] Data = new byte[Size.x * Size.y * 32 * 32 * 4];

            for(int y = 0; y < Size.y; y++)
            {
                for(int x = 0; x < Size.x; x++)
                {
                    ref PlanetTile tile = ref GetTileRef(x, y, layer);

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
            
            LayerTextures[(int)layer] = Utility.TextureUtils.CreateTextureFromRGBA(Data, Size.x * 32, Size.y * 32);


        }

        public void UpdateTilePositions(int x, int y, Layer layer)
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

            ref PlanetTile tile = ref GetTileRef(x, y, layer);
             
            if (tile.PropertiesId >= 0)
            {
                TileProperties.TilePropertiesData properties = 
                                GameState.TileCreationApi.GetTileProperties(tile.PropertiesId);
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
                        ref PlanetTile neighborTile = ref GetTileRef(x + 1, y, layer);
                        neighbors[(int)Neighbor.Right] = neighborTile.PropertiesId;
                    }

                    if (x - 1 >= 0)
                    {
                        ref PlanetTile neighborTile = ref GetTileRef(x - 1, y, layer);
                        neighbors[(int)Neighbor.Left] = neighborTile.PropertiesId;
                    }

                    if (y + 1 < Size.y)
                    {
                        ref PlanetTile neighborTile = ref GetTileRef(x, y + 1, layer);
                        neighbors[(int)Neighbor.Up] = neighborTile.PropertiesId;
                    }

                    if (y - 1 >= 0)
                    {
                        ref PlanetTile neighborTile = ref GetTileRef(x, y - 1, layer);
                        neighbors[(int)Neighbor.Down] = neighborTile.PropertiesId;
                    }


                    TilePosition position = GetTilePosition(neighbors, tile.PropertiesId);

                    // the sprite ids are next to each other in the sprite atlas
                    // we jus thave to know which one to draw based on the offset
                    tile.SpriteId = properties.BaseSpriteId + TilePositionToTileSet[(int)position];
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

        public void UpdateAllTilePositions(Layer layer)
        {
            for(int y = 0; y < Size.y; y++)
            {
                for(int x = 0; x < Size.x; x++)
                {
                    UpdateTilePositions(x, y, layer);
                }
            }
        }
        


        public void UpdateTopTilesMap()
        {
            for(int i = 0; i < Size.x; i++)
            {
                TopTilesMap.Data[i] = 0;
                for(int j = Size.y - 1; j >= 0; j--)
                {
                    ref PlanetTile tile = ref GetTileRef(i, j, Layer.Front);
                    if (tile.PropertiesId != -1)
                    {
                        TopTilesMap.Data[i] = j;
                        break;
                    }
                }
            }
        }

        public void DrawLayer(Layer layer, Material material, Transform transform, int DrawOrder)
        {
            Render.Sprite sprite = new Render.Sprite();
            sprite.Texture = LayerTextures[(int)layer];
            sprite.TextureCoords = new Vector4(0, 0, 1, -1);

            Utility.RenderUtils.DrawSprite(0, 0, 1.0f * Size.x, 1.0f * Size.y, sprite, material, transform, DrawOrder);
            
        }

        public void RemoveTile(int x, int y, Layer layer)
        {
            if (x < 0 || y < 0 | x >= Size.x || y >= Size.y)
            {
                return;
            }


            ref PlanetTile tile = ref GetTileRef(x, y, layer);

            tile.PropertiesId = -1;

            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
                    UpdateTilePositions(i, j, layer);
                }
            }
        }

        public ref NaturalLayerChunk GetNaturalLayerChunk(int x, int y)
        {
            int index = x / NaturalLayerChunkSize.x + (y / NaturalLayerChunkSize.y) * NaturalLayerSize.x;

            return ref NaturalLayerChunkList[index];
        }

        // Is this really the only way to inline a function in c#?
       // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetChunkIndex(int x, int y)
        {
            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;

            return Chunk.IndexList[(x >> 4) * Chunk.Size.y + (y >> 4)];
        }

        private int AddChunk(PlanetTileMapChunk chunk, int x, int y)
        {
            // I feel like resizing by 1 each time is not very efficient... Change it later?
            Array.Resize(ref Chunk.List, Chunk.Next + 1);

            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;
            chunk.ChunkIndexListID = (x >> 4) * Chunk.Size.y + (y >> 4);

            Chunk.IndexList[chunk.ChunkIndexListID] = Chunk.Next + 3;
            Chunk.List[Chunk.Next] = chunk;
            Chunk.Next++;
            return Chunk.Next + 2;
        }

        public PlanetTileMapChunk GetChunk(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0: return Chunk.Error;
                case 1: return Chunk.Empty;
                case 2: return Chunk.Empty; // UNEXPLORED
            }

            Chunk.List[chunkIndex - 3].Usage++;
            return Chunk.List[chunkIndex - 3];
        }

        public ref PlanetTileMapChunk GetChunkRef(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);

            switch (chunkIndex)
            {
                case 0:
                    throw new IndexOutOfRangeException();
                // We are getting a reference here, most likely to edit the chunk / add a tile, so we can't just return an empty chunk
                // Instead, we will just create a new chunk
                case < 3:
                    chunkIndex = AddChunk(new PlanetTileMapChunk(), x, y);
                    break;
            }

            Chunk.List[chunkIndex - 3].Usage++;
            return ref Chunk.List[chunkIndex - 3];
        }

        public void SetChunk(int x, int y, PlanetTile[][,] tiles)
        {
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0:
                    return;
                case < 3:
                    chunkIndex = AddChunk(new PlanetTileMapChunk(), x, y);
                    break;
            }

            Chunk.List[chunkIndex - 3].Seq++;

            int beginX = (int)(x / ChunkSize.x) * ChunkSize.x;
            int beginY = (int)(y / ChunkSize.y) * ChunkSize.y;

        
            for(int layerIndex = 0; layerIndex < LayersCount; layerIndex++)
            {
                for (int i = 0; i < 16; i++)
                    for (int j = 0; j < 16; j++)
                        Tiles[layerIndex][(i + beginX) + (j + beginY) * Size.x] = tiles[layerIndex][i, j];
            }
        }
        public ref PlanetTile GetTileRef(int x, int y, Layer layer)
        {
            ref PlanetTileMapChunk chunk = ref GetChunkRef(x, y);

            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref Tiles[(int)layer][x + y * Size.x];
        }

        public PlanetTile GetTile(int x, int y, Layer layer)
        {
            return GetTileRef(x, y, layer);
        }

        public void SetTile(int x, int y, PlanetTile tile, Layer layer)
        {
            ref PlanetTileMapChunk chunk = ref GetChunkRef(x, y);
            chunk.Seq++; // Updating tile, increment seq
            Tiles[(int)layer][x + y * Size.x] = tile;
        }


        // Sort chunks by most used using quick sort

        private void swap(int index1, int index2)
        {
            // Swap chunks
            (Chunk.List[index1], Chunk.List[index2]) = (Chunk.List[index2], Chunk.List[index1]);

            // Then update chunk index list - This is what storing the Position inside the chunk is most useful for
            Chunk.IndexList[Chunk.List[index1].ChunkIndexListID] = index1 + 3;
            Chunk.IndexList[Chunk.List[index2].ChunkIndexListID] = index2 + 3;
        }

        private int partition(int start, int end)
        {
            // Use negative of the usage to have the list sorted from most used to least used without having to reverse afterwards
            int p = -Chunk.List[start].Usage;

            int count = 0;
            for (int k = start + 1; k <= end; k++)
                if (-Chunk.List[k].Usage <= p)
                    count++;

            int pi = start + count;
            swap(pi, start);

            int i = start, j = end;

            while (i < pi && j > pi)
            {
                while (-Chunk.List[i].Usage <= p) i++;
                while (-Chunk.List[j].Usage > p) j--;

                if (i < pi && j > pi)
                    swap(i++, j--);
            }

            return pi;
        }

        private void quickSort(int start, int end)
        {
            if (start >= end) return;

            int p = partition(start, end);
            quickSort(start, p - 1);
            quickSort(p + 1, end);
        }

        public void SortChunks()
        {
            // Sort chunks from most used to least used
            if (Chunk.List == null || Chunk.List.Length == 0) return;

            quickSort(0, Chunk.Next - 1);
        }
    

        //Take in PlanetTileMap, and map a horizonal line
    public void GenerateFlatPlanet()
        {
            //default size = X...

            //make a single line horizonally across planet
            //from left to right

            //int TileId = GetTileId("default-tile")
           //for x = 0 to x = Planet.Size.X
           //Planet.SetTile(TileId, x, 10)


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

        public static TilePosition GetTilePosition(int[] neighbors, int tileId)
        {
            int biggestMatch = 0;
            TilePosition position = (TilePosition)0;

            // we have 16 different values for the spriteId
            for(int i = 1; i < 16; i++)
            {
                int match = CheckTile(neighbors, i, tileId);

                // pick only tiles with the biggest match count
                if (match > biggestMatch)
                {
                    biggestMatch = match;
                    position = (TilePosition)i;
                }
            }

            return position;
        } 
    }
}
