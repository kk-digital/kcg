namespace Tiles
{
    struct Sprite
    {
        public string Name;
        public int Left;//position in atlas
        public int Top;//position in atlas
        public int Width;
        public int Height;
        public int[,] Texture;
    }
}