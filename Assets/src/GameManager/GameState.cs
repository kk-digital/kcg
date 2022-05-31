public class GameState
{
    private static TileSpriteAtlas.TileSpriteAtlasManager _tileSpriteAtlasManager;
    private static SpriteAtlas.SpriteAtlasManager _spriteAtlasManager;
    private static TileProperties.TileCreationApi _tileCreationApi;
    private static TileSpriteLoader.TileSpriteLoader _tileSpriteLoader;
    private static SpriteLoader.SpriteLoader _spriteLoader;
    private static ImageLoader.FileLoadingManager _fileLoadingManager;
    
    public static TileProperties.TileCreationApi TileCreationApi =>
        _tileCreationApi ??= new TileProperties.TileCreationApi();
    

    public static TileSpriteAtlas.TileSpriteAtlasManager TileSpriteAtlasManager =>
         _tileSpriteAtlasManager ??= new TileSpriteAtlas.TileSpriteAtlasManager();
    

    public static TileSpriteLoader.TileSpriteLoader TileSpriteLoader  =>
         _tileSpriteLoader ??= new TileSpriteLoader.TileSpriteLoader();

    public static SpriteLoader.SpriteLoader SpriteLoader  =>
         _spriteLoader ??= new SpriteLoader.SpriteLoader();
    

    public static ImageLoader.FileLoadingManager FileLoadingManager  =>
        _fileLoadingManager ??= new ImageLoader.FileLoadingManager();
    

    public static SpriteAtlas.SpriteAtlasManager SpriteAtlasManager  =>
        _spriteAtlasManager ??= new SpriteAtlas.SpriteAtlasManager();


  
}
