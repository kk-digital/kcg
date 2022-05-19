using System.Collections;
using System.Collections.Generic;
using System;
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
        public int XSize;
        public int YSize;
        public int PixelFormat; //enum in src/enums, RGBA default

        public Pixel[] PixelData; //4 bytes per pixel RGBA

        //File Properties
        public string FileName; // Filename and path string
        public Int64 Hash; // 64 bit xxHash of image file
        public string FileCreationTime; // time of file modification
        public long FileSize;

        public SpriteSheetData(int SpriteSheetId, int SpriteSheetType,
            int Loaded, int AccessCounter, int XSize,
            int Ysize, int PixelFormat, Pixel[] PixelData,
            string FileName, Int64 Hash, string FileCreationTime, long FileSize)
        {
            this.SpriteSheetId = SpriteSheetId;
            this.SpriteSheetType = SpriteSheetType;
            this.Loaded = Loaded;
            this.AccessCounter = AccessCounter;
            this.XSize = XSize;
            this.YSize = Ysize;
            this.PixelFormat = PixelFormat;
            this.PixelData = PixelData;
            this.FileName = FileName;
            this.Hash = Hash;
            this.FileCreationTime = FileCreationTime;
            this.FileSize = FileSize;
        }
        
        public Color32 GetColorFromPixelArray(int index)
        {
            var pixel = PixelData[index];
            return new Color32(pixel.R, pixel.G, pixel.B, pixel.A);
        }
    }
}
    
   

