using System.Collections.Generic;

namespace ImageLoader
{
    public class SpriteSheetImageLoader : LoaderData
    {
        public static SpriteSheetImageLoader Instance;

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

        public delegate int DGetSpriteSheetID<TSpriteSheetData>(string filename, TSpriteSheetData data);

        public DGetSpriteSheetID<SpriteSheetData> GetSpriteSheetID;

        public SpriteSheetImageLoader()
        {
            GetSpriteSheetID = base.GetID;
            Instance = this;
        }

        public override SpriteSheetData AssignSpriteSheetDatas(string filename, int id)
        {
            const int accessCounter = 0;
            const int loaded = 0;
            const int spriteSheetType = 0;
            const int pixelFormat = 0;
            const int hash = 0;
            
            var imageCount = ++TileSpriteImageLoaderManager.Instance.ImageCount;
            
            TileSpriteImageLoaderManager.Instance.ImageArray(ImageTest.imageData);
            TileSpriteImageLoaderManager.Instance.DictionaryPNGID.Add($"{filename}_{imageCount}", imageCount);

            return new SpriteSheetData(filename, id, spriteSheetType, loaded, accessCounter, pixelFormat, hash);
        }
    }
}
