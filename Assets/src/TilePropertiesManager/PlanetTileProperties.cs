using Enums;

namespace TileProperties
{
    struct PlanetTileProperties
    {
        public string Name; //later use string pool

        public int TileId; //could be TileId or TileId
        public TileDrawProperties TileDrawType; //enum, hint for how tile should be drawn

        public int SpriteId; //spriteId
        public int SpriteId2; //used for composited tiles, like ore

        public PlanetTileLayer Layer;
        public PlanetTileCollisionType TileCollisionType;

        //note: ore is composited, others are just normal

        public byte Durability; //max health of tile
    }
}