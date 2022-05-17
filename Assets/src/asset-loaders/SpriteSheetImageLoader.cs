using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BigGustave;
using System.IO;
namespace ImageLoader
{
    public class SpriteSheetImageLoader : LoaderData
    {
        public static SpriteSheetImageLoader Instance;
        public SpriteSheetData[] SpriteSheet {get => FilesSpriteSheet; set => FilesSpriteSheet = value;}
        public int SpriteSheetCount {get => count; set => count = value;}
        public Dictionary<string, int> DictionarySpriteSheetID {get => DictionaryID; set => DictionaryID = value;}
        public delegate int DGetSpriteSheetID<SpriteSheetData>(string filename,SpriteSheetData data);
        public DGetSpriteSheetID<SpriteSheetData> GetSpriteSheetID;
        public SpriteSheetImageLoader()
        {
            GetSpriteSheetID = new DGetSpriteSheetID<SpriteSheetData>(base.GetID<SpriteSheetData>);
            Instance = this;
        }
        public override SpriteSheetData AssignSpriteSheetDatas(string filename, int id)
        {
            Png png = Png.Open(filename);
            FileInfo fileInfo = new FileInfo(filename);
            var imageID = id;
            var xSize = png.Header.Width;
            var ySize = png.Header.Height;
            var accesCounter = 0;
            var loaded = 0;
            var spriteSheetType = 0;
            var pixelFormat = 0;
            var hash = 0;
            var fileCreationTime = fileInfo.CreationTime;
            var fileSize = fileInfo.Length;
            var numberOfArrays = xSize * ySize;
            PixelsRGBAData[] pixelRGBAData = new PixelsRGBAData[numberOfArrays];
            int reference = 0;
            for(int y = 0; y < ySize; y++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    Pixel getPixels = png.GetPixel(x,y); 
                    byte[] pixelsRGBA = new byte[4] {getPixels.R,getPixels.G,getPixels.B,getPixels.A};
                    pixelRGBAData[reference] = new PixelsRGBAData(pixelsRGBA);
                    reference++;
                }
            }
            return new SpriteSheetData(imageID,spriteSheetType,loaded,accesCounter,xSize,
                                       ySize,pixelFormat,pixelRGBAData,filename,hash,
                                       fileCreationTime.ToString(),fileSize);
        }

    }

}
