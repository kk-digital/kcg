using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ImageLoader 
{
    public struct SpriteSheet 
    {
        int SpriteSheetId;
        int SpriteSheetType; //enum in src/enums
        int Loaded;
        int AccessCounter; //0 at creation, increment every blit or usage operation
        int XSize;
        int YSize;
        int PixelFormat; //enum in src/enums, RGBA default
        PixelsRGBAData[] PixelData; //4 bytes per pixel RGBA
            //File Properties
        string FileName; // Filename and path string
        Int64 Hash; // 64 bit xxHash of image file
        string FileCreationTime; // time of file modification
        long FileSize;
        public SpriteSheet(int SpriteSheetId, int SpriteSheetType, 
                                     int Loaded, int AccessCounter, int XSize, 
                                     int Ysize, int PixelFormat, PixelsRGBAData[] PixelData,
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
    }    
}
    
   

