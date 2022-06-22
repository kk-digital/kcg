using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath.PerlinNoise;
using System.IO;

namespace Planet.VisualEffects
{
    public class PlanetBackgroundStarField
    {
        // Init Condition
        private bool Init;

        // Perlin Field
        private PerlinField2D perlinField;

        // Stars List
        private List<Sprites.Sprite> stars;

        // Star Parallax Depth
        private List<float> starParallaxDepth;

        // For Loop
        int i = 0;

        // Generated Perlin Grid
        float[,] perlinGrid;

        public void Initialize()
        {
            // Create Stars List
            stars = new List<Sprites.Sprite>();

            // Set Width and Height
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

            // Create the sprite
            var star1Sprite = new Sprites.Sprite
            {
                Texture = star1Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to stars list
            stars.Add(star1Sprite);

            // Set Width and Height
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

            // Create the sprite
            var star2Sprite = new Sprites.Sprite
            {
                Texture = star2Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to stars list
            stars.Add(star2Sprite);

            // Set Width and Height
            int star3Width = 8;
            int star3Height = 8;
            Vector2Int star3PngSize = new Vector2Int(star3Width, star3Height);

            // Load image from fil
            var star3Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\Starsheet2.png", star3Width, star3Height);

            // Set Sprite ID from Sprite Atlas
            int star3ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star3Sheet, 0, 2, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] star3spriteData = new byte[star3PngSize.x * star3PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(star3ID, star3spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D star3Tex = Utility.Texture.CreateTextureFromRGBA(star3spriteData, star3PngSize.x, star3PngSize.y);

            // Create the sprite
            var star3Sprite = new Sprites.Sprite
            {
                Texture = star3Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to stars list
            stars.Add(star3Sprite);

            // Set Width and Height
            int star4Width = 8;
            int star4Height = 8;
            Vector2Int star4PngSize = new Vector2Int(star4Width, star4Height);

            // Load image from fil
            var star4Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\Starsheet2.png", star4Width, star4Height);

            // Set Sprite ID from Sprite Atlas
            int star4ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star4Sheet, 2, 2, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] star4spriteData = new byte[star4PngSize.x * star4PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(star4ID, star4spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D star4Tex = Utility.Texture.CreateTextureFromRGBA(star4spriteData, star4PngSize.x, star4PngSize.y);

            // Create the sprite
            var star4Sprite = new Sprites.Sprite
            {
                Texture = star4Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to stars list
            stars.Add(star4Sprite);

            // Set Width and Height
            int star5Width = 8;
            int star5Height = 8;
            Vector2Int star5PngSize = new Vector2Int(star5Width, star5Height);

            // Load image from fil
            var star5Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\Starsheet2.png", star5Width, star5Height);

            // Set Sprite ID from Sprite Atlas
            int star5ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star5Sheet, 3, 2, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] star5spriteData = new byte[star5PngSize.x * star5PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(star5ID, star5spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D star5Tex = Utility.Texture.CreateTextureFromRGBA(star5spriteData, star5PngSize.x, star5PngSize.y);

            // Create the sprite
            var star5Sprite = new Sprites.Sprite
            {
                Texture = star5Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to stars list
            stars.Add(star5Sprite);

            // Create Perlin Field
            perlinField = new PerlinField2D();

            // Initialzie Perlin Field
            perlinField.init(256, 256);

            // Create Parallax Depth of Stars
            starParallaxDepth = new List<float>(stars.Count);

            // Set Parallax Value of every star
            starParallaxDepth.Add(0.15f * Time.deltaTime);
            starParallaxDepth.Add(0.10f * Time.deltaTime);
            starParallaxDepth.Add(0.05f * Time.deltaTime);
            starParallaxDepth.Add(0.20f * Time.deltaTime);
            starParallaxDepth.Add(0.30f * Time.deltaTime);

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
                    int spriteRandom = Random.Range(0, stars.Count);
                    Sprites.Sprite sprite = new Sprites.Sprite();
                    float rand1 = perlinGrid[random, random];
                    
                    switch(spriteRandom)
                    {
                        case 0:
                            sprite = stars[0];
                            break;
                        case 1:
                            sprite = stars[1];
                            break;
                        case 2:
                            sprite = stars[2];
                            break;
                        case 3:
                            sprite = stars[3];
                            break;
                        case 4:
                            sprite = stars[4];
                            break;
                    }

                    if (rand1 >= .5)
                        Utility.Render.DrawSprite(Random.Range(-10, 10), Random.Range(-10, 10), 0.5f, 0.5f, sprite, Material, transform, 0);
                    else
                        Utility.Render.DrawSprite(Random.Range(-100, 100), Random.Range(-100, 100), 0.5f, 0.5f, sprite, Material, transform, 0);

                    
                    transform.GetChild(i).gameObject.AddComponent<Parallax>();
                    transform.GetChild(i).gameObject.GetComponent<Parallax>().parallaxEffect += starParallaxDepth[spriteRandom];
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
