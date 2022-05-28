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

        public float[] GetIntersectionWith(float slope)
        {
            float[] Intersection = new float[2];

            // Rotate slope to match rotation of orbit
            slope = (float)Math.Tan(Math.Atan(slope) - Rotation);

            Intersection[0] = (SemiMajorAxis * SemiMinorAxis) / (float)Math.Sqrt(SemiMajorAxis * SemiMajorAxis * slope * slope + SemiMinorAxis * SemiMinorAxis);
            Intersection[1] = slope * Intersection[0];

            Intersection[0] += CenterX;
            Intersection[1] += CenterY;

            return Intersection;
        }

        public float GetDistanceFrom(OrbitingObjectDescriptor Descriptor)
        {
            float[] Pos = GetPosition();
            float[] TargetPos = Descriptor.GetPosition();

            return (float)Math.Sqrt((Pos[0] - TargetPos[0]) * (Pos[0] - TargetPos[0]) + (Pos[1] - TargetPos[1]) * (Pos[1] - TargetPos[1]));
        }
    }
}
