//using Entitas;
using System;
using Enums;
using UnityEngine;

namespace Tile
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

    public struct Map
    {

        public struct ChunkBehaviour
        {
            public Vector2Int Size;

            public MapChunk[] List;
            public int[] IndexList; // 0 = error, 1 = empty, 2 = unexplored (TODO)

            public int Next;

            public MapChunk Error; // todo: fill this with error tiles
            public MapChunk Empty;
        }
        
        // public static const PlanetTile AirTile = new PlanetTile(); - PlanetTile cannot be const in c#?
        public static readonly Tile.Component AirTile = new();

        public Vector2Int Size;
        public ChunkBehaviour Chunk;
        public TopTilesMap TopTilesMap;
        public PlanetWrapBehavior WrapBehavior;

        public Vector2Int NaturalLayerChunkSize;
        public Vector2Int NaturalLayerSize;

        public MapChunk.NaturalLayerChunk[] NaturalLayerChunkList;
        public int[] OreMap;

        public Texture2D[] LayerTextures;

        public Map(Vector2Int size) : this()
        {
            int layersCount = Enum.GetNames(typeof(Planet.Layer)).Length;

            NaturalLayerChunkSize = new Vector2Int(16, 16);

            Size.x = size.x;
            Size.y = size.y;

            Chunk.Size.x = Size.x >> 4;
            Chunk.Size.y = Size.y >> 4;

            WrapBehavior = PlanetWrapBehavior.NoWrapAround; // Set to WrapAround manually if needed

            Chunk.Next = 0;

            Chunk.IndexList = new int[Chunk.Size.x * Chunk.Size.y];
            Chunk.List = new MapChunk[Chunk.Size.x * Chunk.Size.y];

            TopTilesMap = new TopTilesMap(Size.x);
            OreMap = new int[Size.x * Size.y];
            NaturalLayerSize = new Vector2Int(Size.x / NaturalLayerChunkSize.x + 1, Size.y / NaturalLayerChunkSize.y + 1);
            NaturalLayerChunkList = new MapChunk.NaturalLayerChunk[NaturalLayerSize.x * NaturalLayerSize.y];
            LayerTextures = new Texture2D[layersCount];

            for (int i = 0; i < Chunk.IndexList.Length; i++)
                Chunk.IndexList[i] = 2;
        }

        public void BuildLayerTexture(Planet.Layer layer)
        {
            byte[] Bytes = new byte[32 * 32 * 4];
            byte[] Data = new byte[Size.x * Size.y * 32 * 32 * 4];

            for(int y = 0; y < Size.y; y++)
            {
                for(int x = 0; x < Size.x; x++)
                {
                    ref Tile.Component tile = ref GetTileRef(x, y);

                    int spriteId = -1;

                    if (layer == Planet.Layer.Back)
                    {
                        spriteId = tile.BackSpriteId;
                    }
                    else if (layer == Planet.Layer.Mid)
                    {
                        spriteId = tile.MidSpriteId;
                    }
                    else if (layer == Planet.Layer.Front)
                    {
                        spriteId = tile.FrontSpriteId;
                    }
                    else if (layer == Planet.Layer.Ore)
                    {
                        spriteId = tile.OreSpriteId;
                    }

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

        public void UpdateTileVariants(int x, int y, Planet.Layer layer)
        {
            if (x < 0 || y < 0 | x >= Size.x || y >= Size.y)
            {
                return;
            }

            ref Tile.Component tile = ref GetTileRef(x, y);
            
            if (layer == Planet.Layer.Front)
            {
                if (tile.FrontTilePropertiesId >= 0)
                {
                    int[] neighbors = new int[8];
                    for(int i = 0; i < neighbors.Length; i++)
                    {
                        neighbors[i] = -1;
                    }

                    if (x + 1 < Size.x)
                    {
                        ref Tile.Component neighborTile = ref GetTileRef(x + 1, y);
                        neighbors[(int)Neighbor.Right] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x - 1 >= 0)
                    {
                        ref Tile.Component neighborTile = ref GetTileRef(x - 1, y);
                        neighbors[(int)Neighbor.Left] = neighborTile.FrontTilePropertiesId;
                    }

                    if (y + 1 < Size.y)
                    {
                        ref Tile.Component neighborTile = ref GetTileRef(x, y + 1);
                        neighbors[(int)Neighbor.Top] = neighborTile.FrontTilePropertiesId;
                    }

                    if (y - 1 >= 0)
                    {
                        ref Tile.Component neighborTile = ref GetTileRef(x, y - 1);
                        neighbors[(int)Neighbor.Bottom] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x + 1 < Size.x && y + 1 < Size.y)
                    {
                        ref Tile.Component neighborTile = ref GetTileRef(x + 1, y + 1);
                        neighbors[(int)Neighbor.TopRight] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x - 1 >= 0 && y + 1 < Size.y)
                    {
                        ref Tile.Component neighborTile = ref GetTileRef(x - 1, y + 1);
                        neighbors[(int)Neighbor.TopLeft] = neighborTile.FrontTilePropertiesId;
                    }

                    if (x + 1 < Size.x && y - 1 >= 0)
                    {
                        ref Tile.Component neighborTile = ref GetTileRef(x + 1, y - 1);
                        neighbors[(int)Neighbor.BottomRight] = neighborTile.FrontTilePropertiesId;
                    }
                
                    if (x - 1 >= 0 && y - 1 >= 0)
                    {
                        ref Tile.Component neighborTile = ref GetTileRef(x - 1, y - 1);
                        neighbors[(int)Neighbor.BottomLeft] = neighborTile.FrontTilePropertiesId;
                    }

                    Variant variant = TileNeighbor.GetVariant(neighbors, tile.FrontTilePropertiesId);
                    PropertiesData properties = GameState.CreationAPISystem.GetTileProperties(tile.FrontTilePropertiesId);
                    tile.FrontSpriteId = properties.Variants[(int)variant];
                }
                else
                {
                    tile.FrontSpriteId = -1;
                }
            }



        }

        public void UpdateAllTileVariants(Planet.Layer layer)
        {
            for(int y = 0; y < Size.y; y++)
            {
                for(int x = 0; x < Size.x; x++)
                {
                    UpdateTileVariants(x, y, layer);
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
                    ref Tile.Component tile = ref GetTileRef(i, j);
                    if (tile.BackTilePropertiesId != -1)
                    {
                        TopTilesMap.Data[i] = j;
                        break;
                    }
                }
            }
        }

        public void DrawLayer(Planet.Layer layer, Material material, Transform transform, int DrawOrder)
        {
            Render.Sprite sprite = new Render.Sprite();
            sprite.Texture = LayerTextures[(int)layer];
            sprite.TextureCoords = new Vector4(0, 0, 1, -1);

            Utility.RenderUtils.DrawSprite(0, 0, 1.0f * Size.x, 1.0f * Size.y, sprite, material, transform, DrawOrder);
            
        }

        public void RemoveTile(int x, int y, Planet.Layer layer)
        {
            if (x < 0 || y < 0 | x >= Size.x || y >= Size.y)
            {
                return;
            }

            if (layer == Planet.Layer.Back)
            {
                ref Tile.Component tile = ref GetTileRef(x, y);

                tile.BackTilePropertiesId = -1;
            }
            else if (layer == Planet.Layer.Mid)
            {
                ref Tile.Component tile = ref GetTileRef(x, y);

                tile.MidTilePropertiesId = -1;
            }
            else if (layer == Planet.Layer.Front)
            {
                ref Tile.Component tile = ref GetTileRef(x, y);

                tile.FrontTilePropertiesId = -1;
            }
            else if (layer == Planet.Layer.Ore)
            {
                ref Tile.Component tile = ref GetTileRef(x, y);

                tile.OreTilePropertiesId = -1;
            }

            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
                    UpdateTileVariants(i, j, layer);
                }
            }
        }

        public ref MapChunk.NaturalLayerChunk GetNaturalLayerChunk(int x, int y)
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

        private int AddChunk(MapChunk chunk, int x, int y)
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

        public MapChunk GetChunk(int x, int y)
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

        public ref MapChunk GetChunkRef(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);

            switch (chunkIndex)
            {
                case 0:
                    throw new IndexOutOfRangeException();
                // We are getting a reference here, most likely to edit the chunk / add a tile, so we can't just return an empty chunk
                // Instead, we will just create a new chunk
                case < 3:
                    chunkIndex = AddChunk(new MapChunk(), x, y);
                    break;
            }

            Chunk.List[chunkIndex - 3].Usage++;
            return ref Chunk.List[chunkIndex - 3];
        }

        public void SetChunk(int x, int y, Tile.Component[,] tiles)
        {
            int chunkIndex = GetChunkIndex(x, y);
            switch (chunkIndex)
            {
                case 0:
                    return;
                case < 3:
                    chunkIndex = AddChunk(new MapChunk(), x, y);
                    break;
            }

            Chunk.List[chunkIndex - 3].Seq++;

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    Chunk.List[chunkIndex - 3].Tiles[i, j] = tiles[i, j];
        }
        public ref Tile.Component GetTileRef(int x, int y)
        {
            ref MapChunk chunk = ref GetChunkRef(x, y);

            chunk.Seq++; // We are getting a reference to the tile, so we are probably modifying the tile, hence increment seq

            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
        }

        public Tile.Component GetTile(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);
            return chunkIndex == 1 ? AirTile : Chunk.List[chunkIndex - 3].Tiles[x & 0x0F, y & 0x0f];
        }

        public void SetTile(int x, int y, Tile.Component tile)
        {
            ref MapChunk chunk = ref GetChunkRef(x, y);
            chunk.Seq++; // Updating tile, increment seq
            chunk.Tiles[x & 0x0F, y & 0x0F] = tile;
        }

        public ref Tile.Component getTile(int x, int y)
        {
            ref MapChunk chunk = ref GetChunkRef(x, y);
            return ref chunk.Tiles[x & 0x0F, y & 0x0F];
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
    }
}
