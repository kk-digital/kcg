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
            float[] TargetPos = GetPosition();

            return (float)Math.Sqrt((Pos[0] - TargetPos[0]) * (Pos[0] - TargetPos[0]) + (Pos[1] - TargetPos[1]) * (Pos[1] - TargetPos[1]));
        }

        public bool PlanPath(OrbitingObjectDescriptor Start, OrbitingObjectDescriptor Destination, float AcceptableDeviation)
        {
            // Copy center coordinates from start location
            CenterX = Start.CenterX;
            CenterY = Start.CenterY;

            // Set rotation to match current position of start object
            Rotation = Start.Rotation + Start.RotationalPosition;

            float[] StartPos = Start.GetPosition();

            // Calculate intersection point between line with slope perpendicular to our orbit and the destination target's orbit
            float[] IntersectionAt = Destination.GetIntersectionWith(StartPos[0], StartPos[1], (float)Math.Sin(Rotation) / (float)Math.Cos(Rotation));

            float[] DestinationPos = Destination.GetPosition();
            float[] EndPos = Destination.GetPositionAt(Destination.GetRotationalPositionAt(IntersectionAt[0], IntersectionAt[1]));

            // Start altitude = current altitude of start object
            float StartAltitude = Start.GetDistanceFromCenter();

            // Destination altitude = altitude at intersection point
            float DestinationAltitude = Destination.GetDistanceFromCenterAt(Destination.GetRotationalPositionAt(IntersectionAt[0], IntersectionAt[1]));

            // Choose periapsis and apoapsis from start/destination altitude (lower value = periapsis, higher value = apoapsis)
            float Periapsis = StartAltitude < DestinationAltitude ? StartAltitude : DestinationAltitude;
            float Apoapsis  = StartAltitude > DestinationAltitude ? StartAltitude : DestinationAltitude;

            // Rotate orbit 180 degrees if we're going from high altitude to low
            if (StartAltitude > DestinationAltitude) Rotation += 3.1415926f;

            // Semi major axis = the longer "radius" of the ellipse
            // Can be calculated by adding periapsis and apoapsis together as they are the 2 farthest points on the orbit, and then dividing it in half
            SemiMajorAxis = (Periapsis + Apoapsis) / 2.0f;

            // This value represents how far off center the periapsis and apoapsis are, in other words it's the distance between the periapsis/apoapsis and the nearest focal point
            float EccentricDistance = SemiMajorAxis - Periapsis;

            // Semi minor axis can be calculated from periapsis and semi major axis
            // This formula is derived from the two following formulas:
            // 
            // (1) EccentricDistance = sqrt(SemiMajorAxis^2 - SemiMinorAxis^2)
            // (2) EccentricDistance = SemiMajorAxis - Periapsis
            // 
            // Adding the two into one equation gives
            // 
            // SemiMajorAxis - Periapsis = sqrt(SemiMajorAxis^2 - SemiMinorAxis^2)
            // 
            // Which can be farther solved to
            // 
            // (SemiMajorAxis - Periapsis)^2 = SemiMajorAxis^2 - SemiMinorAxis^2
            // (SemiMajorAxis - Periapsis)^2 - SemiMajorAxis^2 = -SemiMinorAxis^2
            // 
            // And finally taking the negative of the square root you get
            // 
            // Sqrt(SemiMajorAxis^2 - (SemiMajorAxis - Periapsis)^2) = SemiMinorAxis
            // 
            // The term (SemiMajorAxis - Periapsis)^2 can then be expanded and you end up with the following formula

            SemiMinorAxis = (float)Math.Sqrt(2 * SemiMajorAxis * Periapsis - Periapsis * Periapsis);

            RotationalPosition = StartAltitude > DestinationAltitude ? 3.1415926f : 0.0f;

            // Estimating time to apoapsis and how far the target will have moved in that time to try and see
            // whether the orbits are lined up in a way that an encounter is possible.
            
            // This is called apoapsis, but might be the periapsis if we are travelling from high altitude to low altitude.
            // Nevertheless, this makes no difference for the calculation.

            float TimeToApoapsis = 0.0f;
            float TargetRotationalMovement = 0.0f;

            // This could be an integral. However, after messing around with it I'm not sure it would be any faster
            // than this estimate, and this is definitely a lot easier and simpler to read.
            int segments = 128 + (int)((Periapsis + Apoapsis) * 16);
            for (int i = 0; i < segments; i++)
            {
                // Total distance from periapsis to apoapsis is 180 degrees (pi) - so each segment is (pi / amount of segments) long
                float segmentLength = 3.1415926f / segments;

                // Use the segment length to calculate the altitude the ship and destination object will reach after this segment
                float altitude = GetDistanceFromCenterAt(segmentLength * i + RotationalPosition);
                float targetAltitude = Destination.GetDistanceFromCenterAt(segmentLength + TargetRotationalMovement + Destination.RotationalPosition);

                // We then use the altitude to calculate how much time passed, as d = t / altitude^2
                float segmentDuration = segmentLength * altitude * altitude;

                // Add values to our counters
                TimeToApoapsis += segmentDuration;
                TargetRotationalMovement += segmentDuration / targetAltitude / targetAltitude;
            }

            // Turn rotational positions into position vectors
            float[] ApoapsisPos = GetPositionAt(RotationalPosition + 3.1415926f);
            float[] TargetPos = Destination.GetPositionAt(Destination.RotationalPosition + TargetRotationalMovement);

            // Check whether apoapsis is close enough to where the target will be to ensure an encounter
            if (Math.Sqrt((ApoapsisPos[0] - TargetPos[0]) * (ApoapsisPos[0] - TargetPos[0]) + (ApoapsisPos[1] - TargetPos[1]) * (ApoapsisPos[1] - TargetPos[1])) < AcceptableDeviation)
            {
                return true;
            }

            // We were not able to establish an encounter - Reset our descriptor values to be the same as those of the start object
            SemiMajorAxis = Start.SemiMajorAxis;
            SemiMinorAxis = Start.SemiMinorAxis;
            Rotation = Start.Rotation;
            RotationalPosition = Start.RotationalPosition;

            return false;
        }
    }
}
