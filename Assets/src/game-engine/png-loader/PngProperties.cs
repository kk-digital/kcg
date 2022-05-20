using BigGustave;
using UnityEngine;

namespace ImageLoader 
{
   public struct ImageData 
   {
        //TODO: Add in image format enum, or ImageFormatType, RGBA, HDR16 System.IO.Directory.GetFiles()
        public int ImageID;
        public int XSize;
        public int YSize;
        public byte[] PixelsArray;
        public ImageData(int imageID, int xSize, int ySize, byte[] pixelsArray)
        {
            ImageID = imageID;
            XSize = xSize;
            YSize = ySize;
            PixelsArray = pixelsArray;
        }
    }
}


