using UnityEngine;

namespace Sprites
{
    public struct Sprite
    {
        public Texture2D Texture;
        public Vector4 TextureCoords;

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            TextureCoords = new Vector4(0, 0, 1, -1);
        }
    }
}