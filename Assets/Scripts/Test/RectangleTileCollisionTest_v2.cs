using System;
using Enums.Tile;
using KMath;
using PlanetTileMap;
using UnityEngine;

public class RectangleTileCollisionTest_v2 : MonoBehaviour 
{
    // 16x16 background grid
    public TileMap tileMap;
    public Material Material;
    
    public TestSquare square1;
    public TestSquare square2;
    public TestSquare R0; //not used except tests
    public TestSquare R1;
    public TestSquare R2;

    // Colors for primary test squares
    public Color square1_collision_color    = new(0.0f, 1.0f, 0.0f, 1.0f);
    public Color square2_collision_color    = new(0.0f, 0.0f, 1.0f, 1.0f);

    public Color square1_no_collision_color = new(1.0f, 1.0f, 1.0f, 1.0f);
    public Color square2_no_collision_color = new(0.8f, 0.8f, 0.8f, 1.0f);
    
    // Amount of squares in grid
    public Vec2i mapSize   = new(32, 32);

    // Size of square 1
    public Vec2f square1_size = new(5, 5);
    // Size of square 2
    public Vec2f square2_size = new(5, 5);
    public TestSquare SetSquare(Vec2f center, Vec2f halfSize)
    {
        var square1_obj = new GameObject
        {
            name = "Test square"
        };
        var square       = square1_obj.AddComponent<TestSquare>();
        square.xmin      = center.X - halfSize.X;
        square.xmax      = center.X + halfSize.X;
        square.ymin      = center.Y - halfSize.Y;
        square.ymax      = center.Y + halfSize.Y;
        square.draggable = true;

        return square;
    }

    public void SetRegions(float r_xmin, float r_xmax, float r_ymin, float r_ymax, Vec2f velocity)
    {
        var box1_xmin = r_xmin;
        var box1_xmax = r_xmax;
        var box1_ymin = r_ymin;
        var box1_ymax = r_ymax;

        var box2_xmin = r_xmin + velocity.X;
        var box2_xmax = r_xmax + velocity.X;
        var box2_ymin = r_ymin + velocity.Y;
        var box2_ymax = r_ymax + velocity.Y;


        // r - region
        // r1 - region1 and etc.

        int r1_xmin = velocity.X >= 0f ? (int)Math.Floor(box1_xmin) : (int)Math.Floor(box2_xmin);       //round down
        int r1_xmax = velocity.X >= 0f ? (int)Math.Ceiling(box2_xmax) : (int)Math.Ceiling(box1_xmax);   //round up
        int r1_ymin = velocity.Y >= 0f ? (int)Math.Floor(box1_ymax) : (int)Math.Floor(box2_ymin);       //round down
        int r1_ymax = velocity.Y >= 0f ? (int)Math.Ceiling(box2_ymax) : (int)Math.Ceiling(box1_ymin);   //round up

        int r2_xmin = velocity.X >= 0f ? (int)Math.Floor(box1_xmax) : (int)Math.Floor(box2_xmin);       //round down
        int r2_xmax = velocity.X >= 0f ? (int)Math.Ceiling(box2_xmax) : (int)Math.Ceiling(box1_xmin);   //round up
        int r2_ymin = (int)Math.Floor(box1_ymin);                                                       //round down
        int r2_ymax = (int)Math.Ceiling(box1_ymax);                                                     //round up

        R1.xmin = r1_xmin;
        R1.xmax = r1_xmax;
        R1.ymin = r1_ymin;
        R1.ymax = r1_ymax;
        
        R2.xmin = r2_xmin;
        R2.xmax = r2_xmax;
        R2.ymin = r2_ymin;
        R2.ymax = r2_ymax;

        R1.no_collision_color = Color.yellow;
        R1.color              = Color.yellow;
        
        R2.no_collision_color = Color.yellow;
        R2.color              = Color.yellow;
    }

    void Start()
    {
        GameState.TileMapRenderer.Initialize(Material, transform, 7);
        
        GameObject.Find("Main Camera").transform.position = new Vector3(144f / 2f, 144f / 2f, -10);
        tileMap                          = new TileMap(mapSize);

        // Alternate colors for odd and even tiles to make grid more pleasurable to look at
        Color odd_collision_color     = new(1.00f, 0.50f, 0.50f, 1.0f);
        Color even_collision_color    = new(0.75f, 0.25f, 0.25f, 1.0f);

        Color odd_no_collision_color  = new(0.50f, 0.50f, 0.50f, 1.0f);
        Color even_no_collision_color = new(0.25f, 0.25f, 0.25f, 1.0f);
        
        var halfSize1 = new Vec2f(square1_size.X / 2f, square1_size.Y / 2f);
        var center1 = new Vec2f(halfSize1.X, halfSize1.Y);
        square1 = SetSquare(center1, halfSize1);
        square1.collision_color    = square1_collision_color;
        square1.no_collision_color = square1_no_collision_color;
        square1.color              = square1_no_collision_color;
        
        var region1_obj = new GameObject
        {
            name = "Test Region 1"
        };
        R1       = region1_obj.AddComponent<TestSquare>();
        
        var region2_obj = new GameObject
        {
            name = "Test Region 2"
        };
        R2       = region2_obj.AddComponent<TestSquare>();
        SetRegions(square1.xmin, square1.xmax, square1.ymin, square1.ymax, new Vec2f(12f, 12f));
        
        var halfSize2 = new Vec2f(square2_size.X / 2f, square2_size.Y / 2f);
        var center2 = new Vec2f(mapSize.X - halfSize2.X, mapSize.Y - halfSize2.Y);
        square2 = SetSquare(center2, halfSize2);
        square2.collision_color    = square2_collision_color;
        square2.no_collision_color = square2_no_collision_color;
        square2.color              = square2_no_collision_color;
        
        int tilesMoon = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Terrains\\Tiles_Moon.png", 16, 16);
        int oreTileSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Gems\\Hexagon\\gem_hexagon_1.png", 16, 16);
        
        GameState.TileCreationApi.CreateTileProperty(TileID.Ore1);
        GameState.TileCreationApi.SetTilePropertyName("ore_1");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock);
        GameState.TileCreationApi.SetTilePropertyTexture16(oreTileSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();
        
        GameState.TileCreationApi.CreateTileProperty(TileID.Glass);
        GameState.TileCreationApi.SetTilePropertyName("glass");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock);
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(tilesMoon, 11, 10);
        GameState.TileCreationApi.EndTileProperty();
        

        for (int x = 0; x < mapSize.X; x++)
        {
            for (int y = 0; y < mapSize.Y; y++)
            {
                tileMap.SetFrontTile(x, y, TileID.Glass);
            }
        }
    }

    void Update()
    {
        if(square1.draggable) 
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(Input.GetMouseButton(0) && Collisions.Collisions.PointOverlapRect(mouse.x, mouse.y, square1.xmin, square1.xmax, square1.ymin, square1.ymax)) 
            {
                square1.xmin = mouse.x - square1_size.X / 2f;
                square1.xmax = mouse.x + square1_size.X / 2f;
                square1.ymin = mouse.y - square1_size.Y / 2f;
                square1.ymax = mouse.y + square1_size.Y / 2f;
                SetRegions(square1.xmin, square1.xmax, square1.ymin, square1.ymax, new Vec2f(12f, 12f));
            }
        }

        if(square2.draggable) 
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(Input.GetMouseButton(0) && Collisions.Collisions.PointOverlapRect(mouse.x, mouse.y, square2.xmin, square2.xmax, square2.ymin, square2.ymax)) 
            {
                square2.xmin = mouse.x - square2_size.X / 2f;
                square2.xmax = mouse.x + square2_size.X / 2f;
                square2.ymin = mouse.y - square2_size.Y / 2f;
                square2.ymax = mouse.y + square2_size.Y / 2f;
            }
        }

        // Reset colors for all squares
        for (int x = 0; x < mapSize.X; x++)
        {
            for (int y = 0; y < mapSize.Y; y++)
            {
                tileMap.SetFrontTile(x, y, TileID.Glass);
            }
        }
        square1.color = square1.no_collision_color;
        square2.color = square2.no_collision_color;

        // Check collisions for square 1
        for (int x = 0; x < mapSize.X; x++)
        {
            for (int y = 0; y < mapSize.Y; y++)
            {
                if (Collisions.Collisions.RectOverlapRect(square1.xmin, square1.xmax, square1.ymin, square1.ymax,
                                                          x, x + 1f, y, y + 1f))
                {
                    square1.color    = square1.collision_color;
                    tileMap.SetFrontTile(x, y, TileID.Ore1);
                }
            }
        }


        // Check collisions for square 2
        for (int x = 0; x < mapSize.X; x++)
        {
            for (int y = 0; y < mapSize.Y; y++)
            {
                if (Collisions.Collisions.RectOverlapRect(square2.xmin, square2.xmax, square2.ymin, square2.ymax,
                        x, x + 1f, y, y + 1f))
                {
                    square2.color    = square2.collision_color;
                    tileMap.SetFrontTile(x, y, TileID.Ore1);
                }
            }
        }
        
        // check if the sprite atlas textures needs to be updated
        for(int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
        {
            GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
        }

        // check if the tile sprite atlas textures needs to be updated
        for(int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
        {
            GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
        }
        
        GameState.TileMapRenderer.UpdateFrontLayerMesh(tileMap);
        GameState.TileMapRenderer.DrawLayer(MapLayerType.Front);
    }
}
