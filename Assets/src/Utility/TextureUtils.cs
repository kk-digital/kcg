using UnityEngine;

namespace Utils
{
       // we use this helper function to generate a unity Texture2D
        // from pixels
        internal static class TextureUtils
        {
            public static Texture2D CreateTextureFromRGBA(byte[] rgba, int w, int h)
            {

                var res = new Texture2D(w, h, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point
                };

                var pixels = new Color32[w * h];
                for (int x = 0 ; x < w; x++)
                for (int y = 0 ; y < h; y++)
                { 
                    int index = (x + y * w) * 4;
                    var r = rgba[index];
                    var g = rgba[index + 1];
                    var b = rgba[index + 2];
                    var a = rgba[index + 3];

                    pixels[x + y * w] = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
                }
                res.SetPixels32(pixels);
                res.Apply();

                return res;
            }
        }
}