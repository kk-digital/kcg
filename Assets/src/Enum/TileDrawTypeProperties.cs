namespace Enums
{
    public enum TileDrawProperties
    {
        TileDrawPropertyError = 0, //TileLayer.Error
        TileDrawPropertyNormal = 1, //use a single sprite, normal tile
        TileDrawPropertyComposited = 2, //ore tile, uses two sprites per tile; one of top of each other
    }
}