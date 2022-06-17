using KMath.Random;
using System;

namespace KMath.PerlinNoise {
    public class PerlinField2D {
        static public int[] primes =  {
             2,  3,  5,  7, 11,
            13, 17, 19, 23, 29,
            31, 37, 41, 43, 47,
            53, 59, 61, 67, 71
        };
        
        static public float dot(int gi, float x, float y, float z) {
            int[,] g3 = {
                {1, 1, 0}, {-1,  1, 0}, {1, -1,  0}, {-1, -1,  0},
                {1, 0, 1}, {-1,  0, 1}, {1,  0, -1}, {-1,  0, -1},
                {0, 1, 1}, { 0, -1, 1}, {0,  1, -1}, { 0, -1, -1}
            };
        
            return g3[gi, 0] * x
                 + g3[gi, 1] * y
                 + g3[gi, 2] * z;
        }

        static public int fast_floor(float value) {
            return (int)(value >= 0 ? (int)value : (int)value - 1);
        }

        static public float mix(float a, float b, float t) {
            return a + t * (b - a);
        }

        static public float fade(float t) {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public  byte[] ga;
        public float[] grad;

        public int ssize;
        public int xsize;

        public int grad_max;

        public void init(int _xsize, int _grad_max) {
            if(ga != null || grad != null) return;

            xsize = _xsize;
            ssize = xsize * xsize;
            grad_max = _grad_max;

            ga = new byte[ssize];
            grad = new float[2 * grad_max];

            generate_gradient_array();
            generate_gradient_vectors();
        }

        public void generate_gradient_array() {
            for(int i = 0; i < ssize; i++)
                ga[i] = (byte)(Mt19937.genrand_int32() % (ulong)grad_max);
        }

        public void generate_gradient_vectors() {
            for (int i = 0; i < grad_max; i++) {
                float t = 6.28318531f * i * (1.0f / (float)grad_max);
                float x = MathF.Sin(t);
                float y = MathF.Cos(t);

                grad[2 * i + 0] = x;
                grad[2 * i + 1] = y;
            }
        }

        public int get_gradient(int x, int y) {
            x = x % xsize;
            y = y % xsize;

            return ga[x + y * xsize];
        }

        public float dot_grad(int index, float x, float y) {
            return grad[2 * index + 0] * x
                 + grad[2 * index + 1] * y;
        }

        public float _base(float x, float y) {
            x *= xsize;
            y *= xsize;

            int X = fast_floor(x);
            int Y = fast_floor(y);

            x = x - X;
            y = y - Y;

            int gi00 = get_gradient(X + 0, Y + 0);
            int gi01 = get_gradient(X + 0, Y + 1);
            int gi10 = get_gradient(X + 1, Y + 0);
            int gi11 = get_gradient(X + 1, Y + 1);

            // Calculate noise contributions from each of the eight corners

            float n00 = dot_grad(gi00, x,     y);
            float n10 = dot_grad(gi10, x - 1, y);
            float n01 = dot_grad(gi01, x,     y - 1);
            float n11 = dot_grad(gi11, x - 1, y - 1);

            // Compute the fade curve value for each of x, y, z

            float u = fade(x);
            float v = fade(y);

            float nx00 = mix(n00,  n10,  u);
            float nx10 = mix(n01,  n11,  u);
            float nxy  = mix(nx00, nx10, v);

            return nxy * MathF.Sqrt(2.0f); // -1 to 1
        }

        public float noise(float x, float y) {
            return _base(x, y);
        }

        public float one_over_f(float x, float y) {
            float tmp = 0;
            tmp += _base(x, y);
            tmp += 0.50f   * _base( 2 * x,  2 * y);
            tmp += 0.25f   * _base( 4 * x,  4 * y);
            tmp += 0.125f  * _base( 8 * x,  8 * y);
            tmp += 0.0625f * _base(16 * x, 16 * y);
            return tmp;
        }

        public float one_over_f(float x, float y, float persistence) {
            float tmp = 0;
            float m = 1.0f;

            tmp += _base(x, y);
            m *= persistence;

            tmp += m * _base(2 * x, 2 * y);
            m *= persistence;

            tmp += m * _base(4 * x, 4 * y);
            m *= persistence;

            tmp += m * _base(8 * x, 8 * y);

            m *= persistence;
            tmp += m * _base(16 * x, 16 * y);
            return tmp;
        }

        //order 0 is base
        float order(float x, float y, float persistence, int order) {
            float tmp = 0;
            float m = 1.0f;
            int b = 1;

            for (int i = 0; i <= order; i++) {
                tmp += _base(b * x, b * y);
                m *= persistence;
                b *= 2;
            }
            return tmp;
        }

        float abs(float x, float y) {
            float tmp = _base(x, y);
            return MathF.Sqrt(tmp * tmp);
        }
    }
}
