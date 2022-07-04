using System;
using System.Collections.Generic;
using System.Linq;
using Enums.Tile;

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
            
            int baseId = 0;

            for(int j = column; j < column + 4; j++)
            {
                for(int i = row; i < row + 4; i++)
                {
                    //FIX: Dont import GameState, make a method?
                    //TileAtlas is imported by GameState, so TileAtlas should not import GameState
                    int atlasSpriteId = 
                        GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(spriteSheetId, i, j, 0);
                    if (i == row && j == column)
                    {
                        baseId = atlasSpriteId;
                    }
                }
            }
                
            TilePropertyArray[(int)CurrentTileIndex].BaseSpriteId = baseId;
            TilePropertyArray[(int)CurrentTileIndex].IsAutoMapping = true;
        }

        public void SetTilePropertySpriteSheet(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex == TileID.Error) return;
            
            int baseId = 0;
            for(int i = row; i <= row + 4; i++)
            {
                for(int j = column; j <= column + 4; j++)
                {
                    int atlasSpriteId = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(spriteSheetId, i, j, 0);
                    if (i == row && j == column)
                    {
                        baseId = atlasSpriteId;
                    }
                }
            }
            TilePropertyArray[(int)CurrentTileIndex].BaseSpriteId = baseId;
            TilePropertyArray[(int)CurrentTileIndex].IsAutoMapping = true;
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

        public void SetTilePropertyIsExplosive(bool isExplosive)
        {
            if (CurrentTileIndex == TileID.Error) return;
            
            TilePropertyArray[(int)CurrentTileIndex].IsExplosive = isExplosive;
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
