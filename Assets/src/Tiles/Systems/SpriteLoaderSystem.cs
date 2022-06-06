using BigGustave;
using System;
using System.Collections.Generic;

namespace Tile
{
    public class SpriteLoaderSystem
    {
        public Tile.SpriteSheet[] SpriteSheets;
        public int ImageCount;
        public Dictionary<string, int> SpriteSheetID = new();

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
            SpriteSheets[ImageCount - 1] = new Tile.SpriteSheet
            {
                ID = ImageCount - 1,
                SpriteSize = tileWidth,
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
                    SpriteSheets[ImageCount - 1].Data[4 * index + 0] = pixel.R;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 1] = pixel.G;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 2] = pixel.B;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 3] = pixel.A;
                }
            }
        }

        public ref Tile.SpriteSheet GetSpriteSheet(int id)
        {
            return ref SpriteSheets[id];
        }
    }
}
