using BigGustave;
using UnityEngine;

namespace ImageLoader 
{
   public struct ImageData 
   {
        //TODO: Add in image format enum, or ImageFormatType, RGBA, HDR16 System.IO.Directory.GetFiles()
        public int ImageID;
        public Vector2Int Size;
        public byte[] PixelsArray;
        
        public ImageData(string filename, int imageID) : this()
        {
            var png = Png.Open(filename);

            ImageID = imageID;
            Size.x = png.Header.Width;
            Size.y = png.Header.Height;
            CreatePixelsArray(png);
        }
        
        /// <summary>
        /// Creating byte array from PNG
        /// </summary>
        public void CreatePixelsArray(Png png)
        {
            PixelsArray = new byte[4 * Size.x * Size.y];
            
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    var getPixels = png.GetPixel(x, y);
                    int index = y * Size.x + x;
                    PixelsArray[4 * index + 0] = getPixels.R;
                    PixelsArray[4 * index + 1] = getPixels.G;
                    PixelsArray[4 * index + 2] = getPixels.B;
                    PixelsArray[4 * index + 3] = getPixels.A;
                }
            }
        }
        
        /// <summary>
        /// Getting RGBA color bytes from index
        /// </summary>
        public Color32 GetColor(int index)
        {
            var r = PixelsArray[4 * index + 0];
            var g = PixelsArray[4 * index + 1];
            var b = PixelsArray[4 * index + 2]; 
            var a = PixelsArray[4 * index + 3];

            return new Color32(r, g, b, a);
        }
    }
}


