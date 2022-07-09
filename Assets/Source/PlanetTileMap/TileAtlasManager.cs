using Enums.Tile;
using UnityEngine;

namespace PlanetTileMap
{
    public class TileAtlasManager
    {
        private Sprites.SpriteLoader SpriteLoader;
        private Sprites.SpriteAtlas[] SpritesArray;
        private int[] SpriteCount;

        public int Length => SpritesArray.Length;

        public TileAtlasManager(Sprites.SpriteLoader spriteLoader)
        {
            SpriteLoader = spriteLoader;
            SpritesArray = new Sprites.SpriteAtlas[1];
            SpriteCount = new int[1];

            var atlas = new Sprites.SpriteAtlas
            {
                Width = 128,
                Height = 128
            };
            atlas.Data = new byte[4 * 32 * 32 * atlas.Width * atlas.Height]; // 4 * 32 * 32 = 4096
            for(int j = 0; j < atlas.Data.Length; j++)
            {
                atlas.Data[j] = 255;
            }
            SpritesArray[0] = atlas;
        }

        public void UpdateAtlasTexture(int id)
        {
            ref Sprites.SpriteAtlas atlas = ref SpritesArray[id];
            if (atlas.TextureNeedsUpdate)
            {
                atlas.Texture = Utility.Texture.CreateTextureFromRGBA(atlas.Data, atlas.Width * 32, atlas.Height * 32);
                atlas.TextureNeedsUpdate = false;
            }
        }
        
        public ref Sprites.SpriteAtlas GetSpriteAtlas(int id)
        {
            ref Sprites.SpriteAtlas atlas = ref SpritesArray[id];

            return ref SpritesArray[id];
        }

        public int GetGlTextureId(int id, int type = 0)
        {
            Sprites.SpriteAtlas atlas = GetSpriteAtlas(type);
            return atlas.GLTextureID;
        }

        public Texture2D GetTexture(int id, int type = 0)
        {
            ref Sprites.SpriteAtlas atlas = ref GetSpriteAtlas(type);
            return atlas.Texture; 
        }

        public Sprites.Sprite GetSprite(int id, int type = 0)
        {
            var sprite = new Sprites.Sprite();
            ref Sprites.SpriteAtlas atlas = ref GetSpriteAtlas(type);

            sprite.Texture = atlas.Texture;

            int xOffset = (id % atlas.Width) * 32;
            int yOffset = (id / atlas.Height) * 32;
            int width = 32;
            int height = 32;

            sprite.TextureCoords = new Vector4((float)xOffset / (float)(atlas.Width * 32), (float)yOffset / (float)(atlas.Height * 32),
            (float)width / (float)(atlas.Width * 32), (float)height / (float)(atlas.Height * 32));

            return sprite;
        }

        public void GetSpriteBytes(int spriteID, byte[] data, int type = 0)
        {
            ref Sprites.SpriteAtlas atlas = ref SpritesArray[type];

            int xOffset = (spriteID % atlas.Width) * 32;
            int yOffset = (spriteID / atlas.Height) * 32;

            for(int y = 0; y < 32; y++)
            {
                for(int x = 0; x < 32; x++)
                {
                    int index = 4 * (x + y * 32);
                    int atlasindex = 4 * ((yOffset + y) * (atlas.Width * 32) +
                                         (xOffset + x));
                    

                    data[index + 0] = atlas.Data[atlasindex + 0];
                    data[index + 1] = atlas.Data[atlasindex + 1];
                    data[index + 2] = atlas.Data[atlasindex + 2];
                    data[index + 3] = atlas.Data[atlasindex + 3];
                }
            }
        }

        // Copies a tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
        public int CopyTileSpriteToAtlas(int spriteSheetID, int row, int column, int atlasId)
        {
            ref Sprites.SpriteSheet sheet = ref SpriteLoader.SpriteSheets[spriteSheetID];
            ref Sprites.SpriteAtlas atlas = ref SpritesArray[atlasId];
            ref int count = ref SpriteCount[atlasId];
            
            int xOffset = (count % atlas.Width) * 32;
            int yOffset = (count / atlas.Height) * 32;

            for (int y = 0; y < 32; y++)
                for (int x = 0; x < 32; x++)
                {

                    int atlasIndex = 4 * ((yOffset + y)  * (atlas.Width * 32) + (xOffset + x));
                    int sheetIndex = 4 * ((x + row * 32) + ((y + column * 32) * sheet.Width));

                    atlas.Data[atlasIndex + 0] = 
                        sheet.Data[sheetIndex + 0];
                    atlas.Data[atlasIndex + 1] = 
                        sheet.Data[sheetIndex + 1];
                    atlas.Data[atlasIndex + 2] = 
                        sheet.Data[sheetIndex + 2];
                    atlas.Data[atlasIndex + 3] = 
                        sheet.Data[sheetIndex + 3];
                }

            // todo: upload texture to open gl

            atlas.TextureNeedsUpdate = true;

            count++;
            return count - 1;
        }

        // Copies a 16x16 tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
         public int CopyTileSpriteToAtlas16To32(int spriteSheetID, int row, int column, int atlasId)
        {
            ref Sprites.SpriteSheet sheet = ref SpriteLoader.SpriteSheets[spriteSheetID];
            ref Sprites.SpriteAtlas atlas = ref SpritesArray[atlasId];
            ref int count = ref SpriteCount[atlasId];
            
            int xOffset = (count % atlas.Width) * 32;
            int yOffset = (count / atlas.Height) * 32;

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    int sheetindex = 4 * ((x + row * 16) + ((y + column * 16) * sheet.Width));

                    for (int j = 0; j < 2; j++)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int atlasIndex = 4 * ((yOffset + (y * 2) + j) * (atlas.Width * 32) + (xOffset + (x * 2) + i));

                            atlas.Data[atlasIndex + 0] = sheet.Data[sheetindex + 0];
                            atlas.Data[atlasIndex + 1] = sheet.Data[sheetindex + 1];
                            atlas.Data[atlasIndex + 2] = sheet.Data[sheetindex + 2];
                            atlas.Data[atlasIndex + 3] = sheet.Data[sheetindex + 3];
                        }
                    }
                }
            }

            // todo: upload texture to open gl
            atlas.TextureNeedsUpdate = true;
            count++;

            return count - 1;
        }
        

        // Copies a 8x8 tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
        public int CopyTileSpriteToAtlas8To32(int spriteSheetID, int row, int column, int atlasId)
        {
            ref Sprites.SpriteSheet sheet = ref SpriteLoader.SpriteSheets[spriteSheetID];
            ref Sprites.SpriteAtlas atlas = ref SpritesArray[atlasId];
            ref int count = ref SpriteCount[atlasId];
            
            int xOffset = (count % atlas.Width) * 32;
            int yOffset = (count / atlas.Height) * 32;

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    int sheetindex = 4 * ((x + row * 8) + ( (y + column * 8) * sheet.Width));

                    for(int j = 0; j < 4; j++)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            int atlasIndex = 4 * ((yOffset + (y * 4) + j) * (atlas.Width * 32) + (xOffset + (x * 4) + i));
                    
                            atlas.Data[atlasIndex + 0] = sheet.Data[sheetindex + 0];
                            atlas.Data[atlasIndex + 1] = sheet.Data[sheetindex + 1];
                            atlas.Data[atlasIndex + 2] = sheet.Data[sheetindex + 2];
                            atlas.Data[atlasIndex + 3] = sheet.Data[sheetindex + 3];
                        }
                    }

                }

            // todo: upload texture to open gl
            atlas.TextureNeedsUpdate = true;
            count++;

            return count - 1;
        }
    }
}