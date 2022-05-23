//TODO: Dont import Unity
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
    //TODO: Do not inherit LoaderData
    public class TileSpriteImageLoaderManager
    {
        private int Count;
        private Dictionary<string, int> DictionaryID = new();
        private ImageData[] FilesImage;

        public static TileSpriteImageLoaderManager Instance;
        public ImageData[] PNGFile {get => FilesImage; set => FilesImage = value;}
        public ImageData ImageData;
        public int ImageCount {get => Count; set => Count = value;}
        public Dictionary<string, int> DictionaryPNGID {get => DictionaryID; set => DictionaryID = value;}

        public void ImageArray<Data>(Data data)
        {
            Array.Resize(ref FilesImage, Count);
        }

        public TileSpriteImageLoaderManager()
        {
            Instance = this;
        }

        public static void InitStage1()
        {
            Instance = new TileSpriteImageLoaderManager();
        }

        public static void InitStage2()
        {
            
        }

        public ImageData AssignPNGDatas(string filename, int id)
        {
            var pngData = Png.Open(filename);
            return new ImageData(pngData, id);
        }

        public int GetImageID<TData>(string filename, TData data)
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

            Array.Resize(ref FilesImage, Count);
            FilesImage[Count - 1] = AssignPNGDatas(fileInfo.FullName, Count);
        }
    }
}


