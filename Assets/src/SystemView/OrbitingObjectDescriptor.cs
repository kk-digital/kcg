using System;

namespace SystemView
{
    // Class that defines the behaviors of orbiting objects
    // Will be used to define orbiting parameters of planets, asteroids, ships, etc.
    public class OrbitingObjectDescriptor
    {
        public float CenterX;
        public float CenterY;

        public float SemiMinorAxis;
        public float SemiMajorAxis;

        public float Rotation;

        public float RotationalPosition;

        public OrbitingObjectDescriptor()
        {

        }

        public OrbitingObjectDescriptor(OrbitingObjectDescriptor Copy)
        {
            CenterX = Copy.CenterX;
            CenterY = Copy.CenterY;
            SemiMinorAxis = Copy.SemiMinorAxis;
            SemiMajorAxis = Copy.SemiMajorAxis;
            Rotation = Copy.Rotation;
            RotationalPosition = Copy.RotationalPosition;
        }

        public float GetEccentricDistance()
        {
            return (float)Math.Sqrt(SemiMajorAxis * SemiMajorAxis - SemiMinorAxis * SemiMinorAxis);
        }

        public float GetEccentricity()
        {
            return (float)Math.Sqrt(1 - (SemiMinorAxis / SemiMajorAxis) * (SemiMinorAxis / SemiMajorAxis));
        }

        public float GetPeriapsisAltitude()
        {
            return SemiMajorAxis - GetEccentricDistance();
        }

        public float GetApoapsisAltitude()
        {
            return SemiMajorAxis + GetEccentricDistance();
        }

        // Note: orbits are kind of simplified and don't actually take into account
        //       factors like gravity, this leads to the movements not being quite
        //       perfectly accurate, but considering there might be hundreds of thousands
        //       of objects, this is far more efficient.
        public float[] GetPositionAt(float Pos)
        {
            // sine and cosine of the rotation
            float rotsin = (float)Math.Sin(Rotation);
            float rotcos = (float)Math.Cos(Rotation);

            float[] pos = new float[2];

            float x = (float)Math.Cos(Pos);
            float y = (float)Math.Sin(Pos);

            float posx = x * SemiMajorAxis - GetEccentricDistance();
            float posy = y * SemiMinorAxis;

            pos[0] = rotcos * posx - rotsin * posy + CenterX;
            pos[1] = rotsin * posx + rotcos * posy + CenterY;

            return pos;
        }

        public float[] GetPosition()
        {
            return GetPositionAt(RotationalPosition);
        }

        public float GetDistanceFromCenterAt(float Pos)
        {
            float[] pos = GetPositionAt(Pos);

            float dx = pos[0] - CenterX;
            float dy = pos[1] - CenterY;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float GetDistanceFromCenter()
        {
            return GetDistanceFromCenterAt(RotationalPosition);
        }

        // this math is a mess
        public float GetRotationalPositionAt(float x, float y)
        {
            float rotsin = (float)Math.Sin(Rotation);
            float rotcos = (float)Math.Cos(Rotation);

            // not needed - will never happen
            // if (rotcos * rotcos + rotsin * rotsin == 0.0f) throw new IndexOutOfRangeException();

            // not needed - only one of the two coordinates are needed
            // float posx = (((x - CenterX) * rotcos + (y - CenterY) * rotsin) / (rotcos * rotcos + rotsin * rotsin) + GetEccentricDistance()) / SemiMajorAxis;

            float posy = ((y - CenterY) * rotcos + (CenterX - x) * rotsin) / ((rotcos * rotcos + rotsin * rotsin) * SemiMinorAxis);

            return (float)Math.Asin(posy);
        }

        public float[] GetIntersectionWith(float startx, float starty, float slope)
        {
            // sine and cosine of the orbit's rotation
            float rotsin = (float)Math.Sin(Rotation);
            float rotcos = (float)Math.Cos(Rotation);

            // Find both intersection points and return the farther one
            float[] Intersection1 = new float[2];
            float[] Intersection2 = new float[2];

            // Rotate slope to match rotation of orbit
            slope = (float)Math.Tan(Math.Atan(slope) - Rotation);

            // (1) y = mx

            //     x^2   y^2                           b √ (a^2 - x^2)
            // (2) --- + --- = 1            =>   y = ± ---------------
            //     a^2   b^2                                  a

            //            b √ (a^2 - x^2)                      a * b
            // (3) mx = ± ---------------   =>   x = ± --------------------
            //                   a                     √ (a^2 * m^2 + b^2 )

            Intersection1[0] = (float)(SemiMajorAxis * SemiMinorAxis / Math.Sqrt(SemiMajorAxis * SemiMajorAxis * slope * slope + SemiMinorAxis * SemiMinorAxis));
            Intersection1[1] = slope * Intersection1[0];

            Intersection2[0] = -Intersection1[0];
            Intersection2[1] = -Intersection1[1];

            // Rotate points to match orbit's rotation

            (Intersection1[0], Intersection1[1]) = (rotcos * Intersection1[0] - rotsin * Intersection1[1] + CenterX,
                                                    rotsin * Intersection1[0] + rotcos * Intersection1[1] + CenterY);
            (Intersection2[0], Intersection2[1]) = (rotcos * Intersection2[0] - rotsin * Intersection2[1] + CenterX,
                                                    rotsin * Intersection2[0] + rotcos * Intersection2[1] + CenterY);

            float d1 = (float)Math.Sqrt((Intersection1[0] - startx) * (Intersection1[0] - startx) + (Intersection1[1] - starty) * (Intersection1[1] - starty));
            float d2 = (float)Math.Sqrt((Intersection2[0] - startx) * (Intersection2[0] - startx) + (Intersection2[1] - starty) * (Intersection2[1] - starty));

            return d1 > d2 ? Intersection1 : Intersection2;
        }

        public float GetDistanceFrom(OrbitingObjectDescriptor Descriptor)
        {
            float[] Pos = GetPosition();
            float[] TargetPos = Descriptor.GetPosition();

            return (float)Math.Sqrt((Pos[0] - TargetPos[0]) * (Pos[0] - TargetPos[0]) + (Pos[1] - TargetPos[1]) * (Pos[1] - TargetPos[1]));
        }
    }
}
