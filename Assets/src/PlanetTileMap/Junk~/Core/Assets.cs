using System.Collections.Generic;

namespace Tiles.Junk
{
    static class Assets
    {
        public static readonly List<TileType> TileTypes = new List<TileType>();
        public static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
        public static int[,] SpriteAtlas0;

        public static int RegisterTileType(TileType tile)
        {
            tile.ID = TileTypes.Count;
            TileTypes.Add(tile);
            return tile.ID;
        }

        public static void RegisterSprite(Sprite sprite)
        {
            if(!Sprites.TryAdd(sprite.Name, sprite))
                throw new System.Exception($"Sprite with name {sprite.Name} already exists");
        }
    }
}