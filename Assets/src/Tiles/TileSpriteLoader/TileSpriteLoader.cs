using BigGustave;
using System;
using System.Collections.Generic;

namespace TileSpriteLoader
{
    public class TileSpriteLoader
    {
        public SpriteSheet[] SpriteSheets;
        public int ImageCount;
        public Dictionary<string, int> SpriteSheetID = new Dictionary<string, int>();

        public void InitStage1()
        {
            
        }

        public void InitStage2()
        {
        
        }

        public int GetSpriteSheetID(string filename, int tileWidth = 32) // tileWidth needed when first creating sprite sheet
        {
            if (SpriteSheetID.ContainsKey(filename))
            {
                return SpriteSheetID[filename];
            }

            LoadImageFile(filename, tileWidth);
            return SpriteSheetID[filename];
        }

        private void LoadImageFile(string filename, int tileWidth)
        {
            ImageCount++;
            SpriteSheetID.Add(filename, ImageCount - 1);

            Array.Resize(ref SpriteSheets, ImageCount);

            var data = Png.Open(filename);
            SpriteSheets[ImageCount - 1] = new SpriteSheet();
            SpriteSheets[ImageCount - 1].id = ImageCount - 1;
            SpriteSheets[ImageCount - 1].SpriteSize = tileWidth;
            SpriteSheets[ImageCount - 1].Width = data.Header.Width;
            SpriteSheets[ImageCount - 1].Height = data.Header.Height;

            SpriteSheets[ImageCount - 1].Data = new byte[4 * data.Header.Width * data.Header.Height];

            for (int y = 0; y < data.Header.Height; y++)
            {
                for (int x = 0; x < data.Header.Width; x++)
                {
                    var pixel = data.GetPixel(x, y);
                    int index = y * data.Header.Width + x;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 0] = pixel.R;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 1] = pixel.G;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 2] = pixel.B;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 3] = pixel.A;
                }
            }
        }

        public ref SpriteSheet GetSpriteSheet(int id)
        {
            return ref SpriteSheets[id];
        }
    }
}
