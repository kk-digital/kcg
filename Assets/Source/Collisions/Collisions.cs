namespace Collisions
{
    public static class Collisions
    {
        // Checks if square is colliding with another square
        public static bool RectOverlapRect(float r1_xmin, float r1_xmax, float r1_ymin, float r1_ymax, float r2_xmin, float r2_xmax, float r2_ymin, float r2_ymax)
        {
            // are the sides of one rectangle touching the other?

            return r1_xmax >= r2_xmin &&    // r1 right edge past r2 left
                   r1_xmin <= r2_xmax &&    // r1 left edge past r2 right
                   r1_ymax >= r2_ymin &&    // r1 top edge past r2 bottom
                   r1_ymin <= r2_ymax;
        }
    
        // Checks if square is colliding with point
        public static bool PointOverlapRect(float px, float py, float r_xmin, float r_xmax, float r_ymin, float r_ymax)
        {
            // is the point inside the rectangle's bounds?
            return px >= r_xmin &&        // right of the left edge AND
                   px <= r_xmax &&   // left of the right edge AND
                   py >= r_ymin &&        // below the top AND
                   py <= r_ymax;     // above the bottom
        }
    }
}
