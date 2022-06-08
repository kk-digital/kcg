using BigGustave;
using System;
using System.Collections.Generic;

namespace Sprites
{
    public class Loader
    {
        public Sheet[] SpriteSheets;
        public Dictionary<string, int> SpriteSheetID;

        public Loader()
        {
            SpriteSheets = new Sheet[0];
            SpriteSheetID = new Dictionary<string, int>();
        }

        public void InitStage1()
        {
            
        }

        public void InitStage2()
        {
        
        }

        public int GetSpriteSheetID(string filename)
        {

            if (SpriteSheetID.ContainsKey(filename))
            {
                return SpriteSheetID[filename];
            }

            return LoadImageFile(filename);
        }

        private int LoadImageFile(string filename)
        {
            int imageCount = SpriteSheets.Length + 1;

            Array.Resize(ref SpriteSheets, imageCount);

            SpriteSheetID.Add(filename, imageCount - 1);

            var data = Png.Open(filename);
            Sheet sheet = new Sheet();
            sheet.id = imageCount - 1;
            sheet.Width = data.Header.Width;
            sheet.Height = data.Header.Height;

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

        public ref Sheet GetSpriteSheet(int id)
        {
            return ref SpriteSheets[id];
        }
    }
}
