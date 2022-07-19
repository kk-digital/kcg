using System.Runtime.CompilerServices;
using Enums.Tile;
using KMath;

namespace PlanetTileMap
{
    public static class TileGeometry
    {
        // Takes in TilePoint C1, C2 etc index
        // Returns Vec2f point
        public static readonly Vec2f[] TilePointsArray;
        
        // Takes in TileLineSegment L_C0_M2, etc index
        // Returns the start and finish point of line
        public static readonly Line2D[] LinePointsArray;
        
        // Takes in TileShapeAndRotation index
        // Returns number of lines for this shape
        public static readonly int[] ShapeIndex;
        
        static TileGeometry()
        {
            TilePointsArray = new Vec2f[]
            {
                // Error
                default,
                
                // C1, C2, C3, C4
                new(0f, 1f), new(1f, 1f), new(1f, 0f), new(0f, 0f),
                
                // M1, M2, M3, M4
                new(0.5f, 1f), new(1f, 0.5f), new(0.5f, 0f), new(0f, 0.5f)
            };
            
            LinePointsArray = new Line2D[]
            {
                new(TilePointsArray[(int)TilePoint.C1], TilePointsArray[(int)TilePoint.C2]), // L_C1_C2
                new(TilePointsArray[(int)TilePoint.C1], TilePointsArray[(int)TilePoint.C3]), // L_C1_C3
                new(TilePointsArray[(int)TilePoint.C1], TilePointsArray[(int)TilePoint.C4]), // L_C1_C4
                new(TilePointsArray[(int)TilePoint.C1], TilePointsArray[(int)TilePoint.M1]), // L_C1_M1
                new(TilePointsArray[(int)TilePoint.C1], TilePointsArray[(int)TilePoint.M2]), // L_C1_M2
                new(TilePointsArray[(int)TilePoint.C1], TilePointsArray[(int)TilePoint.M3]), // L_C1_M3
                new(TilePointsArray[(int)TilePoint.C1], TilePointsArray[(int)TilePoint.M4]), // L_C1_M4
                
                new(TilePointsArray[(int)TilePoint.C2], TilePointsArray[(int)TilePoint.C1]), // L_C2_C1
                new(TilePointsArray[(int)TilePoint.C2], TilePointsArray[(int)TilePoint.C3]), // L_C2_C3
                new(TilePointsArray[(int)TilePoint.C2], TilePointsArray[(int)TilePoint.C4]), // L_C2_C4
                new(TilePointsArray[(int)TilePoint.C2], TilePointsArray[(int)TilePoint.M1]), // L_C2_M1
                new(TilePointsArray[(int)TilePoint.C2], TilePointsArray[(int)TilePoint.M2]), // L_C2_M2
                new(TilePointsArray[(int)TilePoint.C2], TilePointsArray[(int)TilePoint.M3]), // L_C2_M3
                new(TilePointsArray[(int)TilePoint.C2], TilePointsArray[(int)TilePoint.M4]), // L_C2_M4
                
                new(TilePointsArray[(int)TilePoint.C3], TilePointsArray[(int)TilePoint.C1]), // L_C3_C1
                new(TilePointsArray[(int)TilePoint.C3], TilePointsArray[(int)TilePoint.C2]), // L_C3_C2
                new(TilePointsArray[(int)TilePoint.C3], TilePointsArray[(int)TilePoint.C4]), // L_C3_C4
                new(TilePointsArray[(int)TilePoint.C3], TilePointsArray[(int)TilePoint.M1]), // L_C3_M1
                new(TilePointsArray[(int)TilePoint.C3], TilePointsArray[(int)TilePoint.M2]), // L_C3_M2
                new(TilePointsArray[(int)TilePoint.C3], TilePointsArray[(int)TilePoint.M3]), // L_C3_M3
                new(TilePointsArray[(int)TilePoint.C3], TilePointsArray[(int)TilePoint.M4]), // L_C3_M4
                
                new(TilePointsArray[(int)TilePoint.C4], TilePointsArray[(int)TilePoint.C1]), // L_C4_C1
                new(TilePointsArray[(int)TilePoint.C4], TilePointsArray[(int)TilePoint.C2]), // L_C4_C2
                new(TilePointsArray[(int)TilePoint.C4], TilePointsArray[(int)TilePoint.C3]), // L_C4_C3
                new(TilePointsArray[(int)TilePoint.C4], TilePointsArray[(int)TilePoint.M1]), // L_C4_M1
                new(TilePointsArray[(int)TilePoint.C4], TilePointsArray[(int)TilePoint.M2]), // L_C4_M2
                new(TilePointsArray[(int)TilePoint.C4], TilePointsArray[(int)TilePoint.M3]), // L_C4_M3
                new(TilePointsArray[(int)TilePoint.C4], TilePointsArray[(int)TilePoint.M4]), // L_C4_M4
                
                new(TilePointsArray[(int)TilePoint.M1], TilePointsArray[(int)TilePoint.C1]), // L_M1_C1
                new(TilePointsArray[(int)TilePoint.M1], TilePointsArray[(int)TilePoint.C2]), // L_M1_C2
                new(TilePointsArray[(int)TilePoint.M1], TilePointsArray[(int)TilePoint.C3]), // L_M1_C3
                new(TilePointsArray[(int)TilePoint.M1], TilePointsArray[(int)TilePoint.C4]), // L_M1_M1
                new(TilePointsArray[(int)TilePoint.M1], TilePointsArray[(int)TilePoint.M2]), // L_M1_M2
                new(TilePointsArray[(int)TilePoint.M1], TilePointsArray[(int)TilePoint.M3]), // L_M1_M3
                new(TilePointsArray[(int)TilePoint.M1], TilePointsArray[(int)TilePoint.M4]), // L_M1_M4
                
                new(TilePointsArray[(int)TilePoint.M2], TilePointsArray[(int)TilePoint.C1]), // L_M2_C1
                new(TilePointsArray[(int)TilePoint.M2], TilePointsArray[(int)TilePoint.C2]), // L_M2_C2
                new(TilePointsArray[(int)TilePoint.M2], TilePointsArray[(int)TilePoint.C3]), // L_M2_C3
                new(TilePointsArray[(int)TilePoint.M2], TilePointsArray[(int)TilePoint.C4]), // L_M2_M1
                new(TilePointsArray[(int)TilePoint.M2], TilePointsArray[(int)TilePoint.M1]), // L_M2_M1
                new(TilePointsArray[(int)TilePoint.M2], TilePointsArray[(int)TilePoint.M3]), // L_M2_M3
                new(TilePointsArray[(int)TilePoint.M2], TilePointsArray[(int)TilePoint.M4]), // L_M2_M4
                
                new(TilePointsArray[(int)TilePoint.M3], TilePointsArray[(int)TilePoint.C1]), // L_M3_C1
                new(TilePointsArray[(int)TilePoint.M3], TilePointsArray[(int)TilePoint.C2]), // L_M3_C2
                new(TilePointsArray[(int)TilePoint.M3], TilePointsArray[(int)TilePoint.C3]), // L_M3_C3
                new(TilePointsArray[(int)TilePoint.M3], TilePointsArray[(int)TilePoint.C4]), // L_M3_M1
                new(TilePointsArray[(int)TilePoint.M3], TilePointsArray[(int)TilePoint.M1]), // L_M3_M1
                new(TilePointsArray[(int)TilePoint.M3], TilePointsArray[(int)TilePoint.M2]), // L_M3_M2
                new(TilePointsArray[(int)TilePoint.M3], TilePointsArray[(int)TilePoint.M4]), // L_M3_M4
                
                new(TilePointsArray[(int)TilePoint.M4], TilePointsArray[(int)TilePoint.C1]), // L_M4_C1
                new(TilePointsArray[(int)TilePoint.M4], TilePointsArray[(int)TilePoint.C2]), // L_M4_C2
                new(TilePointsArray[(int)TilePoint.M4], TilePointsArray[(int)TilePoint.C3]), // L_M4_C3
                new(TilePointsArray[(int)TilePoint.M4], TilePointsArray[(int)TilePoint.C4]), // L_M4_M1
                new(TilePointsArray[(int)TilePoint.M4], TilePointsArray[(int)TilePoint.M1]), // L_M4_M1
                new(TilePointsArray[(int)TilePoint.M4], TilePointsArray[(int)TilePoint.M2]), // L_M4_M2
                new(TilePointsArray[(int)TilePoint.M4], TilePointsArray[(int)TilePoint.M3]), // L_M4_M3
            };

            ShapeIndex = new[]
            {
                -1, // Error
                
                0, // EB

                4, // FB

                4, // HB_R1
                4, // HB_R2
                4, // HB_R3
                4, // HB_R4

                3, // TB_R1
                3, // TB_R2
                3, // TB_R3
                3, // TB_R4

                4, // LBT_R1
                4, // LBT_R2
                4, // LBT_R3
                4, // LBT_R4
                4, // LBT_R5
                4, // LBT_R6
                4, // LBT_R7
                4, // LBT_R8

                4, // LBB_R1
                4, // LBB_R2
                4, // LBB_R3
                4, // LBB_R4
                4, // LBB_R5
                4, // LBB_R6
                4, // LBB_R7
                4, // LBB_R8
            };
        }
        
        /// <summary>
        /// Takes in TilePoint enum
        /// </summary>
        /// <returns>Vec2f for point values</returns>
        [MethodImpl((MethodImplOptions) 256)] // Inline
        public static Vec2f GetTilePointPosition(TilePoint point)
        {
            switch (point)
            {
                case TilePoint.Error:
                    return default;
                case TilePoint.C1:
                    return new Vec2f(0f, 1f);
                case TilePoint.C2:
                    return new Vec2f(1f, 1f);
                case TilePoint.C3:
                    return new Vec2f(1f, 0f);
                case TilePoint.C4:
                    return new Vec2f(0f, 0f);
                case TilePoint.M1:
                    return new Vec2f(0.5f, 1f);
                case TilePoint.M2:
                    return new Vec2f(1f, 0.5f);
                case TilePoint.M3:
                    return new Vec2f(0.5f, 0f);
                case TilePoint.M4:
                    return new Vec2f(0f, 0.5f);
                default:
                    return default;
            }
        }
        
        /// <summary>
        /// Takes in TileLineSegment
        /// </summary>
        /// <returns>The start and finish line</returns>
        [MethodImpl((MethodImplOptions) 256)] // Inline
        public static Line2D GetTileLineSegmentPosition(TileLineSegment lineSegment)
        {
            switch (lineSegment)
            {
                case TileLineSegment.Error:
                    return default;
                case TileLineSegment.L_C1_C2:
                    return new Line2D(GetTilePointPosition(TilePoint.C1), GetTilePointPosition(TilePoint.C2));
                case TileLineSegment.L_C1_C3:
                    return new Line2D(GetTilePointPosition(TilePoint.C1), GetTilePointPosition(TilePoint.C3));
                case TileLineSegment.L_C1_C4:
                    return new Line2D(GetTilePointPosition(TilePoint.C1), GetTilePointPosition(TilePoint.C4));
                case TileLineSegment.L_C1_M1:
                    return new Line2D(GetTilePointPosition(TilePoint.C1), GetTilePointPosition(TilePoint.M1));
                case TileLineSegment.L_C1_M2:
                    return new Line2D(GetTilePointPosition(TilePoint.C1), GetTilePointPosition(TilePoint.M2));
                case TileLineSegment.L_C1_M3:
                    return new Line2D(GetTilePointPosition(TilePoint.C1), GetTilePointPosition(TilePoint.M3));
                case TileLineSegment.L_C1_M4:
                    return new Line2D(GetTilePointPosition(TilePoint.C1), GetTilePointPosition(TilePoint.M4));
                case TileLineSegment.L_C2_C1:
                    return new Line2D(GetTilePointPosition(TilePoint.C2), GetTilePointPosition(TilePoint.C1));
                case TileLineSegment.L_C2_C3:
                    return new Line2D(GetTilePointPosition(TilePoint.C2), GetTilePointPosition(TilePoint.C3));
                case TileLineSegment.L_C2_C4:
                    return new Line2D(GetTilePointPosition(TilePoint.C2), GetTilePointPosition(TilePoint.C4));
                case TileLineSegment.L_C2_M1:
                    return new Line2D(GetTilePointPosition(TilePoint.C2), GetTilePointPosition(TilePoint.M1));
                case TileLineSegment.L_C2_M2:
                    return new Line2D(GetTilePointPosition(TilePoint.C2), GetTilePointPosition(TilePoint.M2));
                case TileLineSegment.L_C2_M3:
                    return new Line2D(GetTilePointPosition(TilePoint.C2), GetTilePointPosition(TilePoint.M3));
                case TileLineSegment.L_C2_M4:
                    return new Line2D(GetTilePointPosition(TilePoint.C2), GetTilePointPosition(TilePoint.M4));
                case TileLineSegment.L_C3_C1:
                    return new Line2D(GetTilePointPosition(TilePoint.C3), GetTilePointPosition(TilePoint.C1));
                case TileLineSegment.L_C3_C2:
                    return new Line2D(GetTilePointPosition(TilePoint.C3), GetTilePointPosition(TilePoint.C2));
                case TileLineSegment.L_C3_C4:
                    return new Line2D(GetTilePointPosition(TilePoint.C3), GetTilePointPosition(TilePoint.C4));
                case TileLineSegment.L_C3_M1:
                    return new Line2D(GetTilePointPosition(TilePoint.C3), GetTilePointPosition(TilePoint.M1));
                case TileLineSegment.L_C3_M2:
                    return new Line2D(GetTilePointPosition(TilePoint.C3), GetTilePointPosition(TilePoint.M2));
                case TileLineSegment.L_C3_M3:
                    return new Line2D(GetTilePointPosition(TilePoint.C3), GetTilePointPosition(TilePoint.M3));
                case TileLineSegment.L_C3_M4:
                    return new Line2D(GetTilePointPosition(TilePoint.C3), GetTilePointPosition(TilePoint.M4));
                case TileLineSegment.L_C4_C1:
                    return new Line2D(GetTilePointPosition(TilePoint.C4), GetTilePointPosition(TilePoint.C1));
                case TileLineSegment.L_C4_C2:
                    return new Line2D(GetTilePointPosition(TilePoint.C4), GetTilePointPosition(TilePoint.C2));
                case TileLineSegment.L_C4_C3:
                    return new Line2D(GetTilePointPosition(TilePoint.C4), GetTilePointPosition(TilePoint.C3));
                case TileLineSegment.L_C4_M1:
                    return new Line2D(GetTilePointPosition(TilePoint.C4), GetTilePointPosition(TilePoint.M1));
                case TileLineSegment.L_C4_M2:
                    return new Line2D(GetTilePointPosition(TilePoint.C4), GetTilePointPosition(TilePoint.M2));
                case TileLineSegment.L_C4_M3:
                    return new Line2D(GetTilePointPosition(TilePoint.C4), GetTilePointPosition(TilePoint.M3));
                case TileLineSegment.L_C4_M4:
                    return new Line2D(GetTilePointPosition(TilePoint.C4), GetTilePointPosition(TilePoint.M4));
                case TileLineSegment.L_M1_C1:
                    return new Line2D(GetTilePointPosition(TilePoint.M1), GetTilePointPosition(TilePoint.C1));
                case TileLineSegment.L_M1_C2:
                    return new Line2D(GetTilePointPosition(TilePoint.M1), GetTilePointPosition(TilePoint.C2));
                case TileLineSegment.L_M1_C3:
                    return new Line2D(GetTilePointPosition(TilePoint.M1), GetTilePointPosition(TilePoint.C3));
                case TileLineSegment.L_M1_C4:
                    return new Line2D(GetTilePointPosition(TilePoint.M1), GetTilePointPosition(TilePoint.C4));
                case TileLineSegment.L_M1_M2:
                    return new Line2D(GetTilePointPosition(TilePoint.M1), GetTilePointPosition(TilePoint.M2));
                case TileLineSegment.L_M1_M3:
                    return new Line2D(GetTilePointPosition(TilePoint.M1), GetTilePointPosition(TilePoint.M3));
                case TileLineSegment.L_M1_M4:
                    return new Line2D(GetTilePointPosition(TilePoint.M1), GetTilePointPosition(TilePoint.M4));
                case TileLineSegment.L_M2_C1:
                    return new Line2D(GetTilePointPosition(TilePoint.M2), GetTilePointPosition(TilePoint.C1));
                case TileLineSegment.L_M2_C2:
                    return new Line2D(GetTilePointPosition(TilePoint.M2), GetTilePointPosition(TilePoint.C2));
                case TileLineSegment.L_M2_C3:
                    return new Line2D(GetTilePointPosition(TilePoint.M2), GetTilePointPosition(TilePoint.C3));
                case TileLineSegment.L_M2_C4:
                    return new Line2D(GetTilePointPosition(TilePoint.M2), GetTilePointPosition(TilePoint.C4));
                case TileLineSegment.L_M2_M1:
                    return new Line2D(GetTilePointPosition(TilePoint.M2), GetTilePointPosition(TilePoint.M1));
                case TileLineSegment.L_M2_M3:
                    return new Line2D(GetTilePointPosition(TilePoint.M2), GetTilePointPosition(TilePoint.M3));
                case TileLineSegment.L_M2_M4:
                    return new Line2D(GetTilePointPosition(TilePoint.M2), GetTilePointPosition(TilePoint.M4));
                case TileLineSegment.L_M3_C1:
                    return new Line2D(GetTilePointPosition(TilePoint.M3), GetTilePointPosition(TilePoint.C1));
                case TileLineSegment.L_M3_C2:
                    return new Line2D(GetTilePointPosition(TilePoint.M3), GetTilePointPosition(TilePoint.C2));
                case TileLineSegment.L_M3_C3:
                    return new Line2D(GetTilePointPosition(TilePoint.M3), GetTilePointPosition(TilePoint.C3));
                case TileLineSegment.L_M3_C4:
                    return new Line2D(GetTilePointPosition(TilePoint.M3), GetTilePointPosition(TilePoint.C4));
                case TileLineSegment.L_M3_M1:
                    return new Line2D(GetTilePointPosition(TilePoint.M3), GetTilePointPosition(TilePoint.M1));
                case TileLineSegment.L_M3_M2:
                    return new Line2D(GetTilePointPosition(TilePoint.M3), GetTilePointPosition(TilePoint.M2));
                case TileLineSegment.L_M3_M4:
                    return new Line2D(GetTilePointPosition(TilePoint.M3), GetTilePointPosition(TilePoint.M4));
                case TileLineSegment.L_M4_C1:
                    return new Line2D(GetTilePointPosition(TilePoint.M4), GetTilePointPosition(TilePoint.C1));
                case TileLineSegment.L_M4_C2:
                    return new Line2D(GetTilePointPosition(TilePoint.M4), GetTilePointPosition(TilePoint.C2));
                case TileLineSegment.L_M4_C3:
                    return new Line2D(GetTilePointPosition(TilePoint.M4), GetTilePointPosition(TilePoint.C3));
                case TileLineSegment.L_M4_C4:
                    return new Line2D(GetTilePointPosition(TilePoint.M4), GetTilePointPosition(TilePoint.C4));
                case TileLineSegment.L_M4_M1:
                    return new Line2D(GetTilePointPosition(TilePoint.M4), GetTilePointPosition(TilePoint.M1));
                case TileLineSegment.L_M4_M2:
                    return new Line2D(GetTilePointPosition(TilePoint.M4), GetTilePointPosition(TilePoint.M2));
                case TileLineSegment.L_M4_M3:
                    return new Line2D(GetTilePointPosition(TilePoint.M4), GetTilePointPosition(TilePoint.M3));
                default:
                    return default;
            }
        }

        /// <summary>
        /// Takes in TileShape
        /// </summary>
        /// <returns>TileShape collision lines count</returns>
        [MethodImpl((MethodImplOptions) 256)] // Inline
        public static int GetCollisionLinesCount(TileShape shape)
        {
            switch (shape)
            {
                case TileShape.Error:
                    return -1;
                case TileShape.EmptyBlock:
                    return 0;
                case TileShape.FullBlock:
                    return 4;
                case TileShape.HalfBlock:
                    return 4;
                case TileShape.LBlockBottom:
                    return 4;
                case TileShape.LBlockTop:
                    return 3;
                case TileShape.TriangleBlock:
                    return 3;
                default:
                    return 0;
            }
        }
        
        /// <summary>
        /// Takes in current shape from Property
        /// </summary>
        /// <returns>All collision lines for each TileShape+Rotation</returns>
        public static Line2D[] GetCollisionLines(TileShapeAndRotation shape)
        {
            switch (shape)
            {
                case TileShapeAndRotation.Error:
                    return null;
                case TileShapeAndRotation.EmptyBlock:
                    return null;
                case TileShapeAndRotation.FullBlock:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.HB_R1:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_M4_M2),
                        GetTileLineSegmentPosition(TileLineSegment.L_M2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_M4),
                    };
                case TileShapeAndRotation.HB_R2:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_M1),
                        GetTileLineSegmentPosition(TileLineSegment.L_M1_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_M3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.HB_R3:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_M2),
                        GetTileLineSegmentPosition(TileLineSegment.L_M2_M4),
                        GetTileLineSegmentPosition(TileLineSegment.L_M4_C1),
                    };
                case TileShapeAndRotation.HB_R4:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_M1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_M3),
                        GetTileLineSegmentPosition(TileLineSegment.L_M3_M1),
                    };
                case TileShapeAndRotation.TB_R1:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.TB_R2:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.TB_R3:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C1),
                    };
                case TileShapeAndRotation.TB_R4:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C2),
                    };
                case TileShapeAndRotation.LBT_R1:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_M3),
                        GetTileLineSegmentPosition(TileLineSegment.L_M3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBT_R2:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_M4),
                        GetTileLineSegmentPosition(TileLineSegment.L_M4_C1),
                    };
                case TileShapeAndRotation.LBT_R3:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_M2_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_M2),
                    };
                case TileShapeAndRotation.LBT_R4:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_M2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_M2),
                    };
                case TileShapeAndRotation.LBT_R5:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_M4_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_M4),
                    };
                case TileShapeAndRotation.LBT_R6:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_M1),
                        GetTileLineSegmentPosition(TileLineSegment.L_M1_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBT_R7:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_M2),
                        GetTileLineSegmentPosition(TileLineSegment.L_M2_C1),
                    };
                case TileShapeAndRotation.LBT_R8:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_M3),
                        GetTileLineSegmentPosition(TileLineSegment.L_M3_C2),
                    };
                case TileShapeAndRotation.LBB_R1:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_M1),
                        GetTileLineSegmentPosition(TileLineSegment.L_M1_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBB_R2:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_M2),
                        GetTileLineSegmentPosition(TileLineSegment.L_M2_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBB_R3:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_M3),
                        GetTileLineSegmentPosition(TileLineSegment.L_M3_C1),
                    };
                case TileShapeAndRotation.LBB_R4:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_M4),
                        GetTileLineSegmentPosition(TileLineSegment.L_M4_C2),
                    };
                case TileShapeAndRotation.LBB_R5:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_M2),
                        GetTileLineSegmentPosition(TileLineSegment.L_M2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBB_R6:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_M3),
                        GetTileLineSegmentPosition(TileLineSegment.L_M3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_C1),
                    };
                case TileShapeAndRotation.LBB_R7:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_C1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_M4),
                        GetTileLineSegmentPosition(TileLineSegment.L_M4_C1),
                    };
                case TileShapeAndRotation.LBB_R8:
                    return new[]
                    {
                        GetTileLineSegmentPosition(TileLineSegment.L_M1_C2),
                        GetTileLineSegmentPosition(TileLineSegment.L_C2_C3),
                        GetTileLineSegmentPosition(TileLineSegment.L_C3_C4),
                        GetTileLineSegmentPosition(TileLineSegment.L_C4_M1),
                    };
                default:
                    return null;
            }
        }

        /// <summary>
        /// Takes in current shape from Property
        /// </summary>
        /// <returns>Rotates shape 90 degree clockwise and outputs the new shape enum</returns>
        public static TileShapeAndRotation RotateShape(TileShapeAndRotation shape)
        {
            switch (shape)
            {
                case TileShapeAndRotation.Error:
                    return TileShapeAndRotation.Error;
                
                case TileShapeAndRotation.EmptyBlock:
                    return TileShapeAndRotation.EmptyBlock;
                
                case TileShapeAndRotation.FullBlock:
                    return TileShapeAndRotation.FullBlock;
                
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
        public static TileShapeAndRotation MirrorShape(TileShapeAndRotation shape)
        {
            switch (shape)
            {
                case TileShapeAndRotation.Error:
                    return TileShapeAndRotation.Error;
                case TileShapeAndRotation.EmptyBlock:
                    return TileShapeAndRotation.EmptyBlock;
                case TileShapeAndRotation.FullBlock:
                    return TileShapeAndRotation.FullBlock;
                
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
