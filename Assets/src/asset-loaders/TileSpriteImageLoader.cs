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
//Todo: Rename, SpriteImageLoader, SpriteImageManager?
namespace PNGLoader
{
    //ToDO: Rename, SpiteImageLoaderManager, SpriteImageManager? ImageLoader? ImageLoaderManager?
    //TODOL Rename, TileImageLoader? TileImageManager?
    //RENAME: TileImageManager
    public  static class TileSpriteImageLoaderManager
    {
        //They are not .pngs, they are loaded as pngs, but decoded to image data
        public static ImageData[] PNGFiles;
        public static int ImageCount;
        public static Dictionary<string, int> DictionaryPNGID = new Dictionary<string, int>();
        //TODO: Rename, GetImageId
        //TODO: Input should be path+filename, example assets/tiles/tilesheet_01.png
        public static int GetImageID (string filename) 
        {
            //just an example for maing the filename to be an id
            //BUG: path+filename is relative to /StreamingAssets/
            //BUG: forward slash and backlash
            //TODO: find special to find the directo of StreamingAsset
            int id = 0;
            if(DictionaryPNGID.ContainsKey(filename))
            {
                id = DictionaryPNGID[filename];
                Debug.Log("id found in the png dictionary");
            }
            else 
            {
                
                Debug.Log("id not found in the png dictionary");
                

                FileInfo fileInfo = new DirectoryInfo(Directory.GetCurrentDirectory())
                            .EnumerateFiles(filename, SearchOption.AllDirectories)
                            .FirstOrDefault();
                if(fileInfo.Exists)
                {
                    LoadImageFile(filename,fileInfo);
                }    

            }
            return id;
            // var location = @$"{Application.dataPath}/StreamingAssets\assets\testtemporaryfiles/0{filename}.png";
            // return PNGFiles.Where(w => w.ImageID == Int32.Parse(filename) ).FirstOrDefault().ImageID;
        }   
        
        public static void LoadImageFile(string filename, FileInfo fileInfo)
        {
            ImageCount++;
            Debug.Log($"file found, adding {fileInfo.FullName} into dictionary");
            DictionaryPNGID.Add(filename, ImageCount);
            Array.Resize(ref PNGFiles, ImageCount);
            PNGFiles[ImageCount-1] = AssignPNGDatas(fileInfo.FullName,ImageCount);
        }
        //TODO: InitStage1, InitStage2, etc
        public static void InitStage1()
        {
            // PNGFiles = new ImageData[0]; //Always use a power of 2
        }
        //Testing on assigning PNG Datas
        public static ImageData AssignPNGDatas (string filename, int id)
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

