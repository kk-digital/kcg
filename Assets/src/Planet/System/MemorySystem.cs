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
            GameState.TileCreationApi.CreateTile(0);
            GameState.TileCreationApi.SetTileName("slab1");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(1);
            GameState.TileCreationApi.SetTileName("slab2");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 1, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(2);
            GameState.TileCreationApi.SetTileName("slab3");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 4, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(3);
            GameState.TileCreationApi.SetTileName("slab4");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 5, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(4);
            GameState.TileCreationApi.SetTileName("tile5");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 5, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(5);
            GameState.TileCreationApi.SetTileName("tile6");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 4, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(6);
            GameState.TileCreationApi.SetTileName("tile7");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 7, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(7);
            GameState.TileCreationApi.SetTileName("tile_moon_1");
            GameState.TileCreationApi.SetTileTexture(TilesMoon, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(8);
            GameState.TileCreationApi.SetTileName("ore_1");
            for(int i = 0; i < Enum.GetNames(typeof(TileVariant.Variant)).Length; i++)
            {
                GameState.TileCreationApi.SetTileVariant16(OreTileSheet, 0, 0, (TileVariant.Variant)i);
            }
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(9);
            GameState.TileCreationApi.SetTileName("glass");
            GameState.TileCreationApi.SetTileTexture16(TilesMoon, 12, 11);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 12, 11, TileVariant.Variant.Middle);

            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 13, 11, TileVariant.Variant.Right);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 11, 11, TileVariant.Variant.Left);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 12, 10, TileVariant.Variant.Top);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 12, 12, TileVariant.Variant.Bottom);
            
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 16, 12, TileVariant.Variant.InnerTopRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 17, 12, TileVariant.Variant.InnerTopLeft);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 16, 11, TileVariant.Variant.InnerBottomRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 17, 11, TileVariant.Variant.InnerBottomLeft);


            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 13, 10, TileVariant.Variant.OuterTopRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 11, 10, TileVariant.Variant.OuterTopLeft);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 13, 12, TileVariant.Variant.OuterBottomRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 11, 12, TileVariant.Variant.OuterBottomLeft);

            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 13, 13, TileVariant.Variant.TipRight);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 11, 13, TileVariant.Variant.TipLeft);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 14, 10, TileVariant.Variant.TipTop);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 14, 12, TileVariant.Variant.TipBottom);

            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 14, 11, TileVariant.Variant.TipVertical);
            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 12, 13, TileVariant.Variant.TipHorizontal);

            GameState.TileCreationApi.SetTileVariant16(TilesMoon, 14, 13, TileVariant.Variant.Default);

            GameState.TileCreationApi.EndTile();
        }
    }
}

