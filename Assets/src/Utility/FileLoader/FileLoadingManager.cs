using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BigGustave;
using System.IO;
namespace ImageLoader
{
    public class FileData
    {
        public int fileId {get; set;}
        public string fileName{get; set;} 
        public int reloadCount{get; set;}
        public int deleteCount{get; set;}
        public int refCount{get; set;}
        public int lastUsed{get; set;}
        public DateTime creationDate{get; set;}
        public bool loaded{get; set;}

        public byte[] data{get; set;}

    }

    public class FileLoadingManager
    {
        public static FileLoadingManager _instance;

     public FileLoadingManager GetSingelton()
    {
        if (_instance == null)
        {
            _instance = new FileLoadingManager();
        }


        return _instance;
    }

        private List<FileData> fileDataArray = new List<FileData>();

        public int count {get; set;}
        public Dictionary<string, int> DictionaryID {get; set;}

        public int load(string filePath)
        {
            int id = -1;
            byte[] byteArray = File.ReadAllBytes( filePath );

            if (byteArray != null && byteArray.Length > 0)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FileData fileData = new FileData();
                fileData.data = byteArray;
                fileData.creationDate = fileInfo.CreationTime;
                fileData.loaded = true;

                fileDataArray.Add(fileData);
                id = fileDataArray.Count - 1;

                fileData.fileId = id;
                fileData.reloadCount = 0;
                fileData.deleteCount = 0;
            }

            return id;
        }

        public FileData get(int id)
        {
            if (id < fileDataArray.Count)
            {
                return fileDataArray[id];
            }

            return null;
        }

        public FileData get(string name)
        {
            int value;
            bool exists = DictionaryID.TryGetValue(name, out value);
            if (exists)
            {
                return get(value);
            }


            return null;        

        }

    }

}
