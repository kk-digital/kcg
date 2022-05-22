using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageLoader;
using System.IO;
using System.Linq;
using System;
using BigGustave;
using TileProperties;


public abstract class LoaderData 
{
    protected int Count;
    protected Dictionary<string, int> DictionaryID = new();
    protected ImageData[] FilesImage;
    protected SpriteSheetData[] FilesSpriteSheet;
    protected virtual int GetID<TData> (string filename, TData data) 
    {
        int id = 0;
        if(DictionaryID.ContainsKey(filename))
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
            if(fileInfo is {Exists: true})
            {
                LoadImageFile(filename,fileInfo, data);
            }    
        }
        return id;   
    }
    protected virtual void LoadImageFile<TData>(string filename, FileInfo fileInfo, TData data)
    {
        Count++;
        Debug.Log($"file found, adding {fileInfo.FullName} into dictionary");
        DictionaryID.Add(filename, Count);
        
        switch (data)
        {
            case ImageData:
                Array.Resize(ref FilesImage, Count);
                FilesImage[Count-1] = AssignPNGDatas(fileInfo.FullName, Count);
                break;
            case SpriteSheetData:
                Array.Resize(ref FilesSpriteSheet, Count);
                Array.Resize(ref TilePropertiesManager.Instance.TileProperties, Count);
                FilesSpriteSheet[Count-1] = AssignSpriteSheetDatas(fileInfo.FullName, Count);
                break;
        }
    }

    public void ImageArray<Data>(Data data)
    {
        if(data is ImageData)
        {
            Array.Resize(ref FilesImage, Count);
        }
        if(data is SpriteSheetData)
        {
            Array.Resize(ref FilesSpriteSheet, Count);
        }
    }
    
    public virtual ImageData AssignPNGDatas (string filename, int id) => new();
    public virtual SpriteSheetData AssignSpriteSheetDatas (string filename, int id) => new();   

}
