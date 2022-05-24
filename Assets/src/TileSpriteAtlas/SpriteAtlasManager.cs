using Enums;
using UnityEngine;
using TileSpriteLoader;
using System.Collections;
using System;

namespace SpriteAtlas
{
    public class SpriteAtlasManager
    {
        private SpriteAtlas[] SpritesArray;
        private int[] Count;

        public SpriteAtlasManager()
        {
            SpritesArray = new SpriteAtlas[1];
            Count = new int[1];

            SpriteAtlas atlas = new SpriteAtlas();
            atlas.Width = 128;
            atlas.Height = 128;
            atlas.Data = new byte[4 * 32 * 32 * atlas.Width * atlas.Height]; // 4 * 32 * 32 = 4096

            SpritesArray[0] = atlas;
        }
        
        public ref SpriteAtlas GetSpriteAtlas(int id)
        {
            return ref SpritesArray[id];
        }

        public int GetGlTextureId(int id)
        {
            SpriteAtlas atlas = GetSpriteAtlas(id);
            return atlas.GLTextureID;
        }

        public byte[] GetSpriteBytes(int id)
        {
            byte[] data = new byte[32 * 32 * 4];
            ref SpriteAtlas atlas = ref SpritesArray[0];

            int xOffset = (id % atlas.Width) * 32;
            int yOffset = (id / atlas.Height) * 32;

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
            
            return data;
        }

        // Returns sprite sheet id
        public int Blit(int SpriteSheetID, int Row, int Column)
        {
            SpriteSheet sheet = GameState.TileSpriteLoader.SpriteSheets[SpriteSheetID];
            ref SpriteAtlas atlas = ref SpritesArray[0];
            ref int count = ref Count[0];

            for (int y = 0; y < 32; y++)
                for (int x = 0; x < 32; x++)
                {
                    int xOffset = (count % atlas.Width) * 32;
                    int yOffset = (count / atlas.Height) * 32;
                    /*int atlasindex = 4 * ((yOffset + y) * atlas.Width + (x + xOffset));
                    int sheetindex = 4 * ((x + Row) + ( (y + Column) * sheet.Width));*/

                    int atlasindex = 4 * ((yOffset + y) * (atlas.Width * 32) + (xOffset + x));
                    int sheetindex = 4 * ((x + Row) + ( (y + Column) * sheet.Width));

                    atlas.Data[atlasindex + 0] = sheet.Data[sheetindex + 0];
                    atlas.Data[atlasindex + 1] = sheet.Data[sheetindex + 1];
                    atlas.Data[atlasindex + 2] = sheet.Data[sheetindex + 2];
                    atlas.Data[atlasindex + 3] = sheet.Data[sheetindex + 3];
                }

            // todo: upload texture to open gl

            count++;

            return count - 1;
        }


         public int Blit16(int SpriteSheetID, int Row, int Column)
        {
            SpriteSheet sheet = GameState.TileSpriteLoader.SpriteSheets[SpriteSheetID];
            ref SpriteAtlas atlas = ref SpritesArray[0];
            ref int count = ref Count[0];

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    int xOffset = (count % atlas.Width) * 32;
                    int yOffset = (count / atlas.Height) * 32;
                    /*int atlasindex = 4 * ((yOffset + y) * atlas.Width + (x + xOffset));
                    int sheetindex = 4 * ((x + Row) + ( (y + Column) * sheet.Width));*/

                    //int atlasindex = 4 * 4 * ((yOffset + y) * (atlas.Width * 32) + (xOffset + x));
                    int sheetindex = 4 * ((x + Row * 16) + ( (y + Column * 16) * sheet.Width));

                    for(int j = 0; j < 2; j++)
                    {
                        for(int i = 0; i < 2; i++)
                        {
                            int atlasindex = 4 * ((yOffset + (y * 2) + j) * (atlas.Width * 32) + (xOffset + (x * 2) + i));

                            atlas.Data[atlasindex + 0] = sheet.Data[sheetindex + 0];
                            atlas.Data[atlasindex + 1] = sheet.Data[sheetindex + 1];
                            atlas.Data[atlasindex + 2] = sheet.Data[sheetindex + 2];
                            atlas.Data[atlasindex + 3] = sheet.Data[sheetindex + 3];
                        }
                    }

                }

            // todo: upload texture to open gl

            count++;

            return count - 1;
        }

        public int Blit8(int SpriteSheetID, int Row, int Column)
        {
            SpriteSheet sheet = GameState.TileSpriteLoader.SpriteSheets[SpriteSheetID];
            ref SpriteAtlas atlas = ref SpritesArray[0];
            ref int count = ref Count[0];

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    int xOffset = (count % atlas.Width) * 32;
                    int yOffset = (count / atlas.Height) * 32;
                    /*int atlasindex = 4 * ((yOffset + y) * atlas.Width + (x + xOffset));
                    int sheetindex = 4 * ((x + Row) + ( (y + Column) * sheet.Width));*/

                    //int atlasindex = 4 * 4 * ((yOffset + y) * (atlas.Width * 32) + (xOffset + x));
                    int sheetindex = 4 * ((x + Row * 8) + ( (y + Column * 8) * sheet.Width));

                    for(int j = 0; j < 4; j++)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            int atlasindex = 4 * ((yOffset + (y * 4) + j) * (atlas.Width * 32) + (xOffset + (x * 4) + i));
                    
                            atlas.Data[atlasindex + 0] = sheet.Data[sheetindex + 0];
                            atlas.Data[atlasindex + 1] = sheet.Data[sheetindex + 1];
                            atlas.Data[atlasindex + 2] = sheet.Data[sheetindex + 2];
                            atlas.Data[atlasindex + 3] = sheet.Data[sheetindex + 3];
                        }
                    }

                }

            // todo: upload texture to open gl

            count++;

            return count - 1;
        }
    }
}
