using BigGustave;
using System;
using System.Collections.Generic;

namespace Sprites
{
    public class LoaderSystem
    {
        public Sheet[] SpriteSheets;
        public Dictionary<string, int> SpriteSheetID;

        public LoaderSystem()
        {
            SpriteSheets = Array.Empty<Sheet>();
            SpriteSheetID = new Dictionary<string, int>();
        }

        public void InitStage1()
        {
            
        }

        public void InitStage2()
        {
        
        }

        public int GetSpriteSheetID(string filename) => SpriteSheetID.ContainsKey(filename) ? SpriteSheetID[filename] : LoadImageFile(filename);

        private int LoadImageFile(string filename)
        {
            int imageCount = SpriteSheets.Length + 1;

            Array.Resize(ref SpriteSheets, imageCount);

            SpriteSheetID.Add(filename, imageCount - 1);

            var data = Png.Open(filename);
            var spriteSheet = new Sheet
            {
                ID = imageCount - 1,
                Width = data.Header.Width,
                Height = data.Header.Height,
                Data = new byte[4 * data.Header.Width * data.Header.Height]
            };

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

        public ref Sheet GetSpriteSheet(int id)
        {
            return ref SpriteSheets[id];
        }
    }
}
