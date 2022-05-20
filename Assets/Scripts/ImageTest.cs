using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Enums;
using TileProperties;
using System;
//MonoBehaviors should be in Asset/Script folder?
namespace ImageLoader
{
    public class ImageTest : MonoBehaviour
    {
        public LoaderData ImageLoaderManager;
        public static LoaderData SpriteSheetLoaderManager;
        public static TilePropertiesManager TilesPropertiesManager;
        public static ImageData imageData;
        public SpriteSheetData spriteSheetData;
        private void Awake() 
        {
            ImageLoaderManager = new TileSpriteImageLoaderManager();
            SpriteSheetLoaderManager = new SpriteSheetImageLoader();
            TilesPropertiesManager = new TilePropertiesManager();
            //SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        }
        private void Start() 
        {
            //SpritePixelGeneration();
            GetSpriteFromSpriteSheet();
        }   

        public void SpritePixelGeneration()
        {
            TileSpriteImageLoaderManager.Instance.GetImageID("rock1.png", imageData);
            int xSize = TileSpriteImageLoaderManager.Instance.PNGFile[0].XSize;
            int ySize = TileSpriteImageLoaderManager.Instance.PNGFile[0].YSize;
            Texture2D texture = new Texture2D(xSize,
                                              ySize,
                                              TextureFormat.RGBA32,false );

                                              Debug.Log($"{xSize} x size; {ySize} y size");
            byte R;
            byte G;  
            byte B;  
            byte A;     

                                              Debug.Log($"{xSize} x size; {ySize} y size");
            int count = 0;
            //we're setting up each pixel's rgba according to the png pixels rgba   
            for(int Y = 0; Y < 16; Y++)
            {
                for(int X = 0; X < 16; X++)
                {
                    byte[] pixelArray = TileSpriteImageLoaderManager.Instance.PNGFile[0].PixelsArray;
                    int index = Y*xSize + X;
                    R = pixelArray[4 * index + 0]; //GETTING THE RED COLOR BYTE
                    G = pixelArray[4 * index + 1]; //GETTING THE GREEN COLOR BYTE 
                    B = pixelArray[4 * index + 2]; //GETTING THE BLUE COLOR BYTE  
                    A = pixelArray[4 * index + 3]; //GETTING THE ALPHA COLOR BYTE  
                    texture.SetPixel(X,Y, new Color32(R,G,B,A));
                    count++;
                }
            }
            texture.Apply(true);
            Debug.Log($"{texture.GetPixels()} pixels count");
            GameObject.Find("/Canvas/Image").GetComponent<RawImage>().texture = texture;
        }

        public void GetSpriteFromSpriteSheet()
        {
            string spriteName = "table3.png";
            string description = "A table where things can be placed";
            int tileID = TilePropertiesManager.Instance.TileProperties.Length - 1;

            SpriteSheetImageLoader.Instance.GetSpriteSheetID(spriteName, spriteSheetData);

            TilePropertiesManager.Instance.TileProperties[tileID] = new PlanetTileProperties(spriteName, description
                , tileID
                , TileDrawProperties.TileDrawPropertyNormal, tileID, 0
                , PlanetTileLayer.TileLayerFront, PlanetTileCollisionType.TileCollisionTypeSolid
                , 0);
            int xSize = SpriteSheetImageLoader.Instance.SpriteSheet[0].XSize;
            int ySize = SpriteSheetImageLoader.Instance.SpriteSheet[0].YSize;
            var texture = new Texture2D(xSize,
                ySize,
                TextureFormat.RGBA32, false);
            Debug.Log($"{xSize} x size; {ySize} y size");

            //we're setting up each pixel's RGBA according to the png pixels rgba   
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    int index = y * xSize + x;
                    var color = SpriteSheetImageLoader.Instance.SpriteSheet[0].GetColor(index);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply(true);


            GameObject.Find("/Canvas/Image").GetComponent<RawImage>().texture = texture;
        }
    }
}

