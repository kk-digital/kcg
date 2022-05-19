using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//MonoBehaviors should be in Asset/Script folder?
namespace ImageLoader
{
    public class ImageTest : MonoBehaviour
    {
        public LoaderData ImageLoaderManager;
        public static LoaderData SpriteSheetLoaderManager;
        public static ImageData imageData;
        public SpriteSheetData spriteSheetData;
        private void Awake() 
        {
            ImageLoaderManager = new TileSpriteImageLoaderManager();
            SpriteSheetLoaderManager = new SpriteSheetImageLoader();
        }
        private void Start() 
        {
            SpritePixelGeneration();
            //GetSpriteFromSpriteSheet();
        }   

        public void SpritePixelGeneration()
        {
            TileSpriteImageLoaderManager.Instance.GetImageID("rock1.png", imageData);
            int xSize = TileSpriteImageLoaderManager.Instance.PNGFile[0].xSize;
            int ySize = TileSpriteImageLoaderManager.Instance.PNGFile[0].ySize;
            Texture2D texture = new Texture2D(xSize,
                                              ySize,
                                              TextureFormat.RGBA32,false );
                                              Debug.Log($"{xSize} x size; {ySize} y size");
            byte R;
            byte G;  
            byte B;  
            byte A;     
            for(int Y = 0; Y < 16; Y++)
            {
                for(int X = 0; X < 16; X++)
                {
                    byte[] pixelArray = TileSpriteImageLoaderManager.Instance.PNGFile[0]._PixelsArray;
                    int index = Y*xSize + X;
                    R = pixelArray[4 * index + 0]; //GETTING THE RED COLOR BYTE
                    G = pixelArray[4 * index + 1]; //GETTING THE GREEN COLOR BYTE 
                    B = pixelArray[4 * index + 2]; //GETTING THE BLUE COLOR BYTE  
                    A = pixelArray[4 * index + 3]; //GETTING THE ALPHA COLOR BYTE  
                    texture.SetPixel(X,Y, new Color32(R,G,B,A));
                }
            }
            texture.Apply(true);
            Debug.Log($"{texture.GetPixels()} pixels count");
            GameObject.Find("/Canvas/Image").GetComponent<RawImage>().texture = texture;
        }
        public void GetSpriteFromSpriteSheet()
        {
            SpriteSheetImageLoader.Instance.GetSpriteSheetID("spiderDrill_spritesheet.png",spriteSheetData);
            Texture2D texture = new Texture2D(73,
                                              55,
                                              TextureFormat.RGBA32,false );
                                              Debug.Log($"{73} x size; {55} y size");
            int xSize = SpriteSheetImageLoader.Instance.SpriteSheet[0].XSize;
            int ySize = SpriteSheetImageLoader.Instance.SpriteSheet[0].YSize;                                 
            byte R;
            byte G;  
            byte B;  
            byte A;     
            //we're setting up each pixel's rgba according to the png pixels rgba   
            for(int Y = 0; Y < 55; Y++)
            {
                for(int X = 0; X < 73; X++)
                {
                    byte[] pixelArray = TileSpriteImageLoaderManager.Instance.PNGFile[0]._PixelsArray;
                    int index = Y*xSize + X;
                    R = pixelArray[4 * index + 0]; //GETTING THE RED COLOR BYTE
                    G = pixelArray[4 * index + 1]; //GETTING THE GREEN COLOR BYTE 
                    B = pixelArray[4 * index + 2]; //GETTING THE BLUE COLOR BYTE  
                    A = pixelArray[4 * index + 3]; //GETTING THE ALPHA COLOR BYTE  
                    texture.SetPixel(X,Y, new Color32(R,G,B,A));
                }
            }
            texture.Apply(true);     
            GameObject.Find("/Canvas/Image").GetComponent<RawImage>().texture = texture;   
        }
    }
}

