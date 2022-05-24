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
}