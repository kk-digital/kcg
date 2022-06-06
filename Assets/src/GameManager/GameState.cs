/// <summary>
/// <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors">Static Constructor</a>
/// </summary>
public class GameState
{
    public static readonly Tile.SpriteAtlasManagerSystem TileSpriteAtlasManager;
    public static readonly SpriteAtlas.SpriteAtlasManager SpriteAtlasManager;
    public static readonly Tile.CreationAPISystem CreationAPISystem;
    public static readonly Tile.SpriteLoaderSystem TileSpriteLoader;
    public static readonly SpriteLoader.SpriteLoader SpriteLoader;
    public static readonly ImageLoader.FileLoadingManager FileLoadingManager;
    
    static GameState()
    {
        TileSpriteAtlasManager = new Tile.SpriteAtlasManagerSystem();
        SpriteAtlasManager = new SpriteAtlas.SpriteAtlasManager();
        CreationAPISystem = new Tile.CreationAPISystem();
        TileSpriteLoader = new Tile.SpriteLoaderSystem();
        SpriteLoader = new SpriteLoader.SpriteLoader();
        FileLoadingManager = new ImageLoader.FileLoadingManager();
    }
}
