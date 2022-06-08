namespace Tile
{
    /// <summary> Contains info about tile, include all layers </summary>
    public struct Model
    {
        // Contains the TileProperties Ids for every layer
        public int TileType;

        public int SpriteId;

        //Health
        public byte Durability;

        public static Model EmptyTile()
        {
            Model tile = new Model();
         
            tile.TileType = -1;

            return model;
        }
    }
}