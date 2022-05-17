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
    public  int count;
    public Dictionary<string, int> DictionaryID = new Dictionary<string, int>();
    public ImageData[] FilesImage;
    public SpriteSheet[] FilesSpriteSheet;
    public virtual int GetID (string filename) 
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
                LoadImageFile(filename,fileInfo);
            }    
        }
        return id;   
    }
    public virtual void LoadImageFile(string filename, FileInfo fileInfo)
    {
        count++;
        Debug.Log($"file found, adding {fileInfo.FullName} into dictionary");
        DictionaryID.Add(filename, count);
        if(FilesImage != null)
        {
            Array.Resize(ref FilesImage, count);
            FilesImage[count-1] = AssignPNGDatas(fileInfo.FullName,count);
        }
        if(FilesSpriteSheet != null)
        {
            Array.Resize(ref FilesSpriteSheet, count);
            FilesSpriteSheet[count-1] = AssignSpriteSheetDatas(fileInfo.FullName,count);
        }
    }
    public virtual ImageData AssignPNGDatas (string filename, int id) => new ImageData();
    public virtual SpriteSheet AssignSpriteSheetDatas (string filename, int id) => new SpriteSheet();
       

}
