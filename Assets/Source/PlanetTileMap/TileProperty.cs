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
        public TileID TileID;
        public int BaseSpriteId;
        
        public string Name; //later use string pool
        public string Description; //later use string pool

        public TileShapeType ShapeType;

        /// <summary>
        /// To map neighbour tiles or not
        /// </summary>
        public bool IsAutoMapping; 

        public SpriteRuleType SpriteRuleType;

        public CollisionType TileCollisionType;
        public bool IsExplosive;

        //note: ore is composited, others are just normal
        public byte Durability; //max health of tile
        
        public bool IsSolid => TileCollisionType == CollisionType.Solid;

        public TileProperty(TileID tileID, int baseSpriteId) : this()
        {
            TileID = tileID;
            BaseSpriteId = baseSpriteId;
        }
        
        [MethodImpl((MethodImplOptions) 256)] // Inline
        public Vec2f GetTilePointPosition(TilePointType point)
        {
            return point switch
            {
                TilePointType.Error => default,
                TilePointType.C1 => new Vec2f(0f, 1f),
                TilePointType.C2 => new Vec2f(1f, 1f),
                TilePointType.C3 => new Vec2f(1f, 0f),
                TilePointType.C4 => new Vec2f(0f, 0f),
                TilePointType.M1 => new Vec2f(0.5f, 1f),
                TilePointType.M2 => new Vec2f(1f, 0.5f),
                TilePointType.M3 => new Vec2f(0.5f, 0f),
                TilePointType.M4 => new Vec2f(0f, 0.5f),
                _ => default
            };
        }
    }
}