using BigGustave;
using System;
using System.Collections.Generic;

namespace SpriteLoader
{
    public struct SpriteSheet
    {
        public byte[] Data;

        public int id;
        public int SpriteSize;

        public int Width;
        public int Height;
    }

    public class SpriteLoader
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

        public int GetSpriteSheetID(string filename, int width, int height) // tileWidth needed when first creating sprite sheet
        {

            // Note(Mahdi) : Not Implemented
           return 0;
        }

        private void LoadImageFile(string filename, int tileWidth)
        {
            // Note(Mahdi) : Not Implemented
        }

        public ref SpriteSheet GetSpriteSheet(int id)
        {
            return ref SpriteSheets[id];
        }
    }
}
