using System;

namespace SystemView
{
    // Class that defines the behaviors of orbiting objects
    // Will be used to define orbiting parameters of planets, asteroids, ships, etc.
    public class OrbitingObjectDescriptor
    {
        // Math symbols used in equations
        // 
        // a = semi major axis
        // b = semi minor axis
        // q = periapsis
        // Q = apoapsis
        // ω = rotation
        // E = distance from focal point to radius
        // e = eccentricity
        // cx = x coordinate of center
        // cy = y coordinate of center

        public float CenterX;
        public float CenterY;

        public float SemiMinorAxis;
        public float SemiMajorAxis;

        public float Rotation;

        public float RotationalPosition;

        public float Mass;

        // Fast inverse square root using Quake's floating bit hack
        // Using more accurate 0x5f375a86 instead of the original 0x5f3759df
        static unsafe float Q_rsqrt(float number)
        {
            int i;
            float x2, y;

            x2 = number * 0.5f;
            y = number;

            i = *(int*)&number;
            i = 0x5f375a86 - (i >> 1);

            y = *(float*)&i;
            y = y * (1.5f - (x2 * y * y));

            return y;
        }

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

        // this function is basically the reverse of the GetPositionAt function
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
            // would be faster if C# had a function that calls x86's fsincos instruction (as it calculates both sin and cos in one single instruction)
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

            // Intersection1[0] = (float)(SemiMajorAxis * SemiMinorAxis / Math.Sqrt(SemiMajorAxis * SemiMajorAxis * slope * slope + SemiMinorAxis * SemiMinorAxis));

            // Use fast inverse square root for faster speed
            Intersection1[0] = SemiMajorAxis * SemiMinorAxis * Q_rsqrt(SemiMajorAxis * SemiMajorAxis * slope * slope + SemiMinorAxis * SemiMinorAxis);
            Intersection1[1] = slope * Intersection1[0];

            Intersection2[0] = -Intersection1[0];
            Intersection2[1] = -Intersection1[1];

            // Rotate points to match orbit's rotation

            // (1) x = cos(ω) * x_0 - sin(ω) * y_0 + cx

            // (2) y = sin(ω) * x_0 + cos(ω) * y_0 + cy

            (Intersection1[0], Intersection1[1]) = (rotcos * Intersection1[0] - rotsin * Intersection1[1] + CenterX,
                                                    rotsin * Intersection1[0] + rotcos * Intersection1[1] + CenterY);
            (Intersection2[0], Intersection2[1]) = (rotcos * Intersection2[0] - rotsin * Intersection2[1] + CenterX,
                                                    rotsin * Intersection2[0] + rotcos * Intersection2[1] + CenterY);

            // (1) d = √(𐤃x^2 + 𐤃y^2)

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

        public bool PlanPath(OrbitingObjectDescriptor Destination, float AcceptableDeviation)
        {
            // Make a copy of the current state in case we cannot establish an orbit from our current position
            OrbitingObjectDescriptor Start = new OrbitingObjectDescriptor(this);
            float[] StartPos = GetPosition();

            // Turn orbit so that our rotational position is 0
            Rotation += RotationalPosition;
            RotationalPosition = 0.0f;

            // Find intersection between planned orbit and destination orbit
            float[] IntersectionAt = Destination.GetIntersectionWith(StartPos[0], StartPos[1], (float)Math.Tan(Rotation));

            // Start altitude = current altitude of start object
            float StartAltitude = Start.GetDistanceFromCenter();

            // Destination altitude = altitude at intersection point
            float DestinationAltitude = Destination.GetDistanceFromCenterAt(Destination.GetRotationalPositionAt(IntersectionAt[0], IntersectionAt[1]));

            // Choose periapsis and apoapsis from start/destination altitude (lower value = periapsis, higher value = apoapsis)
            float Periapsis = StartAltitude < DestinationAltitude ? StartAltitude : DestinationAltitude;
            float Apoapsis = StartAltitude > DestinationAltitude ? StartAltitude : DestinationAltitude;

            // Rotate orbit 180 degrees if we're going from high altitude to low
            if (StartAltitude > DestinationAltitude)
            {
                Rotation += 3.1415926f;
                RotationalPosition = 3.1415926f;
            }

            // Semi major axis = the longer "radius" of the ellipse
            // Can be calculated by adding periapsis and apoapsis together as they are the 2 farthest points on the orbit, and then dividing it in half
            SemiMajorAxis = (Periapsis + Apoapsis) / 2.0f;

            // This value represents how far off center the periapsis and apoapsis are, in other words it's the distance between the periapsis/apoapsis and the nearest focal point
            float EccentricDistance = SemiMajorAxis - Periapsis;

            // (1) E     = √(a^2 - b^2)

            // (2) E     = a - q

            // (3) a - q = √(a^2 - b^2)   =>   b = √(q * (2a - q))

            SemiMinorAxis = (float)Math.Sqrt(Periapsis * (2 * SemiMajorAxis - Periapsis));

            float TimeToEncounter = 0.0f;
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
                TimeToEncounter += segmentDuration;
                TargetRotationalMovement += segmentDuration / targetAltitude / targetAltitude;
            }

            float[] TargetPosAtEncounter = Destination.GetPositionAt(Destination.RotationalPosition + TargetRotationalMovement);

            // Check whether apoapsis is close enough to where the target will be to ensure an encounter
            // 
            // if (Math.Sqrt((IntersectionAt[0] - TargetPosAtEncounter[0]) * (IntersectionAt[0] - TargetPosAtEncounter[0])
            //  + (IntersectionAt[1] - TargetPosAtEncounter[1]) * (IntersectionAt[1] - TargetPosAtEncounter[1]))
            //  < AcceptableDeviation)
            // 
            // Instead of calculating square root, just square the acceptable deviation instead. Much faster this way.

            float dx = IntersectionAt[0] - TargetPosAtEncounter[0];
            float dy = IntersectionAt[1] - TargetPosAtEncounter[1];

            dx *= dx;
            dy *= dy;

            if (dx + dy < AcceptableDeviation * AcceptableDeviation)
                return true;

            SemiMajorAxis = Start.SemiMajorAxis;
            SemiMinorAxis = Start.SemiMinorAxis;
            Rotation = Start.Rotation;
            RotationalPosition = Start.RotationalPosition;

            return false;
        }

        public float[] GetVelocityAt(float Pos)
        {
            // (1) x = cos(ω) * (cos(M) * a - √(a^2 - b^2)) - sin(ω) * sin(M) * b + cx

            // (2) y = sin(ω) * (cos(M) * a - √(a^2 - b^2)) + cos(ω) * sin(M) * b + cy

            //          𐤃x
            // (3) vx = -- = -a * sin(M) * cos(ω) - b * cos(M) * sin(ω)
            //          𐤃M

            //          𐤃y
            // (4) vy = -- =  b * cos(M) * cos(ω) - a * sin(M) * sin(ω)
            //          𐤃M

            float[] Velocity = new float[2];

            float possin = (float)Math.Sin(Pos);
            float poscos = (float)Math.Cos(Pos);

            float rotsin = (float)Math.Sin(Rotation);
            float rotcos = (float)Math.Cos(Rotation);

            Velocity[0] = -SemiMajorAxis * possin * rotcos - SemiMinorAxis * poscos * rotsin;
            Velocity[1] =  SemiMinorAxis * poscos * rotcos - SemiMajorAxis * possin * rotsin;

            return Velocity;
        }

        public float[] GetVelocity()
        {
            return GetVelocityAt(RotationalPosition);
        }
    }
}
