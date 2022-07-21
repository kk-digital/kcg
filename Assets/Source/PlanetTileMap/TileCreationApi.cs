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
        public TileMaterial[] TileMaterialArray;
        public TileProperty[] TilePropertyArray;
        public int CurrentTileMaterialType;
        public int CurrentTileIndex;
        
        public TileCreationApi()
        {
            TilePropertyArray = new TileProperty[4096];
            TileMaterialArray = new TileMaterial[1024];

            for (int i = 0; i < TilePropertyArray.Length; i++)
            {
                TilePropertyArray[i].MaterialType = TileMaterialType.Error;
                TilePropertyArray[i].TileID = -1;
            }
            
            CurrentTileIndex = 1;
            CurrentTileMaterialType = -1;
        }

        public ref TileMaterial GetMaterial(TileMaterialType MaterialType)
        {
            return ref TileMaterialArray[(int)MaterialType];
        }

        public ref TileProperty GetTileProperty(int tileID)
        {
            return ref TilePropertyArray[tileID];
        }


        public void BeginMaterial(TileMaterialType MaterialType)
        {
            if ((int)MaterialType >= TileMaterialArray.Length)   
            {
                System.Array.Resize(ref TileMaterialArray, TileMaterialArray.Length * 2);
            }

            CurrentTileMaterialType = (int)MaterialType;  
            TileMaterialArray[CurrentTileMaterialType].MaterialType = (TileMaterialType)CurrentTileMaterialType; 
            TileMaterialArray[CurrentTileMaterialType].StartTileIndex = CurrentTileIndex;
        }

        public int CreateTileProperty()
        {
            Utils.Assert(CurrentTileMaterialType != -1);

            if (CurrentTileIndex >= TilePropertyArray.Length)   
            {
                System.Array.Resize(ref TilePropertyArray, TilePropertyArray.Length * 2);
            }

            TilePropertyArray[CurrentTileIndex].TileID = CurrentTileIndex;
            TilePropertyArray[CurrentTileIndex].MaterialType = (TileMaterialType)CurrentTileMaterialType;

            return CurrentTileIndex;
        }


        public void SetMaterialName(string name)
        {
            Utils.Assert(CurrentTileIndex != (int)TileMaterialType.Error);

            TileMaterialArray[(int)CurrentTileMaterialType].Name = name;
        }

        public void SetMaterialDurability(byte durability)
        {
            Utils.Assert(CurrentTileIndex != (int)TileMaterialType.Error);

            TileMaterialArray[(int)CurrentTileMaterialType].Durability = durability;
        }

        public void SetMaterialCannotBeRemoved(bool flag)
        {
            Utils.Assert(CurrentTileIndex != (int)TileMaterialType.Error);
            
            TileMaterialArray[(int)CurrentTileMaterialType].CannotBeRemoved = flag;
        }

        public void SetMaterialSpriteRuleType(SpriteRuleType spriteRuleType)
        {
            Utils.Assert(CurrentTileIndex != (int)TileMaterialType.Error);

            TileMaterialArray[(int)CurrentTileMaterialType].SpriteRuleType = spriteRuleType;
        }
                
        public void EndMaterial()
        {

        }

        public void EndTileProperty()
        {
            CurrentTileIndex++;
        }

        /*public void CreateTileProperty(TileID tileID)
        {
            if (tileID == TileID.Error) return;

            TilePropertyArray[(int)CurrentTileIndex].TileID = tileID;
            CurrentTileIndex = tileID;
        }*/

        public void SetTilePropertyShape(TileShape shape, TileShapeAndRotation shapeAndRotation)
        {
            Utils.Assert(CurrentTileIndex != 0);

            TilePropertyArray[(int) CurrentTileIndex].BlockShapeType = shape;
            TilePropertyArray[(int) CurrentTileIndex].BlockShapeAndRotation = shapeAndRotation;
        }

        /*public void SetTilePropertySpriteSheet16(int spriteSheetId, int row, int column)
        {
            Utils.Assert(CurrentTileIndex != 0);
            
           

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
        }*/

        /*public void SetTilePropertySpriteSheet(int spriteSheetId, int row, int column)
        {
            Utils.Assert(CurrentTileIndex != 0);
            
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
        }*/

        public void SetTilePropertyTexture(int spriteSheetId, int row, int column)
        {
            Utils.Assert(CurrentTileIndex != 0);
            
            //FIX: Dont import GameState, make a method?
            //TileAtlas is imported by GameState, so TileAtlas should not import GameState
            int atlasSpriteId = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(spriteSheetId, row, column, 0);
            TilePropertyArray[(int)CurrentTileIndex].SpriteId = atlasSpriteId;
        }

        public void SetTilePropertyTexture16(int spriteSheetId, int row, int column)
        {
            Utils.Assert(CurrentTileIndex != 0);
              
            int atlasSpriteId = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(spriteSheetId, row, column, 0);
            TilePropertyArray[(int)CurrentTileIndex].SpriteId = atlasSpriteId;
            
        }

        public void SetTilePropertyCollisionType(CollisionType type)
        {
            Utils.Assert(CurrentTileIndex != 0);

            TilePropertyArray[(int)CurrentTileIndex].CollisionIsoType = type;
        }

    }
}
