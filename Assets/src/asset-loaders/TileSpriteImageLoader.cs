//using System.Collections;
//using System.Collections.Generic;

//TODO: Dont import Unity
using UnityEngine;

using BigGustave;
//using Entitas;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace ImageLoader
{
    public class TileSpriteImageLoaderManager : LoaderData
    {
        public static TileSpriteImageLoaderManager Instance;
        public ImageData[] PNGFile {get => FilesImage; set => FilesImage = value;}
        public int ImageCount {get => count; set => count = value;}
        public Dictionary<string, int> DictionaryPNGID {get => DictionaryID; set => DictionaryID = value;}
        public delegate int DGetImageID(string filename);
        public DGetImageID GetImageID;
        public TileSpriteImageLoaderManager()
        {
            GetImageID = new DGetImageID(base.GetID);
            Instance = this;
        }
        public override ImageData AssignPNGDatas(string filename, int id)
        {
            Png png = Png.Open(filename);
            var imageID = id;
            var xSize = png.Header.Width;
            var ySize = png.Header.Height;
            int numberOfArrays = xSize * ySize;
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
            return new ImageData(imageID,xSize,ySize,pixelRGBAData);
        }
    }
}

