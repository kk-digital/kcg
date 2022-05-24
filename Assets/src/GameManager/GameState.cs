





public class GameState
{
    private static SpriteAtlas.SpriteAtlasManager _spriteAtlasManager;
    private static TileProperties.TileCreationApi _tileCreationApi;


    public static TileProperties.TileCreationApi TileCreationApi
    {
        get
        {
            if (_tileCreationApi == null)
            {
                _tileCreationApi = new TileProperties.TileCreationApi();
                CreateDefaultTiles();
            }

            return _tileCreationApi;
        }
    }

   public static SpriteAtlas.SpriteAtlasManager SpriteAtlasManager
   {
       get 
       {
           if (_spriteAtlasManager == null)
           {
               _spriteAtlasManager = new SpriteAtlas.SpriteAtlasManager();
           }

           return _spriteAtlasManager;
       }
   }


   //NOTE(Mahdi): this is used to create some test tiles
   // to make sure the system is working
   public static void CreateDefaultTiles()
   {
       int MetalSlabsTileSheet = 
                TileSpriteLoader.TileSpriteLoader.Instance.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Tiles_metal_slabs\\Tiles_metal_slabs.png");
       int StoneBulkheads = 
                TileSpriteLoader.TileSpriteLoader.Instance.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tile_wallbase\\Tiles_stone_bulkheads.png");


       // creating the tiles
       TileCreationApi.CreateTile(0);
       TileCreationApi.SetTileName("slab1");
       TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 0, 0);
       TileCreationApi.EndTile();

       TileCreationApi.CreateTile(1);
       TileCreationApi.SetTileName("slab2");
       TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 1, 0);
       TileCreationApi.EndTile();

       TileCreationApi.CreateTile(2);
       TileCreationApi.SetTileName("slab3");
       TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 4, 0);
       TileCreationApi.EndTile();

       TileCreationApi.CreateTile(3);
       TileCreationApi.SetTileName("slab4");
       TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 5, 0);
       TileCreationApi.EndTile();

       TileCreationApi.CreateTile(4);
       TileCreationApi.SetTileName("tile5");
       TileCreationApi.SetTileTexture16(StoneBulkheads, 5, 1);
       TileCreationApi.EndTile();

       TileCreationApi.CreateTile(5);
       TileCreationApi.SetTileName("tile6");
       TileCreationApi.SetTileTexture16(StoneBulkheads, 4, 1);
       TileCreationApi.EndTile();

       TileCreationApi.CreateTile(6);
       TileCreationApi.SetTileName("tile7");
       TileCreationApi.SetTileTexture16(StoneBulkheads, 7, 1);
       TileCreationApi.EndTile();


   }
}