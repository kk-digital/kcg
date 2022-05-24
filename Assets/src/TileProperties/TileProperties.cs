using Enums;

namespace TileProperties
{
    public struct TileProperties
    {
        public string Name; //later use string pool
        public string Description;

        public int TileId; //could be TileId or TileId
        public TileDrawProperties TileDrawType; //enum, hint for how tile should be drawn

        public bool IsExplosive;

        public int SpriteId; //spriteId
        public int SpriteId2; //used for composited tiles, like ore

        public PlanetTileLayer Layer;
        public PlanetTileCollisionType TileCollisionType;

        //note: ore is composited, others are just normal

        public byte Durability; //max health of tile
        
        //In case after the first newly created tileproperty with already properties being set,
        //you might want to change it anytime by accessing the id of a tile
        public void SetDescription(string description)
        {
            this.Description = description;
        }
        public void SetDurability(byte durability)
        {
            this.Durability = durability;
        }
        public void SetCollisionType(PlanetTileCollisionType collisionType)
        {
            this.TileCollisionType = collisionType;
        }
        public TileProperties(string Name, string Description, int TileId,
                                    TileDrawProperties TileDrawType, int SpriteId,
                                    int SpriteId2, PlanetTileLayer Layer, 
                                    PlanetTileCollisionType TileCollisionType,
                                    byte Durability, bool isExplosive = false)
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
            this.IsExplosive = isExplosive;
        }
    }
}