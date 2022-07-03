using KMath.Random;
using System;

namespace KMath.PerlinNoise
{
    public class PerlinOctave3D {
        private int runs;

        public int octaves;
        public PerlinField3D[] octave_array;
        public float[] cache;
        public float cache_persistence;
        public byte cache_seed;

        public int map_dim_x;
        public int map_dim_y;
        public int map_dim_z;

        public PerlinOctave3D(int _octaves, int _map_dim_x, int _map_dim_y, int _map_dim_z) {
            runs = 0;
            cache_persistence = 0.0f;
            cache_seed = 0;

            octaves = _octaves;
            octave_array = new PerlinField3D[octaves];

            map_dim_x = _map_dim_x;
            map_dim_y = _map_dim_y;

            //for(int i=0; i<octaves; i++) octave_array[i].init(pow(2,i+2), 15);
            //for(int i=0; i<octaves; i++) octave_array[i].init(2*(i+1)+1, 4);
            //for(int i=0; i<octaves; i++) octave_array[i].init((i*(i+1))+1, 4);

            cache = new float[(map_dim_x / 4) * (map_dim_y / 4) * (map_dim_z / 4)];

            for(int i = 0; i < octaves; i++) octave_array[i].init(PerlinField2D.primes[i + 1], PerlinField2D.primes[i + 1]);
        }

        public float sample(float x, float y, float z, float persistence) {
            float p = 1.0f;
            float tmp = 0.0f;

            for(int i = 0; i < octaves; i++) {
                tmp += octave_array[i]._base(x, y, z);
                p *= persistence;
            }

            return tmp;
        }

        public void save_octaves(float f) { }

        public void set_persistence(float persistence) {
            if(cache_persistence == persistence) return;
            cache_persistence = persistence;
            populate_cache(persistence);
        }

        public void set_param(float persistence, bool change_seed = true, ulong new_seed = 0) {
            if(runs == 0) {
                if(change_seed) {
                    System.Random rng = new System.Random();
                    Mt19937.seed_twister(new_seed != 0 ? new_seed : ((ulong)rng.Next() << 32 | (ulong)rng.Next()));
                }
                for(int i = 0; i < octaves; i++)
                    octave_array[i].generate_gradient_array();
            }

            if(persistence != cache_persistence || runs == 0) {
                cache_persistence = persistence;
                populate_cache(persistence);
            }

            runs++;
        }

        public void populate_cache(float persistence) {
            int max_x = map_dim_x / 4;
            int max_y = map_dim_y / 4;
            int max_z = map_dim_z / 4;

            float x, y, z;

            for(int k = 0; k < max_z; k++)
                for(int i = 0; i < max_x; i++)
                    for(int j = 0; j < max_y; j++) {
                        x = i * (4.0f / map_dim_x);
                        y = j * (4.0f / map_dim_y);
                        z = k * (4.0f / map_dim_z); // NOTE -- this used to be 4.0/512.0f
                                                    // There are no overhangs with that level

                        cache[k * max_x * max_y + j * max_x + i] = sample(x, y, z, persistence);
                    }
        }
    }
}
