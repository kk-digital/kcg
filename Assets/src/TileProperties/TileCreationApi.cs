using Enums;
using System.Collections;
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

namespace TileProperties
{
    //https://github.com/kk-digital/kcg/issues/89

    //ALL TILES CREATED OR USED IN GAME HAVE TO BE CREATED HERE
    //ALL TILES ARE CREATED FROM FUNCTIONS IN THIS FILE
    //ALL SPRITES FOR TILES ARE SET AND ASSIGNED FROM THIS API

    public class TileCreationApi
    {
        // Start is called before the first frame update

        private int CurrentTileIndex;
        private TilePropertiesData[] PropertiesArray;

        private  Dictionary<string, int> NameToID;

        public TileCreationApi()
        {
            NameToID = new Dictionary<string, int>();
            PropertiesArray = new TilePropertiesData[1024];
            CurrentTileIndex = -1;
        }

        public TilePropertiesData GetTileProperties(int TileId)
        {
            if (TileId >= 0 && TileId < PropertiesArray.Length)
            {
                return PropertiesArray[TileId];
            }

            return new TilePropertiesData();
        }

        public TilePropertiesData GetTileProperties(string name)
        {
            int value;
            bool exists = NameToID.TryGetValue(name, out value);
            if (exists)
            {
                return GetTileProperties(value);
            }

            return new TilePropertiesData();
        }

        public void CreateTile(int TileId)
        {
            int oldSize = PropertiesArray.Length;
            while (TileId >= PropertiesArray.Length)
            {
                TilePropertiesData[] newArray = new TilePropertiesData[PropertiesArray.Length * 2];
                for(int i = 0; i < oldSize; i++)
                {
                    newArray[i] = PropertiesArray[i];
                }

                PropertiesArray = newArray;
            }

            CurrentTileIndex = TileId;
            if (CurrentTileIndex != -1)
            {
                PropertiesArray[CurrentTileIndex].TileId = CurrentTileIndex;
            }
        }

        public void SetTileName(string name)
        {
            if (CurrentTileIndex != -1)
            {
                int value;
                bool exists = NameToID.TryGetValue(name, out value);
                if (!exists)
                {
                     NameToID.Add(name, CurrentTileIndex);
                }

                PropertiesArray[CurrentTileIndex].Name = name;
            }
        }

        public void SetTileTexture(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex != -1)
            {
                int atlasSpriteId = 
                    GameState.SpriteAtlasManager.Blit(spriteSheetId, row, column);
                PropertiesArray[CurrentTileIndex].SpriteId = atlasSpriteId;
            }
        }

        public void SetTileTexture16(int spriteSheetId, int row, int column)
        {
            if (CurrentTileIndex != -1)
            {
                int atlasSpriteId = 
                    GameState.SpriteAtlasManager.Blit16(spriteSheetId, row, column);
                PropertiesArray[CurrentTileIndex].SpriteId = atlasSpriteId;
            }
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

        public void SetTileCollisionTile(PlanetTileCollisionType type)
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

        public void EndTile()
        {
            CurrentTileIndex = -1;
        }

        public TilePropertiesData GetTile(int x, int y)
        {
            // 0, 0 = 0
            // 32, 0 = 1
            // 64, 0 = 2
            // return PropertiesArray[(x / 32) + (y / 32) * width];
            return PropertiesArray[CurrentTileIndex];
        }
    }

}
