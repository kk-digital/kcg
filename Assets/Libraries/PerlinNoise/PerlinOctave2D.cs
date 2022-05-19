using PRNG;
using System;

namespace PerlinNoise {
    public class PerlinOctave2D {
        public int octaves;
        public PerlinField2D[] octave_array;

        public float[] cache;
        public float cache_persistence;
        public ulong cache_seed;

        private int map_dim_x;
        private int map_dim_y;

        private int runs;
        private Random rng;

        private void _init(int _octaves, int _map_dim_x, int _map_dim_y, bool change_seed = true, ulong new_seed = 0) {
            rng = new Random();
            runs = 0;
            cache = null;
            cache_persistence = 0.0f;
            cache_seed = 0;

            map_dim_x = _map_dim_x;
            map_dim_y = _map_dim_y;

            octaves = _octaves;

            if(change_seed)
                Mt19937.seed_twister(new_seed != 0 ? new_seed : (ulong)rng.Next() << 32 | (ulong)rng.Next());
            

            octave_array = new PerlinField2D[octaves];

            cache = new float[(map_dim_x / 4) * (map_dim_y / 4)];

            for(int i = 0; i < octaves; i++)
                octave_array[i].init(PerlinField2D.primes[i + 1], 16);
        }

        public PerlinOctave2D(int octaves, int dim_x, int dim_y) { _init(octaves, dim_x, dim_y); }
        public PerlinOctave2D(int octaves, int dim_x, int dim_y, bool change_seed, ulong new_seed) { _init(octaves, dim_x, dim_y, change_seed, new_seed); }

        public void set_persistence(float persistence) {
            if(cache_persistence == persistence) return;
            cache_persistence = persistence;
            populate_cache(persistence);
        }

        public void set_param(float persistence, bool change_seed = true, ulong new_seed = 0) {
            bool update = false;

            if(runs == 0) {
                update = true;
                Mt19937.seed_twister(new_seed != 0 ? new_seed : (ulong)rng.Next() << 32 | (ulong)rng.Next());

                for (int i = 0; i < octaves; i++)
                    octave_array[i].generate_gradient_array();
            }

            if(persistence != cache_persistence || update) 
                populate_cache(cache_persistence = persistence);

            runs++;
        }

        public void populate_cache(float persistence) {
            int xmax = map_dim_x / 4;
            int ymax = map_dim_y / 4;

            float x, y;

            for(int i = 0; i < xmax; i++)
                for(int j = 0; j < ymax; j++) {
                    x = i * 4.0f / (float)map_dim_x;
                    y = j * 4.0f / (float)map_dim_y;

                    cache[j * xmax + i] = sample(x, y, persistence);
                }
        }

        
        public float sample(float x, float y, float persistence) {
            float p = 1.0f;
            float tmp = 0.0f;

            for(int i = 0; i < octaves; i++) {
                tmp += octave_array[i]._base(x, y);
                p *= persistence;
            }

            return tmp;
        }

        float sample2(float x, float y, float persistence) {
            float p = 1.0f;
            float tmp = 0.0f;

            for(int i = 0; i < octaves; i++) {
                Console.WriteLine("octave " + i + ": " + octave_array[i]._base(x, y));
                tmp += p * octave_array[i]._base(x, y);
                p *= persistence;
            }
            
            Console.WriteLine("tmp = " + tmp + " x, y = " + x + ", " + y);
            return tmp;
        }
        
        public void save_octaves() {
            float xresf = 1.0f / (float)map_dim_x;
            float yresf = 1.0f / (float)map_dim_y;

            float[] output = new float[map_dim_x * map_dim_y * octaves];

            PerlinField2D m;
            for(int k = 0; k < octaves; k++) {
                m = octave_array[k];
                int zoff = k * map_dim_x * map_dim_y;

                for(int i = 0; i < map_dim_x; i++)
                    for(int j = 0; j < map_dim_y; j++) {
                        float x = i * xresf;
                        float y = j * yresf;

                        output[i + j * map_dim_y + zoff] = m._base(x, y);
                    }
            }

            //save_perlin("octave_map_01", out, xres, yres*octaves);

            for(int k = 0; k < octaves; k++)
                for(int i = 0; i < map_dim_x; i++)
                    for(int j = 0; j < map_dim_y; j++)
                        output[i + j * map_dim_y + k * map_dim_x * map_dim_y] = 0.0f;

            for(int k = 0; k < octaves; k++) 
                for(int k2 = 0; k2 < k; k2++) {
                    m = octave_array[k2];
                    float p = 1.0f;
                    int zoff = k * map_dim_x * map_dim_y;

                    for(int i = 0; i < map_dim_x; i++)
                        for(int j = 0; j < map_dim_y; j++) {
                            float x = i * xresf;
                            float y = j * yresf;

                            output[i + j * map_dim_y + zoff] += p * m._base(x, y);
                        }

                    p *= 0.50f;
                }


            //save_png("octave_map_01", output, xres, yres*octaves);
            //save_perlin("octave_map_02", output, xres, yres*octaves);
        }

        public void save_octaves2(int degree, string filename) {
            int xres = 128;
            int yres = 128;

            float xresf = 1.0f / (float)xres;
            float yresf = 1.0f / (float)yres;

            float[] output = new float[xres * yres * octaves * degree];

            int line_width = xres * degree;

            for(int i = 0; i < xres * yres * octaves * degree; i++) output[i] = 0.0f;

            PerlinField2D m;

            for(int k = 0; k<octaves; k++) {
                m = octave_array[k];
                int zoff = k * yres * line_width;

                for(int i = 0; i<xres; i++)
                    for(int j = 0; j<yres; j++) {
                        float x = i * xresf;
                        float y = j * yresf;

                        output[i+j * line_width + zoff] += m._base(x, y);
                    }
            }

            for(int n = 1; n < degree; n++) {
                float persistence = ((float)n + 1.0f) / (float)degree;

                int xoff = n * xres;

                for(int k = 0; k < octaves; k++) {
                    int yoff = k * yres * line_width;
                    float p = 1.0f;

                    for(int k2 = 0; k2 <= k; k2++) {
                        m = octave_array[k2];

                        for(int i = 0; i < xres; i++)
                            for(int j = 0; j < yres; j++) {
                                float x = i * xresf;
                                float y = j * yresf;

                                output[(i + xoff) + (j * line_width + yoff)] += p * m._base(x, y);
                            }

                        p *= persistence;
                    }
                }

            }
            //save_png("octave_map_01", output, xres, yres*octaves);
            //save_perlin(filename, output, xres*degree, yres*octaves);
        }
    }
}
