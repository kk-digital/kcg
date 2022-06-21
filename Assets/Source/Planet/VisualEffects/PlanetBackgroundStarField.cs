using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath.PerlinNoise;
using System.IO;

namespace Planet.VisualEffects
{
    public class PlanetBackgroundStarField
    {
        // Stored Properties
        private bool Init;
        private PerlinField2D perlinField;
        private List<Sprites.Sprite> stars;
        int i = 0;
        float[,] perlinGrid;

        public void Initialize()
        {
            stars = new List<Sprites.Sprite>();

            int star1Width = 8;
            int star1Height = 8;
            Vector2Int star1PngSize = new Vector2Int(star1Width, star1Height);

            // Load image from fil
            var star1Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\Starsheet2.png", star1Width, star1Height);

            // Set Sprite ID from Sprite Atlas
            int star1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 0, Enums.AtlasType.Particle);
            star1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 1, Enums.AtlasType.Particle);
            star1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 2, Enums.AtlasType.Particle);
            star1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 3, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] star1spriteData = new byte[star1PngSize.x * star1PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(star1ID, star1spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D star1Tex = Utility.Texture.CreateTextureFromRGBA(star1spriteData, star1PngSize.x, star1PngSize.y);

            var star1Sprite = new Sprites.Sprite
            {
                Texture = star1Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            stars.Add(star1Sprite);

            int star2Width = 8;
            int star2Height = 8;
            Vector2Int star2PngSize = new Vector2Int(star2Width, star2Height);

            // Load image from fil
            var star2Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\Starsheet2.png", star2Width, star2Height);

            // Set Sprite ID from Sprite Atlas
            int star2ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star2Sheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] star2spriteData = new byte[star2PngSize.x * star2PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(star2ID, star2spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D star2Tex = Utility.Texture.CreateTextureFromRGBA(star2spriteData, star2PngSize.x, star2PngSize.y);

            var star2Sprite = new Sprites.Sprite
            {
                Texture = star2Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            stars.Add(star2Sprite);

            perlinField = new PerlinField2D();
            perlinField.init(256, 256);

            Init = true;
        }

        public void Draw(Material Material, Transform transform, int drawOrder)
        {
            if (Init)
            {
                for (; i < 20; i++)
                {
                    perlinGrid = GenPerlin(256, 256, 2, 20);

                    int random = Random.Range(0, 256);
                    int spriteRandom = Random.Range(0, 2);
                    Sprites.Sprite sprite = new Sprites.Sprite();
                    float rand1 = perlinGrid[random, random];
                    Debug.Log(spriteRandom);
                    switch(spriteRandom)
                    {
                        case 0:
                            sprite = stars[0];
                            if (rand1 >= .5)
                                Utility.Render.DrawSprite(Random.Range(-10, 10), Random.Range(-10, 10), 1, 1, sprite, Material, transform, drawOrder);
                            else
                                Utility.Render.DrawSprite(Random.Range(-100, 100), Random.Range(-100, 100), 1, 1, sprite, Material, transform, drawOrder);
                            break;
                        case 1:
                            sprite = stars[1];
                            if (rand1 >= .5)
                                Utility.Render.DrawSprite(Random.Range(-10, 10), Random.Range(-10, 10), 1, 1, sprite, Material, transform, drawOrder);
                            else
                                Utility.Render.DrawSprite(Random.Range(-100, 100), Random.Range(-100, 100), 1, 1, sprite, Material, transform, drawOrder);
                            break;
                    }

                }
            }
        }

        private float[,] GenPerlin(int width, int height, int contrast, int scale)
        {
            float[,] grid = new float[width, height];
            var max = 1.414f / (1.9f * contrast);
            var min = -1.414f / (1.9f * contrast);
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var result = perlinField.noise(x * scale, y * scale);
                    result = Mathf.Clamp(result - min / (max - min), 0.0f, 1.0f);
                    grid[x, y] = result;
                }
            }

            return grid;
        }
    }   
}
