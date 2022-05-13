using UnityEngine;

namespace PlanetTileMap.Unity
{
    static class TextureBuilder
    {
        public static Texture2D Build(int[,] rgba)
        {
            var w = rgba.GetLength(0);
            var h = rgba.GetLength(1);

            var res = new Texture2D(w, h, TextureFormat.RGBA32, false);
            res.filterMode = FilterMode.Point;

            var pixels = new Color32[w * h];
            for (int x = 0 ; x < w; x++)
            for (int y = 0 ; y < h; y++)
            { 
                var p = rgba[x, h - y - 1];
                pixels[x + y * w] = new Color32((byte)((p >> 24) & 0xff), (byte)((p >> 16) & 0xff), (byte)((p >> 8) & 0xff), (byte)(p & 0xff));
            }
            res.SetPixels32(pixels);
            res.Apply();

            return res;
        }
    }
}