using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath.PerlinNoise;

namespace Planet.VisualEffects
{
    public class PlanetBackgroundParallaxLayer
    {
        // Init Condition
        private bool Init;

        // Perlin Field
        private PerlinField2D perlinField;

        // Planets List
        private List<Sprites.Sprite> planets;

        // Planet Parallax Depth
        private List<float> planetParallaxDepth;

        // For Loop
        int i = 0;

        // Generated Perlin Grid
        float[,] perlinGrid;

        // Space Sprite
        Sprites.Sprite spaceSprite;

        // Space Loop
        int j = 0;

        public void Initialize()
        {
            planets = new List<Sprites.Sprite>();

            // Set Width and Height
            int planet1Width = 32;
            int planet1Height = 32;
            Vector2Int planet1PngSize = new Vector2Int(planet1Width, planet1Height);
            
            // Load image from file
            var planet1Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\galaxy_256x256.png", planet1Width, planet1Height);

            // Set Sprite ID from Sprite Atlas
            int planet1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet1Sheet, 0, 5, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet1spriteData = new byte[planet1PngSize.x * planet1PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet1ID, planet1spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet1Tex = Utility.Texture.CreateTextureFromRGBA(planet1spriteData, planet1PngSize.x, planet1PngSize.y);

            // Create the sprite
            var planet1Sprite = new Sprites.Sprite
            {
                Texture = planet1Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet1Sprite);

            // Set Width and Height
            int planet2Width = 32;
            int planet2Height = 32;
            Vector2Int planet2PngSize = new Vector2Int(planet2Width, planet2Height);

            // Load image from file
            var planet2Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\galaxy_256x256.png", planet2Width, planet2Height);

            // Set Sprite ID from Sprite Atlas
            int planet2ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet2Sheet, 0, 6, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet2spriteData = new byte[planet2PngSize.x * planet2PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet2ID, planet2spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet2Tex = Utility.Texture.CreateTextureFromRGBA(planet2spriteData, planet2PngSize.x, planet2PngSize.y);

            // Create the sprite
            var planet2Sprite = new Sprites.Sprite
            {
                Texture = planet2Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet2Sprite);

            // Set Width and Height
            int planet3Width = 32;
            int planet3Height = 32;
            Vector2Int planet3PngSize = new Vector2Int(planet3Width, planet3Height);

            // Load image from file
            var planet3Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\galaxy_256x256.png", planet3Width, planet3Height);

            // Set Sprite ID from Sprite Atlas
            int planet3ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet3Sheet, 7, 4, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet3spriteData = new byte[planet3PngSize.x * planet3PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet3ID, planet3spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet3Tex = Utility.Texture.CreateTextureFromRGBA(planet3spriteData, planet3PngSize.x, planet3PngSize.y);

            // Create the sprite
            var planet3Sprite = new Sprites.Sprite
            {
                Texture = planet3Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet3Sprite);

            // Set Width and Height
            int planet4Width = 32;
            int planet4Height = 32;
            Vector2Int planet4PngSize = new Vector2Int(planet4Width, planet4Height);

            // Load image from file
            var planet4Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\galaxy_256x256.png", planet4Width, planet4Height);

            // Set Sprite ID from Sprite Atlas
            int planet4ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet4Sheet, 4, 5, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet4spriteData = new byte[planet4PngSize.x * planet4PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet4ID, planet4spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet4Tex = Utility.Texture.CreateTextureFromRGBA(planet4spriteData, planet4PngSize.x, planet4PngSize.y);

            // Create the sprite
            var planet4Sprite = new Sprites.Sprite
            {
                Texture = planet4Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet4Sprite);

            // Set Width and Height
            int planet5Width = 32;
            int planet5Height = 32;
            Vector2Int planet5PngSize = new Vector2Int(planet5Width, planet5Height);

            // Load image from file
            var planet5Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\galaxy_256x256.png", planet5Width, planet5Height);

            // Set Sprite ID from Sprite Atlas
            int planet5ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet5Sheet, 7, 5, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet5spriteData = new byte[planet5PngSize.x * planet5PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet5ID, planet5spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet5Tex = Utility.Texture.CreateTextureFromRGBA(planet5spriteData, planet5PngSize.x, planet5PngSize.y);

            // Create the sprite
            var planet5Sprite = new Sprites.Sprite
            {
                Texture = planet5Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet5Sprite);

            // Create Perlin Field
            perlinField = new PerlinField2D();

            // Initialzie Perlin Field
            perlinField.init(256, 256);

            // Create Parallax Depth of Stars
            planetParallaxDepth = new List<float>(planets.Count);

            // Set Parallax Value of every star
            planetParallaxDepth.Add(0.01f * Time.deltaTime);
            planetParallaxDepth.Add(0.05f * Time.deltaTime);
            planetParallaxDepth.Add(0.05f * Time.deltaTime);
            planetParallaxDepth.Add(0.05f * Time.deltaTime);
            planetParallaxDepth.Add(0.05f * Time.deltaTime);

            // Set Width and Height
            int spaceWidth = 32;
            int spaceHeight = 32;
            Vector2Int space1PngSize = new Vector2Int(spaceWidth, spaceWidth);

            // Load image from file
            var space1Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\starfield\\stars\\galaxy_256x256.png", spaceWidth, spaceHeight);

            // Set Sprite ID from Sprite Atlas
            int space1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(space1Sheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] space1spriteData = new byte[space1PngSize.x * space1PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(space1ID, space1spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D space1Tex = Utility.Texture.CreateTextureFromRGBA(space1spriteData, space1PngSize.x, space1PngSize.y);

            // Create the sprite
            spaceSprite = new Sprites.Sprite
            {
                Texture = space1Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };

            Init = true;
        }

        public void Draw(Material Material, Transform transform, int drawOrder)
        {
            if (Init)
            {
                for (; i < 10; i++)
                {
                    perlinGrid = GenPerlin(256, 256, 2, 30);

                    int random = Random.Range(0, 256);
                    int spriteRandom = Random.Range(0, planets.Count);
                    Sprites.Sprite sprite = new Sprites.Sprite();
                    float rand1 = perlinGrid[random, random];

                    switch (spriteRandom)
                    {
                        case 0:
                            sprite = planets[0];
                            break;
                        case 1:
                            sprite = planets[1];
                            break;
                        case 2:
                            sprite = planets[2];
                            break;
                        case 3:
                            sprite = planets[3];
                            break;
                        case 4:
                            sprite = planets[4];
                            break;
                    }

                    if (rand1 >= .5)
                        Utility.Render.DrawSprite(Random.Range(-10, 10), Random.Range(-10, 10), 1, 1, sprite, Material, transform, 1);
                    else
                        Utility.Render.DrawSprite(Random.Range(-100, 100), Random.Range(-100, 100), 1, 1, sprite, Material, transform, 1);

                    for(int k = 0; k < transform.childCount; k++)
                    {
                        if (!transform.GetChild(k).gameObject.GetComponent<Parallax>())
                            transform.GetChild(k).gameObject.AddComponent<Parallax>();
                        transform.GetChild(k).gameObject.GetComponent<Parallax>().parallaxEffect += planetParallaxDepth[spriteRandom];
                    }
                }

                for (; j < 1; j++)
                {
                    Utility.Render.DrawSprite(-1000000, -1000000, 9999999, 9999999, spaceSprite, Material, transform, 0);
                }
            }

        }

        private float[,] GenPerlin(int width, int height, int contrast, int scale)
        {
            float[,] grid = new float[width, height];
            var max = 1.414f / (1.9f * contrast);
            var min = -1.414f / (1.9f * contrast);
            for (int x = 0; x < width; x++)
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

