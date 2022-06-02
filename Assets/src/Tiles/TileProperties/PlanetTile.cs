namespace TileProperties
{
    /// <summary> Contains info about tile, include all layers </summary>
    public struct PlanetTile
    {
        // Check if this tile actually exist on Array
        public bool Initialized;
        
        // Contains the TileProperties Ids for every layer
        public int BackTilePropertiesId;
        public int MidTilePropertiesId;
        public int FrontTilePropertiesId;
        public int OreTilePropertiesId;

        //Health
        public byte Durability;

        public static PlanetTile EmptyTile()
        {
            PlanetTile tile = new PlanetTile();
            tile.Initialized = false;
         
            tile.BackTilePropertiesId = -1;
            tile.MidTilePropertiesId = -1;
            tile.FrontTilePropertiesId = -1;
            tile.OreTilePropertiesId = -1;

            return tile;
        }
    }
}