using Enums;

namespace TileProperties
{
    struct PlanetTileProperties
    {
        public string Name; //later use string pool
        public string Description;

        public int TileId; //could be TileId or TileId
        public TileDrawProperties TileDrawType; //enum, hint for how tile should be drawn

        public int SpriteId; //spriteId
        public int SpriteId2; //used for composited tiles, like ore

        public PlanetTileLayer Layer;
        public PlanetTileCollisionType TileCollisionType;

        //note: ore is composited, others are just normal

        public byte Durability; //max health of tile
        public PlanetTileProperties(string Name, string Description, int TileId,
                                    TileDrawProperties TileDrawType, int SpriteId,
                                    int SpriteId2, PlanetTileLayer Layer, 
                                    PlanetTileCollisionType TileCollisionType,
                                    byte Durability)
        {
            this.Name = Name;
            this.Description = Description;
            this.TileId = TileId;
            this.TileDrawType = TileDrawType;
            this.SpriteId = SpriteId;
            this.SpriteId2 = SpriteId;
            this.Layer = Layer;
            this.TileCollisionType = TileCollisionType;
            this.Durability = Durability;
        }
    }
}