using TileSpriteLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteAtlas
{
    public struct SpriteAtlas
    {
        public int AtlasID;
        public int GLTextureID;

        public int Width;
        public int Height;

        public byte[] Data;
        public RectpackSharp.PackingRectangle[] Rectangles;
    }

    
    public class SpriteAtlasManager
    {
        private SpriteAtlas[] SpritesArray;

        public SpriteAtlasManager()
        {
            SpritesArray = new SpriteAtlas[1];

            SpriteAtlas atlas = new SpriteAtlas();
            atlas.Width = 128;
            atlas.Height = 128;
            atlas.Data = new byte[4 * atlas.Width * atlas.Height]; // 4 * 32 * 32 = 4096
            atlas.Rectangles = new RectpackSharp.PackingRectangle[0];

            for(int i = 0; i < atlas.Data.Length; i++)
            {
                atlas.Data[i] = 255;
            }

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

        // use the id to find the Sprite coordinates in the list
        // and return the sprite RGBA8 that correspond
        public void GetSpriteBytes(int id, byte[] data)
        {
            ref SpriteAtlas atlas = ref SpritesArray[0];
            if (id >= 0 && id < atlas.Rectangles.Length)
            {
                RectpackSharp.PackingRectangle rectangle = atlas.Rectangles[id];

                int xOffset = (int)rectangle.X;
                int yOffset = (int)rectangle.Y;
                int Width = (int)rectangle.Width;
                int Height = (int)rectangle.Height;


                for(int y = 0; y < Height; y++)
                {
                    for(int x = 0; x < Width; x++)
                    {
                        int index = 4 * (x + y * Width);
                        int atlasindex = 4 * ((yOffset + y) * (atlas.Width) +
                                            (xOffset + x));
                        
                        //RGBA8
                        data[index + 0] = 
                            atlas.Data[atlasindex + 0];
                        data[index + 1] = 
                            atlas.Data[atlasindex + 1];
                        data[index + 2] = 
                            atlas.Data[atlasindex + 2];
                        data[index + 3] = 
                            atlas.Data[atlasindex + 3];
                    }
                }
            }
        }

        // generic blit function used to add a sprite 
        // to the sprite atlas
        public int CopySpriteToAtlas(int spriteSheetID, int row, int column, int atlasId)
        {
            ref SpriteAtlas atlas = ref SpritesArray[atlasId];
            int oldSize = atlas.Rectangles.Length;
            SpriteLoader.SpriteSheet sheet = GameState.SpriteLoader.SpriteSheets[spriteSheetID];

            int index = atlas.Rectangles.Length;

            // we keep the old atlas data to copy
            // pixel from for each sprite
            byte[] oldAtlasData = atlas.Data;
            int oldWidth = atlas.Width;
            int oldHeight = atlas.Height;

        
            // we keep a list of the old packing data for each sprite
            // (position, size) provided to use by RecpackSharp library
            RectpackSharp.PackingRectangle[] old = 
                            new RectpackSharp.PackingRectangle[oldSize];

            // we can use Array.Copy for this
            for(int i = 0; i < oldSize; i++)
            {
                old[i] = atlas.Rectangles[i];
            }


            // we need a new array of rectangles
            atlas.Rectangles = new RectpackSharp.PackingRectangle[oldSize + 1];
            for(int i = 0; i < oldSize; i++)
            {
                atlas.Rectangles[i] = old[i];
            }

            // we add the new sprite packing rectangle in the array
            atlas.Rectangles[index] = 
                 new RectpackSharp.PackingRectangle(0, 0, 
                                    (uint)sheet.tileWidth, (uint)sheet.tileHeight, index);


            // calling the retpackSharp library
            // this line of code will pack the sprites
            // so the rectangle position may chang
            // in that case we keep track of the rectangles using the Id
            RectpackSharp.RectanglePacker.Pack(atlas.Rectangles, out var bounds);

            // for quick access -> mapping old to new
            int[] IndicesMap = new int[atlas.Rectangles.Length];
            for(int i = 0; i < atlas.Rectangles.Length; i++)
            {
                ref RectpackSharp.PackingRectangle rect = ref atlas.Rectangles[i];
                IndicesMap[rect.Id] = i;
            }

            // we will create a new atlas if the old one does not fit
            // keep will choose a power of 2 atlas width, height
            var widthPowerOf2 = (int)Math.Pow(2, Math.Ceiling(Math.Log(bounds.Width, 2)));
            var heightPowerOf2 = (int)Math.Pow(2, Math.Ceiling(Math.Log(bounds.Height, 2)));

            atlas.Width = Math.Max(widthPowerOf2, atlas.Width);
            atlas.Height = Math.Max(heightPowerOf2, atlas.Height);
            atlas.Data = new byte[4 * atlas.Width * atlas.Height]; 

            for(int i = 0; i < atlas.Data.Length; i++)
            {
                atlas.Data[i] = 255;
            }

            // here we copy all the old sprites from the old atlas
            // to the new one
            // the atlas can change completely depends on how 
            // the sprites are packed
            foreach(RectpackSharp.PackingRectangle oldRect in old)
            {
                int rectIndex = IndicesMap[oldRect.Id];
                RectpackSharp.PackingRectangle rect = atlas.Rectangles[rectIndex];

                for(int y = 0; y < (int)oldRect.Height; y++)
                {
                    for(int x = 0; x < (int)oldRect.Width; x++)
                    {
                        int oldIndex = 4 * ((x + (int)oldRect.X) + 
                            (y + (int)oldRect.Y) * (int)oldWidth);
                        int newIndex = 4 * ((x + (int)rect.X) + 
                            (y + (int)rect.Y) * atlas.Width);

                        // RGBA8
                        atlas.Data[newIndex] = oldAtlasData[oldIndex];
                        atlas.Data[newIndex + 1] = oldAtlasData[oldIndex + 1];
                        atlas.Data[newIndex + 2] = oldAtlasData[oldIndex + 2];
                        atlas.Data[newIndex + 3] = oldAtlasData[oldIndex + 3];
                    }
                }
            }

            // finaly we will copy the new sprite in the correct position
            ref RectpackSharp.PackingRectangle rectangle = ref atlas.Rectangles[IndicesMap[index]];

            int xOffset = (int)rectangle.X;
            int yOffset = (int)rectangle.Y;

            for (int y = 0; y < sheet.tileHeight; y++)
            {
                for (int x = 0; x < sheet.tileWidth; x++)
                {
                    int atlasIndex = 4 * ((yOffset + y)  * (atlas.Width) + (xOffset + x));
                    int sheetIndex = 4 * ((x + row * sheet.tileWidth) + 
                                    ((y + column * sheet.tileHeight) * sheet.Width));

                    // RGBA8
                    atlas.Data[atlasIndex + 0] = 
                            sheet.Data[sheetIndex + 0];
                    atlas.Data[atlasIndex + 1] = 
                            sheet.Data[sheetIndex + 1];
                    atlas.Data[atlasIndex + 2] = 
                            sheet.Data[sheetIndex + 2];
                    atlas.Data[atlasIndex + 3] = 
                            sheet.Data[sheetIndex + 3];
                }
            }

           return index;
        }
    }
}