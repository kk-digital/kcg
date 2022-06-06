using System;

namespace Planet
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
            MetalSlabsTileSheet = GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Tiles_metal_slabs\\Tiles_metal_slabs.png");
            StoneBulkheads = GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tile_wallbase\\Tiles_stone_bulkheads.png");
            TilesMoon = GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png");
            OreTileSheet = GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png");
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
            for(int i = 0; i < Enum.GetNames(typeof(Tile.Variant)).Length; i++)
            {
                GameState.CreationAPISystem.SetTileVariant16(OreTileSheet, 0, 0, (Tile.Variant)i);
            }
            GameState.CreationAPISystem.EndTile();

            GameState.CreationAPISystem.CreateTile(9);
            GameState.CreationAPISystem.SetTileName("glass");
            GameState.CreationAPISystem.SetTileTexture16(TilesMoon, 12, 11);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 12, 11, Tile.Variant.Middle);

            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 13, 11, Tile.Variant.Right);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 11, 11, Tile.Variant.Left);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 12, 10, Tile.Variant.Top);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 12, 12, Tile.Variant.Bottom);
            
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 16, 12, Tile.Variant.InnerTopRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 17, 12, Tile.Variant.InnerTopLeft);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 16, 11, Tile.Variant.InnerBottomRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 17, 11, Tile.Variant.InnerBottomLeft);


            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 13, 10, Tile.Variant.OuterTopRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 11, 10, Tile.Variant.OuterTopLeft);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 13, 12, Tile.Variant.OuterBottomRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 11, 12, Tile.Variant.OuterBottomLeft);

            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 13, 13, Tile.Variant.TipRight);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 11, 13, Tile.Variant.TipLeft);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 14, 10, Tile.Variant.TipTop);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 14, 12, Tile.Variant.TipBottom);

            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 14, 11, Tile.Variant.TipVertical);
            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 12, 13, Tile.Variant.TipHorizontal);

            GameState.CreationAPISystem.SetTileVariant16(TilesMoon, 14, 13, Tile.Variant.Default);

            GameState.CreationAPISystem.EndTile();
        }
    }
}

