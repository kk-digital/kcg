using System;

namespace KMath
{
    public struct Cylinder2D
    {
        public float Radius;
        public Line2D Line;

        public Cylinder2D(float radius, Vec2f pointA, Vec2f pointB)
        {
            Radius = radius;
            Line = new Line2D(pointA, pointB);
        }

        /*
            The intersection against a capsule, rather than a
            cylinder, is performed in a comparable manner by replacing the intersection with the
            endcap planes with intersection against hemispherical endcaps.
        */
        // TODO: NOT WORKING, Implement Capsule rather then Cylinder
        public bool Intersects(Line2D seg, ref float t)
        {
            Vec2f d = Line.B - Line.A, m = seg.A - Line.A, n = seg.B - seg.A;
            float md = Vec2f.Dot(m, d);
            float nd = Vec2f.Dot(n, d);
            float dd = Vec2f.Dot(d, d);
            // Test if segment fully outside either endcap of cylinder
            if (md < 0.0f && md + nd < 0.0f) return false; // Segment outside ’Line.PointA’ side of cylinder
            if (md > dd && md + nd > dd) return false; // Segment outside ’Line.PointB’ side of cylinder
            float nn = Vec2f.Dot(n, n);
            float mn = Vec2f.Dot(m, n);
            float a = dd * nn - nd * nd;
            float k = Vec2f.Dot(m, m) - Radius * Radius;
            float c = dd * k - md * md;
            if (Math.Abs(a) < float.Epsilon)
            {
                // Segment runs parallel to cylinder axis
                if (c > 0.0f) return true; // ’a’ and thus the segment lie outside cylinder
                // Now known that segment intersects cylinder; figure out how it intersects
                if (md < 0.0f) t = -mn / nn; // Intersect segment against ’Line.PointA’ endcap
                else if (md > dd) t = (nd - mn) / nn; // Intersect segment against ’Line.PointB’ endcap
                else t = 0.0f; // ’a’ lies inside cylinder
                return true;
            }

            float b = dd * mn - nd * md;
            float discr = b * b - a * c;
            if (discr < 0.0f) return false; // No real roots; no intersection
            t = (-b - (float) Math.Sqrt(discr)) / a;
            if (t < 0.0f || t > 1.0f) return false; // Intersection lies outside segment
            if (md + t * nd < 0.0f)
            {
                // Intersection outside cylinder on ’Line.PointA’ side
                if (nd <= 0.0f) return false; // Segment pointing away from endcap
                t = -md / nd;
                // Keep intersection if Dot(S(t) - Line.PointA, S(t) - Line.PointA) <= r∧2
                return k + 2 * t * (mn + t * nn) <= 0.0f;
            }

            if (md + t * nd > dd)
            {
                // Intersection outside cylinder on ’Line.PointB’ side
                if (nd >= 0.0f) return false; // Segment pointing away from endcap
                t = (dd - md) / nd;
                // Keep intersection if Dot(S(t) - Line.PointB, S(t) - Line.PointB) <= r∧2
                return k + dd - 2 * md + t * (2 * (mn - nd) + t * nn) <= 0.0f;
            }

            // Segment intersects cylinder between the endcaps; t is correct
            return true;
        }
    }
}
