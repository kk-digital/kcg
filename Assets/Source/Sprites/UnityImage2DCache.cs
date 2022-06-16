using UnityEngine;
using KMath;
using System.Collections.Generic;

namespace Sprites
{




    public class UnityImage2DCache
    {

        public static int LayerCount = 5;

        public Dictionary < int, Texture2D > [] Dictionary;


        public UnityImage2DCache()
        {
            Dictionary = new Dictionary < int, Texture2D > [LayerCount];

            for(int i = 0; i < LayerCount; i++)
            {
                Dictionary[i] = new Dictionary < int, Texture2D > ();
            }
        }


        public Texture2D Get(int spriteId, Enums.AtlasType type)
        {
            if (!(Dictionary[(int)type].ContainsKey(spriteId)))
            {
                Vec2i dimensions = Game.State.SpriteAtlasManager.GetSpriteDimensions(spriteId, type);

                byte[] spriteData = new byte[dimensions.X * dimensions.Y * 4];
                Game.State.SpriteAtlasManager.GetSpriteBytes(spriteId, spriteData, type);
                var texture = Utility.Texture.CreateTextureFromRGBA(spriteData, dimensions.X, dimensions.Y);

                Dictionary[(int)type][spriteId] = texture;
            }

            return Dictionary[(int)type][spriteId];
        }

    }
}