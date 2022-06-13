using KMath;

namespace Tile
{
    /// <summary> Contains info about tile, include all layers </summary>
    public struct Tile
    {
        public static readonly Tile EmptyTile = new() {Type = -1, SpriteId = -1};
        public static readonly Vec2i Size = new(1, 1);
        
        // Contains the TileProperties Ids for every layer
        public int Type;
        public int SpriteId;

        public Box Borders;

        //public Verticies BoxBorders;
        //Health
        public byte Durability;

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