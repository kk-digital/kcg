﻿using System;

namespace SystemView {
    
    /* 
     * Helper class. Contains a lot of useful functions and constants used in most of the system view code.
     */

    public static class Tools {
                                                                    //                                  -11    m³
        public const float gravitational_constant = 6.67408e-11f;   //  G                   6.67408 * 10    -------
                                                                    //                                      kg * s²

        public const float twopi                  = 6.2831852f;     // 2π                   6.2831852
        public const float pi                     = 3.1415926f;     //  π                   3.1415926

                                                                    //  π
        public const float halfpi                 = 1.5707963f;     // ---                  1.5707963
                                                                    //  2

        public const float  sqrt2                 = 1.4142136f;     // √ 2                  1.4142136

                                                                    //  1
        public const float rsqrt2                 = 0.7071068f;     // ---                  0.7071068
                                                                    // √ 2

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
    }
}