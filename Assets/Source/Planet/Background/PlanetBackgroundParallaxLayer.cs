using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath.PerlinNoise;
using Enums.Tile;
using Utility;

namespace Planet.Background
{
    public class PlanetBackgroundParallaxLayer
    {
        // Init Condition
        private bool Init;

        // Perlin Field
        private PerlinField2D perlinField;

        // Generated Perlin Grid
        float[,] perlinGrid;

        public Utility.FrameMesh Planet;
        public Utility.FrameMesh Space;
        public Utility.FrameMesh Star;

        private List<int> PlanetSpriteIDs;

        private List<int> StarSpriteIDs;

        private List<float> planetRandomGridX;
        private List<float> planetRandomGridY;

        private List<float> starRandomGridX;
        private List<float> starRandomGridY;

        private int SpaceID = 0;

        public void Initialize(Material material, Transform transform)
        {
            PlanetSpriteIDs = new List<int>();
            StarSpriteIDs = new List<int>();
            StarSpriteIDs.Capacity = 50;
            planetRandomGridX = new List<float>();
            planetRandomGridY = new List<float>();
            starRandomGridX = new List<float>();
            starRandomGridY = new List<float>();

            // Set Width and Height
            int planet1Width = 32;
            int planet1Height = 32;
            
            // Load image from file
            var planetSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\galaxy_256x256.png", planet1Width, planet1Height);

            // Set Sprite ID from Sprite Atlas
            int planet1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 0, 5, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet1ID);

            // Set Sprite ID from Sprite Atlas
            int planet2ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 0, 6, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet2ID);

            // Set Sprite ID from Sprite Atlas
            int planet3ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 7, 4, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet3ID);

            // Set Sprite ID from Sprite Atlas
            int planet4ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 4, 5, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet4ID);

            // Set Sprite ID from Sprite Atlas
            int planet5ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 7, 5, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet5ID);

            // Set Sprite ID from Sprite Atlas
            int planet6ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 4, 4, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet6ID);

            // Set Sprite ID from Sprite Atlas
            int planet7ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 6, 4, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet7ID);

            // Set Sprite ID from Sprite Atlas
            int planet8ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 0, 4, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet8ID);

            // Set Sprite ID from Sprite Atlas
            int planet9ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 2, 4, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet9ID);

            // Set Sprite ID from Sprite Atlas
            int planet10ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 3, 4, Enums.AtlasType.BackGround);

            PlanetSpriteIDs.Add(planet10ID);

            // Set Width and Height
            int star1Width = 16;
            int star1Height = 16;

            // Load image from fil
            var star1Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\PlanetBackground\\StarField\\Stars\\starfield_test_16x16_tiles_8x8_tile_grid_128x128.png", star1Width, star1Height);

            // Set Sprite ID from Sprite Atlas
            int star1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 0, Enums.AtlasType.BackGround);
            star1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 1, Enums.AtlasType.BackGround);
            star1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 2, Enums.AtlasType.BackGround);
            star1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 3, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star1ID);

            // Set Sprite ID from Sprite Atlas
            int star2ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 0, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star2ID);

            // Set Sprite ID from Sprite Atlas
            int star3ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 0, 2, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star3ID);

            // Set Sprite ID from Sprite Atlas
            int star4ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 2, 2, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star4ID);

            // Set Sprite ID from Sprite Atlas
            int star5ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 3, 2, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star5ID);

            // Set Sprite ID from Sprite Atlas
            int star6ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 1, 3, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star6ID);

            // Set Sprite ID from Sprite Atlas
            int star7ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 2, 3, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star7ID);

            // Set Sprite ID from Sprite Atlas
            int star8ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 3, 3, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star8ID);

            // Set Sprite ID from Sprite Atlas
            int star9ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 4, 3, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star9ID);

            // Set Sprite ID from Sprite Atlas
            int star10ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(star1Sheet, 4, 0, Enums.AtlasType.BackGround);

            StarSpriteIDs.Add(star10ID);

            // Set Sprite ID from Sprite Atlas
            SpaceID = GameState.SpriteAtlasManager.CopySpriteToAtlas(planetSheet, 0, 0, Enums.AtlasType.BackGround);

            Planet = new Utility.FrameMesh("BackgroundGameobjects", material, transform,
                    GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.BackGround), 2);
            Planet.obj.AddComponent<Parallax>();
            Planet.obj.GetComponent<Parallax>().parallaxEffect = 0.1f;

            Star = new Utility.FrameMesh("Stars", material, transform,
                    GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.BackGround), 1);
            Star.obj.AddComponent<Parallax>();
            Star.obj.GetComponent<Parallax>().parallaxEffect = 0.05f;

            Space = new Utility.FrameMesh("Space", material, transform,
                    GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.BackGround), 0);

            // Create Perlin Field
            perlinField = new PerlinField2D();

            // Initialzie Perlin Field
            perlinField.init(256, 256);

            perlinGrid = GenPerlin(256, 256, 3, 20);

            int random = Random.Range(0, 256);
            float rand1 = perlinGrid[random, random];

            for(int i = 0; i < PlanetSpriteIDs.Count; i++)
            {
                if (rand1 >= .5)
                {
                    planetRandomGridX.Add(Random.Range(-10.0f, 10.0f));
                    planetRandomGridY.Add(Random.Range(-10.0f, 10.0f));
                }
                else
                {
                    planetRandomGridX.Add(Random.Range(-100.0f, 100.0f));
                    planetRandomGridY.Add(Random.Range(-100.0f, 100.0f));
                }
            }

            for (int i = 0; i < 50; i++)
            {
                if (rand1 >= .5)
                {
                    starRandomGridX.Add(Random.Range(-10.0f, 10.0f));
                    starRandomGridY.Add(Random.Range(-10.0f, 10.0f));
                }
                else
                {
                    starRandomGridX.Add(Random.Range(-100.0f, 100.0f));
                    starRandomGridY.Add(Random.Range(-100.0f, 100.0f));
                }

                int starRandom = Random.Range(0, 10);
                StarSpriteIDs.Add(StarSpriteIDs[starRandom]);
            }

            Init = true;
        }

        public void Draw()
        {
            UpdateMesh();
            UpdateStars();
            UpdateSpace();
            DrawMesh();
        }

        private void UpdateMesh()
        {
            if(Init)
            {
                int index = 0;
                Planet.Clear();
                for (int n = 0; n < PlanetSpriteIDs.Count; n++)
                {
                    int spriteId = PlanetSpriteIDs[n];

                    Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.BackGround).TextureCoords;

                    var x = planetRandomGridX[n];
                    var y = planetRandomGridY[n];
                    var width = 1;
                    var height = 1;

                    // Update UVs
                    Planet.UpdateUV(textureCoords, (index) * 4);
                    // Update Vertices
                    Planet.UpdateVertex((index * 4), x, y, width, height);
                    index++;
                }
            }
        }

        private void UpdateSpace()
        {
            if (Init)
            {
                Space.Clear();

                Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(SpaceID, Enums.AtlasType.BackGround).TextureCoords;

                var x = Camera.main.transform.position.x - 50;
                var y = Camera.main.transform.position.y - 50;
                var width = Camera.main.pixelWidth;
                var height = Camera.main.pixelHeight;

                // Update UVs
                Space.UpdateUV(textureCoords, 0);
                // Update Vertices
                Space.UpdateVertex(0, x, y, width, height);  
            }
        }

        private void UpdateStars()
        {
            if (Init)
            {
                int index = 0;
                Star.Clear();
                for (int n = 0; n < 50; n++)
                {
                    int spriteId = StarSpriteIDs[n];

                    Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.BackGround).TextureCoords;

                    var x = starRandomGridX[n];
                    var y = starRandomGridY[n];
                    var width = 1;
                    var height = 1;

                    // Update UVs
                    Star.UpdateUV(textureCoords, (index) * 4);
                    // Update Vertices
                    Star.UpdateVertex((index * 4), x, y, width, height);
                    index++;
                }
            }
        }

        private void DrawMesh()
        {
            Utility.Render.DrawFrame(ref Planet, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.BackGround));
            Utility.Render.DrawFrame(ref Star, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.BackGround));
            Utility.Render.DrawFrame(ref Space, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.BackGround));
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
