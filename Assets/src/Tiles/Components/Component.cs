namespace Tile
{
    /// <summary> Contains info about tile, include all layers </summary>
    public struct Component
    {
        // Contains the TileProperties Ids for every layer
        public int BackTilePropertiesId;
        public int MidTilePropertiesId;
        public int FrontTilePropertiesId;
        public int OreTilePropertiesId;

        public int BackSpriteId;
        public int MidSpriteId;
        public int FrontSpriteId;
        public int OreSpriteId;

        //Health
        public byte Durability;

        public static Tile.Component EmptyTile()
        {
            var tile = new Tile.Component
            {
                BackTilePropertiesId = -1,
                MidTilePropertiesId = -1,
                FrontTilePropertiesId = -1,
                OreTilePropertiesId = -1,
                BackSpriteId = -1,
                MidSpriteId = -1,
                FrontSpriteId = -1,
                OreSpriteId = -1
            };

            return tile;
        }
    }
}