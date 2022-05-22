//TODO: Dont import Unity
using BigGustave;
using System.Collections.Generic;

namespace ImageLoader
{
    //TODO: Do not inherit LoaderData
    public class TileSpriteImageLoaderManager : LoaderData
    {
        public static TileSpriteImageLoaderManager Instance;
        public ImageData[] PNGFile {get => FilesImage; set => FilesImage = value;}
        public ImageData ImageData;
        public int ImageCount {get => Count; set => Count = value;}
        public Dictionary<string, int> DictionaryPNGID {get => DictionaryID; set => DictionaryID = value;}
        public delegate int DGetImageID<TImageData>(string filename, TImageData data);
        public DGetImageID<ImageData> GetImageID;
        public TileSpriteImageLoaderManager()
        {
            GetImageID = base.GetID;
            Instance = this;
        }
        public static void InitStage1()
        {
            Instance = new TileSpriteImageLoaderManager();
        }
        public static void InitStage2()
        {
            
        }
        public override ImageData AssignPNGDatas(string filename, int id)
        {
            var pngData = Png.Open(filename);
            return new ImageData(pngData, id);
        }
    }
}


