using System.Collections.Generic;

namespace Tiles
{
    static class Assets
    {
        public static readonly List<TileType> TileTypes = new List<TileType>();
        public static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

        public static int RegisterTileType(TileType tile)
        {
            tile.ID = TileTypes.Count;
            TileTypes.Add(tile);
            return tile.ID;
        }

        public static void RegisterSprite(Sprite sprite)
        {
            Sprites.Add(sprite.Name, sprite);
        }
    }
}