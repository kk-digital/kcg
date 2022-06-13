using BigGustave;
using System;
using System.Collections.Generic;

namespace Sprites
{
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

        public int GetSpriteSheetID(string filename, int spriteWidth, int spriteHeight)
        {

            if (SpriteSheetID.ContainsKey(filename))
            {
                return SpriteSheetID[filename];
            }

            return LoadImageFile(filename, spriteWidth, spriteHeight);
        }

        private int LoadImageFile(string filename, int spriteWidth, int spriteHeight)
        {
            int imageCount = SpriteSheets.Length + 1;

            Array.Resize(ref SpriteSheets, imageCount);

            SpriteSheetID.Add(filename, imageCount - 1);

            var data = Png.Open(filename);
            SpriteSheet sheet = new SpriteSheet();
            sheet.Index = imageCount - 1;
            sheet.Width = data.Header.Width;
            sheet.Height = data.Header.Height;
            sheet.SpriteWidth = spriteWidth;
            sheet.SpriteHeight = spriteHeight;

            sheet.Data = new byte[4 * data.Header.Width * data.Header.Height];

            for (int y = 0; y < data.Header.Height; y++)
            {
                for (int x = 0; x < data.Header.Width; x++)
                {
                    var pixel = data.GetPixel(x, y);
                    int index = y * data.Header.Width + x;
                    sheet.Data[4 * index + 0] = pixel.R;
                    sheet.Data[4 * index + 1] = pixel.G;
                    sheet.Data[4 * index + 2] = pixel.B;
                    sheet.Data[4 * index + 3] = pixel.A;
                }
            }

            SpriteSheets[imageCount - 1] = sheet;

            return imageCount - 1;
        }

        public ref SpriteSheet GetSpriteSheet(int id)
        {
            return ref SpriteSheets[id];
        }
    }
}
