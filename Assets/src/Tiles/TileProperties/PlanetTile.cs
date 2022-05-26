namespace TileProperties
{
    /// <summary> Contains info about tile, include all layers </summary>
    public struct PlanetTile
    {
        // Check if this tile actually exist on Array
        public bool Initialized;
        
        // Contains the TileProperties Ids for every layer
        public int[] TileIdPerLayer;

        public sbyte[] LayerOffsetX;
        public sbyte[] LayerOffsetY;

        //Health
        public byte Durability;

        public static PlanetTile EmptyTile()
        {
            PlanetTile tile = new PlanetTile();
            tile.Initialized = false;
         
            tile.TileIdPerLayer = new int[4];
            tile.LayerOffsetX = new sbyte[4];
            tile.LayerOffsetY = new sbyte[4];

            return tile;
        }
    }

}