using BigGustave;
using System;
using System.Collections.Generic;

namespace SpriteLoader
{
    public struct SpriteSheet
    {
        public byte[] Data;

        public int id;
        public int tileWidth;
        public int tileHeight;

        public int Width;
        public int Height;
    }

    public class SpriteLoader
    {
        public SpriteSheet[] SpriteSheets;
        public Dictionary<string, int> SpriteSheetID;

        public SpriteLoader()
        {
            SpriteSheets = new SpriteSheet[0];
            SpriteSheetID = new Dictionary<string, int>();
        }

        public void InitStage1()
        {
            
        }

        public void InitStage2()
        {
        
        }

        public int GetSpriteSheetID(string filename, int width, int height)
        {

            if (SpriteSheetID.ContainsKey(filename))
            {
                return SpriteSheetID[filename];
            }

            return LoadImageFile(filename, width, height);
        }

        private int LoadImageFile(string filename, int tileWidth, int tileHeight)
        {
            int imageCount = SpriteSheets.Length + 1;

            Array.Resize(ref SpriteSheets, imageCount);

            SpriteSheetID.Add(filename, imageCount - 1);

            var data = Png.Open(filename);
            SpriteSheet spriteSheet = new SpriteSheet();
            spriteSheet.id = imageCount - 1;
            spriteSheet.tileWidth = tileWidth;
            spriteSheet.tileHeight = tileHeight;
            spriteSheet.Width = data.Header.Width;
            spriteSheet.Height = data.Header.Height;

            spriteSheet.Data = new byte[4 * data.Header.Width * data.Header.Height];

            for (int y = 0; y < data.Header.Height; y++)
            {
                for (int x = 0; x < data.Header.Width; x++)
                {
                    var pixel = data.GetPixel(x, y);
                    int index = y * data.Header.Width + x;
                    spriteSheet.Data[4 * index + 0] = pixel.R;
                    spriteSheet.Data[4 * index + 1] = pixel.G;
                    spriteSheet.Data[4 * index + 2] = pixel.B;
                    spriteSheet.Data[4 * index + 3] = pixel.A;
                }
            }

            SpriteSheets[imageCount - 1] = spriteSheet;

            return imageCount - 1;
        }

        public ref SpriteSheet GetSpriteSheet(int id)
        {
            return ref SpriteSheets[id];
        }
    }
}
