using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planet.VisualEffects
{
    public class PlanetBackgroundParallaxLayer
    {
        // Stored Properties
        private bool Init;
        private int i = 0;

        // Earth
        private Texture2D earthTex;
        private Sprites.Sprite earth;
        private Vector2Int pngSize;

        public void Initialize()
        {
            int width = 208;
            int height = 160;
            pngSize = new Vector2Int(width, height);

            // Load image from fil
            var spriteSheetID = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\planets_background\\earth_fx.png", width, height);

            // Set Sprite ID from Sprite Atlas
            int spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] spriteData = new byte[pngSize.x * pngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(spriteId, spriteData, Enums.AtlasType.Particle);

            // Set Texture
            earthTex = Utility.Texture.CreateTextureFromRGBA(spriteData, pngSize.x, pngSize.y);

            earth = new Sprites.Sprite
            {
                Texture = earthTex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };

            Init = true;
        }

        public void Draw(Material Material, Transform transform, int drawOrder)
        {
            if(Init)
            {
                for (; i < 5; i++)
                {
                    Utility.Render.DrawSprite(0, 0, 10, 10, earth, Material, transform, drawOrder);
                }
            }
        }
    }
}

