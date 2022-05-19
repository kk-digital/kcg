using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageLoader;
using System.IO;
using System.Linq;
using System;
using BigGustave;
public abstract class LoaderData 
{
    protected int count;
    protected Dictionary<string, int> DictionaryID = new Dictionary<string, int>();
    protected ImageData[] FilesImage;
    protected SpriteSheetData[] FilesSpriteSheet;
    protected virtual int GetID<Data> (string filename, Data data) 
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
            if(fileInfo.Exists)
            {
                LoadImageFile(filename,fileInfo, data);
            }    
        }
        return id;   
    }
    protected virtual void LoadImageFile<Data>(string filename, FileInfo fileInfo, Data data)
    {
        count++;
        Debug.Log($"file found, adding {fileInfo.FullName} into dictionary");
        DictionaryID.Add(filename, count);
        if(data is ImageData)
        {
            ImageArray(data);
            FilesImage[count-1] = AssignPNGDatas(fileInfo.FullName,count);
        }
        if(data is SpriteSheetData)
        {
            ImageArray(data);
            FilesSpriteSheet[count-1] = AssignSpriteSheetDatas(fileInfo.FullName,count);
        }
    }
    public void ImageArray<Data>(Data data)
    {
        if(data is ImageData)
        {
            Array.Resize(ref FilesImage, count);
        }
        if(data is SpriteSheetData)
        {
            Array.Resize(ref FilesSpriteSheet, count);
        }
    }
    public virtual ImageData AssignPNGDatas (string filename, int id) => new ImageData();
    public virtual SpriteSheetData AssignSpriteSheetDatas (string filename, int id) => new SpriteSheetData();
       

}
