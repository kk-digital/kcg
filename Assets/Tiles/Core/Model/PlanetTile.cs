namespace Tiles
{
    //TODO: actually map has more 4 layers, need to fix
    /// <summary> Contains info about tile, include all layers </summary>
    struct PlanetTile
    {
        //Back tile
        public int BackSpriteId;
        public int BackTileId;

        //Mid tile
        public int MidSpriteId;
        public int MidTileId;

        //Front tile
        public int FrontSpriteId;
        public int FrontTileId;
        
        //Furniture
        public int FurnitureSpriteId;
        public int FurnitureTileId;
        public sbyte FurnitureOffsetX;
        public sbyte FurnitureOffsetY;

        //Health
        public byte Durability;
    }
}