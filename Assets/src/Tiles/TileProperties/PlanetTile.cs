namespace TileProperties
{
    /// <summary> Contains info about tile, include all layers </summary>
    public struct PlanetTile
    {
        // Contains the TileProperties Ids for every layer
        public int TileType;

        public int SpriteId;

        //Health
        public byte Durability;

        public static PlanetTile EmptyTile()
        {
            PlanetTile tile = new PlanetTile();
         
            tile.TileType = -1;

            tile.SpriteId = -1;

            return tile;
        }
    }
}