using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
namespace ImageLoader 
{
  

   public struct ImageData 
   {
        //TODO: Add in image format enum, or ImageFormatType, RGBA, HDR16 System.IO.Directory.GetFiles()
        public int ImageID;
        public int xSize;
        public int ySize;
        public byte[] _PixelsArray;
        public ImageData(int ImageID, int xSize, int ySize, byte[] _PixelsArray)
        {
            this.ImageID = ImageID;
            this.xSize = xSize;
            this.ySize = ySize;
            this._PixelsArray = _PixelsArray;
        } 
   }
}

