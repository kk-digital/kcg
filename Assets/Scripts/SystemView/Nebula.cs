using System;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class Nebula : MonoBehaviour {
            public int            seed;
            public int            layers;
            public int            colors;
            public int            width;
            public int            height;
            public float          opacity;
            public float          contrast;
            public float          cutoff;           // Also sometimes called black level

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

            private float[] circular_blur(float[] noise, int w, int h, float r) {
                if(r <= 1.0f) return noise;

                float[] blurred = new float[w * h];

                for(int x = 0; x < w; x++)
                    for(int y = 0; y < h; y++) {
                        float pixels  = 0.0f;
                        float value   = 0.0f;

                        int   large_r = (int)Math.Ceiling(r);

                        for(int x0 = x - large_r; x0 <= x; x0++) {
                            int x1 = x + x - x0;
                            if(x0 < 0 || x1 >= w) continue;

                            for(int y0 = y - large_r; y0 <= y; y0++) {
                                int y1 = y + y - y0;
                                if(y0 < 0 || y1 >= h) continue;

                                int local_x = x - x0;
                                int local_y = y - y0;

                                float distance = Tools.magnitude(local_x, local_y);

                                if(distance > r) continue;

                                pixels += 4 * distance / r;
                                value  += (noise[x0 + y0 * w]
                                       +   noise[x1 + y0 * w]
                                       +   noise[x0 + y1 * w]
                                       +   noise[x1 + y1 * w]) * (1.0f - distance / r);
                            }
                        }

                        blurred[x + y * w] = value / pixels;
                    }

                return blurred;
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
                float cutoff    = 0.05f;
                float sharpness = 0.95f;

                for(int i = 0; i < width * height; i++) {
                    float        c = 255.0f * (noise[i] - (1.0f - cutoff));
                    if(c < 0.0f) c = 0.0f;
                    noise[i]       = 1.0f - (float)Math.Pow(sharpness, c);
                }

                return noise;
            }

            private float[] distort(float[] noise, float strength, int distortion_scale) {
                float[] distortion_noise = smoothen_noise(generate_noise(1.0f, width / distortion_scale, height / distortion_scale),
                                                                               width / distortion_scale, height / distortion_scale, distortion_scale);
                float[] distorted        = new float[width * height];

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        float distortion_angle = distortion_noise[x + y * width] * Tools.twopi;
                        float distort_x_by     = (float)Math.Cos(distortion_angle) * strength;
                        float distort_y_by     = (float)Math.Sin(distortion_angle) * strength;

                        float original_x       = x + distort_x_by;
                        float original_y       = y + distort_y_by;

                        if(original_x < 0) original_x = width  + original_x;
                        if(original_y < 0) original_y = height + original_y;

                        int   x0 = (int)original_x;
                        int   x1 = (int)original_x + 1;
                        int   y0 = (int)original_y;
                        int   y1 = (int)original_y + 1;

                        float dx = original_x - x0;
                        float dy = original_y - y0;

                        float p0 = smootherstep(noise[x0 % width + (y0 % height) * width], noise[x1 % width + (y0 % height) * width], dx);
                        float p1 = smootherstep(noise[x0 % width + (y1 % height) * width], noise[x1 % width + (y1 % height) * width], dx);

                        distorted[x + y * width] = smootherstep(p0, p1, dy);
                    }

                return distorted;
            }

            private float[] mask(float[] noise, int repetitions) {
                float[] masking_noise = generate_noise(1.0f, width, height);

                for(int i = 0; i < repetitions; i++)
                    for(int x = 0; x < width; x++)
                        for(int y = 0; y < height; y++) {
                        
                            int x0 =  x - 1;
                            int x1 = (x + 1) % width;
                            int y0 =  y - 1;
                            int y1 = (y + 1) % height;

                            if(x0 < 0) x0 = width  + x0;
                            if(y0 < 0) y0 = height + y0;

                            float sum = masking_noise[x0 + y0 * width]
                                      + masking_noise[x  + y0 * width]
                                      + masking_noise[x1 + y0 * width]
                                      + masking_noise[x0 + y  * width]
                                      + masking_noise[x  + y  * width]
                                      + masking_noise[x1 + y  * width]
                                      + masking_noise[x0 + y1 * width]
                                      + masking_noise[x  + y1 * width]
                                      + masking_noise[x1 + y1 * width];

                            if(sum < 4.0f) masking_noise[x + y * width]  = 0.00f;
                            else           masking_noise[x + y * width] += 0.25f;
                        }

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++)
                        noise[x + y * width] *= masking_noise[x + y * width];

                return noise;
            }

            private float[] soften(float[] noise, int repetitions) {
                for(int i = 0; i < repetitions; i++)
                    for(int x = 0; x < width; x++)
                        for(int y = 0; y < height; y++) {
                            int   x0             =  x;
                            int   x1             = (x + 1) % width;

                            int   y0             =  y;
                            int   y1             = (y + 1) % height;

                            float v0             = smootherstep(noise[x0 + y0 * width], noise[x1 + y0 * width], (noise[x0 + y0 * width] + noise[x1 + y0 * width]) * 0.5f);
                            float v1             = smootherstep(noise[x0 + y1 * width], noise[x1 + y1 * width], (noise[x0 + y1 * width] + noise[x1 + y1 * width]) * 0.5f);

                            noise[x + y * width] = smootherstep(v0, v1, (v0 + v1) * 0.5f);
                        }

                return noise;
            }

            private float[] circular_mask(float[] noise) {
                float[] output = new float[width * height];

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        float half_width  = width  * 0.5f;
                        float half_height = height * 0.5f;
                        float local_x     = x - half_width;
                        float local_y     = y - half_height;

                        float d           = local_x * local_x / (half_width  * half_width)
                                          + local_y * local_y / (half_height * half_height);

                        if(d <= 0.15f)
                            output[x + y * width] = noise[x + y * width];
                        else if(d <= 0.5f)
                            output[x + y * width] = smootherstep(0.0f, noise[x + y * width], (0.5f - d) / 0.35f);
            }

                return output;
            }

            private float truncate(float f) {
                if(f < 0.0f) return 0.0f;
                if(f > 1.0f) return 1.0f;
                             return    f;
            }

            private void generate() {
                rng = new(seed);

                Color[] pixels    = new Color[width * height];

                for(int i = 0; i < width * height; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                float base_r = (float)rng.NextDouble() * 0.8f;
                float base_g = (float)rng.NextDouble() * 0.8f;
                float base_b = (float)rng.NextDouble() * 0.8f;

                float[] base_alpha = soften(distort(smoothen_noise(generate_noise(1.0f,
                                                                                  width  / 64,
                                                                                  height / 64),
                                                                                  width  / 64,
                                                                                  height / 64,
                                                                                  64),
                                                                                  16.0f,
                                                                                  16),
                                                                                  4);

                base_alpha = circular_blur(base_alpha, width, height, 16.0f);

                base_alpha = circular_mask(base_alpha);

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        pixels[x + y * width].r = base_r;
                        pixels[x + y * width].g = base_g;
                        pixels[x + y * width].b = base_b;
                        pixels[x + y * width].a = base_alpha[x + y * width];
                    }

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

                        float[] layer_alpha = smoothen_noise(blur_noise(generate_noise(strength,
                                                                                       width  / scale,
                                                                                       height / scale),
                                                                                       width  / scale,
                                                                                       height / scale),
                                                                                       width  / scale,
                                                                                       height / scale,
                                                                                       scale);

                        for(int x = 0; x < width; x++)
                            for(int y = 0; y < height; y++) {
                                float a              = layer_alpha[x + y * width] * 1.5f;
                                alpha[x + y * width] = a  +  alpha[x + y * width] * (1.0f - a);
                            }
                    }

                    alpha = exponential_filter(alpha);

                    alpha = soften(distort(mask(alpha, 8), 16.0f, 64), 8);

                    alpha = circular_mask(alpha);

                    float target_x = rng.Next(width);
                    float target_y = rng.Next(height);

                    for(int x = 0; x < width; x++)
                        for(int y = 0; y < height; y++) {
                            float a                 = smootherstep(alpha[x + y * width] * 0.3f, 0.0f, 1.0f - pixels[x + y * width].a);

                            float d                 = Tools.get_distance(x, y, target_x, target_y) / Tools.get_distance(0, 0, width, height);

                            float r                 = smootherstep(r0, r1, d);
                            float g                 = smootherstep(g0, g1, d);
                            float b                 = smootherstep(b0, b1, d);

                            float blended_alpha     =      a +                           pixels[x + y * width].a * (1.0f - a);
                            pixels[x + y * width].r = (r * a + pixels[x + y * width].r * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].g = (g * a + pixels[x + y * width].g * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].b = (b * a + pixels[x + y * width].b * pixels[x + y * width].a * (1.0f - a)) / blended_alpha;
                            pixels[x + y * width].a = blended_alpha;
                        }
                }

                float contrast_factor =  1.06f * (contrast * 0.5f + 1.00f)
                                      / (1.00f * (1.06f - contrast * 0.5f));

                for(int i = 0; i < width * height; i++) {
                    // Increase contrast
                    pixels[i].r = truncate(contrast_factor * (pixels[i].r - 0.5f) + 0.5f);
                    pixels[i].g = truncate(contrast_factor * (pixels[i].g - 0.5f) + 0.5f);
                    pixels[i].b = truncate(contrast_factor * (pixels[i].b - 0.5f) + 0.5f);

                    // Adjust black level
                    pixels[i].r = truncate((pixels[i].r - cutoff) / (1.0f - cutoff));
                    pixels[i].g = truncate((pixels[i].g - cutoff) / (1.0f - cutoff));
                    pixels[i].b = truncate((pixels[i].b - cutoff) / (1.0f - cutoff));
                    pixels[i].a = truncate((pixels[i].a - cutoff) / (1.0f - cutoff));

                    // Adjust opacity
                    pixels[i].a *= opacity;
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

                renderer.transform.Translate(new Vector3(0.0f, 0.0f, 5.0f));
            }
        }
    }
}
    