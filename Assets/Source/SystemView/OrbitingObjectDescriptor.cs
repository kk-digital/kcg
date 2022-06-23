using System;
// using UnityEngine; // For DebugOut

namespace Source {
    namespace SystemView {
        // Class that defines the behaviors of orbiting objects
        // Will be used to define orbiting parameters of planets, asteroids, ships, etc.
        public class OrbitingObjectDescriptor {
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

            public OrbitingObjectDescriptor(SpaceObject obj) {
                eccentricity_vector = new float[2];
                self                = obj;
            }

            public OrbitingObjectDescriptor(OrbitingObjectDescriptor descriptor) {
                eccentricity_vector = new float[2];
                self                = new();

                copy(descriptor);
            }

            public OrbitingObjectDescriptor(OrbitingObjectDescriptor descriptor, SpaceObject obj) {
                eccentricity_vector = new float[2];
                self                = obj;

                copy(descriptor);
            }

            public OrbitingObjectDescriptor(SpaceObject obj, SpaceObject center, float a, float b, float w, float M) {
                semimajoraxis = a;
                semiminoraxis = b;
                rotation      = w;
                mean_anomaly  = M;
            
                central_body  = center;
                self          = obj;

                compute();
            }

            public void copy(OrbitingObjectDescriptor descriptor) {
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

            public void compute() {
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

                eccentric_anomaly      = get_eccentric_anomaly_at(mean_anomaly);
                true_anomaly           = get_true_anomaly(eccentric_anomaly);
                heliocentric_distance  = get_distance_from_center_at(true_anomaly);

                float[] pos            = get_position_at(true_anomaly, heliocentric_distance);
                float[] vel            = get_velocity_at(heliocentric_distance, eccentric_anomaly);

                self.posx              = pos[0];
                self.posy              = pos[1];
                self.velx              = vel[0];
                self.vely              = vel[1];

                //               b²
                // ε = √ (1.0f - --)
                //               a²

                eccentricity           = (float)Math.Sqrt(1.0f -(semiminoraxis * semiminoraxis) / (semimajoraxis * semimajoraxis));

                periapsis              = get_distance_from_center_at(0.0f);
                apoapsis               = get_distance_from_center_at(Tools.pi);

                linear_eccentricity    = semimajoraxis - periapsis;

                // Eccentricity vector is a vector pointing from apoapsis to periapsis, with magnitude equal to the scalar eccentricity

                float[] apoapsis_pos   = get_position_at(Tools.pi, apoapsis);
                float[] periapsis_pos  = get_position_at(0.0f,    periapsis);

                eccentricity_vector[0] = periapsis_pos[0] - apoapsis_pos[0];
                eccentricity_vector[1] = periapsis_pos[1] - apoapsis_pos[1];

                float magnitude        = Tools.magnitude(eccentricity_vector);

                eccentricity_vector[0] = eccentricity_vector[0] / magnitude * eccentricity;
                eccentricity_vector[1] = eccentricity_vector[1] / magnitude * eccentricity;
            }

            public float get_eccentric_anomaly_at(float mean) {
                if(eccentricity <= 1.0f) {
                    // Eccentric anomaly is defined by Kepler's equation

                    // M = E - ε sin(E)

                    // However this does not have a closed form solution, so
                    // we use Newton's method to approximate eccentric anomaly

                    // Newton's method:
                    // 
                    //             f(xn)
                    // x    = x  - ------
                    //  n+1    n   f'(xn)

                    // Applied to eccentric anomaly formula:
                    // 
                    //             En - ε sin(En) - M
                    // E    = E  - ------------------
                    //  n+1    n      1 - ε cos(En)


                    while(mean <        0.0f) mean  = Tools.twopi + mean;
                    while(mean > Tools.twopi) mean -= Tools.twopi;

                    float estimate                  =  mean;
                    float result                    =  0.0f;
                    const float delta               = 1E-5f;

                    do {
                        estimate -= (estimate - eccentricity * (float)Math.Sin(estimate) - mean) / (1.0f - eccentricity * (float)Math.Cos(estimate));

                        while(estimate <        0.0f) estimate  = Tools.twopi + estimate;
                        while(estimate > Tools.twopi) estimate -= Tools.twopi;

                        result = estimate  - eccentricity * (float)Math.Sin(estimate);
                    } while(result - mean > delta || result - mean < -delta);

                    return estimate;
                } else {
                    // Eccentric anomaly is defined by Kepler's equation

                    // M = ε sinh(E) - E

                    // However this does not have a closed form solution, so
                    // we use Newton's method to approximate eccentric anomaly

                    // Newton's method:
                    // 
                    //             f(xn)
                    // x    = x  - ------
                    //  n+1    n   f'(xn)

                    // Applied to eccentric anomaly formula:
                    // 
                    //             ε sinh(En) - En - M
                    // E    = E  - -------------------
                    //  n+1    n      ε cosh(En) - 1


                    while(mean <        0.0f) mean  = Tools.twopi + mean;
                    while(mean > Tools.twopi) mean -= Tools.twopi;

                    float estimate                  =  mean;
                    float result                    =  0.0f;
                    const float delta               = 1E-2f;

                    do {
                        estimate -= (eccentricity * (float)Math.Sinh(estimate) - estimate - mean) / (eccentricity * (float)Math.Cosh(estimate) - 1.0f);
                        
                        while(estimate <        0.0f) estimate  = Tools.twopi + estimate;
                        while(estimate > Tools.twopi) estimate -= Tools.twopi;

                        result = eccentricity * (float)Math.Sinh(estimate) - estimate;

                        while(result   <        0.0f) result    = Tools.twopi + result;
                        while(result   > Tools.twopi) result   -= Tools.twopi;
                    } while(result - mean > delta || result - mean < -delta);

                    return estimate;
                }
            }

            public float get_eccentric_anomaly() { return get_eccentric_anomaly_at(mean_anomaly); }

            public float get_true_anomaly() { return get_true_anomaly(get_eccentric_anomaly()); }

            public float get_true_anomaly(float eccentric) {
                if(eccentricity <= 1.0f) {

                    //              1 + ε      E
                    // ν = 2 atan(√ ----- tan(---))
                    //              1 - ε      2

                    return 2.0f * (float)Math.Atan (Math.Sqrt((1.0f + eccentricity) / (1.0f - eccentricity)) * Math.Tan (eccentric * 0.5f));

                } else {

                    //              ε - 1       E  
                    // ν = 2 atan(√ ----- tanh(---) (ε + 1))
                    //              ε + 1       2

                    return 2.0f * (float)Math.Atan(
                                         Math.Sqrt((eccentricity - 1.0f)  / (eccentricity + 1.0f))
                                *        Math.Tanh( eccentric    * 0.5f)) * (eccentricity + 1.0f);

                }
            }

            public float[] get_position_at(float true_anom, float radius) {
                // →          cos(ν)
                // r = r  * ( sin(ν) )
                //      c       0

                float posx = (float)Math.Cos(true_anom) * radius;
                float posy = (float)Math.Sin(true_anom) * radius;

                // Rotate the position along the orbit's rotational offset
            
                // Sine and cosine of the rotational offset
                float rotsin = (float)Math.Sin(rotation);
                float rotcos = (float)Math.Cos(rotation);

                float[] pos = new float[2];

                pos[0] = rotcos * posx - rotsin * posy + central_body.posx;
                pos[1] = rotsin * posx + rotcos * posy + central_body.posy;

                return pos;
            }

            public float[] get_position() {
                return get_position_at(true_anomaly, heliocentric_distance);
            }

            public float get_distance_from_center_at(float true_anom) {
                if(eccentricity < 1.0f) {

                    //           1 - ε²
                    // r = a --------------
                    //       1 + ε * cos(ν)

                    return semimajoraxis * (1 - eccentricity * eccentricity) / (1 + eccentricity * (float)Math.Cos(true_anom));

                } else {

                    //          (ε² - 1)
                    // r = a --------------
                    //       1 + ε * cos(ν) 

                    return semimajoraxis * (eccentricity * eccentricity - 1) / (1 + eccentricity * (float)Math.Cos(true_anom));

                }
            }

            public float get_distance_from_center() {
                return get_distance_from_center_at(get_true_anomaly(get_eccentric_anomaly_at(mean_anomaly)));
            }

            // this function is basically the reverse of the GetPositionAt function
            public float get_rotational_position_at(float x, float y) {
                float rotsin = (float)Math.Sin(rotation);
                float rotcos = (float)Math.Cos(rotation);

                // not needed - will never happen
                // if(rotcos * rotcos + rotsin * rotsin == 0.0f) throw new IndexOutOfRangeException();

                // not needed - only one of the two coordinates are needed
                // float posx = (((x - CentralBody.PosX) * rotcos +(y - CentralBody.PosY) * rotsin) / (rotcos * rotcos + rotsin * rotsin) + GetEccentricDistance()) / SemiMajorAxis;

                float posy = ((y - central_body.posy) * rotcos +(central_body.posx - x) * rotsin) / ((rotcos * rotcos + rotsin * rotsin) * semiminoraxis);

                return(float)Math.Asin(posy);
            }

            public float[] get_intersection_with(float startx, float starty, float slope) {
                // sine and cosine of the orbit's rotation
                // would be faster if C# had a function that calls x86's fsincos instruction(as it calculates both sin and cos in one single instruction)
                float rotsin = (float)Math.Sin(rotation);
                float rotcos = (float)Math.Cos(rotation);

                // Find both intersection points and return the farther one
                float[] intersection1 = new float[2];
                float[] intersection2 = new float[2];

                // Rotate slope to match rotation of orbit
                slope = (float)Math.Tan(Math.Atan(slope) - rotation);

                // (1) y = mx

                //     x²   y²                           b √(a² - x²)
                // (2) -- + -- = 1            =>   y = ± -------------
                //     a²   b²                                 a

                //            b √(a² - x²)                    a * b
                // (3) mx = ± -------------   =>   x = ± ----------------
                //                  a                    √(a² * m² + b²)

                // Use fast inverse square root for faster speed
                intersection1[0] = semimajoraxis * semiminoraxis / (float)Math.Sqrt(semimajoraxis * semimajoraxis * slope * slope + semiminoraxis * semiminoraxis);
                intersection1[1] = slope * intersection1[0];
 
                intersection2[0] = -intersection1[0];
                intersection2[1] = -intersection1[1];
 
                // Rotate points to match orbit's rotation
 
                // (1) x = cos(ω) * x_0 - sin(ω) * y_0 + cx
 
                // (2) y = sin(ω) * x_0 + cos(ω) * y_0 + cy
 
               (intersection1[0], intersection1[1]) = (rotcos * intersection1[0] - rotsin * intersection1[1] + central_body.posx,
                                                       rotsin * intersection1[0] + rotcos * intersection1[1] + central_body.posy);
               (intersection2[0], intersection2[1]) = (rotcos * intersection2[0] - rotsin * intersection2[1] + central_body.posx,
                                                       rotsin * intersection2[0] + rotcos * intersection2[1] + central_body.posy);
 
                // (1) d = √(𐤃x² + 𐤃y²)
 
                float d1 = Tools.magnitude((intersection1[0] - startx),(intersection1[1] - starty));
                float d2 = Tools.magnitude((intersection2[0] - startx),(intersection2[1] - starty));

                return d1 > d2 ? intersection1 : intersection2;
            }

            public float get_distance_from(OrbitingObjectDescriptor descriptor) {
                float[] pos = get_position();
                float[] target_pos = descriptor.get_position();

                return Tools.magnitude((pos[0] - target_pos[0]),(pos[1] - target_pos[1]));
            }

            public bool plan_path(OrbitingObjectDescriptor destination, float acceptable_deviation) {
                // Make a copy of the current state in case we cannot establish an orbit from our current position
                OrbitingObjectDescriptor start = new OrbitingObjectDescriptor(this);
                float[] start_pos = get_position();

                // Turn orbit so that our rotational position is 0
                rotation    += true_anomaly;
                mean_anomaly = 0.0f;

                // Find intersection between planned orbit and destination orbit
                float[] intersetion_at     = destination.get_intersection_with(start_pos[0], start_pos[1],(float)Math.Tan(rotation));

                // Start altitude = current altitude of start object
                float start_altitude       = start.get_distance_from_center();

                // Destination altitude = altitude at intersection point
                float destination_altitude = destination.get_distance_from_center_at(destination.get_rotational_position_at(intersetion_at[0], intersetion_at[1]));

                // Choose periapsis and apoapsis from start/destination altitude(lower value = periapsis, higher value = apoapsis)
                periapsis = start_altitude < destination_altitude ? start_altitude : destination_altitude;
                apoapsis  = start_altitude > destination_altitude ? start_altitude : destination_altitude;

                // Rotate orbit 180 degrees if we're going from high altitude to low
                if(start_altitude > destination_altitude) {
                    rotation    += Tools.pi;
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
                compute();

                float time_to_encounter          = orbital_period * 0.5f;
                float target_rotational_movement = (time_to_encounter / destination.orbital_period) * Tools.twopi;

                float true_anom                  = destination.get_true_anomaly(destination.get_eccentric_anomaly_at(destination.mean_anomaly + target_rotational_movement));
                float[] target_pos_at_encounter  = destination.get_position_at(true_anom, destination.get_distance_from_center_at(true_anom));

                // Check whether apoapsis is close enough to where the target will be to ensure an encounter
                float dx = intersetion_at[0] - target_pos_at_encounter[0];
                float dy = intersetion_at[1] - target_pos_at_encounter[1];

                dx *= dx;
                dy *= dy;

                if(dx + dy < acceptable_deviation * acceptable_deviation) return true;

                copy(start);

                return false;
            }

            public float[] calculate_required_deltav(OrbitingObjectDescriptor destination, float acceleration, float acceptable_deviation) {
                float[] time_required = new float[2];

                OrbitingObjectDescriptor new_orbit = new OrbitingObjectDescriptor(this);

                if(!new_orbit.plan_path(destination, acceptable_deviation)) return null;

                float[] current_vel = get_velocity();
                float[]  target_vel = new_orbit.get_velocity();

                time_required[0] = (target_vel[0] - current_vel[0]) / acceleration;
                time_required[1] = (target_vel[1] - current_vel[1]) / acceleration;

                return time_required; 
            }

            public float[] get_velocity_at(float radius, float E) {
                // →   √(μa)         -sin(E)
                // v = ------ ( √(1 - ε²) cos(E) )
                //       rc             0

                float factor = (float)Math.Sqrt(standard_gravitational_parameter * semimajoraxis) / radius;

                float speedx = -factor * (float) Math.Sin(E);
                float speedy =  factor * (float)(Math.Sqrt(1 - eccentricity * eccentricity) * Math.Cos(E));

                // Rotate the velocity along the orbit's rotational offset

                // Sine and cosine of the rotational offset
                float rotsin = (float)Math.Sin(rotation);
                float rotcos = (float)Math.Cos(rotation);

                float[] velocity = new float[2];

                velocity[0] = rotcos * speedx - rotsin * speedy;
                velocity[1] = rotsin * speedx + rotcos * speedy;

                return velocity;
            }

            public float[] get_velocity() {
                return get_velocity_at(heliocentric_distance, eccentric_anomaly);
            }

            public void update_position(float dt) {
                mean_anomaly         += dt * mean_motion;
                eccentric_anomaly     = get_eccentric_anomaly_at(mean_anomaly);
                true_anomaly          = get_true_anomaly(eccentric_anomaly);
                heliocentric_distance = get_distance_from_center_at(true_anomaly);
                float[] pos           = get_position_at(true_anomaly, heliocentric_distance);
                float[] vel           = get_velocity_at(heliocentric_distance, eccentric_anomaly);
                self.posx             = pos[0];
                self.posy             = pos[1];
                self.velx             = vel[0];
                self.vely             = vel[1];

                if   (mean_anomaly      <        0.0f) mean_anomaly      += Tools.twopi;
                while(mean_anomaly      > Tools.twopi) mean_anomaly      -= Tools.twopi;

                if   (eccentric_anomaly <        0.0f) eccentric_anomaly += Tools.twopi;
                if   (true_anomaly      <        0.0f) true_anomaly      += Tools.twopi;
            }

            /*public void DebugOut() {
                Debug.Log("Orbiting object debug log: ");
                Debug.Log("a: " + SemiMajorAxis + " b: " + SemiMinorAxis + " ω: " + Rotation + " P: " + OrbitalPeriod + " μ: " + StandardGravitationalParameter + " ε: " + Eccentricity + " D: " + EccentricDistance);
                Debug.Log("n: " + MeanMotion + " M: " + MeanAnomaly + " E: " + EccentricAnomaly + " ν: " + TrueAnomaly);
                Debug.Log("x: " + Self.PosX + " y: " + Self.PosX + " vx: " + Self.VelX + " vy: " + Self.VelY);
            }*/

                public void change_frame_of_reference(SpaceObject new_frame_of_reference) {
                float posx = self.posx - central_body.posx;
                float posy = self.posy - central_body.posy;

                float velx = self.velx - central_body.velx;
                float vely = self.vely - central_body.vely;

                central_body = new_frame_of_reference;

                // μ = GM

                standard_gravitational_parameter = Tools.gravitational_constant * central_body.mass;

                // h = r x v   =>   h = x * v  - y * v
                //                           y        x

                float angular_momentum = posx * vely - posy * velx;

                //                     v  * h          v  * h
                //     v x h    r       y        x      x        y
                // e = ----- - --- = ( ------ - ---, - ------ - --- )
                //       μ     |r|        μ     |r|       μ     |r|

                float pos_magnitude = (float)Math.Sqrt(posx * posx + posy * posy);

                eccentricity_vector[0] =  (vely * angular_momentum / standard_gravitational_parameter) - posx / pos_magnitude;
                eccentricity_vector[1] = -(velx * angular_momentum / standard_gravitational_parameter) - posy / pos_magnitude;

                // ε = |e|

                eccentricity = Tools.magnitude(eccentricity_vector);

                //         h²
                // a = ---------
                //     μ(1 - ε²)

                semimajoraxis = angular_momentum * angular_momentum / (standard_gravitational_parameter * (1.0f - eccentricity * eccentricity));

                if(eccentricity <= 1.0f) {

                    //            b²
                    // ε = √ (1 - -)   =>    b = a √ (1 - ε²)
                    //            a²

                    semiminoraxis = semimajoraxis * (float)Math.Sqrt(1.0f - eccentricity * eccentricity);

                    //           e
                    //            y
                    // ω = atan( -- )
                    //           e
                    //            x

                    rotation = (float)Math.Atan(eccentricity_vector[1] / eccentricity_vector[0]);

                    //        μ
                    // n = √ ---
                    //        a³

                    mean_motion = (float)Math.Sqrt(standard_gravitational_parameter / (semimajoraxis * semimajoraxis * semimajoraxis));

                } else {

                    // 
                    // b = -a  √ (e² - 1)
                    //

                    semiminoraxis = -semimajoraxis * (float)Math.Sqrt(eccentricity * eccentricity - 1.0f);

                    //        μ
                    // n = √ ---
                    //       -a³

                    mean_motion = (float)Math.Sqrt(standard_gravitational_parameter / (-semimajoraxis * semimajoraxis * semimajoraxis));

                }

                //     2 * π
                // P = -----
                //       n

                orbital_period = Tools.twopi / mean_motion;

                periapsis = get_distance_from_center_at(0.0f);
                apoapsis  = get_distance_from_center_at(Tools.pi);

                linear_eccentricity = semimajoraxis - periapsis;

                if(eccentricity_vector[0] < 0.0f) rotation += Tools.pi;

                //                                      r            r
                // →          cos(ν)                     x            y
                // r = r  * ( sin(ν) )   =>   ν = acos( -- ) = asin( -- )
                //      c       0                       r            r
                //                                       c            c

                true_anomaly = (float)Math.Atan2(posy, posx) - rotation;

                if(eccentricity <= 1.0f) {

                    //                                                 1 + ε             ν
                    //              1 + ε     E                      √ ----- (1 - ε) tan(-)
                    // ν = 2 atan(√ ----- tan(-))   =>   E = 2 atan(   1 - ε             2  )
                    //              1 - ε     2                      ---------------------- 
                    //                                                        1 + ε

                    float one_plus_ecc  = 1 + eccentricity;
                    float one_minus_ecc = 1 - eccentricity;

                    eccentric_anomaly   = 2.0f * (float)Math.Atan((Math.Sqrt(one_plus_ecc / one_minus_ecc)
                                               * one_minus_ecc
                                               * Math.Tan(true_anomaly / 2)) / one_plus_ecc);

                    // M = E - ε sin(E)

                    mean_anomaly = eccentric_anomaly - eccentricity * (float)Math.Sin(eccentric_anomaly);

                } else {

                    //                                                    1 + ε             ν
                    //               1 + ε      E                       √ ----- (1 - ε) tan(-)
                    // ν = 2 atanh(√ ----- tanh(-))   =>   E = 2 atanh(   1 - ε             2  )
                    //               1 - ε      2                       ---------------------- 
                    //                                                           1 + ε

                    float one_plus_ecc  = 1 + eccentricity;
                    float one_minus_ecc = 1 - eccentricity;

                    eccentric_anomaly   = 2.0f * (float)Math.Atanh((Math.Sqrt(one_plus_ecc / one_minus_ecc)
                                               * one_minus_ecc
                                               * Math.Tan(true_anomaly / 2)) / one_plus_ecc);

                    // M = E - ε sinh(E)

                    mean_anomaly = eccentric_anomaly - eccentricity * (float)Math.Sinh(eccentric_anomaly);

                }

                while(mean_anomaly      < 0.0f) mean_anomaly      += Tools.twopi;
                while(eccentric_anomaly < 0.0f) eccentric_anomaly += Tools.twopi;
                while(true_anomaly      < 0.0f) true_anomaly      += Tools.twopi;
            }
        }
    }
}
