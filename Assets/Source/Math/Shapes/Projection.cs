using System;

namespace KMath
{
    public struct Projection
    {
        public float Min;
        public float Max;

        public bool Overlap(Projection other)
        {
            return Min - other.Max > 0 || other.Min - Max > 0;
        }

        public static Projection Create(Vertices vertices, Vec2f axis)
        {
            var proj = new Projection
            {
                Min = Vec2f.Dot(axis, vertices.List[0])
            };

            proj.Max = proj.Min;

            foreach (var vertex in vertices.List)
            {
                var dot = Vec2f.Dot(axis, vertex);

                proj.Min = Math.Min(proj.Min, dot);
                proj.Max = Math.Max(proj.Max, dot);
            }

            return proj;
        }
    }
}

