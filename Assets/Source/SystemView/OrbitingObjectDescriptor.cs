using System;
// using UnityEngine; // For DebugOut

namespace SystemView
{
    // Class that defines the behaviors of orbiting objects
    // Will be used to define orbiting parameters of planets, asteroids, ships, etc.
    public class OrbitingObjectDescriptor
    {
                                                             //                                  -11    m³
        const  float   GravitationalConstant = 6.67408E-11f; // G                    6.67408 * 10    -------
                                                             //                                      kg * s²

        public float   SemiMajorAxis;                        // a                    Half of the major axis
        public float   SemiMinorAxis;                        // b                    Half of the minor axis
        public float   Rotation;                             // ω                    Orbit's rotation across the Z-axis
        public float   OrbitalPeriod;                        // P                    Amount of time needed for body to complete an orbit
        public float   MeanMotion;                           // n                    Angular speed required to complete the orbit assuming constant speed
        public float   MeanAnomaly;                          // M                    Position as a fraction of the body's orbital period
        public float   EccentricAnomaly;                     // E                    Required to calculate true anomaly and velocity
        public float   TrueAnomaly;                          // ν                    Real angular position of body
        public float[] EccentricityVector;                   // e                    Dimensionless vector pointing from apoapsis to periapsis, with magnitude equal to the scalar eccentricity
        public float   Eccentricity;                         // ε                    Orbital eccentricity (ε = 0: central, 0 < ε < 1: elliptical, ε = 1: parabolic, ε > 1: hyperbolic)
        public float   EccentricDistance;                    // D                    Distance between foci and radius
        public float   Periapsis;                            // q                    Lowest distance between orbit and center of gravity
        public float   Apoapsis;                             // Q                    Highest distance between orbit and center of gravity
        public float   StandardGravitationalParameter;       // μ                    μ = G(m1 + m2)

        public float   HeliocentricDistance;                 // r                    Distance from central body at current position
                                                             //  c

        public SystemViewBody CentralBody;
        public SystemViewBody Self;

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
            y = y * (1.5f - (x2 * y * y));

            return y;
        }

        public OrbitingObjectDescriptor(SystemViewBody Body)
        {
            EccentricityVector = new float[2];
            Self               = Body;
        }

        public OrbitingObjectDescriptor(OrbitingObjectDescriptor Descriptor, SystemViewBody Body)
        {
            EccentricityVector = new float[2];
            Self               = Body;

            Copy(Descriptor);
        }

        public OrbitingObjectDescriptor(SystemViewBody Body, SystemViewBody Center, float a, float b, float w, float M)
        {
            SemiMajorAxis = a;
            SemiMinorAxis = b;
            Rotation      = w;
            MeanAnomaly   = M;
            
            CentralBody   = Center;
            Self          = Body;

            Compute();
        }

        public void Copy(OrbitingObjectDescriptor Descriptor)
        {
            EccentricityVector[0]          = Descriptor.EccentricityVector[0];
            EccentricityVector[1]          = Descriptor.EccentricityVector[1];
            CentralBody                    = Descriptor.CentralBody;
            SemiMinorAxis                  = Descriptor.SemiMinorAxis;
            SemiMajorAxis                  = Descriptor.SemiMajorAxis;
            Rotation                       = Descriptor.Rotation;
            OrbitalPeriod                  = Descriptor.OrbitalPeriod;
            MeanMotion                     = Descriptor.MeanMotion;
            MeanAnomaly                    = Descriptor.MeanAnomaly;
            EccentricAnomaly               = Descriptor.EccentricAnomaly;
            TrueAnomaly                    = Descriptor.TrueAnomaly;
            Eccentricity                   = Descriptor.Eccentricity;
            EccentricDistance              = Descriptor.EccentricDistance;
            Periapsis                      = Descriptor.Periapsis;
            Apoapsis                       = Descriptor.Apoapsis;

            Self.PosX                      = Descriptor.Self.PosX;
            Self.PosY                      = Descriptor.Self.PosY;
            Self.VelX                      = Descriptor.Self.VelX;
            Self.VelY                      = Descriptor.Self.VelY;
            HeliocentricDistance           = Descriptor.HeliocentricDistance;

            StandardGravitationalParameter = GravitationalConstant * CentralBody.Mass;
        }

        public void Compute()
        {
            // μ = GM

            StandardGravitationalParameter = GravitationalConstant * CentralBody.Mass;

            //        μ
            // n = √ --
            //       a³

            MeanMotion           = (float)Math.Sqrt(StandardGravitationalParameter / (SemiMajorAxis * SemiMajorAxis * SemiMajorAxis));

            //     2*π
            // P = ---
            //      n

            OrbitalPeriod        = 2.0f * 3.1415926f / MeanMotion;

            EccentricAnomaly     = GetEccentricAnomalyAt(MeanAnomaly);
            TrueAnomaly          = GetTrueAnomaly(EccentricAnomaly);
            HeliocentricDistance = GetDistanceFromCenterAt(TrueAnomaly);

            float[] Pos          = GetPositionAt(TrueAnomaly, HeliocentricDistance);
            float[] Vel          = GetVelocityAt(HeliocentricDistance, EccentricAnomaly);

            Self.PosX            = Pos[0];
            Self.PosY            = Pos[1];
            Self.VelX            = Vel[0];
            Self.VelY            = Vel[1];

            //               b²
            // ε = √ (1.0f - --)
            //               a²

            Eccentricity = (float)Math.Sqrt(1.0f - (SemiMinorAxis * SemiMinorAxis) / (SemiMajorAxis * SemiMajorAxis));

            Periapsis = GetDistanceFromCenterAt(0.0f);
            Apoapsis  = GetDistanceFromCenterAt(3.1415926f);

            EccentricDistance = SemiMajorAxis - Periapsis;

            // Eccentricity vector is a vector pointing from apoapsis to periapsis, with magnitude equal to the scalar eccentricity

            float[] ApoapsisPos  = GetPositionAt(3.1415926f, Apoapsis);
            float[] PeriapsisPos = GetPositionAt(0.0f,       Periapsis);

            EccentricityVector[0] = PeriapsisPos[0] - ApoapsisPos[0];
            EccentricityVector[1] = PeriapsisPos[1] - ApoapsisPos[1];

            float Magnitude = (float)Math.Sqrt(EccentricityVector[0] * EccentricityVector[0] + EccentricityVector[1] * EccentricityVector[1]);

            EccentricityVector[0] = EccentricityVector[0] / Magnitude * Eccentricity;
            EccentricityVector[1] = EccentricityVector[1] / Magnitude * Eccentricity;
        }

        public float GetEccentricAnomalyAt(float Mean)
        {
            // Eccentric anomaly is defined by Kepler's equation

            // M = E - ε sin(E)

            // However this does not have a closed form solution, so
            // we use Newton's method to approximate eccentric anomaly

            // Newton's method:
            // 
            //             f (xn)
            // x    = x  - ------
            //  n+1    n   f'(xn)

            // Applied to eccentric anomaly formula:
            // 
            //             En - ε sin(En) - M
            // E    = E  - ------------------
            //  n+1    n      1 - ε cos(En)


            while (Mean < 0.0f) Mean = 2.0f * 3.1415926f + Mean;
            while (Mean > 2.0f * 3.1415926f) Mean -= 2.0f * 3.1415926f;

            float Estimate = Mean;

            float Result   = 0.0f;

            const float Delta = 1E-5f;

            do
            {
                Estimate = Estimate - (Estimate - Eccentricity * (float)Math.Sin(Estimate) - Mean) / (1.0f - Eccentricity * (float)Math.Cos(Estimate));

                while (Estimate < 0.0f) Estimate = 2.0f * 3.1415926f + Estimate;
                while (Estimate > 2.0f * 3.1415926f) Estimate -= 2.0f * 3.1415926f;

                Result   =  Estimate  - Eccentricity * (float)Math.Sin(Estimate);
            } while (Result - Mean > Delta || Result - Mean < -Delta);

            return Estimate;
        }

        public float GetEccentricAnomaly()
        {
            return GetEccentricAnomalyAt(MeanAnomaly);
        }

        public float GetTrueAnomaly(float EccentricAnomaly)
        {
            //              1 + e     E
            // ν = 2 atan(√ ----- tan(-))
            //              1 - e     2

            return 2.0f * (float)Math.Atan(Math.Sqrt((1.0f + Eccentricity) / (1.0f - Eccentricity)) * Math.Tan(EccentricAnomaly / 2.0f));
        }

        public float[] GetPositionAt(float True, float Radius)
        {
            // →          cos(ν)
            // r = r  * ( sin(ν) )
            //      c       0

            float posx = (float)Math.Cos(True) * Radius;
            float posy = (float)Math.Sin(True) * Radius;

            // Rotate the position along the orbit's rotational offset
            
            // Sine and cosine of the rotational offset
            float rotsin = (float)Math.Sin(Rotation);
            float rotcos = (float)Math.Cos(Rotation);

            float[] pos = new float[2];

            pos[0] = rotcos * posx - rotsin * posy + CentralBody.PosX;
            pos[1] = rotsin * posx + rotcos * posy + CentralBody.PosY;

            return pos;
        }

        public float[] GetPosition()
        {
            return GetPositionAt(TrueAnomaly, HeliocentricDistance);
        }

        public float GetDistanceFromCenterAt(float True)
        {
            //           1 - e²
            // r = a --------------
            //       1 + e * cos(ν)

            return SemiMajorAxis * (1 - Eccentricity * Eccentricity) / (1 + Eccentricity * (float)Math.Cos(True));
        }

        public float GetDistanceFromCenter()
        {
            return GetDistanceFromCenterAt(GetTrueAnomaly(GetEccentricAnomalyAt(MeanAnomaly)));
        }

        // this function is basically the reverse of the GetPositionAt function
        public float GetRotationalPositionAt(float x, float y)
        {
            float rotsin = (float)Math.Sin(Rotation);
            float rotcos = (float)Math.Cos(Rotation);

            // not needed - will never happen
            // if (rotcos * rotcos + rotsin * rotsin == 0.0f) throw new IndexOutOfRangeException();

            // not needed - only one of the two coordinates are needed
            // float posx = (((x - CentralBody.PosX) * rotcos + (y - CentralBody.PosY) * rotsin) / (rotcos * rotcos + rotsin * rotsin) + GetEccentricDistance()) / SemiMajorAxis;

            float posy = ((y - CentralBody.PosY) * rotcos + (CentralBody.PosX - x) * rotsin) / ((rotcos * rotcos + rotsin * rotsin) * SemiMinorAxis);

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

            //     x²   y²                           b √ (a² - x²)
            // (2) -- + -- = 1            =>   y = ± -------------
            //     a²   b²                                 a

            //            b √ (a² - x²)                    a * b
            // (3) mx = ± -------------   =>   x = ± ----------------
            //                  a                    √ (a² * m² + b²)

            // Use fast inverse square root for faster speed
            Intersection1[0] = SemiMajorAxis * SemiMinorAxis * Q_rsqrt(SemiMajorAxis * SemiMajorAxis * slope * slope + SemiMinorAxis * SemiMinorAxis);
            Intersection1[1] = slope * Intersection1[0];

            Intersection2[0] = -Intersection1[0];
            Intersection2[1] = -Intersection1[1];

            // Rotate points to match orbit's rotation

            // (1) x = cos(ω) * x_0 - sin(ω) * y_0 + cx

            // (2) y = sin(ω) * x_0 + cos(ω) * y_0 + cy

            (Intersection1[0], Intersection1[1]) = (rotcos * Intersection1[0] - rotsin * Intersection1[1] + CentralBody.PosX,
                                                    rotsin * Intersection1[0] + rotcos * Intersection1[1] + CentralBody.PosY);
            (Intersection2[0], Intersection2[1]) = (rotcos * Intersection2[0] - rotsin * Intersection2[1] + CentralBody.PosX,
                                                    rotsin * Intersection2[0] + rotcos * Intersection2[1] + CentralBody.PosY);

            // (1) d = √(𐤃x² + 𐤃y²)

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
            OrbitingObjectDescriptor Start = new OrbitingObjectDescriptor(this, Self);
            float[] StartPos = GetPosition();

            // Turn orbit so that our rotational position is 0
            Rotation += TrueAnomaly;
            MeanAnomaly = 0.0f;

            // Find intersection between planned orbit and destination orbit
            float[] IntersectionAt = Destination.GetIntersectionWith(StartPos[0], StartPos[1], (float)Math.Tan(Rotation));

            // Start altitude = current altitude of start object
            float StartAltitude = Start.GetDistanceFromCenter();

            // Destination altitude = altitude at intersection point
            float DestinationAltitude = Destination.GetDistanceFromCenterAt(Destination.GetRotationalPositionAt(IntersectionAt[0], IntersectionAt[1]));

            // Choose periapsis and apoapsis from start/destination altitude (lower value = periapsis, higher value = apoapsis)
            Periapsis = StartAltitude < DestinationAltitude ? StartAltitude : DestinationAltitude;
            Apoapsis  = StartAltitude > DestinationAltitude ? StartAltitude : DestinationAltitude;

            // Rotate orbit 180 degrees if we're going from high altitude to low
            if (StartAltitude > DestinationAltitude)
            {
                Rotation += 3.1415926f;
                MeanAnomaly = 3.1415926f;
            }

            // Semi major axis = the longer "radius" of the ellipse
            // Can be calculated by adding periapsis and apoapsis together as they are the 2 farthest points on the orbit, and then dividing it in half
            SemiMajorAxis = (Periapsis + Apoapsis) / 2.0f;

            // This value represents how far off center the periapsis and apoapsis are, in other words it's the distance between the periapsis/apoapsis and the nearest focal point
            EccentricDistance = SemiMajorAxis - Periapsis;

            // (1) E     = √(a² - b²)

            // (2) E     = a - q

            // (3) a - q = √(a² - b²)   =>   b = √(q * (2a - q))

            SemiMinorAxis = (float)Math.Sqrt(Periapsis * (2 * SemiMajorAxis - Periapsis));

            // Compute all other values
            Compute();

            float TimeToEncounter          = OrbitalPeriod / 2.0f;
            float TargetRotationalMovement = (TimeToEncounter / Destination.OrbitalPeriod) * 2.0f * 3.1415926f;

            float True = Destination.GetTrueAnomaly(Destination.GetEccentricAnomalyAt(Destination.MeanAnomaly + TargetRotationalMovement));
            float[] TargetPosAtEncounter = Destination.GetPositionAt(True, Destination.GetDistanceFromCenterAt(True));

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

            Copy(Start);

            return false;
        }

        public float[] GetVelocityAt(float Radius, float E)
        {
            // →   √ (μa)        -sin(E)
            // v = ------ ( √(1 - e²) cos(E) )
            //       rc             0

            float Factor = (float)Math.Sqrt(StandardGravitationalParameter * SemiMajorAxis) / Radius;

            float SpeedX = -Factor * (float) Math.Sin(E);
            float SpeedY =  Factor * (float)(Math.Sqrt(1 - Eccentricity * Eccentricity) * Math.Cos(E));

            // Rotate the velocity along the orbit's rotational offset

            // Sine and cosine of the rotational offset
            float rotsin = (float)Math.Sin(Rotation);
            float rotcos = (float)Math.Cos(Rotation);

            float[] Velocity = new float[2];

            Velocity[0] = rotcos * SpeedX - rotsin * SpeedY;
            Velocity[1] = rotsin * SpeedX + rotcos * SpeedY;

            return Velocity;
        }

        public float[] GetVelocity()
        {
            return GetVelocityAt(HeliocentricDistance, EccentricAnomaly);
        }

        public void UpdatePosition(float dt)
        {
            MeanAnomaly         += dt * MeanMotion;
            EccentricAnomaly     = GetEccentricAnomalyAt(MeanAnomaly);
            TrueAnomaly          = GetTrueAnomaly(EccentricAnomaly);
            HeliocentricDistance = GetDistanceFromCenterAt(TrueAnomaly);
            float[] Pos          = GetPositionAt(TrueAnomaly, HeliocentricDistance);
            float[] Vel          = GetVelocityAt(HeliocentricDistance, EccentricAnomaly);
            Self.PosX            = Pos[0];
            Self.PosY            = Pos[1];
            Self.VelX            = Vel[0];
            Self.VelY            = Vel[1];
        }

        /*public void DebugOut()
        {
            Debug.Log("Orbiting object debug log: ");
            Debug.Log("a: " + SemiMajorAxis + " b: " + SemiMinorAxis + " ω: " + Rotation + " P: " + OrbitalPeriod + " μ: " + StandardGravitationalParameter + " ε: " + Eccentricity + " D: " + EccentricDistance);
            Debug.Log("n: " + MeanMotion + " M: " + MeanAnomaly + " E: " + EccentricAnomaly + " ν: " + TrueAnomaly);
            Debug.Log("x: " + Self.PosX + " y: " + Self.PosX + " vx: " + Self.VelX + " vy: " + Self.VelY);
        }*/

        public void ChangeFrameOfReference(SystemViewBody NewFrameOfReference)
        {
            float PosX = Self.PosX - CentralBody.PosX;
            float PosY = Self.PosY - CentralBody.PosY;

            float VelX = Self.VelX - CentralBody.VelX;
            float VelY = Self.VelY - CentralBody.VelY;

            CentralBody = NewFrameOfReference;

            // μ = GM

            StandardGravitationalParameter = GravitationalConstant * CentralBody.Mass;

            // h = r x v   =>   h = x * v  - y * v
            //                           y        x

            float AngularMomentum = PosX * VelY - PosY * VelX;

            //                     v  * h          v  * h
            //     v x h    r       y        x      x        y
            // e = ----- - --- = ( ------ - ---, - ------ - --- )
            //       μ     |r|        μ     |r|       μ     |r|

            float PosMagnitude = (float)Math.Sqrt(PosX * PosX + PosY * PosY);

            EccentricityVector[0] =  (VelY * AngularMomentum / StandardGravitationalParameter) - PosX / PosMagnitude;
            EccentricityVector[1] = -(VelX * AngularMomentum / StandardGravitationalParameter) - PosY / PosMagnitude;

            // ε = |e|

            Eccentricity = (float)Math.Sqrt(EccentricityVector[0] * EccentricityVector[0] + EccentricityVector[1] * EccentricityVector[1]);

            //         h²
            // a = ---------
            //     μ(1 - ε²)

            SemiMajorAxis = AngularMomentum * AngularMomentum / (StandardGravitationalParameter * (1.0f - Eccentricity * Eccentricity));

            //            b²
            // ε = √ (1 - -)   =>    b = √ ((1 - ε²) * a²)
            //            a²

            SemiMinorAxis = (float)Math.Sqrt(SemiMajorAxis * SemiMajorAxis * (1.0f - Eccentricity * Eccentricity));

            //           e
            //            y
            // ω = atan( -- )
            //           e
            //            x

            Rotation = (float)Math.Atan(EccentricityVector[1] / EccentricityVector[0]);

            //        μ
            // n = √ --
            //       a³

            MeanMotion = (float)Math.Sqrt(StandardGravitationalParameter / (SemiMajorAxis * SemiMajorAxis * SemiMajorAxis));

            //     2*π
            // P = ---
            //      n

            OrbitalPeriod = 2.0f * 3.1415926f / MeanMotion;

            Periapsis = GetDistanceFromCenterAt(0.0f);
            Apoapsis = GetDistanceFromCenterAt(3.1415926f);

            EccentricDistance = SemiMajorAxis - Periapsis;

            // -- this is wrong -- //

            //                                      r            r
            // →          cos(ν)                     x            y
            // r = r  * ( sin(ν) )   =>   ν = acos( -- ) = asin( -- )
            //      c       0                       r            r
            //                                       c            c

            float Radius = (float)Math.Sqrt(PosX * PosX + PosY * PosY);

            TrueAnomaly = (float)Math.Acos(PosX / Radius);

            //          x                  x
            // cos(E) = -   =>   E = acos( - )
            //          a                  a

            EccentricAnomaly = (float)Math.Acos(PosX / SemiMajorAxis);

            // M = E - ε sin(E)

            MeanAnomaly = EccentricAnomaly - Eccentricity * (float)Math.Sin(EccentricAnomaly);

            if (EccentricityVector[0] < 0.0f) Rotation += 3.1415926f;

            // DebugOut();
        }
    }
}
