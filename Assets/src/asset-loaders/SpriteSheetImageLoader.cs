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
        public SpriteSheet[] SpriteSheet {get => FilesSpriteSheet; set => FilesSpriteSheet = value;}
        public int SpriteSheetCount {get => count; set => count = value;}
        public Dictionary<string, int> DictionarySpriteSheetID {get => DictionaryID; set => DictionaryID = value;}
        public delegate int DGetSpriteSheetID(string filename);
        public DGetSpriteSheetID GetSpriteSheetID;
        public SpriteSheetImageLoader()
        {
            GetSpriteSheetID = new DGetSpriteSheetID(base.GetID);
            Instance = this;
        }
        public override SpriteSheet AssignSpriteSheetDatas(string filename, int id)
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
                    //Debug.Log($"{pixelRGBAData[reference].PixelsRGBA[0]} red value, {pixelRGBAData[reference].PixelsRGBA[1]} green value,  {pixelRGBAData[reference].PixelsRGBA[2]} blue value");  
                    reference++;
                }
            }
            return new SpriteSheet(imageID,spriteSheetType,loaded,accesCounter,xSize,
                                  ySize,pixelFormat,pixelRGBAData,filename,hash,
                                  fileCreationTime.ToString(),fileSize);
        }

    }

}
