namespace Enums.Tile
{
    public enum DrawType
    {
        Error = 0,
        Normal = 1, //use a single sprite, normal tile
        Composited = 2, //ore tile, uses two sprites per tile; one of top of each other
    }
}