using PRNG;
using System;

namespace PerlinNoise {
    public class PerlinField3D {
        public  byte[] ga;
        public float[] grad;

        public int ssize;
        public int xsize, xsize2;
        public int zsize;

        public float xscale;
        public float zscale;

        public const int grad_max = 12;

        public void init(int _xsize, int _zsize) {
            if(_xsize < 1 || _zsize < 1) return;

            xsize = _xsize;
            zsize = _zsize;
            ssize = xsize * xsize * zsize;
            xsize2 = xsize * xsize;

            ga = new byte[ssize];

            generate_gradient_array();
        }

        public void generate_gradient_array() {
            for(int i = 0; i < ssize; i++) ga[i] = (byte)(Mt19937.genrand_int32() % grad_max);
        }

        public byte get_gradient(int x, int y, int z) {
            x = x % xsize;
            y = y % xsize;
            z = z % zsize;

            return ga[x + y * xsize + z * xsize2];
        }

        public float _base(float x, float y, float z) {
            x *= xsize;
            y *= xsize;
            z *= zsize;

            //get grid point
            int X = PerlinField2D.fast_floor(x);
            int Y = PerlinField2D.fast_floor(y);
            int Z = PerlinField2D.fast_floor(z);

            x = x - X;
            y = y - Y;
            z = z - Z;

            int gi000 = get_gradient(X + 0, Y + 0, Z + 0);
            int gi001 = get_gradient(X + 0, Y + 0, Z + 1);
            int gi010 = get_gradient(X + 0, Y + 1, Z + 0);
            int gi011 = get_gradient(X + 0, Y + 1, Z + 1);

            int gi100 = get_gradient(X + 1, Y + 0, Z + 0);
            int gi101 = get_gradient(X + 1, Y + 0, Z + 1);
            int gi110 = get_gradient(X + 1, Y + 1, Z + 0);
            int gi111 = get_gradient(X + 1, Y + 1, Z + 1);

            // Calculate noise contributions from each of the eight corners
            float n000 = PerlinField2D.dot(gi000, x,     y,     z);
            float n100 = PerlinField2D.dot(gi100, x - 1, y,     z);
            float n010 = PerlinField2D.dot(gi010, x,     y - 1, z);
            float n110 = PerlinField2D.dot(gi110, x - 1, y - 1, z);
            float n001 = PerlinField2D.dot(gi001, x,     y,     z - 1);
            float n101 = PerlinField2D.dot(gi101, x - 1, y,     z - 1);
            float n011 = PerlinField2D.dot(gi011, x,     y - 1, z - 1);
            float n111 = PerlinField2D.dot(gi111, x - 1, y - 1, z - 1);
            // Compute the fade curve value for each of x, y, z

            float u = PerlinField2D.fade(x);
            float v = PerlinField2D.fade(y);
            float w = PerlinField2D.fade(z);

            // Interpolate along x the contributions from each of the corners
            float nx00 = PerlinField2D.mix(n000, n100, u);
            float nx01 = PerlinField2D.mix(n001, n101, u);
            float nx10 = PerlinField2D.mix(n010, n110, u);
            float nx11 = PerlinField2D.mix(n011, n111, u);
            // Interpolate the four results along y
            float nxy0 = PerlinField2D.mix(nx00, nx10, v);
            float nxy1 = PerlinField2D.mix(nx01, nx11, v);
            // Interpolate the two last results along z
            float nxyz = PerlinField2D.mix(nxy0, nxy1, w);

            return nxyz * 0.707106781f;   //-1 to 1
        }

        float noise(float x, float y, float z) {
            return _base(x, y, z);
        }

        float one_over_f(float x, float y, float z) {
            float tmp = 0;
            tmp += _base(x, y, z);
            tmp += 0.50f * _base(2 * x, 2 * y, 2 * z);
            tmp += 0.25f * _base(4 * x, 4 * y, 2 * z);
            return tmp;
        }
    }
}
