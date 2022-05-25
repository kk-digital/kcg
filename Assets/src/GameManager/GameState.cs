public class GameState
{
    private static SpriteAtlas.SpriteAtlasManager _spriteAtlasManager;
    private static TileProperties.TileCreationApi _tileCreationApi;
    private static TileProperties.TilePropertiesManager _tilePropertiesManager;
    private static TileSpriteLoader.TileSpriteLoader _tileSpriteLoader;
    private static ImageLoader.FileLoadingManager _fileLoadingManager;
    
    public static TileProperties.TilePropertiesManager TilePropertiesManager => _tilePropertiesManager ??= new TileProperties.TilePropertiesManager();
    public static TileProperties.TileCreationApi TileCreationApi => _tileCreationApi ??= new TileProperties.TileCreationApi();
    public static SpriteAtlas.SpriteAtlasManager SpriteAtlasManager => _spriteAtlasManager ??= new SpriteAtlas.SpriteAtlasManager();
    public static TileSpriteLoader.TileSpriteLoader TileSpriteLoader => _tileSpriteLoader ??= new TileSpriteLoader.TileSpriteLoader();
    public static ImageLoader.FileLoadingManager FileLoadingManager => _fileLoadingManager ??= new ImageLoader.FileLoadingManager();

  
}
