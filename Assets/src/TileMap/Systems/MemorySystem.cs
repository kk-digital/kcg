using System;

namespace TileMap
{
    public class MemorySystem
    {
        public static readonly MemorySystem Instance;

        public readonly int MetalSlabsTileSheet;
        public readonly int StoneBulkheads;
        public readonly int TilesMoon;
        public readonly int OreTileSheet;

        static MemorySystem()
        {
            Instance = new MemorySystem();
        }

        private MemorySystem()
        {
            MetalSlabsTileSheet = GameState.SpriteLoaderSystem.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Tiles_metal_slabs\\Tiles_metal_slabs.png");
            StoneBulkheads = GameState.SpriteLoaderSystem.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tile_wallbase\\Tiles_stone_bulkheads.png");
            TilesMoon = GameState.SpriteLoaderSystem.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png");
            OreTileSheet = GameState.SpriteLoaderSystem.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png");
        }

        public void InitializeTiles()
        {
            // creating the tiles
            GameState.CreationAPISystem.CreateTile(0);
            GameState.CreationAPISystem.SetTileName("slab1");
            GameState.CreationAPISystem.SetTileTexture16(MetalSlabsTileSheet, 0, 0);
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(1);
            GameState.CreationAPISystem.SetTileName("slab2");
            GameState.CreationAPISystem.SetTileTexture16(MetalSlabsTileSheet, 1, 0);
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(2);
            GameState.CreationAPISystem.SetTileName("slab3");
            GameState.CreationAPISystem.SetTileTexture16(MetalSlabsTileSheet, 4, 0);
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(3);
            GameState.CreationAPISystem.SetTileName("slab4");
            GameState.CreationAPISystem.SetTileTexture16(MetalSlabsTileSheet, 5, 0);
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(4);
            GameState.CreationAPISystem.SetTileName("tile5");
            GameState.CreationAPISystem.SetTileTexture16(StoneBulkheads, 5, 1);
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(5);
            GameState.CreationAPISystem.SetTileName("tile6");
            GameState.CreationAPISystem.SetTileTexture16(StoneBulkheads, 4, 1);
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(6);
            GameState.CreationAPISystem.SetTileName("tile7");
            GameState.CreationAPISystem.SetTileTexture16(StoneBulkheads, 7, 1);
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(7);
            GameState.CreationAPISystem.SetTileName("tile_moon_1");
            GameState.CreationAPISystem.SetTileTexture(TilesMoon, 0, 0);
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(8);
            GameState.CreationAPISystem.SetTileName("ore_1");
            for(int i = 0; i < Enum.GetNames(typeof(Enums.TileVariants)).Length; i++)
            {
                GameState.CreationAPISystem.SetTileVariant16(OreTileSheet, 0, 0, (Enums.TileVariants)i);
            }
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(9);
            GameState.CreationAPISystem.SetTileName("glass");
            GameState.CreationAPISystem.SetTileTexture16(TilesMoon, 12, 11);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 12, 11, Enums.TileVariants.Middle);

            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 13, 11, Enums.TileVariants.Right);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 11, 11, Enums.TileVariants.Left);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 12, 10, Enums.TileVariants.Top);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 12, 12, Enums.TileVariants.Bottom);
            
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 16, 12, Enums.TileVariants.InnerTopRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 17, 12, Enums.TileVariants.InnerTopLeft);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 16, 11, Enums.TileVariants.InnerBottomRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 17, 11, Enums.TileVariants.InnerBottomLeft);


            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 13, 10, Enums.TileVariants.OuterTopRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 11, 10, Enums.TileVariants.OuterTopLeft);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 13, 12, Enums.TileVariants.OuterBottomRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 11, 12, Enums.TileVariants.OuterBottomLeft);

            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 13, 13, Enums.TileVariants.TipRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 11, 13, Enums.TileVariants.TipLeft);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 14, 10, Enums.TileVariants.TipTop);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 14, 12, Enums.TileVariants.TipBottom);

            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 14, 11, Enums.TileVariants.TipVertical);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 12, 13, Enums.TileVariants.TipHorizontal);

            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 14, 13, Enums.TileVariants.Default);

            GameState.CreationAPISystem.EndTile();
        }
    }
}

