using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PNGLoader 
{
   public struct ImageData
   {
        public int ImageID;
        public int xSize;
        public int ySize;
        public byte[] Pixels;
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

