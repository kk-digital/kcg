using Enums.Tile;

namespace KMath
{
    public struct Shape
    {
        public Line2D[] Lines;

        public TilePoint YMax;
        public TilePoint YMin;
        public TilePoint XMax;
        public TilePoint XMin;

        public Shape(Line2D[] lines, TilePoint yMin, TilePoint yMax, TilePoint xMin, TilePoint xMax)
        {
            Lines = lines;
            YMax = yMax;
            YMin = yMin;
            XMax = xMax;
            XMin = xMin;
        }
    }
}
