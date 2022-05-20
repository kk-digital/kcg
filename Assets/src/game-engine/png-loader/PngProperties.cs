using BigGustave;
using UnityEngine;

namespace ImageLoader 
{
   public struct ImageData 
   {
        //TODO: Add in image format enum, or ImageFormatType, RGBA, HDR16 System.IO.Directory.GetFiles()
        public int ImageID;
        public Vector2Int Size;
        public Png Data;
        
        public ImageData(Png data, int imageID) : this()
        {
            Data = data;
            ImageID = imageID;
            Size.x = Data.Header.Width;
            Size.y = Data.Header.Height;
        }
        
        public Color32 GetPixelColor(int x, int y)
        {
            var pixel = Data.GetPixel(x, y);
            var r = pixel.R;
            var g = pixel.G;
            var b = pixel.B;
            var a = pixel.A;

            return new Color32(r, g, b, a);
        }
    }
}


