using UnityEngine;

namespace Sprites
{
    public struct Model
    {
        public Texture2D Texture;
        public Vector4 TextureCoords;

        public Model(Texture2D texture)
        {
            Texture = texture;
            TextureCoords = new Vector4(0, 0, 1, -1);
        }
    }
}