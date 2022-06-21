using System.Runtime.CompilerServices;
using KMath;

namespace Tile
{
    //TODO: add material type for block
    //TODO: per material coefficient of restitution, coefficient of static friction and coefficient of dynamic friction
    //TODO: Want to use elliptical/capsule collider eventually too, not just box collider
    //TODO: Each Tile type has as collision type enum, determining collision behavior/lines
    /// <summary> Contains info about tile, include all layers </summary>
    public struct Tile
    {
        public static readonly Tile Empty = new() {Type = -1, SpriteId = -1};
        public static readonly Vec2f Size = new(1, 1);
        
        // Contains the TileProperties Ids for every layer
        public int Type;
        public int SpriteId;

        public AABB2D Borders;
        /// <summary>
        /// Index position based on Chunk
        /// </summary>
        public int Index;

        public Tile(Vec2f position) : this()
        {
            Type = -1;
            SpriteId = -1;
            Borders = new AABB2D(position, (Vec2f)Size);
            Index = GetTileIndex((int)position.X, (int)position.Y);
        }
        
        
        /// <summary>
        /// Getting Tile index by Chunk Dimensions. INLINED
        /// </summary>
        /// <param name="x">TileMap coordinates</param>
        /// <param name="y">TileMap coordinates</param>
        /// <returns>Tile index</returns>
        [MethodImpl((MethodImplOptions) 256)]
        public static int GetTileIndex(int x, int y)
        {
            // x & 0x0f == x AND 15
            // EX: 16 AND 15 == 0, 13 AND 15 == 13
            // (<< 4) == (* 16) 
            return (x & 0x0f) + ((y & 0x0f) << 4);
        }


        // TODO: Refactor
        public int CheckTile(int[] neighbors, int rules, int tileId)
        {
            // 16 different values can be stored
            // using only 4 bits for the
            // adjacent tiles 

            int[] neighborBit = {
                0x1, 0x2, 0x4, 0x8
            };

            int match = 0;
            // number of total neighbors is 4 right/left/down/up
            for(int i = 0; i < neighbors.Length; i++)
            {
                // check if we have to have the same tileId
                // in this particular neighbor                      
                if ((rules & neighborBit[i]) == neighborBit[i])
                {
                    // if this neighbor does not match return -1 immediately
                    if (neighbors[i] != tileId) return -1;
                    match++;
                }
            }


            return match;
        }

        // TODO: Refactor
        public Enums.Tile.Position GetTilePosition(int[] neighbors, int tileId)
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