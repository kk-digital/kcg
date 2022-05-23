using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageLoader;
using System.IO;
using System.Linq;
using System;
using BigGustave;
using TileProperties;

namespace ImageLoader
{
    public class SpriteSheetImageLoader
    {
        private int Count;
        private Dictionary<string, int> DictionaryID = new();
        private SpriteSheetData[] FilesSpriteSheet;

        public static SpriteSheetImageLoader Instance;

        public void ImageArray<Data>(Data data)
        {
            Array.Resize(ref FilesSpriteSheet, Count);
        }

        public SpriteSheetData[] SpriteSheet
        {
            get => FilesSpriteSheet;
            set => FilesSpriteSheet = value;
        }

        public int SpriteSheetCount
        {
            get => Count;
            set => Count = value;
        }

        public Dictionary<string, int> DictionarySpriteSheetID
        {
            get => DictionaryID;
            set => DictionaryID = value;
        }

        public SpriteSheetImageLoader()
        {
            Instance = this;
        }

        public static void InitStage1()
        {
            Instance = new SpriteSheetImageLoader();
        }
        
        public static void InitStage2()
        {
            
        }

        public SpriteSheetData AssignSpriteSheetDatas(string filename, int id)
        {
            const int accessCounter = 0;
            const int loaded = 0;
            const int spriteSheetType = 0;
            const int pixelFormat = 0;
            const int hash = 0;
            
            var imageCount = ++TileSpriteImageLoaderManager.Instance.ImageCount;
            
            TileSpriteImageLoaderManager.Instance.ImageArray(ImageTest.imageData);
            TileSpriteImageLoaderManager.Instance.DictionaryPNGID.Add($"{filename}_{imageCount}", imageCount);

            var data = Png.Open(filename);
            return new SpriteSheetData(data, filename, id, spriteSheetType, loaded, accessCounter, pixelFormat, hash);
        }

        public int GetSpriteSheetID<TData>(string filename, TData data)
        {
            int id = 0;
            if (DictionaryID.ContainsKey(filename))
            {
                id = DictionaryID[filename];
                Debug.Log("id found in the dictionary");
            }
            else
            {
                Debug.Log("id not found in the dictionary");
                FileInfo fileInfo = new DirectoryInfo(Directory.GetCurrentDirectory())
                                .EnumerateFiles(filename, SearchOption.AllDirectories)
                                .FirstOrDefault();
                if (fileInfo is { Exists: true })
                {
                    LoadImageFile(filename, fileInfo, data);
                }
            }
            return id;
        }

        private void LoadImageFile<TData>(string filename, FileInfo fileInfo, TData data)
        {
            Count++;
            Debug.Log($"file found, adding {fileInfo.FullName} into dictionary");
            DictionaryID.Add(filename, Count);

            Array.Resize(ref FilesSpriteSheet, Count);
            Array.Resize(ref TilePropertiesManager.Instance.TileProperties, Count);
            FilesSpriteSheet[Count - 1] = AssignSpriteSheetDatas(fileInfo.FullName, Count);
        }
    }
}
