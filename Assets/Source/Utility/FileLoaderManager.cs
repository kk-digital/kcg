using System.Collections.Generic;
using System;
using System.IO;

namespace Utility
{
    public class FileData
    {
        public int fileId { get; set; }
        public string fileName { get; set; }
        public int reloadCount { get; set; }
        public int deleteCount { get; set; }
        public int refCount { get; set; }
        public int lastUsed { get; set; }
        public DateTime creationDate { get; set; }
        public bool loaded { get; set; }

        public byte[] data { get; set; }

    }

    public class FileLoadingManager
    {

        private List<FileData> fileDataArray = new List<FileData>();

        public int count { get; set; }
        public Dictionary<string, int> DictionaryID { get; set; }

        //TODO(mahdi): add logic to unload data automatically
        // when they are unused




        //TODO(mahdi): when the files DateTime changes. add logic
        // so that we reload the data from disk to ram




        //TODO(mahdi): an improvement to this is to make
        // a list of indices that not used so we dont have to do a lookup
        // every time we insert an element
        // we just push and pop the list
        public int GetSmallestFreeID()
        {
            int result = -1;

            for (int index = 0; index < fileDataArray.Count; index++)
            {
                FileData thisFileData = fileDataArray[index];

                if (!thisFileData.loaded)
                {
                    result = index;
                    break;
                }
            }

            return result;
        }

        public int Load(string filePath)
        {
            int id = -1;
            byte[] byteArray = File.ReadAllBytes(filePath);


            if (byteArray != null && byteArray.Length > 0)
            {

                FileInfo fileInfo = new FileInfo(filePath);
                FileData fileData = new FileData();
                fileData.data = byteArray;
                fileData.fileName = filePath;
                fileData.creationDate = fileInfo.CreationTime;
                fileData.loaded = true;

                // look for an empty spot on the array
                id = GetSmallestFreeID();

                // if there is no empty spot just make one
                if (id == -1)
                {
                    fileDataArray.Add(fileData);
                    id = fileDataArray.Count - 1;
                }
                else
                {
                    fileDataArray[id] = fileData;
                }

                // add the filepath/id in the dictionary
                DictionaryID.Add(filePath, id);

                fileData.fileId = id;
                fileData.reloadCount = 0;
                fileData.deleteCount = 0;
            }

            return id;
        }

        public void Unload(int id)
        {
            if (id < fileDataArray.Count)
            {
                FileData fileData = fileDataArray[id];

                if (!fileData.loaded)
                {
                    if (fileData.data != null)
                    {
                        // if the data is set to null the garbage collector
                        // will delete it for us (c#)
                        fileData.data = null;
                        fileData.loaded = false;

                        // remove the fileData name from the dictionary
                        DictionaryID.Remove(fileData.fileName);
                    }
                }
                else
                {
                    // it was already unloaded
                }
            }
        }

        public bool Unload(string name)
        {
            bool found = false;

            int value;
            bool exists = DictionaryID.TryGetValue(name, out value);
            if (exists)
            {
                Unload(value);
                found = true;
            }

            return found;
        }

        public FileData Get(int id)
        {
            FileData result = null;

            if (id < fileDataArray.Count)
            {
                FileData fileData = fileDataArray[id];

                if (fileData.loaded)
                {
                    // the fileData is ready to use
                    result = fileData;
                }
                else
                {
                    // id was found but the fileData was not loaded
                    // or was previously unloaded
                }
            }

            return result;
        }

        // get FileData by name #not efficient
        public FileData Get(string name)
        {
            int value;
            bool exists = DictionaryID.TryGetValue(name, out value);
            if (exists)
            {
                return Get(value);
            }


            return null;
        }
    }
}
