using BigGustave;

namespace ImageLoader 
{
    public struct ImageData 
   {
        //TODO: Add in image format enum, or ImageFormatType, RGBA, HDR16 System.IO.Directory.GetFiles()
        public int ImageID;
        public int xSize;
        public int ySize;
        public Pixel[] PixelsArray;

        public ImageData(int ImageID, int xSize, int ySize, Pixel[] pixelsArray)
        {
            this.ImageID = ImageID;
            this.xSize = xSize;
            this.ySize = ySize;
            
            var pixelLength = pixelsArray.Length;
            PixelsArray = new Pixel[pixelLength];
            for(int i = 0; i < pixelLength; i++)
            {
                PixelsArray[i] = pixelsArray[i];   
            }
        } 
   }
}

