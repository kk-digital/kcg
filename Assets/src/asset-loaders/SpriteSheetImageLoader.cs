using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BigGustave;
using System.IO;
namespace ImageLoader
{
    public class SpriteSheetImageLoader : LoaderData
    {
        public static SpriteSheetImageLoader Instance;
        public SpriteSheetData[] SpriteSheet {get => FilesSpriteSheet; set => FilesSpriteSheet = value;}
        public int SpriteSheetCount {get => Count; set => Count = value;}
        public Dictionary<string, int> DictionarySpriteSheetID {get => DictionaryID; set => DictionaryID = value;}
        public delegate int DGetSpriteSheetID<SpriteSheetData>(string filename,SpriteSheetData data);
        public DGetSpriteSheetID<SpriteSheetData> GetSpriteSheetID;
        public SpriteSheetImageLoader()
        {
            GetSpriteSheetID = base.GetID;
            Instance = this;
        }

        public override SpriteSheetData AssignSpriteSheetDatas(string filename, int id)
        {
            Png png = Png.Open(filename);
            FileInfo fileInfo = new FileInfo(filename);
            var imageID = id;
            var xSize = png.Header.Width;
            var ySize = png.Header.Height;
            var accesCounter = 0;
            var loaded = 0;
            var spriteSheetType = 0;
            var pixelFormat = 0;
            var hash = 0;
            var fileCreationTime = fileInfo.CreationTime;
            var fileSize = fileInfo.Length;
            var numberOfArrays = xSize * ySize;
            var pixelsArray = new List<byte>(4 * xSize * ySize);
            //test of taking 8 sprites from spritesheet in 1st row
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    Pixel getPixels = png.GetPixel(x, y);
                    int index = y * xSize + x;
                    pixelsArray[4 * index + 0] = getPixels.R;
                    pixelsArray[4 * index + 1] = getPixels.G;
                    pixelsArray[4 * index + 2] = getPixels.B;
                    pixelsArray[4 * index + 3] = getPixels.A;
                }
            }

            TileSpriteImageLoaderManager.Instance.ImageCount += 1;
            int imageCount = TileSpriteImageLoaderManager.Instance.ImageCount;
            TileSpriteImageLoaderManager.Instance.ImageArray(ImageTest.imageData);
            TileSpriteImageLoaderManager.Instance.DictionaryPNGID.Add($"{filename}_{imageCount}", imageCount);
            return new SpriteSheetData(imageID, spriteSheetType, loaded, accesCounter, xSize,
                ySize, pixelFormat, filename, hash,
                fileCreationTime.ToString(), fileSize, pixelsArray);
        }

    }

}
