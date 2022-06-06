using Enums;
using System;

namespace Tile
{
    public struct PropertiesData
    {
        public string Name; //later use string pool
        public string Description;
        public int TileId;
        
        public TileDrawProperties TileDrawType; //enum, hint for how tile should be drawn

        public int[] Variants;

        public int SpriteId
        {
            set => Variants[(int)Variant.Middle] = value;
            get => Variants[(int)Variant.Middle];
        }

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

        public bool IsSolid => TileCollisionType == PlanetTileCollisionType.TileCollisionTypeSolid;

        private PropertiesData(string name, string description, int tileId) : this()
        {
            Name = name;
            Description = description;
            TileId = tileId;
            int variantsCount = Enum.GetNames(typeof(Variant)).Length;
            Variants = new int[variantsCount];

        }

        private PropertiesData(string name, string description, int tileId,
            TileDrawProperties tileDrawType, int spriteId) : this(name, description, tileId)
        {
            TileDrawType = tileDrawType;
            SpriteId = spriteId;

        }

        public PropertiesData(string name, string description, int tileId,
            TileDrawProperties tileDrawType, int spriteId,
            PlanetTileCollisionType tileCollisionType, byte durability,
             bool isExplosive = false)
            : this(name, description, tileId, tileDrawType, spriteId)
        {
            TileCollisionType = tileCollisionType;
            Durability = durability;
            IsExplosive = isExplosive;
        }
    }
}