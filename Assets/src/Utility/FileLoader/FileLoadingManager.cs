using System.Collections.Generic;
using System.Linq;

namespace Assets.src.Utility.FileLoader
{
    public struct FileLoadingManager
    {
        /// <summary>
        /// A map of FileId to Filename
        /// </summary>
        Dictionary<int, string> Mappings;

        FileData[] FileProperties;

        IntegerIdGenerator integerIdGenerator;

        void InitCheck()
        {
            if (Mappings == null) Mappings = new Dictionary<int, string>();
            if (FileProperties == null) FileProperties = new FileData[] { };
            if (integerIdGenerator == null) integerIdGenerator = new IntegerIdGenerator();
        }

        public void Load(FileData fileData)
        {
            InitCheck();

            fileData.FileId = integerIdGenerator.NewId();

            FileProperties = FileProperties.Append(fileData).ToArray();
            Mappings.Add(fileData.FileId, fileData.Filename);
        }

        public void Unload(int fileId)
        {
            if (Mappings.ContainsKey(fileId)) Mappings.Remove(fileId);

            FileProperties = FileProperties.Where(x => x.FileId != fileId).ToArray();
        }

        public int GetFileId(string filename) => Mappings.FirstOrDefault(x => x.Value == filename).Key;

        public FileData GetFileData(int fileId)
        {
            return FileProperties.FirstOrDefault(x => x.FileId == fileId);
        }
    }
}
