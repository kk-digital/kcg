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
        private Texture2D tex;
        private bool Init;
        private Vector2Int pngSize;
        private PerlinField2D perlinField;
        private List<Sprites.Sprite> stars;
        int i = 0;
        float[,] perlinGrid;

        public void Initialize()
        {
            int width = 8;
            int height = 8;
            pngSize = new Vector2Int(width, height);

            stars = new List<Sprites.Sprite>();

            // Load image from fil
            var spriteSheetID = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\Starsheet2.png", width, height);

            // Set Sprite ID from Sprite Atlas
            int spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 0, Enums.AtlasType.Particle);
            spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 1, Enums.AtlasType.Particle);
            spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 2, Enums.AtlasType.Particle);
            spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteSheetID, 0, 3, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] spriteData = new byte[pngSize.x * pngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(spriteId, spriteData, Enums.AtlasType.Particle);

            // Set Texture
            tex = Utility.Texture.CreateTextureFromRGBA(spriteData, pngSize.x, pngSize.y);

            perlinField = new PerlinField2D();
            perlinField.init(256, 256);

            Init = true;
        }

        public void Draw(Material Material, Transform transform, int drawOrder)
        {
            if (Init)
            {
                var sprite = new Sprites.Sprite
                {
                    Texture = tex,
                    TextureCoords = new Vector4(0, 0, 1, 1)
                };

                for (; i < 20; i++)
                {
                    stars.Add(sprite);

                    perlinGrid = GenPerlin(256, 256, 2, 20);

                    int random = Random.Range(0, 256);

                    float rand1 = perlinGrid[random, random];

                    if (rand1 >= .5)
                        Utility.Render.DrawSprite(Random.Range(-10, 10), Random.Range(-10, 10), 1, 1, sprite, Material, transform, drawOrder);
                    else
                        Utility.Render.DrawSprite(Random.Range(-100, 100), Random.Range(-100, 100), 1, 1, sprite, Material, transform, drawOrder);

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

        private Texture2D CreateTextureFromRGBA(byte[] rgba, int w, int h)
        {
            var res = new Texture2D(w, h, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };

            var pixels = new Color32[w * h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int index = (x + y * w) * 4;
                    var r = rgba[index];
                    var g = rgba[index + 1];
                    var b = rgba[index + 2];
                    var a = rgba[index + 3];

                    pixels[x + y * w] = new Color32(r, g, b, a);
                }
            }

            res.SetPixels32(pixels);
            res.Apply();

            return res;
        }
    }   
}
