using System;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class Nebula : MonoBehaviour {
            public int            seed;
            public int            layers;
            public int            color_layers;
            public int            width;
            public int            height;

            public Texture2D      texture;
            public SpriteRenderer renderer;

            private System.Random rng;

            private float smootherstep(float a, float b, float w) {
                if(w < 0.0f) return a;
                if(w > 1.0f) return b;

                return (b - a) * ((w * (w * 6.0f - 15.0f) + 10.0f) * w * w * w) + a;
            }

            private float[] generate_noise(float strength, int w, int h) {
                float[] noise = new float[w * h];

                // Generate random noise

                for(int x = 1; x < w - 1; x++)
                    for(int y = 1; y < h - 1; y++) {
                        noise[x + y * w] = (float)rng.NextDouble() * strength;
                        if(noise[x + y * w] > 1.0f) noise[x + y * w] = 1.0f;
                    }

                return noise;
            }

            private float[] blur_noise(float[] noise, int w, int h) {

                for(int x = 1; x < w - 1; x++)
                    for(int y = 1; y < h - 1; y++) {

                        float center  =   0.20f *  noise[x + y * w];
                        float sides   =  0.125f * (noise[x - 1 +  y      * w] + noise[x + 1 +  y      * w]
                                                +  noise[x     + (y - 1) * w] + noise[x     + (y + 1) * w]);
                        float corners = 0.0625f * (noise[x - 1 + (y - 1) * w] + noise[x + 1 + (y - 1) * w]
                                                +  noise[x - 1 + (y + 1) * w] + noise[x + 1 + (y + 1) * w]);

                        noise[x + y * w] = center + sides + corners;

                    }

                return noise;
            }

            private float[] smoothen_noise(float[] noise, int w, int h, int scale) {
                int   scaled_w = w * scale;
                int   scaled_h = h * scale;
                float scale_factor = 1.0f / scale;

                float[] smoothed_noise = new float[scaled_w * scaled_h];

                // Horizontal smoothing

                for(int x = 0; x < w - 1; x++)
                    for(int y = 0; y < h; y++)
                        for(int i = 0; i < scale; i++)
                            smoothed_noise[i + x * scale + y * scale * scaled_w] = smootherstep(noise[ x      + y * w],
                                                                                                noise[(x + 1) + y * w],
                                                                                                (float)i * scale_factor);

                // Vertical smoothing
                for(int x = 0; x < scaled_w; x++)
                    for(int y = 0; y < h - 1; y++)
                        for(int i = 0; i < scale; i++)
                            smoothed_noise[x + (y * scale + i) * scaled_w]       = smootherstep(smoothed_noise[x +  y      * scale * scaled_w],
                                                                                                smoothed_noise[x + (y + 1) * scale * scaled_w],
                                                                                                (float)i * scale_factor);

                return smoothed_noise;
            }

            private float[] exponential_filter(float[] noise) {
                float cutoff    = 0.20f;
                float sharpness = 0.95f;

                for(int i = 0; i < width * height; i++) {
                    float        c = 255.0f * (noise[i] - (1.0f - cutoff));
                    if(c < 0.0f) c = 0.0f;
                    noise[i]       = 1.0f - (float)Math.Pow(sharpness, c);
                }

                return noise;
            }

            private void generate() {
                rng = new(seed);

                Color[] pixels    = new Color[width * height];

                for(int i = 0; i < width * height; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                for(int color = 0; color < color_layers; color++) {

                    float r = (float)rng.NextDouble();
                    float g = (float)rng.NextDouble();
                    float b = (float)rng.NextDouble();

                    float[] alpha = new float[width * height];

                    for(int layer = 0; layer < layers; layer++) {

                        int   scale         = 1 << layers - layer - 1;
                        float strength      = scale / layers;

                        float[] layer_alpha = exponential_filter(smoothen_noise(blur_noise(generate_noise(strength,
                                                                                                          width  / scale,
                                                                                                          height / scale),
                                                                                                          width  / scale,
                                                                                                          height / scale),
                                                                                                          width  / scale,
                                                                                                          height / scale,
                                                                                                                   scale));

                        for(int x = 0; x < width; x++)
                            for(int y = 0; y < height; y++) {
                                float a              = layer_alpha[x + y * width];
                                alpha[x + y * width] = a  +  alpha[x + y * width] * (1.0f - a);
                            }
                    }

                    for(int x = 0; x < width; x++)
                        for(int y = 0; y < height; y++) {
                            float a                 = alpha[x + y * width];

                            float blended_alpha     =      a +                           pixels[x + y * width].a * (1.0f - a);
                            pixels[x + y * width].r = (r * a + pixels[x + y * width].r * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].g = (g * a + pixels[x + y * width].g * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].b = (b * a + pixels[x + y * width].b * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].a = blended_alpha;
                        }
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
            }

            private void Update() {
                
            }
        }
    }
}
    