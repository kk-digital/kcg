using System.IO;
using BigGustave;
using UnityEngine;

namespace ImageLoader
{
    public struct SpriteSheetData
    {
        public int SpriteSheetId;
        public int SpriteSheetType; //enum in src/enums
        public int Loaded;
        public int AccessCounter; //0 at creation, increment every blit or usage operation
        public Vector2Int Size;
        public int PixelFormat; //enum in src/enums, RGBA default

        #region FileProperties

        public string FileName; // Filename and path string
        public long Hash; // 64 bit xxHash of image file
        public string FileCreationTime; // time of file modification
        public long FileSize;
        public Png Data;

        #endregion

        public SpriteSheetData(Png pngData, string fileName, int spriteSheetId, int spriteSheetType, int loaded, int accessCounter,
            int pixelFormat, long hash) : this()
        {
            var fileInfo = new FileInfo(fileName);

            Data = pngData;
            SpriteSheetId = spriteSheetId;
            SpriteSheetType = spriteSheetType;
            Loaded = loaded;
            AccessCounter = accessCounter;
            Size.x = Data.Header.Width;
            Size.y = Data.Header.Height;
            PixelFormat = pixelFormat;
            FileName = fileName;
            Hash = hash;
            FileCreationTime = fileInfo.CreationTime.ToString();
            FileSize = fileInfo.Length;
        }

        /// <summary>
        /// Getting RGBA color bytes from index
        /// </summary>
        public Color32 GetPixelColor(int x, int y)
        {
            var pixel = Data.GetPixel(x, y);
            var r = pixel.R;
            var g = pixel.G;
            var b = pixel.B;
            var a = pixel.A;

            return new Color32(r, g, b, a);
        }
    }
}
    
   

