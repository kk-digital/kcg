using System;
using System.Threading;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class AccretionDisk : MonoBehaviour {
            public int            seed;
            public int            layers;
            public int            width;
            public int            height;
            public float          opacity;
            public float          contrast;
            public float          cutoff;           // Also sometimes called black level
            public bool           stationary;
            public float          spin;             // Angular velocity in degrees/second at farthest point
            public float          brightness;
            public Vector3        center;           // Center point to spin around

            public Texture2D      texture;
            public SpriteRenderer renderer;

            private System.Random rng;

            private Color[]       pixels;
            private float[]       alpha;
            private float         last_time;

            private void generate() {

                rng = new(seed);

                pixels    = new Color[width * height];

                for(int i = 0; i < width * height; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                float[] base_alpha = ProceduralImages.generate_noise(rng, 1.0f,             width / 64, height / 64);
                        base_alpha = ProceduralImages.smoothen_noise(base_alpha,            width / 64, height / 64, 64);
                        base_alpha = ProceduralImages.distort(rng,   base_alpha, 16.0f, 16, width,      height);
                        base_alpha = ProceduralImages.soften(        base_alpha, 4,         width,      height);
                        base_alpha = ProceduralImages.circular_blur( base_alpha,            width,      height, 16.0f);
                        base_alpha = ProceduralImages.circular_mask( base_alpha,            width,      height);

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++)
                        pixels[x + y * width].a = base_alpha[x + y * width];

                float   halfw = width  / 2;
                float   halfh = height /  2;

                alpha = new float[width * height];

                for(int layer = 0; layer < layers; layer++) {

                    int   scale         = 1 << layers - layer + 1;
                    float strength      = scale / layers;

                    float[] layer_alpha = ProceduralImages.generate_noise(rng, strength, width / scale, height / scale);
                            layer_alpha = ProceduralImages.blur_noise(   layer_alpha,    width / scale, height / scale);
                            layer_alpha = ProceduralImages.smoothen_noise(layer_alpha,   width / scale, height / scale, scale);

                    for(int x = 0; x < width; x++)
                        for(int y = 0; y < height; y++) {
                            float a              = layer_alpha[x + y * width] * 1.5f;
                            alpha[x + y * width] = a  +  alpha[x + y * width] * (1.0f - a);
                        }
                }

                alpha = ProceduralImages.exponential_filter(alpha,            width, height);
                alpha = ProceduralImages.distort(rng,       alpha, 32.0f, 64, width, height);
                alpha = ProceduralImages.soften(            alpha,         8, width, height);

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        int i = x + y * width;

                        float a                 = alpha[i];

                        float d                 = 1.0f - Tools.magnitude(x / halfw - 1.0f, y / halfh - 1.0f);

                        float r, g, b;

                        if(d < 0.25f) {
                            float val           =  d         / 0.25f;

                            r                   = Tools.smoothstep(0.0f,               0.25f * brightness, val);
                            g                   = Tools.smoothstep(0.0f,               0.03f * brightness, val);
                            b                   = Tools.smoothstep(0.0f,               0.00f * brightness, val);
                            a                  *= Tools.smoothstep(0.0f,               0.50f,              val);
                        } else if(d < 0.5f) {
                            float val           = (d - 0.25f) / 0.25f;

                            r                   = Tools.smoothstep(0.25f * brightness, 0.80f * brightness, val);
                            g                   = Tools.smoothstep(0.03f * brightness, 0.40f * brightness, val);
                            b                   = Tools.smoothstep(0.00f * brightness, 0.20f * brightness, val);
                            a                  *= Tools.smoothstep(0.50f,              1.00f,              val);
                        } else if(d < 0.75f) {
                            float val           = (d - 0.50f) / 0.25f;

                            r                   = Tools.smoothstep(0.80f * brightness, 1.00f * brightness, val);
                            g                   = Tools.smoothstep(0.40f * brightness, 1.00f * brightness, val);
                            b                   = Tools.smoothstep(0.20f * brightness, 0.40f * brightness, val);
                            a                  *= Tools.smoothstep(1.00f,              1.70f,              val);
                        } else {
                            float val           = (d - 0.75f) / 0.25f;

                            r                   = Tools.smoothstep(1.00f * brightness, 1.00f * brightness, val);
                            g                   = Tools.smoothstep(1.00f * brightness, 1.00f * brightness, val);
                            b                   = Tools.smoothstep(0.40f * brightness, 0.90f * brightness, val);
                            a                  *= Tools.smoothstep(1.70f,              4.00f,              val);
                        }

                        pixels[i].r = r;
                        pixels[i].g = g;
                        pixels[i].b = b;
                        pixels[i].a = a;

                        // Adjust contrast
                        ProceduralImages.adjust_contrast   (ref pixels[i].r, ref pixels[i].g, ref pixels[i].b, contrast);

                        // Adjust black level
                        ProceduralImages.adjust_black_level(ref pixels[i].r, ref pixels[i].g, ref pixels[i].b, ref pixels[i].a, cutoff);

                        // Adjust opacity
                        pixels[i].a *= opacity;

                        alpha[i]     = pixels[i].a;
                    }

                texture = new Texture2D(width, height);
                texture.filterMode = FilterMode.Trilinear;
                texture.SetPixels(pixels);
                texture.Apply();

            }

            // TODO: Convert to GPU code / shader for much higher performance
            private void thread_function(int id, float current_spin) {
                int startx      = id % 4 * width  / 4;
                int starty      = id / 4 * height / 4;

                int endx        = startx + width  / 4;
                int endy        = starty + height / 4;

                int half_width  =          width  / 2;
                int half_height =          height / 2;

                int maxr        = (width + height) / 4;

                for(int x = startx; x < endx; x++)
                    for(int y = starty; y < endy; y++) {
                        float r = Tools.get_distance(x, y, half_width, half_height) / maxr;

                        if(r > 1.0f || r == 0.0f) continue;

                        float a  = Tools.get_angle(half_width - x, half_height - y);
                              a += (current_spin > Tools.pi ? Tools.pi : current_spin) / r;

                        float original_x = half_width  * (1.0f + r * (float)Math.Cos(a));
                        float original_y = half_height * (1.0f + r * (float)Math.Sin(a));

                        int x0 = (int)original_x;
                        int x1 = (int)original_x + 1;
                        int y0 = (int)original_y;
                        int y1 = (int)original_y + 1;

                        if(x1 < 0 || x1 >= width)  x1 = x0;
                        if(y1 < 0 || y1 >= height) y1 = y0;

                        float dx = original_x - x0;
                        float dy = original_y - y0;

                        float v0 = Tools.smootherstep(alpha[x0 + y0 * width], alpha[x1 + y0 * width], dx);
                        float v1 = Tools.smootherstep(alpha[x0 + y1 * width], alpha[x1 + y1 * width], dx);

                        pixels[x + y * width].a = Tools.smootherstep(v0, v1, dy);
                    }
            }

            private void do_spin(float current_spin) {
                Thread[] Ts = new Thread[16];

                Ts[ 0] = new Thread(new ThreadStart(() => thread_function( 0, current_spin)));
                Ts[ 1] = new Thread(new ThreadStart(() => thread_function( 1, current_spin)));
                Ts[ 2] = new Thread(new ThreadStart(() => thread_function( 2, current_spin)));
                Ts[ 3] = new Thread(new ThreadStart(() => thread_function( 3, current_spin)));
                Ts[ 4] = new Thread(new ThreadStart(() => thread_function( 4, current_spin)));
                Ts[ 5] = new Thread(new ThreadStart(() => thread_function( 5, current_spin)));
                Ts[ 6] = new Thread(new ThreadStart(() => thread_function( 6, current_spin)));
                Ts[ 7] = new Thread(new ThreadStart(() => thread_function( 7, current_spin)));
                Ts[ 8] = new Thread(new ThreadStart(() => thread_function( 8, current_spin)));
                Ts[ 9] = new Thread(new ThreadStart(() => thread_function( 9, current_spin)));
                Ts[10] = new Thread(new ThreadStart(() => thread_function(10, current_spin)));
                Ts[11] = new Thread(new ThreadStart(() => thread_function(11, current_spin)));
                Ts[12] = new Thread(new ThreadStart(() => thread_function(12, current_spin)));
                Ts[13] = new Thread(new ThreadStart(() => thread_function(13, current_spin)));
                Ts[14] = new Thread(new ThreadStart(() => thread_function(14, current_spin)));
                Ts[15] = new Thread(new ThreadStart(() => thread_function(15, current_spin)));

                foreach(Thread T in Ts) T.Start();
                foreach(Thread T in Ts) T.Join();

                texture.SetPixels(pixels);
                texture.Apply();
            }

            private void Start() {
                generate();

                renderer        = gameObject.AddComponent<SpriteRenderer>();
                last_time       = Time.time;

                if(spin != 0.0f &&  stationary) do_spin(Tools.normalize_angle(spin * Tools.deg));

                renderer.sprite = Sprite.Create(texture,
                                                new Rect(0, 0, width, height),
                                                new Vector2(0.5f, 0.5f));
            }

            private void Update() {
                if(spin != 0.0f && !stationary) {
                    float current_spin = Time.time * spin * Tools.deg;

                    if(current_spin > Tools.pi)
                        renderer.transform.RotateAround(center, new Vector3(0.0f, 0.0f, 1.0f), spin * (Time.time - last_time));
                    else
                        do_spin(current_spin);

                    renderer.sprite = Sprite.Create(texture,
                                                    new Rect(0, 0, width, height),
                                                    new Vector2(0.5f, 0.5f));
                }

                last_time = Time.time;
            }
        }
    }
}
