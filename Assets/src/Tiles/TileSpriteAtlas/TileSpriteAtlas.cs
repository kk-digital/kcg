using UnityEngine;

namespace TileSpriteAtlas
{
    public struct TileSpriteAtlas
    {
        public int AtlasID;
        public int GLTextureID;

        public int Width;
        public int Height;

        public byte[] Data;
        public Texture2D Texture;

        public void CreateData()
        {
            
        }
    }
}
