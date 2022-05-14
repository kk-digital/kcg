using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BigGustave;
using Entitas;
using System;
using System.Linq;

namespace PNGLoader
{
    public  static class PngLoaderManager
    {
        public static ImageData[] PNGFiles;
        public static int GetPNGID (string filename) 
        {
            //just an example for maing the filename to be an id
            var location = @$"{Application.dataPath}/StreamingAssets\assets\testtemporaryfiles/0{filename}.png";
            return PNGFiles.Where(w => w.ImageID == Int32.Parse(filename) ).FirstOrDefault().ImageID;
        }   
        
        public static void InitializePNGTest()
        {
            PNGFiles = new ImageData[5]; 
            LoadPNGDatas(); 
        }
        //trying to show the png datas
        public static void DebugImageDatas() 
        {    
            foreach(ImageData i in PngLoaderManager.PNGFiles)
            {
                Debug.Log($"{i.ImageID} image id {i.xSize} = x size, {i.ySize} = y size ");
            }   
        }
        //A test to load PNGDatas
        public static void LoadPNGDatas()
        {
            int id = 0;
            //0 is reserved and error
            //-1 return spritesheet int-1 on error
            for(int x = 1; x < PngLoaderManager.PNGFiles.Length; x++)
            {
                PngLoaderManager.PNGFiles[x] = AssignPNGDatas(@$"{Application.dataPath}/StreamingAssets\assets\temporarypngfiles/{id}.png",id);
                id++;
            }
        }
        //Testing on assigning PNG Datas
        public static Func<string,int, ImageData> AssignPNGDatas = (filename,id) 
        =>  {
            Png png = Png.Open(filename);
            var imageID = id;
            var xSize = png.Width;
            var ySize = png.Height;
            Pixel getPixels = png.GetPixel(0,7); 
            byte[] pixels = new byte[4] {getPixels.R,getPixels.G,getPixels.B,getPixels.A};
            return new ImageData(imageID,xSize,ySize,pixels);  
            };
    }
}

