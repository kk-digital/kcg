using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Enums.Tile;
using KMath;

//MOST IMPORTANT TILE

/*

CreateTile(TileId)
SetTileName("Regolith") //map from string to TileId
SetTileLayer(TileMapLayerBackground)
SetTileTexture(TileSpriteId, 2,10) //2nd row, 10th column of TileSpriteId
SetTilePropertyISExplosive(true)
SetTileDurability(60)
EndTile()

Atlas is a pixel array
Atlas starts empty
Sprites are copied to Atlas and we get a AtlasSpriteId

SetTileTexture(TileSpriteId, 2,10) //2nd row, 10th column of TileSpriteId
- What does this do?
-- It blits (copy) the Sprite from TileSpriteLoader (TileSpriteSheetId)
-- to the TileSpriteAtlas
-- AND get the AtlasSpriteId (index into the Atlas texture sheet)

SetTileId(5)
// TileType, TileLayer, Name
DefineTile(BlockTypeSolid, LayerForegound, "regolith");
SetTileTexture(ImageId2, 2,10); //2nd row, 10th column, of i
push_texture(); //some might have more than one

SetTilePropertyIsExplosive(true); //example
SetTileDurability(60);

SetTileTextDescription("Regolith is a kind of dust commonly found on the surface of astronomical objects,\n");
EndTile();
*/

namespace PlanetTileMap
{
    //https://github.com/kk-digital/kcg/issues/89

    //ALL TILES CREATED OR USED IN GAME HAVE TO BE CREATED HERE
    //ALL TILES ARE CREATED FROM FUNCTIONS IN THIS FILE
    //ALL SPRITES FOR TILES ARE SET AND ASSIGNED FROM THIS API

    public class TileCreationApi
    {
        // Start is called before the first frame update
        private TileID CurrentTileIndex;
        public TileProperty[] TilePropertyArray;
        
        // Takes in TilePoint C1, C2 etc index
        // Returns Vec2f point
        public static readonly Vec2f[] TilePointsArray;
        
        // Takes in TileLineSegment L_C0_M2, etc index
        // Returns the start and finish point of line
        public static readonly Line2D[] LinePointsArray;
        
        // Takes in TileShapeAndRotation index
        // Returns number of lines for this shape
        public static readonly int[] ShapeIndex;

        static TileCreationApi()
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
        
        public TileCreationApi()
        {
            var tilePropertyArray = new TileProperty[4096];

            for (int i = 0; i < tilePropertyArray.Length; i++)
            {
                tilePropertyArray[i].TileID = TileID.Error;
                tilePropertyArray[i].BaseSpriteId = -1;
            }

            TilePropertyArray = tilePropertyArray;
            
            CurrentTileIndex = TileID.Error;
        }
        
        /// <summary>
        /// Takes in TilePoint enum
        /// </summary>
        /// <returns>Vec2f for point values</returns>
        [MethodImpl((MethodImplOptions) 256)] // Inline
        public Vec2f GetTilePointPosition(TilePoint point)
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
        public Line2D GetTileLineSegmentPosition(TileLineSegment lineSegment)
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
        public int GetCollisionLinesCount(TileShape shape)
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

        public ref TileProperty GetTileProperty(TileID tileID)
        {
            return ref TilePropertyArray[(int)tileID];
        }

        public void CreateTileProperty(TileID tileID)
        {
            if (tileID == TileID.Error) return;

            TilePropertyArray[(int)CurrentTileIndex].TileID = tileID;
            CurrentTileIndex = tileID;
        }

        public void SetTilePropertyName(string name)
        {
            if (CurrentTileIndex == TileID.Error) return;

            TilePropertyArray[(int)CurrentTileIndex].Name = name;
        }

        public void SetTilePropertySpriteSheet16(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex == TileID.Error) return;
            
           

            if (TilePropertyArray[(int)CurrentTileIndex].SpriteRuleType == SpriteRuleType.R1 ||
                TilePropertyArray[(int)CurrentTileIndex].SpriteRuleType == SpriteRuleType.R2)
            {
                int baseId = 0;
                for(int j = column; j < column + 4; j++)
                {
                    for(int i = row; i < row + 4; i++)
                    {
                        //FIX: Dont import GameState, make a method?
                        //TileAtlas is imported by GameState, so TileAtlas should not import GameState
                        int atlasSpriteId = 
                            GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(spriteSheetId, i, j, 0);

                        // the first sprite id is the baseId
                        if (i == row && j == column)
                        {
                            baseId = atlasSpriteId;
                        }
                    }
                }
   
                TilePropertyArray[(int)CurrentTileIndex].BaseSpriteId = baseId;
                TilePropertyArray[(int)CurrentTileIndex].IsAutoMapping = true;
            }
            else if (TilePropertyArray[(int)CurrentTileIndex].SpriteRuleType == SpriteRuleType.R3)
            {
                int baseId = 0;
                for(int x = column; x < column + 5; x++)
                {
                    for(int y = row; y < row + 11; y++)
                    {
                        //FIX: Dont import GameState, make a method?
                        //TileAtlas is imported by GameState, so TileAtlas should not import GameState
                        int atlasSpriteId = 
                            GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(spriteSheetId, y, x, 0);

                        // the first sprite id is the baseId
                        if (x == column && y == row)
                        {
                            baseId = atlasSpriteId;
                        }
                    }
                }
   
                TilePropertyArray[(int)CurrentTileIndex].BaseSpriteId = baseId;
                TilePropertyArray[(int)CurrentTileIndex].IsAutoMapping = true;
            }
        }

        public void SetTilePropertySpriteSheet(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex == TileID.Error) return;
            
            if (TilePropertyArray[(int)CurrentTileIndex].SpriteRuleType == SpriteRuleType.R1 ||
            TilePropertyArray[(int)CurrentTileIndex].SpriteRuleType == SpriteRuleType.R2)
            {
                int baseId = 0;
                
                for(int i = row; i <= row + 4; i++)
                {
                    for(int j = column; j <= column + 4; j++)
                    {
                        int atlasSpriteId = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(spriteSheetId, i, j, 0);

                        // the first sprite id is the baseId
                        if (i == row && j == column)
                        {
                            baseId = atlasSpriteId;
                        }
                    }
                }
                TilePropertyArray[(int)CurrentTileIndex].BaseSpriteId = baseId;
                TilePropertyArray[(int)CurrentTileIndex].IsAutoMapping = true;
            }
            else if (TilePropertyArray[(int)CurrentTileIndex].SpriteRuleType == SpriteRuleType.R3)
            {
                int baseId = 0;
                for(int x = column; x < column + 5; x++)
                {
                    for(int y = row; y < row + 11; y++)
                    {
                        //FIX: Dont import GameState, make a method?
                        //TileAtlas is imported by GameState, so TileAtlas should not import GameState
                        int atlasSpriteId = 
                            GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(spriteSheetId, y, x, 0);

                        // the first sprite id is the baseId
                        if (x == column && y == row)
                        {
                            baseId = atlasSpriteId;
                        }
                    }
                }

                TilePropertyArray[(int)CurrentTileIndex].BaseSpriteId = baseId;
                TilePropertyArray[(int)CurrentTileIndex].IsAutoMapping = true;
            }
        }

        public void SetTilePropertyTexture(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex == TileID.Error) return;
            
            //FIX: Dont import GameState, make a method?
            //TileAtlas is imported by GameState, so TileAtlas should not import GameState
            int atlasSpriteId = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(spriteSheetId, row, column, 0);
            TilePropertyArray[(int)CurrentTileIndex].BaseSpriteId = atlasSpriteId;
            TilePropertyArray[(int)CurrentTileIndex].IsAutoMapping = false;
        }

        public void SetTilePropertyTexture16(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex == TileID.Error) return;
              
            int atlasSpriteId = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(spriteSheetId, row, column, 0);
            TilePropertyArray[(int)CurrentTileIndex].BaseSpriteId = atlasSpriteId;
            TilePropertyArray[(int)CurrentTileIndex].IsAutoMapping = false;
            
        }

        public void SetTilePropertyCollisionType(CollisionType type)
        {
            if (CurrentTileIndex == TileID.Error) return;

            TilePropertyArray[(int)CurrentTileIndex].TileCollisionType = type;
        }

        
        public void SetTilePropertyDurability(byte durability)
        {
            if (CurrentTileIndex == TileID.Error) return;

            TilePropertyArray[(int)CurrentTileIndex].Durability = durability;
        }

        public void SetTilePropertyDescription(byte durability)
        {
            if (CurrentTileIndex == TileID.Error) return;
            
            TilePropertyArray[(int)CurrentTileIndex].Durability = durability;
        }

        public void SetSpriteRuleType(SpriteRuleType spriteRuleType)
        {
            Utils.Assert((int)CurrentTileIndex >= 0 && (int)CurrentTileIndex < TilePropertyArray.Length);

            TilePropertyArray[(int)CurrentTileIndex].SpriteRuleType = spriteRuleType;
        }

       /* public void SetTilePropertyVariant(int spriteSheetId, int row, int column, PlanetTileMap.TilePosition variant)
        {
            if (CurrentTileIndex != -1)
            {
                int atlasSpriteId = 
                    GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(spriteSheetId, row, column, 0);
                PropertiesArray[CurrentTileIndex].Variants[(int)variant] = atlasSpriteId;
                
            }
        }

        public void SetTilePropertyVariant16(int spriteSheetId, int row, int column, PlanetTileMap.TilePosition variant)
        {
            if (CurrentTileIndex != -1)
            {
                int atlasSpriteId = 
                    GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(spriteSheetId, row, column, 0);
                PropertiesArray[CurrentTileIndex].Variants[(int)variant] = atlasSpriteId;
            }
        }*/

        public void EndTileProperty()
        {
            CurrentTileIndex = TileID.Error;
        }
    }
}
