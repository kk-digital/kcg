using UnityEngine;

namespace Sprites
{
    public struct Atlas
    {
        public int AtlasID;
        public int GLTextureID;

        public int Width;
        public int Height;

        public byte[] Data;
        public Texture2D Texture;
        public RectpackSharp.PackingRectangle[] Rectangles;
    }
}
