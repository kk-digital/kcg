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
        
        /// <summary>
        /// Takes in current shape from Property
        /// </summary>
        /// <returns>All collision lines for each TileShape+Rotation</returns>
        public Line2D[] GetCollisionLines()
        {
            switch (Shape)
            {
                case TileShapeAndRotation.Error:
                    return null;
                case TileShapeAndRotation.EB:
                    return null;
                case TileShapeAndRotation.FB:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.HB_R1:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M4_M2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_M4),
                    };
                case TileShapeAndRotation.HB_R2:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_M1),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M1_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.HB_R3:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_M2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M2_M4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M4_C1),
                    };
                case TileShapeAndRotation.HB_R4:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_M3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M3_M1),
                    };
                case TileShapeAndRotation.TB_R1:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.TB_R2:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.TB_R3:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C1),
                    };
                case TileShapeAndRotation.TB_R4:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C2),
                    };
                case TileShapeAndRotation.LBT_R1:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_M3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBT_R2:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_M4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M4_C1),
                    };
                case TileShapeAndRotation.LBT_R3:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M2_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_M2),
                    };
                case TileShapeAndRotation.LBT_R4:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_M2),
                    };
                case TileShapeAndRotation.LBT_R5:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M4_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_M4),
                    };
                case TileShapeAndRotation.LBT_R6:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_M1),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M1_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBT_R7:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_M2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M2_C1),
                    };
                case TileShapeAndRotation.LBT_R8:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_M3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M3_C2),
                    };
                case TileShapeAndRotation.LBB_R1:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_M1),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M1_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBB_R2:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_M2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M2_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBB_R3:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_M3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M3_C1),
                    };
                case TileShapeAndRotation.LBB_R4:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_M4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M4_C2),
                    };
                case TileShapeAndRotation.LBB_R5:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_M2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBB_R6:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_M3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBB_R7:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_M4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M4_C1),
                    };
                case TileShapeAndRotation.LBB_R8:
                    return new[]
                    {
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_M1_C2),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GameState.TileCreationApi.GetTileLineSegmentPosition(TileLineSegment.L_C4_M1),
                    };
                default:
                    return null;
            }
        }

        /// <summary>
        /// Takes in current shape from Property
        /// </summary>
        /// <returns>Rotates shape 90 degree clockwise and outputs the new shape enum</returns>
        public TileShapeAndRotation RotateShape()
        {
            switch (Shape)
            {
                case TileShapeAndRotation.Error:
                    return TileShapeAndRotation.Error;
                
                case TileShapeAndRotation.EB:
                    return TileShapeAndRotation.EB;
                
                case TileShapeAndRotation.FB:
                    return TileShapeAndRotation.FB;
                
                case TileShapeAndRotation.HB_R1:
                    return TileShapeAndRotation.HB_R2;
                case TileShapeAndRotation.HB_R2:
                    return TileShapeAndRotation.HB_R3;
                case TileShapeAndRotation.HB_R3:
                    return TileShapeAndRotation.HB_R4;
                case TileShapeAndRotation.HB_R4:
                    return TileShapeAndRotation.HB_R1;
                
                case TileShapeAndRotation.TB_R1:
                    return TileShapeAndRotation.TB_R2;
                case TileShapeAndRotation.TB_R2:
                    return TileShapeAndRotation.TB_R3;
                case TileShapeAndRotation.TB_R3:
                    return TileShapeAndRotation.TB_R4;
                case TileShapeAndRotation.TB_R4:
                    return TileShapeAndRotation.TB_R1;
                
                case TileShapeAndRotation.LBT_R1:
                    return TileShapeAndRotation.LBT_R2;
                case TileShapeAndRotation.LBT_R2:
                    return TileShapeAndRotation.LBT_R3;
                case TileShapeAndRotation.LBT_R3:
                    return TileShapeAndRotation.LBT_R4;
                case TileShapeAndRotation.LBT_R4:
                    return TileShapeAndRotation.LBT_R1;
                case TileShapeAndRotation.LBT_R5:
                    return TileShapeAndRotation.LBT_R6;
                case TileShapeAndRotation.LBT_R6:
                    return TileShapeAndRotation.LBT_R7;
                case TileShapeAndRotation.LBT_R7:
                    return TileShapeAndRotation.LBT_R8;
                case TileShapeAndRotation.LBT_R8:
                    return TileShapeAndRotation.LBT_R5;
                
                case TileShapeAndRotation.LBB_R1:
                    return TileShapeAndRotation.LBB_R2;
                case TileShapeAndRotation.LBB_R2:
                    return TileShapeAndRotation.LBB_R3;
                case TileShapeAndRotation.LBB_R3:
                    return TileShapeAndRotation.LBB_R4;
                case TileShapeAndRotation.LBB_R4:
                    return TileShapeAndRotation.LBB_R1;
                case TileShapeAndRotation.LBB_R5:
                    return TileShapeAndRotation.LBB_R6;
                case TileShapeAndRotation.LBB_R6:
                    return TileShapeAndRotation.LBB_R7;
                case TileShapeAndRotation.LBB_R7:
                    return TileShapeAndRotation.LBB_R8;
                case TileShapeAndRotation.LBB_R8:
                    return TileShapeAndRotation.LBB_R5;
                default:
                    return TileShapeAndRotation.Error;
            }
        }
        
        /// <summary>
        /// Takes in current shape from Property
        /// </summary>
        /// <returns>flips/mirrors the shape over the x axis and return the new shape enum</returns>
        public TileShapeAndRotation MirrorShape()
        {
            switch (Shape)
            {
                case TileShapeAndRotation.Error:
                    return TileShapeAndRotation.Error;
                case TileShapeAndRotation.EB:
                    return TileShapeAndRotation.EB;
                case TileShapeAndRotation.FB:
                    return TileShapeAndRotation.FB;
                
                case TileShapeAndRotation.HB_R1:
                    return TileShapeAndRotation.HB_R1;
                case TileShapeAndRotation.HB_R2:
                    return TileShapeAndRotation.HB_R4;
                case TileShapeAndRotation.HB_R3:
                    return TileShapeAndRotation.HB_R3;
                case TileShapeAndRotation.HB_R4:
                    return TileShapeAndRotation.HB_R2;
                
                case TileShapeAndRotation.TB_R1:
                    return TileShapeAndRotation.TB_R4;
                case TileShapeAndRotation.TB_R2:
                    return TileShapeAndRotation.TB_R3;
                case TileShapeAndRotation.TB_R3:
                    return TileShapeAndRotation.TB_R2;
                case TileShapeAndRotation.TB_R4:
                    return TileShapeAndRotation.TB_R1;
                case TileShapeAndRotation.LBT_R1:
                    return TileShapeAndRotation.LBT_R8;
                case TileShapeAndRotation.LBT_R2:
                    return TileShapeAndRotation.LBT_R7;
                case TileShapeAndRotation.LBT_R3:
                    return TileShapeAndRotation.LBT_R6;
                case TileShapeAndRotation.LBT_R4:
                    return TileShapeAndRotation.LBT_R5;
                case TileShapeAndRotation.LBT_R5:
                    return TileShapeAndRotation.LBT_R4;
                case TileShapeAndRotation.LBT_R6:
                    return TileShapeAndRotation.LBT_R3;
                case TileShapeAndRotation.LBT_R7:
                    return TileShapeAndRotation.LBT_R2;
                case TileShapeAndRotation.LBT_R8:
                    return TileShapeAndRotation.LBT_R1;
                
                case TileShapeAndRotation.LBB_R1:
                    return TileShapeAndRotation.LBB_R8;
                case TileShapeAndRotation.LBB_R2:
                    return TileShapeAndRotation.LBB_R7;
                case TileShapeAndRotation.LBB_R3:
                    return TileShapeAndRotation.LBB_R6;
                case TileShapeAndRotation.LBB_R4:
                    return TileShapeAndRotation.LBB_R5;
                case TileShapeAndRotation.LBB_R5:
                    return TileShapeAndRotation.LBB_R4;
                case TileShapeAndRotation.LBB_R6:
                    return TileShapeAndRotation.LBB_R3;
                case TileShapeAndRotation.LBB_R7:
                    return TileShapeAndRotation.LBB_R2;
                case TileShapeAndRotation.LBB_R8:
                    return TileShapeAndRotation.LBB_R1;
                default:
                    return TileShapeAndRotation.Error;
            }
        }
    }
}
