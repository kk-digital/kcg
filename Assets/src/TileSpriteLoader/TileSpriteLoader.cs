using BigGustave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TileSpriteLoader
{
    public class TileSpriteLoader
    {
        public static TileSpriteLoader Instance;
        public SpriteSheet[] SpriteSheets;
        public int ImageCount;
        public Dictionary<string, int> SpriteSheetID;

        public TileSpriteLoader()
        {
            Instance = this;
        }

        public static void InitStage1()
        {
            Instance = new TileSpriteLoader();
        }

        public static void InitStage2()
        {
        
        }

        public int GetSpriteSheetID(string filename, int tileWidth = 32) // tileWidth needed when first creating sprite sheet
        {
            if (SpriteSheetID.ContainsKey(filename))
            {
                Debug.Log("id found in the dictionary");
                return SpriteSheetID[filename];
            }
            else
            {
                Debug.Log("id not found in the dictionary");
                FileInfo fileInfo = new DirectoryInfo(Directory.GetCurrentDirectory())
                                .EnumerateFiles(filename, SearchOption.AllDirectories)
                                .FirstOrDefault();
                if (fileInfo is { Exists: true })
                {
                    LoadImageFile(filename, fileInfo, tileWidth);
                    return SpriteSheetID[filename];
                }
            }
            return -1;
        }

        private void LoadImageFile(string filename, FileInfo fileInfo, int tileWidth)
        {
            ImageCount++;
            Debug.Log($"file found, adding {fileInfo.FullName} into dictionary");
            SpriteSheetID.Add(filename, ImageCount - 1);

            Array.Resize(ref SpriteSheets, ImageCount - 1);

            var data = Png.Open(filename);
            SpriteSheets[ImageCount - 1] = new SpriteSheet();
            SpriteSheets[ImageCount - 1].id = ImageCount - 1;
            SpriteSheets[ImageCount - 1].SpriteSize = tileWidth;
            SpriteSheets[ImageCount - 1].Width = data.Header.Width;
            SpriteSheets[ImageCount - 1].Height = data.Header.Height;

            SpriteSheets[ImageCount - 1].Data = new byte[4 * data.Header.Width * data.Header.Height];

            for (int y = 0; y < data.Header.Height; y++)
            {
                for (int x = 0; x < data.Header.Width; x++)
                {
                    var pixel = data.GetPixel(x, y);
                    int index = y * data.Header.Width + x;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 0] = pixel.R;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 1] = pixel.G;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 2] = pixel.B;
                    SpriteSheets[ImageCount - 1].Data[4 * index + 3] = pixel.A;
                }
            }
        }

        public ref SpriteSheet GetSpriteSheet(int id)
        {
            return ref SpriteSheets[id];
        }
    }
}
