using System.Collections;
using System.Collections.Generic;
using System;
using BigGustave;
using UnityEngine;

namespace ImageLoader
{
    public struct SpriteSheetData
    {
        public int SpriteSheetId { get; private set; }
        public int SpriteSheetType { get; private set; } //enum in src/enums
        public int Loaded { get; private set; }
        public int AccessCounter { get; private set; } //0 at creation, increment every blit or usage operation
        public int XSize { get; private set; }
        public int YSize { get; private set; }
        public int PixelFormat { get; private set; } //enum in src/enums, RGBA default

        #region FileProperties

        public string FileName { get; private set; } // Filename and path string
        public Int64 Hash { get; private set; } // 64 bit xxHash of image file
        public string FileCreationTime { get; private set; } // time of file modification
        public long FileSize { get; private set; }
        public List<byte> PixelsArray { get; private set; }

        #endregion

        public SpriteSheetData(int spriteSheetId, int spriteSheetType,
            int loaded, int accessCounter, int xSize,
            int ySize, int pixelFormat,
            string fileName, long hash, string fileCreationTime,
            long fileSize, List<byte> pixelsArray)
        {
            SpriteSheetId = spriteSheetId;
            SpriteSheetType = spriteSheetType;
            Loaded = loaded;
            AccessCounter = accessCounter;
            XSize = xSize;
            YSize = ySize;
            PixelFormat = pixelFormat;
            FileName = fileName;
            Hash = hash;
            FileCreationTime = fileCreationTime;
            FileSize = fileSize;
            PixelsArray = pixelsArray;
        }
    }
}
    
   

