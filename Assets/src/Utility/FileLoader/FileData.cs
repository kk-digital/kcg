using System;

namespace Assets.src.Utility.FileLoader
{
    public struct FileData
    {
        public int FileId;
        public string Filename; // Filename and path string
        public Int64 Hash; // 64 bit xxHash of image file
        public int FileCreationTime; // time of file modification
        public int FileSize; //int, size on disc
        public int UsageCount; //incremented by consumer applications
        public int ReloadCount; //how many times file was reloaded
        public int DeleteCount; //how many times file was deleted
        public int RefCount; //just ref count
        public int LastUsed; //unix seconds time from program startup
        public int CreationDate; //unix seconds time since program creation to first load
        public int Loaded; //true/false for whether data is loaded into memory
        public byte[] ByteArray;
    }
}
