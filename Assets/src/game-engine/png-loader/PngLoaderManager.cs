//using System.Collections;
//using System.Collections.Generic;

//TODO: Dont import Unity
using UnityEngine;

using BigGustave;
//using Entitas;
using System;
using System.Linq;
using System.IO;

//Todo: Rename, SpriteImageLoader, SpriteImageManager?
namespace PNGLoader
{
    //ToDO: Rename, SpiteImageLoaderManager, SpriteImageManager? ImageLoader? ImageLoaderManager?
    //TODOL Rename, TileImageLoader? TileImageManager?
    //RENAME: TileImageManager
    public  static class PngLoaderManager
    {
        //They are not .pngs, they are loaded as pngs, but decoded to image data
        public static ImageData[] PNGFiles;
        public static int NumberOfPNG;

        //TODO: Rename, GetImageId
        //TODO: Input should be path+filename, example assets/tiles/tilesheet_01.png
        public static int GetPNGID (string filename) 
        {
            //just an example for maing the filename to be an id
            //BUG: path+filename is relative to /StreamingAssets/
            //BUG: forward slash and backlash
            //TODO: find special to find the directo of StreamingAsset
            var location = @$"{Application.dataPath}/StreamingAssets\assets\testtemporaryfiles/0{filename}.png";
            return PNGFiles.Where(w => w.ImageID == Int32.Parse(filename) ).FirstOrDefault().ImageID;
        }   
        
        //TODO: InitStage1, InitStage2, etc
        public static void InitStage1()
        {
            // PNGFiles = new ImageData[0]; //Always use a power of 2
            LoadPNGDatas(); 
        }
        //trying to show the png datas
        public static void DebugImageDatas() 
        {    
            foreach(ImageData i in PngLoaderManager.PNGFiles)
            {
                ///Debug.Log($"{i.ImageID} image id {i.xSize} = x size, {i.ySize} = y size ");
            }   
        }
        //A test to load PNGDatas

        public static void EnterSubDirectories(DirectoryInfo currentDirectory)
        {
            string[] subDirectories = Directory.GetDirectories(currentDirectory.FullName, "*", SearchOption.TopDirectoryOnly);
            int numberOfSubDirectories = subDirectories.Length;

            if(numberOfSubDirectories > 0)
            {
                foreach(string directoryName in subDirectories)
                {
                    DirectoryInfo newCurrentDirectory = new DirectoryInfo(directoryName);
                    FilesIteration(newCurrentDirectory);
                    EnterSubDirectories(newCurrentDirectory);
                }
                
            }
        
        }
        public static void FilesIteration(DirectoryInfo newCurrentDirectory)
        {
            FileInfo[] files = newCurrentDirectory.GetFiles("*.png");
            foreach(FileInfo fileInfo in files)
            {
                Debug.Log($"{fileInfo.Name} and {newCurrentDirectory.FullName}");
                NumberOfPNG++;
                Array.Resize<ImageData>(ref PNGFiles,NumberOfPNG);
            }
        }
        public static void LoadPNGDatas()
        {
            int id = 0;
            
            DirectoryInfo mainDirectory = new DirectoryInfo(@$"{Application.dataPath}\StreamingAssets");
            string[] directories = Directory.GetDirectories(mainDirectory.FullName, "*", SearchOption.TopDirectoryOnly);
            FilesIteration(mainDirectory);
            EnterSubDirectories(mainDirectory);

            PNGFiles[0] = AssignPNGDatas(@$"{Application.dataPath}\StreamingAssets\assets\luis\terrains\moon_rock_2.png",id);
            id++;
            Debug.Log($"{NumberOfPNG} number of PNGS");
        }
        //Testing on assigning PNG Datas
        public static Func<string,int, ImageData> AssignPNGDatas = (filename,id) 
        =>  {
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
                    //Debug.Log($"{pixelRGBAData[reference].PixelsRGBA[0]} red value");  
                    reference++;
                }
            }

            return new ImageData(imageID,xSize,ySize,pixelRGBAData);  
            };
    }
}

