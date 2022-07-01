using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath.PerlinNoise;

namespace Planet.Background
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
            var planet1Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet1Width, planet1Height);

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
            var planet2Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet2Width, planet2Height);

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
            var planet3Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet3Width, planet3Height);

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
            var planet4Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet4Width, planet4Height);

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
            var planet5Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet5Width, planet5Height);

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

            // Set Width and Height
            int planet6Width = 32;
            int planet6Height = 32;
            Vector2Int planet6PngSize = new Vector2Int(planet6Width, planet6Height);

            // Load image from file
            var planet6Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet6Width, planet6Height);

            // Set Sprite ID from Sprite Atlas
            int planet6ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet6Sheet, 4, 4, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet6spriteData = new byte[planet6PngSize.x * planet6PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet6ID, planet6spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet6Tex = Utility.Texture.CreateTextureFromRGBA(planet6spriteData, planet6PngSize.x, planet6PngSize.y);

            // Create the sprite
            var planet6Sprite = new Sprites.Sprite
            {
                Texture = planet6Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet6Sprite);

            // Set Width and Height
            int planet7Width = 32;
            int planet7Height = 32;
            Vector2Int planet7PngSize = new Vector2Int(planet7Width, planet7Height);

            // Load image from file
            var planet7Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet7Width, planet7Height);

            // Set Sprite ID from Sprite Atlas
            int planet7ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet7Sheet, 6, 4, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet7spriteData = new byte[planet7PngSize.x * planet7PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet7ID, planet7spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet7Tex = Utility.Texture.CreateTextureFromRGBA(planet7spriteData, planet7PngSize.x, planet7PngSize.y);

            // Create the sprite
            var planet7Sprite = new Sprites.Sprite
            {
                Texture = planet7Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet7Sprite);

            // Set Width and Height
            int planet8Width = 32;
            int planet8Height = 32;
            Vector2Int planet8PngSize = new Vector2Int(planet8Width, planet8Height);

            // Load image from file
            var planet8Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet8Width, planet8Height);

            // Set Sprite ID from Sprite Atlas
            int planet8ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet8Sheet, 0, 4, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet8spriteData = new byte[planet8PngSize.x * planet8PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet8ID, planet8spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet8Tex = Utility.Texture.CreateTextureFromRGBA(planet8spriteData, planet8PngSize.x, planet8PngSize.y);

            // Create the sprite
            var planet8Sprite = new Sprites.Sprite
            {
                Texture = planet8Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet8Sprite);

            // Set Width and Height
            int planet9Width = 32;
            int planet9Height = 32;
            Vector2Int planet9PngSize = new Vector2Int(planet9Width, planet9Height);

            // Load image from file
            var planet9Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet9Width, planet9Height);

            // Set Sprite ID from Sprite Atlas
            int planet9ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet9Sheet, 2, 4, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet9spriteData = new byte[planet9PngSize.x * planet9PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet9ID, planet9spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet9Tex = Utility.Texture.CreateTextureFromRGBA(planet9spriteData, planet9PngSize.x, planet9PngSize.y);

            // Create the sprite
            var planet9Sprite = new Sprites.Sprite
            {
                Texture = planet9Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet9Sprite);

            // Set Width and Height
            int planet10Width = 32;
            int planet10Height = 32;
            Vector2Int planet10PngSize = new Vector2Int(planet10Width, planet10Height);

            // Load image from file
            var planet10Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet10Width, planet10Height);

            // Set Sprite ID from Sprite Atlas
            int planet10ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planet10Sheet, 3, 4, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] planet10spriteData = new byte[planet10PngSize.x * planet10PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(planet10ID, planet10spriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D planet10Tex = Utility.Texture.CreateTextureFromRGBA(planet10spriteData, planet10PngSize.x, planet10PngSize.y);

            // Create the sprite
            var planet10Sprite = new Sprites.Sprite
            {
                Texture = planet10Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            // Add created sprite to planets list
            planets.Add(planet10Sprite);

            // Create Perlin Field
            perlinField = new PerlinField2D();

            // Initialzie Perlin Field
            perlinField.init(256, 256);

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
                        case 5:
                            sprite = planets[5];
                            break;
                        case 6:
                            sprite = planets[6];
                            break;
                        case 7:
                            sprite = planets[7];
                            break;
                        case 8:
                            sprite = planets[8];
                            break;
                        case 9:
                            sprite = planets[9];
                            break;
                    }

                    if (rand1 >= .5)
                        Utility.Render.DrawBackground(Random.Range(-10, 10), Random.Range(-10, 10), 1, 1, sprite, Material, transform, 1);
                    else
                        Utility.Render.DrawBackground(Random.Range(-100, 100), Random.Range(-100, 100), 1, 1, sprite, Material, transform, 1);

                    for(int k = 0; k < transform.childCount; k++)
                    {
                        if (!transform.GetChild(k).gameObject.GetComponent<Parallax>())
                            transform.GetChild(k).gameObject.AddComponent<Parallax>();

                        transform.GetChild(k).gameObject.GetComponent<Parallax>().parallaxEffect = 0.01f;
                    }
                }

                for (; j < 1; j++)
                {
                    Utility.Render.DrawQuadColor(-1000000, -1000000, 9999999, 9999999, new Color(0.02745f, 0.03137f, 0.09804f, 1), Material, transform, 0);
                }
            }

        }

        // Generates Perlin Map
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

