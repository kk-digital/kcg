using System;

namespace Source {
    namespace SystemView {
    
        /* 
         * Helper class. Contains a lot of useful functions and constants used in most of the system view code.
         */

        public static class Tools {
            public const bool  debug                  = true;           // When enabled additional debug information is shown

                                                                        //                                  -11    m³
            public const float gravitational_constant = 6.67408e-11f;   //  G                   6.67408 * 10    -------
                                                                        //                                      kg * s²

            public const float twopi                  = 6.2831852f;     // 2π                   6.2831852                   360.0°
            public const float pi                     = 3.1415926f;     //  π                   3.1415926                   180.0°

                                                                        //  π
            public const float halfpi                 = 1.5707963f;     // ---                  1.5707963                    90.0°
                                                                        //  2

                                                                        //  π
            public const float quarterpi              = 0.7853982f;     // ---                  0.7853982                    45.0°
                                                                        //  4

                                                                        //  π
            public const float sixthpi                = 0.5235988f;     // ---                  0.5235988                    30.0°
                                                                        //  6

                                                                        //  π
            public const float eigthpi                = 0.3926991f;     // ---                  0.3926991                    22.5°
                                                                        //  8

            public const float  sqrt2                 = 1.4142136f;     // √ 2                  1.4142136

                                                                        //  1
            public const float rsqrt2                 = 0.7071068f;     // ---                  0.7071068
                                                                        // √ 2

                                                                        //  π                   Multiply by this to convert from deg to rad
            public const float deg                    = 0.0174533f;     // ---
                                                                        // 180°                 Divide   by this to convert from rad to deg

            /*
             * Quick and simple function to calculate the magnitude of a vector
             */

            public static float magnitude(float[] arr) {
                float magnitude = 0.0f;

                foreach(float f in arr) magnitude += f * f;

                return (float)Math.Sqrt(magnitude);
            }

            public static float magnitude(float x, float y) {
                return (float)Math.Sqrt(x * x + y * y);
            }

            public static float magnitude(float x, float y, float z) {
                return (float)Math.Sqrt(x * x + y * y + z * z);
            }

            /*
             * Returns the distance between two objects
             */
            public static float get_distance(float x1, float y1, float x2, float y2) {
                float dx = x1 - x2;
                float dy = y1 - y2;
                return (float)Math.Sqrt(dx * dx + dy * dy);
            }

            /*
             * Quick and simple function to get the angle a vector points to
             */
            public static float get_angle(float x, float y) {
                float angle = (float)Math.Acos(x / magnitude(x, y));
                return y >= 0.0f ? angle : twopi - angle;
            }

            /*
             * Clamps an angle between 0 and 2π
             */
            public static float normalize_angle(float angle) {
                angle %= twopi;
                if (angle < 0.0f) angle += twopi;
                return angle;
            }

            /*
             * Picks a value between a and b, using a smoothstep curve.
             * 
             * For w <= 0.0 it returns a.
             * For w >= 1.0 it returns b.
             * For 0.0 < w < 1.0 it returns a value between a and b along a smoothstep curve.
             */
            public static float smoothstep(float a, float b, float w) {
                if(w < 0.0f) return a;
                if(w > 1.0f) return b;

                return (b - a) * w * w * (3.0f - 2.0f * w) + a;
            }

            /*
             * Picks a value between a and b, using a smootherstep curve.
             * 
             * For w <= 0.0 it returns a.
             * For w >= 1.0 it returns b.
             * For 0.0 < w < 1.0 it returns a value between a and b along a smootherstep curve.
             */
            public static float smootherstep(float a, float b, float w) {
                if(w < 0.0f) return a;
                if(w > 1.0f) return b;

                return (b - a) * w * w * w * (w * (w * 6.0f - 15.0f) + 10.0f) + a;
            }
        }
    }
}
