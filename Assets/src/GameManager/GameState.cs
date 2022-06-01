/// <summary>
/// <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors">Static Constructor</a>
/// </summary>
public class GameState
{
    public static readonly TileSpriteAtlas.TileSpriteAtlasManager TileSpriteAtlasManager;
    public static readonly SpriteAtlas.SpriteAtlasManager SpriteAtlasManager;
    public static readonly TileProperties.TileCreationApi TileCreationApi;
    public static readonly TileSpriteLoader.TileSpriteLoader TileSpriteLoader;
    public static readonly SpriteLoader.SpriteLoader SpriteLoader;
    public static readonly ImageLoader.FileLoadingManager FileLoadingManager;
    
    static GameState()
    {
        TileSpriteAtlasManager = new TileSpriteAtlas.TileSpriteAtlasManager();
        SpriteAtlasManager = new SpriteAtlas.SpriteAtlasManager();
        TileCreationApi = new TileProperties.TileCreationApi();
        TileSpriteLoader = new TileSpriteLoader.TileSpriteLoader();
        SpriteLoader = new SpriteLoader.SpriteLoader();
        FileLoadingManager = new ImageLoader.FileLoadingManager();
    }
}
