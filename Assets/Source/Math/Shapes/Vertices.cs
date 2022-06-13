using Enums;

namespace KMath
{
    public static class VerticesExt
    {
        public static Vertices CreateVertices(this ref Box box)
        {
            var vertices = new Vertices
            {
                Type = VerticesType.Box,
                List = new Vec2f[4]
            };

            vertices.List[0] = box.BottomLeft;
            vertices.List[1] = box.BottomRight;
            vertices.List[2] = box.TopLeft;
            vertices.List[3] = box.TopRight;

            return vertices;
        }
    }
    
    public struct Vertices
    {
        public VerticesType Type;

        public Vec2f[] List;

        public static Vec2f[] GetAxes(Vec2f[] vertices)
        {
            if (vertices.Length <= 1) return null;
            
            var count = vertices.Length == 2 ? 1 : vertices.Length;
            var axes = new Vec2f[count];

            for (int i = 1; i < count; i++)
            {
                var vec = vertices[i] - vertices[i - 1];
                var normal = Vec2f.Perpendicular(vec);

                if (normal.magnitude != 0)
                {
                    normal.X *= 1 / normal.magnitude;
                    normal.Y *= 1 / normal.magnitude;
                }
                
                axes[i - 1] = normal;
            }

            return axes;
        }
    }
}


