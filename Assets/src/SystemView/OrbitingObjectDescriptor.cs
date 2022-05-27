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
            // sine and cosine of position
            float sin = (float)Math.Sin(Pos);
            float cos = (float)Math.Cos(Pos);

            // sine and cosine of the rotation
            float rotsin = (float)Math.Sin(Rotation);
            float rotcos = (float)Math.Cos(Rotation);

            float[] pos = new float[2];

            float x = 1.0f;
            float y = 0.0f;

            (x, y) = (cos * x - sin * y, sin * x + cos * y);

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
    }
}
