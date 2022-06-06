﻿using UnityEngine;

namespace Tile
{
    public class SpriteAtlasManagerSystem
    {
        private Sprites.Atlas[] SpritesArray;
        private int[] Count;

        public SpriteAtlasManagerSystem()
        {
            SpritesArray = new Sprites.Atlas[1];
            Count = new int[1];

            var atlas = new Sprites.Atlas();
            atlas.Width = 9;
            atlas.Height = 9;
            atlas.Data = new byte[4 * 32 * 32 * atlas.Width * atlas.Height]; // 4 * 32 * 32 = 4096
            for(int j = 0; j < atlas.Data.Length; j++)
            {
                atlas.Data[j] = 255;
            }
            SpritesArray[0] = atlas;
        }
        
        public ref Sprites.Atlas GetSpriteAtlas(int id)
        {
            return ref SpritesArray[id];
        }

        public int GetGlTextureId(int id)
        {
            Sprites.Atlas atlas = GetSpriteAtlas(id);
            return atlas.GLTextureID;
        }

        public Texture2D GetTexture(int id)
        {
            ref Sprites.Atlas atlas = ref GetSpriteAtlas(id);
            return atlas.Texture; 
        }

        public Render.Sprite GetSprite(int id)
        {
            var sprite = new Render.Sprite();
            ref Sprites.Atlas atlas = ref GetSpriteAtlas(0);

            sprite.Texture = atlas.Texture;

            int xOffset = (id % atlas.Width) * 32;
            int yOffset = (id / atlas.Height) * 32;
            int width = 32;
            int height = 32;

            sprite.TextureCoords = new Vector4(xOffset / (float)(atlas.Width * 32), yOffset / (float)(atlas.Height * 32),
            width / (float)(atlas.Width * 32), height / (float)(atlas.Height * 32));

            return sprite;
        }

        public void GetSpriteBytes(int id, byte[] data)
        {
            ref var atlas = ref SpritesArray[0];

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
        }

        // Copies a tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
        public int CopyTileSpriteToAtlas(int spriteSheetID, int row, int column, int atlasId)
        {
            var sheet = GameState.SpriteLoaderSystem.SpriteSheets[spriteSheetID];
            ref var atlas = ref SpritesArray[atlasId];
            ref int count = ref Count[atlasId];
            
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

            count++;
            atlas.Texture = Utility.TextureUtils.CreateTextureFromRGBA(atlas.Data, atlas.Width * 32, atlas.Height * 32);
            
            return count - 1;
        }

        // Copies a 16x16 tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
         public int CopyTileSpriteToAtlas16To32(int spriteSheetID, int row, int column, int atlasId)
        {
            var sheet = GameState.SpriteLoaderSystem.SpriteSheets[spriteSheetID];
            ref var atlas = ref SpritesArray[atlasId];
            ref int count = ref Count[atlasId];
            
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

            count++;
            atlas.Texture = Utility.TextureUtils.CreateTextureFromRGBA(atlas.Data, atlas.Width * 32, atlas.Height * 32);
            
            return count - 1;
        }
        

        // Copies a 8x8 tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
        public int CopyTileSpriteToAtlas8To32(int spriteSheetID, int row, int column, int atlasId)
        {
            var sheet = GameState.SpriteLoaderSystem.SpriteSheets[spriteSheetID];
            ref var atlas = ref SpritesArray[atlasId];
            ref int count = ref Count[atlasId];
            
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

            count++;
            atlas.Texture = Utility.TextureUtils.CreateTextureFromRGBA(atlas.Data, atlas.Width * 32, atlas.Height * 32);
            

            return count - 1;
        }
    }
}