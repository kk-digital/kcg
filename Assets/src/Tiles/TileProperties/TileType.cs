using Enums;
using System;

namespace TileProperties
{
    public struct TileType
    {
        public string Name; //later use string pool
        public string Description;
        public int TileId;
        public bool AutoMapping;
        
        public TileDrawProperties TileDrawType; //enum, hint for how tile should be drawn

        public int BaseSpriteId;

        public PlanetTileCollisionType TileCollisionType;
        public bool IsExplosive;

        //note: ore is composited, others are just normal

        public byte Durability; //max health of tile
        
        //In case after the first newly created tileproperty with already properties being set,
        //you might want to change it anytime by accessing the id of a tile
        public void SetDescription(string description)
        {
            Description = description;
        }
        public void SetDurability(byte durability)
        {
            Durability = durability;
        }
        public void SetCollisionType(PlanetTileCollisionType collisionType)
        {
            TileCollisionType = collisionType;
        }

        public bool IsSolid 
        {
            get
            {
                return TileCollisionType == PlanetTileCollisionType.TileCollisionTypeSolid;
            }
        }

        private TileType(string name, string description, int baseSpriteId) : this()
        {
            Name = name;
            Description = description;
            BaseSpriteId = baseSpriteId;
            

        }

        private TileType(string name, string description, int baseSpriteId,
            TileDrawProperties tileDrawType, int spriteId) : this(name, description, baseSpriteId)
        {
            TileDrawType = tileDrawType;

        }

        public TileType(string name, string description, int baseSpriteId,
            TileDrawProperties tileDrawType, int spriteId,
            PlanetTileCollisionType tileCollisionType, byte durability,
             bool isExplosive = false)
            : this(name, description, baseSpriteId, tileDrawType, spriteId)
        {
            TileCollisionType = tileCollisionType;
            Durability = durability;
            IsExplosive = isExplosive;
        }
    }
}