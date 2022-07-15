using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class AccretionDisk : MonoBehaviour {
            public int            seed;
            public int            layers;
            public int            colors;
            public int            width;
            public int            height;
            public float          opacity;
            public float          contrast;
            public float          cutoff;           // Also sometimes called black level
            public float          spin;             // Angular velocity in degrees/second
            public Vector3        center;           // Center point to spin around

            public Texture2D      texture;
            public SpriteRenderer renderer;

            private System.Random rng;
            private float         last_time;

            private void generate() {

                rng = new(seed);

                Color[] pixels    = new Color[width * height];

                for(int i = 0; i < width * height; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                float base_r = 0.6f + (float)rng.NextDouble() * 0.2f;
                float base_g = 0.6f + (float)rng.NextDouble() * 0.2f;
                float base_b = 0.4f + (float)rng.NextDouble() * 0.4f;

                float[] base_alpha = ProceduralImages.generate_noise(rng, 1.0f,             width / 64, height / 64);
                        base_alpha = ProceduralImages.smoothen_noise(base_alpha,            width / 64, height / 64, 64);
                        base_alpha = ProceduralImages.distort(rng,   base_alpha, 16.0f, 16, width,      height);
                        base_alpha = ProceduralImages.soften(        base_alpha, 4,         width,      height);
                        base_alpha = ProceduralImages.circular_blur( base_alpha,            width,      height, 16.0f);
                        base_alpha = ProceduralImages.circular_mask( base_alpha,            width,      height);

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        pixels[x + y * width].r = base_r;
                        pixels[x + y * width].g = base_g;
                        pixels[x + y * width].b = base_b;
                        pixels[x + y * width].a = base_alpha[x + y * width];
                    }

                float max_dist      = Tools.get_distance(0, 0, width,     height);
                float max_half_dist = Tools.get_distance(0, 0, width / 2, height / 2);

                for(int color = 0; color < colors; color++) {

                    float r0 = base_r * 0.6f + (float)rng.NextDouble() * base_r * 0.80f;
                    float g0 = base_g * 0.6f + (float)rng.NextDouble() * base_g * 0.80f;
                    float b0 = base_b * 0.6f + (float)rng.NextDouble() * base_b * 0.80f;

                    float r1 = base_r * 0.6f + (float)rng.NextDouble() * base_r * 0.80f;
                    float g1 = base_g * 0.6f + (float)rng.NextDouble() * base_g * 0.80f;
                    float b1 = base_b * 0.6f + (float)rng.NextDouble() * base_b * 0.80f;

                    float[] alpha = new float[width * height];

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
                    alpha = ProceduralImages.mask(   rng,       alpha,         8, width, height);
                    alpha = ProceduralImages.distort(rng,       alpha, 16.0f, 64, width, height);
                    alpha = ProceduralImages.soften(            alpha,         8, width, height);
                    alpha = ProceduralImages.swirl(             alpha,            width, height);

                    float target_x = rng.Next(width);
                    float target_y = rng.Next(height);

                    for(int x = 0; x < width; x++)
                        for(int y = 0; y < height; y++) {
                            float a                 = Tools.smootherstep(alpha[x + y * width] * 0.3f, 0.0f, 1.0f - pixels[x + y * width].a);

                            float d                 = Tools.get_distance(x, y, target_x,  target_y) / max_dist;

                            float r                 = Tools.smootherstep(r0, r1, d);
                            float g                 = Tools.smootherstep(g0, g1, d);
                            float b                 = Tools.smootherstep(b0, b1, d);

                            float blended_alpha     =      a +                           pixels[x + y * width].a * (1.0f - a);
                            pixels[x + y * width].r = (r * a + pixels[x + y * width].r * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].g = (g * a + pixels[x + y * width].g * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].b = (b * a + pixels[x + y * width].b * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].a = blended_alpha;
                        }
                }


                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        int i = x + y * width;

                        // Adjust contrast
                        ProceduralImages.adjust_contrast(ref pixels[i].r, ref pixels[i].g, ref pixels[i].b, contrast);

                        // Adjust black level
                        ProceduralImages.adjust_black_level(ref pixels[i].r, ref pixels[i].g, ref pixels[i].b, ref pixels[i].a, cutoff);

                        // Adjust opacity
                        pixels[i].a *= opacity;

                        // Fade pixels that are farther away from center using a smootherstep curve
                        pixels[i].a  = Tools.smootherstep(pixels[i].a * 2.0f, 0.0f, Tools.get_distance(x, y, width / 2, height / 2) / max_half_dist);
                    }

                texture = new Texture2D(width, height);
                texture.filterMode = FilterMode.Trilinear;
                texture.SetPixels(pixels);

                texture.Apply();

            }

            private void Start() {
                generate();

                renderer        = gameObject.AddComponent<SpriteRenderer>();
                renderer.sprite = Sprite.Create(texture,
                                                new Rect(0, 0, width, height),
                                                new Vector2(0.5f, 0.5f));

                last_time       = Time.time;
            }

            private void Update() {
                float current_time = Time.time - last_time;
                      last_time    = Time.time;

                renderer.transform.RotateAround(center, new Vector3(0.0f, 0.0f, 1.0f), -current_time * spin);
            }
        }
    }
}
