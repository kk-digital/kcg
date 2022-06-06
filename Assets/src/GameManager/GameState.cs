/// <summary>
/// <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors">Static Constructor</a>
/// </summary>
public class GameState
{
    public static readonly Tile.SpriteAtlasManagerSystem TileSpriteAtlasManager;
    public static readonly Sprites.AtlasManagerSystem SpriteAtlasManager;
    public static readonly Tile.CreationAPISystem CreationAPISystem;
    public static readonly Sprites.LoaderSystem SpriteLoaderSystem;
    public static readonly ImageLoader.FileLoadingManager FileLoadingManager;
    
    static GameState()
    {
        TileSpriteAtlasManager = new Tile.SpriteAtlasManagerSystem();
        SpriteAtlasManager = new Sprites.AtlasManagerSystem();
        CreationAPISystem = new Tile.CreationAPISystem();
        SpriteLoaderSystem = new Sprites.LoaderSystem();
        FileLoadingManager = new ImageLoader.FileLoadingManager();
    }
}
