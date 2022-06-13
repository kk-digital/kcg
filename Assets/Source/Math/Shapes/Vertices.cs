using Enums;

namespace KMath
{
    public struct Vertices
    {
        public VerticesType Type;

        public Vec2f[] List;

        public Vertices(int count, VerticesType type)
        {
            List = new Vec2f[count];
            Type = type;
        }

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


