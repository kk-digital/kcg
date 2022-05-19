using BigGustave;
using UnityEngine;

namespace ImageLoader 
{
  

   public struct ImageData 
    public struct ImageData 
   {
        //TODO: Add in image format enum, or ImageFormatType, RGBA, HDR16 System.IO.Directory.GetFiles()
        public int ImageID;
        public int xSize;
        public int ySize;
        public byte[] _PixelsArray;
        public ImageData(int ImageID, int xSize, int ySize, byte[] _PixelsArray)
        public Pixel[] PixelsArray;

        public ImageData(int ImageID, int xSize, int ySize, Pixel[] pixelsArray)
        {
            this.ImageID = ImageID;
            this.xSize = xSize;
            this.ySize = ySize;
            this._PixelsArray = _PixelsArray;
        } 
            
            var pixelLength = pixelsArray.Length;
            PixelsArray = new Pixel[pixelLength];
            for(int i = 0; i < pixelLength; i++)
            {
                PixelsArray[i] = pixelsArray[i];   
            }
        }

        public Color32 GetColorFromPixelArray(int index)
        {
            var pixel = PixelsArray[index];
            return new Color32(pixel.R, pixel.G, pixel.B, pixel.A);
        }
   }
}

