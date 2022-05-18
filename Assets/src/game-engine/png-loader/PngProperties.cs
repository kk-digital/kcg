using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
namespace PNGLoader 
{
   public struct PixelsRGBAData
   {
        public byte[] PixelsRGBA; 

        public PixelsRGBAData(byte[] PixelsRGBA)
        {
            this.PixelsRGBA = new byte[4];
            this.PixelsRGBA = PixelsRGBA;
        }   
   }

   [Serializable]
   public struct ImageData
   {
        //TODO: Add in image format enum, or ImageFormatType, RGBA, HDR16 System.IO.Directory.GetFiles()
        public int ImageID;
        public int xSize;
        public int ySize;
        public PixelsRGBAData[] PixelsArray;

        public ImageData(int ImageID, int xSize, int ySize, PixelsRGBAData[] PixelsArray )
        {
            this.ImageID = ImageID;
            this.xSize = xSize;
            this.ySize = ySize;
            int numberOfArrays = PixelsArray.Length;
            this.PixelsArray = new PixelsRGBAData[numberOfArrays];
            for(int i = 0; i < numberOfArrays; i++)
            {
                PixelsArray[i].PixelsRGBA = PixelsArray[i].PixelsRGBA;   
            }
        } 
   }
}

