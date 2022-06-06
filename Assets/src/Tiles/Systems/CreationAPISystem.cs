using System;
using Enums;
using System.Collections.Generic;

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

namespace Tile
{
    //https://github.com/kk-digital/kcg/issues/89

    //ALL TILES CREATED OR USED IN GAME HAVE TO BE CREATED HERE
    //ALL TILES ARE CREATED FROM FUNCTIONS IN THIS FILE
    //ALL SPRITES FOR TILES ARE SET AND ASSIGNED FROM THIS API

    public class CreationAPISystem
    {
        // Start is called before the first frame update

        private int CurrentTileIndex;
        private PropertiesData[] PropertiesArray;

        private  Dictionary<string, int> NameToID;

        public CreationAPISystem()
        {
            NameToID = new Dictionary<string, int>();
            PropertiesArray = new PropertiesData[1024];
            for(int i = 0; i < PropertiesArray.Length; i++)
            {
                PropertiesArray[i] = new PropertiesData("", "", 0, TileDrawProperties.TileDrawPropertyNormal, 0,
                                PlanetTileCollisionType.TileCollisionTypeSolid, 100, false);
            }
            CurrentTileIndex = -1;
        }

        public PropertiesData GetTileProperties(int TileId)
        {
            if (TileId >= 0 && TileId < PropertiesArray.Length)
            {
                return PropertiesArray[TileId];
            }

            return new PropertiesData();
        }

        public PropertiesData GetTileProperties(string name)
        {
            int value;
            bool exists = NameToID.TryGetValue(name, out value);
            if (exists)
            {
                return GetTileProperties(value);
            }

            return new PropertiesData();
        }

        public void CreateTile(int TileId)
        {
            while (TileId >= PropertiesArray.Length)
            {
                Array.Resize(ref PropertiesArray, PropertiesArray.Length * 2);
            }

            CurrentTileIndex = TileId;
            if (CurrentTileIndex != -1)
            {
                PropertiesArray[CurrentTileIndex].TileId = CurrentTileIndex;
            }
        }

        public void SetTileName(string name)
        {
            if (CurrentTileIndex == -1) return;
            
            if (!NameToID.ContainsKey(name))
            {
                NameToID.Add(name, CurrentTileIndex);
            }

            PropertiesArray[CurrentTileIndex].Name = name;
        }

        public void SetTileTexture(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex != -1)
            {
                int atlasSpriteId = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(spriteSheetId, row, column, 0);
                PropertiesArray[CurrentTileIndex].SpriteId = atlasSpriteId;
            }
        }

        public void SetTileTexture16(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex == -1) return;
            
            int atlasSpriteId = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(spriteSheetId, row, column, 0);
            PropertiesArray[CurrentTileIndex].SpriteId = atlasSpriteId;
        }

        public void SetTilePropertyIsExplosive(bool isExplosive)
        {
            if (CurrentTileIndex != -1)
            {
                PropertiesArray[CurrentTileIndex].IsExplosive = isExplosive;
            }
        }

        public void SetTileDrawType(TileDrawProperties type)
        {
            if (CurrentTileIndex != -1)
            {
                PropertiesArray[CurrentTileIndex].TileDrawType = type;
            }
        }

        public void SetTileCollisionType(PlanetTileCollisionType type)
        {
            if (CurrentTileIndex != -1)
            {
                PropertiesArray[CurrentTileIndex].TileCollisionType = type;
            }
        }

        
        public void SetTileDurability(byte durability)
        {
            if (CurrentTileIndex != -1)
            {
                PropertiesArray[CurrentTileIndex].Durability = durability;
            }
        }

        public void SetTileDescription(byte durability)
        {
            if (CurrentTileIndex != -1)
            {
                PropertiesArray[CurrentTileIndex].Durability = durability;
            }
        }

        public void SetTileVariant(int spriteSheetId, int row, int column, TileVariants tileVariants)
        {
            if (CurrentTileIndex != -1)
            {
                int atlasSpriteId = 
                    GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(spriteSheetId, row, column, 0);
                PropertiesArray[CurrentTileIndex].Variants[(int)tileVariants] = atlasSpriteId;
                
            }
        }

        public void SetTileVariant16(int spriteSheetId, int row, int column, TileVariants tileVariants)
        {
            if (CurrentTileIndex != -1)
            {
                int atlasSpriteId = 
                    GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(spriteSheetId, row, column, 0);
                PropertiesArray[CurrentTileIndex].Variants[(int)tileVariants] = atlasSpriteId;
            }
        }

        public void EndTile()
        {
            CurrentTileIndex = -1;
        }

        public PropertiesData GetTile(int x, int y)
        {
            // 0, 0 = 0
            // 32, 0 = 1
            // 64, 0 = 2
            // return PropertiesArray[(x / 32) + (y / 32) * width];
            return PropertiesArray[CurrentTileIndex];
        }
    }

}
