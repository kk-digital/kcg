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
        private void Start() 
        {
            //SpritePixelGeneration();
            GetSpriteFromSpriteSheet();
        }

        public void SpritePixelGeneration()
        {
            TileSpriteImageLoaderManager.Instance.GetImageID("rock1.png", imageData);
            int xSize = TileSpriteImageLoaderManager.Instance.PNGFile[0].Size.x;
            int ySize = TileSpriteImageLoaderManager.Instance.PNGFile[0].Size.y;
            var texture = new Texture2D(xSize, ySize, TextureFormat.RGBA32, false);

            Debug.Log($"{xSize} x size; {ySize} y size");
            Debug.Log($"{xSize} x size; {ySize} y size");
            //we're setting up each pixel's rgba according to the png pixels rgba   
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    var color = TileSpriteImageLoaderManager.Instance.PNGFile[0].GetColor(x, y);
                    texture.SetPixel(x, y, color);
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

            TilePropertiesManager.Instance.TileProperties[tileID] = new TileProperties.TileProperties(spriteName, description,
                tileID, TileDrawProperties.TileDrawPropertyNormal, tileID, 0, PlanetTileLayer.TileLayerFront,
                PlanetTileCollisionType.TileCollisionTypeSolid, 0);

            int xSize = SpriteSheetImageLoader.Instance.SpriteSheet[0].Size.x;
            int ySize = SpriteSheetImageLoader.Instance.SpriteSheet[0].Size.y;
            var texture = new Texture2D(xSize, ySize, TextureFormat.RGBA32, false);

            Debug.Log($"{xSize} x size; {ySize} y size");

            //we're setting up each pixel's RGBA according to the png pixels rgba   
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    var color = SpriteSheetImageLoader.Instance.SpriteSheet[0].GetColor(x, y);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply(true);
            GameObject.Find("/Canvas/Image").GetComponent<RawImage>().texture = texture;
        }
    }
}

