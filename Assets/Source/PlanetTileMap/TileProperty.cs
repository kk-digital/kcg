using System;
using System.Runtime.CompilerServices;
using Enums.Tile;
using KMath;

//TODO: add material type for block
//TODO: per material coefficient of restitution, coefficient of static friction and coefficient of dynamic friction
//TODO: Want to use elliptical/capsule collider eventually too, not just box collider
//TODO: Each Tile type has as collision type enum, determining collision behavior/lines

namespace PlanetTileMap
{
    /// <summary>
    /// Integer id for tile type, look up tile properties in TilePropertyManager by ID
    /// </summary>
    public struct TileProperty
    {
        public string Name; //later use string pool
        public string Description; //later use string pool
        
        public TileID TileID;
        public int BaseSpriteId;
        
        public byte Durability; //max health of tile
        
        /// <summary>
        /// To map neighbour tiles or not
        /// </summary>
        public bool IsAutoMapping; 

        public SpriteRuleType SpriteRuleType;

        public CollisionType TileCollisionType;
        
        public TileShapeAndRotation Shape;

        public bool IsSolid => TileCollisionType == CollisionType.Solid;

        public TileProperty(TileID tileID, int baseSpriteId) : this()
        {
            TileID = tileID;
            BaseSpriteId = baseSpriteId;
        }
    }
}
