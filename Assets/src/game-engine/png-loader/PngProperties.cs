using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PNGLoader 
{
   public struct ImageData
   {
        //TODO: Add in image format enum, or ImageFormatType, RGBA, HDR16
        public int ImageID;
        public int xSize;
        public int ySize;
        public byte[] Pixels; //PixelArray? Put note that its RGBA, 4 byes per pixel
        public ImageData(int ImageID, int xSize, int ySize, byte[] Pixels)
        {
            this.ImageID = ImageID;
            this.xSize = xSize;
            this.ySize = ySize;
            this.Pixels = new byte[4];
            this.Pixels = Pixels;
        } 
   }
}

