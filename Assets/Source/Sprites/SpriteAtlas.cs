using UnityEngine;

namespace Sprites
{
    public struct SpriteAtlas
    {
        public int AtlasID;
        public int GLTextureID;

        public int Width;
        public int Height;

        public byte[] Data;
        public Texture2D Texture;
        public bool TextureNeedsUpdate;
        public RectpackSharp.PackingRectangle[] Rectangles;


        public void UpdateTexture()
        {
            if (TextureNeedsUpdate)
            {
                TextureNeedsUpdate = false;

                 Texture = Utility.Texture.CreateTextureFromRGBA(Data, Width, Height);
            }
        }
    }
}
