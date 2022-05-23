using Enums;

namespace TileProperties
{
    public struct TileProperties
    {
        public string Name; //later use string pool
        public string Description;

        public int TileId; //could be TileId or TileId
        public TileDrawProperties TileDrawType; //enum, hint for how tile should be drawn

        public int SpriteId; //spriteId
        public int SpriteId2; //used for composited tiles, like ore
        
        public PlanetTileLayer Layer;
        public PlanetTileCollisionType TileCollisionType;
        public bool TilePropertyIsExplosive;

        //note: ore is composited, others are just normal

        public byte Durability; //max health of tile
        
        //In case after the first newly created tileproperty with already properties being set,
        //you might want to change it anytime by accessing the id of a tile
        public void SetTileID(int TileId) => this.TileId = TileId;
        public void SetTileTexture(int row, int column)
        {
            //WIP
        }
        public void SetPropertyIsExplosive(bool TilePropertyIsExplosive) => this.TilePropertyIsExplosive = TilePropertyIsExplosive;
        public void DefineTile(PlanetTileCollisionType collisionType, PlanetTileLayer planetTileLayer, string name)
        {
            this.TileCollisionType = collisionType;
            this.Layer = planetTileLayer;
            this.Name = name;
        }
        public void SetDescription(string Description) => this.Description = Description;
        public void SetDurability(byte Durability) => this.Durability = Durability;
        public void SetCollisionType(PlanetTileCollisionType TileCollisionType) => this.TileCollisionType = TileCollisionType;
        public TileProperties(string Name, string Description, int TileId,
                                    TileDrawProperties TileDrawType, int SpriteId,
                                    int SpriteId2, PlanetTileLayer Layer, 
                                    PlanetTileCollisionType TileCollisionType,
                                    byte Durability, bool TilePropertyIsExplosive)
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
            this.TilePropertyIsExplosive = TilePropertyIsExplosive;
        }
    }
}