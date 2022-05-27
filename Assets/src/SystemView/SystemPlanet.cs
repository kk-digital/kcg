using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SystemView
{
    public class SystemPlanet
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

        public float GetDistanceFromCenter()
        {
            float[] pos = GetPosition();

            float dx = pos[0] - CenterX;
            float dy = pos[1] - CenterY;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
