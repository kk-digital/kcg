using Enums;
using System;

namespace Tile
{
    public struct Type
    {
        public int ID;
        public string Name; //later use string pool
        public string Description;

        public bool AutoMapping;
        
        public Enums.Tile.DrawType TileDrawType; //enum, hint for how tile should be drawn

        public int BaseSpriteId;

        public Enums.Tile.CollisionType TileCollisionType;
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
        public void SetCollisionType(Enums.Tile.CollisionType collisionType)
        {
            TileCollisionType = collisionType;
        }

        public bool IsSolid => TileCollisionType == Enums.Tile.CollisionType.Solid;

        private Type(string name, string description, int baseSpriteId) : this()
        {
            Name = name;
            Description = description;
            BaseSpriteId = baseSpriteId;
            

        }

        private Type(string name, string description, int baseSpriteId,
            Enums.Tile.DrawType tileDrawType, int spriteId) : this(name, description, baseSpriteId)
        {
            TileDrawType = tileDrawType;

        }

        public Type(string name, string description, int baseSpriteId,
            Enums.Tile.DrawType tileDrawType, int spriteId,
            Enums.Tile.CollisionType tileCollisionType, byte durability,
             bool isExplosive = false)
            : this(name, description, baseSpriteId, tileDrawType, spriteId)
        {
            TileCollisionType = tileCollisionType;
            Durability = durability;
            IsExplosive = isExplosive;
        }
    }
}