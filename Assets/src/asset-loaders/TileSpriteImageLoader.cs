//TODO: Dont import Unity
using BigGustave;
using System.Collections.Generic;

namespace ImageLoader
{
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

        public override ImageData AssignPNGDatas(string filename, int id)
        {
            var png = Png.Open(filename);
            var xSize = png.Header.Width;
            var ySize = png.Header.Height;
            byte[] pixelsArray = new byte[4 * xSize * ySize];
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    Pixel getPixels = png.GetPixel(x, y);
                    int index = y * xSize + x;
                    pixelsArray[4 * index + 0] = getPixels.R;
                    pixelsArray[4 * index + 1] = getPixels.G;
                    pixelsArray[4 * index + 2] = getPixels.B;
                    pixelsArray[4 * index + 3] = getPixels.A;
                }
            }

            return new ImageData(id, xSize, ySize, pixelsArray);
        }
    }
}


