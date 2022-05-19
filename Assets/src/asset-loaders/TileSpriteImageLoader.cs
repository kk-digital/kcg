//using System.Collections;
//using System.Collections.Generic;

//TODO: Dont import Unity

using BigGustave;
//using Entitas;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
namespace ImageLoader
{
    public class TileSpriteImageLoaderManager : LoaderData
    {
        public static TileSpriteImageLoaderManager Instance;
        public ImageData[] PNGFile {get => FilesImage; set => FilesImage = value;}
        public ImageData imageData;        
        public int ImageCount {get => count; set => count = value;}
        public Dictionary<string, int> DictionaryPNGID {get => DictionaryID; set => DictionaryID = value;}
        public delegate int DGetImageID<ImageData>(string filename, ImageData data);
        public DGetImageID<ImageData> GetImageID;
        public TileSpriteImageLoaderManager()
        {
            GetImageID = new DGetImageID<ImageData>(base.GetID<ImageData>);
            Instance = this;
        }
        public override ImageData AssignPNGDatas(string filename, int id)
        {
            Png png = Png.Open(filename);
            var imageID = id;
            var xSize = png.Header.Width;
            var ySize = png.Header.Height;
            int numberOfArrays = xSize * ySize;
            byte[] pixelsArray = new byte[4 * xSize * ySize];
            for(int y = 0; y < ySize; y++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    Pixel getPixels = png.GetPixel(x,y); 
                    int index = y*xSize + x;
                    pixelsArray[4 * index + 0] = getPixels.R;
                    pixelsArray[4 * index + 1] = getPixels.G;
                    pixelsArray[4 * index + 2] = getPixels.B;
                    pixelsArray[4 * index + 3] = getPixels.A;
                }
            }
            return new ImageData(imageID,xSize,ySize,pixelsArray);
        }
    }
}

