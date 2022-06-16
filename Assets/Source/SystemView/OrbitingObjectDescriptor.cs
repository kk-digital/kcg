using System;
// using UnityEngine; // For DebugOut

namespace SystemView
{
    // Class that defines the behaviors of orbiting objects
    // Will be used to define orbiting parameters of planets, asteroids, ships, etc.
    public class OrbitingObjectDescriptor
    {
        public float   semimajoraxis;                        // a                    Half of the major axis
        public float   semiminoraxis;                        // b                    Half of the minor axis
        public float   rotation;                             // ω                    Orbit's rotation across the Z-axis
        public float   orbital_period;                       // P                    Amount of time needed for body to complete an orbit
        public float   mean_motion;                          // n                    Angular speed required to complete the orbit assuming constant speed
        public float   mean_anomaly;                         // M                    Position as a fraction of the body's orbital period
        public float   eccentric_anomaly;                    // E                    Required to calculate true anomaly and velocity
        public float   true_anomaly;                         // ν                    Real angular position of body
        public float[] eccentricity_vector;                  // e                    Dimensionless vector pointing from apoapsis to periapsis, with magnitude equal to the scalar eccentricity
        public float   eccentricity;                         // ε                    Orbital eccentricity (ε = 0: central, 0 < ε < 1: elliptical, ε = 1: parabolic, ε > 1: hyperbolic)
        public float   linear_eccentricity;                  // D                    Distance between foci and radius
        public float   periapsis;                            // q                    Lowest distance between orbit and center of gravity
        public float   apoapsis;                             // Q                    Highest distance between orbit and center of gravity
        public float   standard_gravitational_parameter;     // μ                    μ = G(m1 + m2)

        public float   heliocentric_distance;                // r                    Distance from central body at current position
                                                             //  c

        public SpaceObject central_body;
        public SpaceObject self;

        public OrbitingObjectDescriptor(SpaceObject obj)
        {
            eccentricity_vector = new float[2];
            self                = obj;
        }

        public OrbitingObjectDescriptor(OrbitingObjectDescriptor descriptor)
        {
            eccentricity_vector = new float[2];
            self                = new();

            Copy(descriptor);
        }

        public OrbitingObjectDescriptor(OrbitingObjectDescriptor descriptor, SpaceObject obj)
        {
            eccentricity_vector = new float[2];
            self                = obj;

            Copy(descriptor);
        }

        public OrbitingObjectDescriptor(SpaceObject obj, SpaceObject center, float a, float b, float w, float M)
        {
            semimajoraxis = a;
            semiminoraxis = b;
            rotation      = w;
            mean_anomaly  = M;
            
            central_body  = center;
            self          = obj;

            Compute();
        }

        public void Copy(OrbitingObjectDescriptor descriptor)
        {
            eccentricity_vector[0]           = descriptor.eccentricity_vector[0];
            eccentricity_vector[1]           = descriptor.eccentricity_vector[1];
            central_body                     = descriptor.central_body;
            semiminoraxis                    = descriptor.semiminoraxis;
            semimajoraxis                    = descriptor.semimajoraxis;
            rotation                         = descriptor.rotation;
            orbital_period                   = descriptor.orbital_period;
            mean_motion                      = descriptor.mean_motion;
            mean_anomaly                     = descriptor.mean_anomaly;
            eccentric_anomaly                = descriptor.eccentric_anomaly;
            true_anomaly                     = descriptor.true_anomaly;
            eccentricity                     = descriptor.eccentricity;
            linear_eccentricity              = descriptor.linear_eccentricity;
            periapsis                        = descriptor.periapsis;
            apoapsis                         = descriptor.apoapsis;
            heliocentric_distance            = descriptor.heliocentric_distance;

            self.posx                        = descriptor.self.posx;
            self.posy                        = descriptor.self.posy;
            self.velx                        = descriptor.self.velx;
            self.vely                        = descriptor.self.vely;

            standard_gravitational_parameter = Tools.gravitational_constant * central_body.mass;
        }

        public void Compute()
        {
            // μ = GM

            standard_gravitational_parameter = Tools.gravitational_constant * central_body.mass;

            //        μ
            // n = √ --
            //       a³

            mean_motion            = (float)Math.Sqrt(standard_gravitational_parameter / (semimajoraxis * semimajoraxis * semimajoraxis));

            //     2*π
            // P = ---
            //      n
            
            orbital_period         = Tools.twopi / mean_motion;

            eccentric_anomaly      = GetEccentricAnomalyAt(mean_anomaly);
            true_anomaly           = GetTrueAnomaly(eccentric_anomaly);
            heliocentric_distance  = GetDistanceFromCenterAt(true_anomaly);

            float[] pos            = GetPositionAt(true_anomaly, heliocentric_distance);
            float[] vel            = GetVelocityAt(heliocentric_distance, eccentric_anomaly);

            self.posx              = pos[0];
            self.posy              = pos[1];
            self.velx              = vel[0];
            self.vely              = vel[1];

            //               b²
            // ε = √ (1.0f - --)
            //               a²

            eccentricity           = (float)Math.Sqrt(1.0f - (semiminoraxis * semiminoraxis) / (semimajoraxis * semimajoraxis));

            periapsis              = GetDistanceFromCenterAt(0.0f);
            apoapsis               = GetDistanceFromCenterAt(Tools.pi);

            linear_eccentricity    = semimajoraxis - periapsis;

            // Eccentricity vector is a vector pointing from apoapsis to periapsis, with magnitude equal to the scalar eccentricity

            float[] ApoapsisPos    = GetPositionAt(Tools.pi, apoapsis);
            float[] PeriapsisPos   = GetPositionAt(0.0f,    periapsis);

            eccentricity_vector[0] = PeriapsisPos[0] - ApoapsisPos[0];
            eccentricity_vector[1] = PeriapsisPos[1] - ApoapsisPos[1];

            float Magnitude        = Tools.magnitude(eccentricity_vector);

            eccentricity_vector[0] = eccentricity_vector[0] / Magnitude * eccentricity;
            eccentricity_vector[1] = eccentricity_vector[1] / Magnitude * eccentricity;
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


            while (Mean <        0.0f) Mean  = Tools.twopi + Mean;
            while (Mean > Tools.twopi) Mean -= Tools.twopi;

            float Estimate    = Mean;

            float Result      = 0.0f;

            const float Delta = 1E-5f;

            do
            {
                Estimate      = Estimate - (Estimate - eccentricity * (float)Math.Sin(Estimate) - Mean) / (1.0f - eccentricity * (float)Math.Cos(Estimate));

                while (Estimate <        0.0f) Estimate  = Tools.twopi + Estimate;
                while (Estimate > Tools.twopi) Estimate -= Tools.twopi;

                Result        =  Estimate  - eccentricity * (float)Math.Sin(Estimate);
            } while (Result - Mean > Delta || Result - Mean < -Delta);

            return Estimate;
        }

        public float GetEccentricAnomaly()
        {
            return GetEccentricAnomalyAt(mean_anomaly);
        }

        public float GetTrueAnomaly(float EccentricAnomaly)
        {
            //              1 + ε     E
            // ν = 2 atan(√ ----- tan(-))
            //              1 - ε     2

            return 2.0f * (float)Math.Atan(Math.Sqrt((1.0f + eccentricity) / (1.0f - eccentricity)) * Math.Tan(EccentricAnomaly / 2.0f));
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
            float rotsin = (float)Math.Sin(rotation);
            float rotcos = (float)Math.Cos(rotation);

            float[] pos = new float[2];

            pos[0] = rotcos * posx - rotsin * posy + central_body.posx;
            pos[1] = rotsin * posx + rotcos * posy + central_body.posy;

            return pos;
        }

        public float[] GetPosition()
        {
            return GetPositionAt(true_anomaly, heliocentric_distance);
        }

        public float GetDistanceFromCenterAt(float True)
        {
            //           1 - ε²
            // r = a --------------
            //       1 + ε * cos(ν)

            return semimajoraxis * (1 - eccentricity * eccentricity) / (1 + eccentricity * (float)Math.Cos(True));
        }

        public float GetDistanceFromCenter()
        {
            return GetDistanceFromCenterAt(GetTrueAnomaly(GetEccentricAnomalyAt(mean_anomaly)));
        }

        // this function is basically the reverse of the GetPositionAt function
        public float GetRotationalPositionAt(float x, float y)
        {
            float rotsin = (float)Math.Sin(rotation);
            float rotcos = (float)Math.Cos(rotation);

            // not needed - will never happen
            // if (rotcos * rotcos + rotsin * rotsin == 0.0f) throw new IndexOutOfRangeException();

            // not needed - only one of the two coordinates are needed
            // float posx = (((x - CentralBody.PosX) * rotcos + (y - CentralBody.PosY) * rotsin) / (rotcos * rotcos + rotsin * rotsin) + GetEccentricDistance()) / SemiMajorAxis;

            float posy = ((y - central_body.posy) * rotcos + (central_body.posx - x) * rotsin) / ((rotcos * rotcos + rotsin * rotsin) * semiminoraxis);

            return (float)Math.Asin(posy);
        }

        public float[] GetIntersectionWith(float startx, float starty, float slope)
        {
            // sine and cosine of the orbit's rotation
            // would be faster if C# had a function that calls x86's fsincos instruction (as it calculates both sin and cos in one single instruction)
            float rotsin = (float)Math.Sin(rotation);
            float rotcos = (float)Math.Cos(rotation);

            // Find both intersection points and return the farther one
            float[] Intersection1 = new float[2];
            float[] Intersection2 = new float[2];

            // Rotate slope to match rotation of orbit
            slope = (float)Math.Tan(Math.Atan(slope) - rotation);

            // (1) y = mx

            //     x²   y²                           b √ (a² - x²)
            // (2) -- + -- = 1            =>   y = ± -------------
            //     a²   b²                                 a

            //            b √ (a² - x²)                    a * b
            // (3) mx = ± -------------   =>   x = ± ----------------
            //                  a                    √ (a² * m² + b²)

            // Use fast inverse square root for faster speed
            Intersection1[0] = semimajoraxis * semiminoraxis / (float)Math.Sqrt(semimajoraxis * semimajoraxis * slope * slope + semiminoraxis * semiminoraxis);
            Intersection1[1] = slope * Intersection1[0];
 
            Intersection2[0] = -Intersection1[0];
            Intersection2[1] = -Intersection1[1];
 
            // Rotate points to match orbit's rotation
 
            // (1) x = cos(ω) * x_0 - sin(ω) * y_0 + cx
 
            // (2) y = sin(ω) * x_0 + cos(ω) * y_0 + cy
 
            (Intersection1[0], Intersection1[1]) = (rotcos * Intersection1[0] - rotsin * Intersection1[1] + central_body.posx,
                                                    rotsin * Intersection1[0] + rotcos * Intersection1[1] + central_body.posy);
            (Intersection2[0], Intersection2[1]) = (rotcos * Intersection2[0] - rotsin * Intersection2[1] + central_body.posx,
                                                    rotsin * Intersection2[0] + rotcos * Intersection2[1] + central_body.posy);
 
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
            OrbitingObjectDescriptor Start = new OrbitingObjectDescriptor(this);
            float[] StartPos = GetPosition();

            // Turn orbit so that our rotational position is 0
            rotation   += true_anomaly;
            mean_anomaly = 0.0f;

            // Find intersection between planned orbit and destination orbit
            float[] IntersectionAt    = Destination.GetIntersectionWith(StartPos[0], StartPos[1], (float)Math.Tan(rotation));

            // Start altitude = current altitude of start object
            float StartAltitude       = Start.GetDistanceFromCenter();

            // Destination altitude = altitude at intersection point
            float DestinationAltitude = Destination.GetDistanceFromCenterAt(Destination.GetRotationalPositionAt(IntersectionAt[0], IntersectionAt[1]));

            // Choose periapsis and apoapsis from start/destination altitude (lower value = periapsis, higher value = apoapsis)
            periapsis = StartAltitude < DestinationAltitude ? StartAltitude : DestinationAltitude;
            apoapsis  = StartAltitude > DestinationAltitude ? StartAltitude : DestinationAltitude;

            // Rotate orbit 180 degrees if we're going from high altitude to low
            if (StartAltitude > DestinationAltitude)
            {
                rotation   += Tools.pi;
                mean_anomaly = Tools.pi;
            }

            // Semi major axis = the longer "radius" of the ellipse
            // Can be calculated by adding periapsis and apoapsis together as they are the 2 farthest points on the orbit, and then dividing it in half
            semimajoraxis = (periapsis + apoapsis) / 2.0f;

            // This value represents how far off center the periapsis and apoapsis are, in other words it's the distance between the periapsis/apoapsis and the nearest focal point
            linear_eccentricity = semimajoraxis - periapsis;

            // (1) E     = √(a² - b²)

            // (2) E     = a - q

            // (3) a - q = √(a² - b²)   =>   b = √(q * (2a - q))

            semiminoraxis = (float)Math.Sqrt(periapsis * (2 * semimajoraxis - periapsis));

            // Compute all other values
            Compute();

            float TimeToEncounter          = orbital_period * 0.5f;
            float TargetRotationalMovement = (TimeToEncounter / Destination.orbital_period) * Tools.twopi;

            float True                   = Destination.GetTrueAnomaly(Destination.GetEccentricAnomalyAt(Destination.mean_anomaly + TargetRotationalMovement));
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

        public float[] ForceEncounter(OrbitingObjectDescriptor Destination, float Acceleration)
        {
            float[] RequiredVelocityChange = new float[2];

            OrbitingObjectDescriptor NewOrbit = new OrbitingObjectDescriptor(this);

            // todo: this function

            return RequiredVelocityChange; 
        }

        public float[] GetVelocityAt(float Radius, float E)
        {
            // →   √ (μa)        -sin(E)
            // v = ------ ( √(1 - ε²) cos(E) )
            //       rc             0

            float Factor = (float)Math.Sqrt(standard_gravitational_parameter * semimajoraxis) / Radius;

            float SpeedX = -Factor * (float) Math.Sin(E);
            float SpeedY =  Factor * (float)(Math.Sqrt(1 - eccentricity * eccentricity) * Math.Cos(E));

            // Rotate the velocity along the orbit's rotational offset

            // Sine and cosine of the rotational offset
            float rotsin = (float)Math.Sin(rotation);
            float rotcos = (float)Math.Cos(rotation);

            float[] Velocity = new float[2];

            Velocity[0] = rotcos * SpeedX - rotsin * SpeedY;
            Velocity[1] = rotsin * SpeedX + rotcos * SpeedY;

            return Velocity;
        }

        public float[] GetVelocity()
        {
            return GetVelocityAt(heliocentric_distance, eccentric_anomaly);
        }

        public void UpdatePosition(float dt)
        {
            mean_anomaly         += dt * mean_motion;
            eccentric_anomaly     = GetEccentricAnomalyAt(mean_anomaly);
            true_anomaly          = GetTrueAnomaly(eccentric_anomaly);
            heliocentric_distance = GetDistanceFromCenterAt(true_anomaly);
            float[] Pos          = GetPositionAt(true_anomaly, heliocentric_distance);
            float[] Vel          = GetVelocityAt(heliocentric_distance, eccentric_anomaly);
            self.posx            = Pos[0];
            self.posy            = Pos[1];
            self.velx            = Vel[0];
            self.vely            = Vel[1];

            if    (mean_anomaly      < 0.0f)              mean_anomaly      += Tools.twopi;
            while (mean_anomaly      > Tools.twopi) mean_anomaly      -= Tools.twopi;

            if    (eccentric_anomaly < 0.0f)              eccentric_anomaly += Tools.twopi;
            if    (true_anomaly      < 0.0f)              true_anomaly      += Tools.twopi;
        }

        /*public void DebugOut()
        {
            Debug.Log("Orbiting object debug log: ");
            Debug.Log("a: " + SemiMajorAxis + " b: " + SemiMinorAxis + " ω: " + Rotation + " P: " + OrbitalPeriod + " μ: " + StandardGravitationalParameter + " ε: " + Eccentricity + " D: " + EccentricDistance);
            Debug.Log("n: " + MeanMotion + " M: " + MeanAnomaly + " E: " + EccentricAnomaly + " ν: " + TrueAnomaly);
            Debug.Log("x: " + Self.PosX + " y: " + Self.PosX + " vx: " + Self.VelX + " vy: " + Self.VelY);
        }*/

        public void ChangeFrameOfReference(SpaceObject NewFrameOfReference)
        {
            float PosX = self.posx - central_body.posx;
            float PosY = self.posy - central_body.posy;

            float VelX = self.velx - central_body.velx;
            float VelY = self.vely - central_body.vely;

            central_body = NewFrameOfReference;

            // μ = GM

            standard_gravitational_parameter = Tools.gravitational_constant * central_body.mass;

            // h = r x v   =>   h = x * v  - y * v
            //                           y        x

            float AngularMomentum = PosX * VelY - PosY * VelX;

            //                     v  * h          v  * h
            //     v x h    r       y        x      x        y
            // e = ----- - --- = ( ------ - ---, - ------ - --- )
            //       μ     |r|        μ     |r|       μ     |r|

            float PosMagnitude = (float)Math.Sqrt(PosX * PosX + PosY * PosY);

            eccentricity_vector[0] =  (VelY * AngularMomentum / standard_gravitational_parameter) - PosX / PosMagnitude;
            eccentricity_vector[1] = -(VelX * AngularMomentum / standard_gravitational_parameter) - PosY / PosMagnitude;

            // ε = |e|

            eccentricity = (float)Math.Sqrt(eccentricity_vector[0] * eccentricity_vector[0] + eccentricity_vector[1] * eccentricity_vector[1]);

            //         h²
            // a = ---------
            //     μ(1 - ε²)

            semimajoraxis = AngularMomentum * AngularMomentum / (standard_gravitational_parameter * (1.0f - eccentricity * eccentricity));

            //            b²
            // ε = √ (1 - -)   =>    b = √ ((1 - ε²) * a²)
            //            a²

            semiminoraxis = (float)Math.Sqrt(semimajoraxis * semimajoraxis * (1.0f - eccentricity * eccentricity));

            //           e
            //            y
            // ω = atan( -- )
            //           e
            //            x

            rotation = (float)Math.Atan(eccentricity_vector[1] / eccentricity_vector[0]);

            //        μ
            // n = √ --
            //       a³

            mean_motion = (float)Math.Sqrt(standard_gravitational_parameter / (semimajoraxis * semimajoraxis * semimajoraxis));

            //     2*π
            // P = ---
            //      n

            orbital_period = Tools.twopi / mean_motion;

            periapsis = GetDistanceFromCenterAt(0.0f);
            apoapsis  = GetDistanceFromCenterAt(Tools.pi);

            linear_eccentricity = semimajoraxis - periapsis;
            
            if (eccentricity_vector[0] < 0.0f) rotation += Tools.pi;

            //                                      r            r
            // →          cos(ν)                     x            y
            // r = r  * ( sin(ν) )   =>   ν = acos( -- ) = asin( -- )
            //      c       0                       r            r
            //                                       c            c

            true_anomaly = (float)Math.Atan2(PosY, PosX) - rotation;

            //                                                 1 + ε             ν
            //              1 + ε     E                      √ ----- (1 - ε) tan(-)
            // ν = 2 atan(√ ----- tan(-))   =>   ε = 2 atan(   1 - ε             2  )
            //              1 - ε     2                      ---------------------- 
            //                                                        1 + ε

            float OnePlusEcc  = 1 + eccentricity;
            float OneMinusEcc = 1 - eccentricity;

            eccentric_anomaly = 2.0f * (float)Math.Atan((Math.Sqrt(OnePlusEcc / OneMinusEcc) * OneMinusEcc * Math.Tan(true_anomaly / 2)) / OnePlusEcc);

            // M = E - ε sin(E)

            mean_anomaly = eccentric_anomaly - eccentricity * (float)Math.Sin(eccentric_anomaly);

            if (mean_anomaly      < 0.0f) mean_anomaly      += Tools.twopi;
            if (eccentric_anomaly < 0.0f) eccentric_anomaly += Tools.twopi;
            if (true_anomaly      < 0.0f) true_anomaly      += Tools.twopi;
        }
    }
}
