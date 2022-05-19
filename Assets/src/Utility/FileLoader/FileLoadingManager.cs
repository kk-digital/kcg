using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.Utility.FileLoader
{
    public struct FileLoadingManager
    {
        /// <summary>
        /// A map of FileId to Filename
        /// </summary>
        public Dictionary<int, string> Mappings;

        public FileData[] FileProperties;

        /// <summary>
        /// Unload file
        /// </summary>
        public void Unload(int fileId)
        {
            if (Mappings.ContainsKey(fileId)) Mappings.Remove(fileId);

            FileProperties = FileProperties.Where(x => x.FileId != fileId).ToArray();
        }

        public int GetFileId(string filename) => Mappings.FirstOrDefault(x => x.Value == filename).Key;

        public FileData GetFileData(int fileId) => FileProperties.FirstOrDefault(x => x.FileId == fileId);
    }
}
