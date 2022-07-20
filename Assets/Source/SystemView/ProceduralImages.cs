using System;

// DEPRECATED
// 
// Most of these functions have been deprecated and replaced by new compute shaders
// The new compute shaders can be found in Shaders/SystemView/Compute

namespace Source {
    namespace SystemView {
        public static class ProceduralImages {
            // This function generates random noise from a seeded System.Random

            public static float[] generate_noise(Random rng, float strength, int w, int h) {
                float[] noise = new float[w * h];

                for(int x = 1; x < w - 1; x++)
                    for(int y = 1; y < h - 1; y++) {
                        noise[x + y * w] = (float)rng.NextDouble() * strength;
                        if(noise[x + y * w] > 1.0f) noise[x + y * w] = 1.0f;
                    }

                return noise;
            }

            // Blurs noise in a 3x3 grid
            // 
            // Center pixel is weighed highest
            // Side pixels (left/right/up/down) are weighed second highest
            // Corner pixels (top left/top right/bottom right/bottom left) are weighed third highest

            [Obsolete("The blur noise function has been deprecated. Use the compute shader in Shaders/SystemView/Compute/blur_noise.compute instead.")]
            public static float[] blur_noise(float[] noise, int w, int h) {

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

            // Blurs pixels within the defined radius
            // 
            // The values of pixels closer to the center are prioritized over the values of those farther away

            [Obsolete("The circular blur function has been deprecated. Use the compute shader in Shaders/SystemView/Compute/circular_blur.compute instead.")]
            public static float[] circular_blur(float[] noise, float r, int w, int h) {
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

            // Scales up noise smoothly, following a smootherscale curve.

            [Obsolete("The smoothen noise function has been deprecated. Use the compute shader in Shaders/SystemView/Compute/scale_noise.compute instead.")]
            public static float[] smoothen_noise(float[] noise, int w, int h, int scale) {
                int   scaled_w = w * scale;
                int   scaled_h = h * scale;
                float scale_factor = 1.0f / scale;

                float[] smoothed_noise = new float[scaled_w * scaled_h];

                // Horizontal smoothing

                for(int x = 0; x < w - 1; x++)
                    for(int y = 0; y < h; y++)
                        for(int i = 0; i < scale; i++)
                            smoothed_noise[i + x * scale + y * scale * scaled_w] = Tools.smootherstep(noise[x      + y * w],
                                                                                                      noise[(x + 1) + y * w],
                                                                                                      (float)i * scale_factor);

                // Vertical smoothing
                for(int x = 0; x < scaled_w; x++)
                    for(int y = 0; y < h - 1; y++)
                        for(int i = 0; i < scale; i++)
                            smoothed_noise[x + (y * scale + i) * scaled_w]       = Tools.smootherstep(smoothed_noise[x +  y      * scale * scaled_w],
                                                                                                      smoothed_noise[x + (y + 1) * scale * scaled_w],
                                                                                                      (float)i * scale_factor);

                return smoothed_noise;
            }

            // Applies an exponential filter to noise, which filters out noise with low values and produces a nicer result.

            [Obsolete("The exponential filter function has been deprecated. Use the compute shader in Shaders/SystemView/Compute/exponential_filter.compute instead.")]
            public static float[] exponential_filter(float[] noise, int w, int h) {
                float cutoff    = 0.05f;
                float sharpness = 0.95f;

                for(int i = 0; i < w * h; i++) {
                    float        c = 255.0f * (noise[i] - (1.0f - cutoff));
                    if(c < 0.0f) c = 0.0f;
                    noise[i]       = 1.0f - (float)Math.Pow(sharpness, c);
                }

                return noise;
            }

            // Distorts the noise using a randomly generated distortion map.
            // The result of this are many curves and swivels which give a more natural feel.

            [Obsolete("The distort function has been deprecated. Use the compute shader in Shaders/SystemView/Compute/distortion_shader.compute instead.")]
            public static float[] distort(Random rng, float[] noise, float strength, int distortion_scale, int w, int h) {
                float[] distortion_noise = smoothen_noise(generate_noise(rng, 1.0f, w / distortion_scale, h / distortion_scale),
                                                                                    w / distortion_scale, h / distortion_scale, distortion_scale);
                float[] distorted        = new float[w * h];

                for(int x = 0; x < w; x++)
                    for(int y = 0; y < h; y++) {
                        float distortion_angle = distortion_noise[x + y * w] * Tools.twopi;
                        float distort_x_by     = (float)Math.Cos(distortion_angle) * strength;
                        float distort_y_by     = (float)Math.Sin(distortion_angle) * strength;

                        float original_x       = x + distort_x_by;
                        float original_y       = y + distort_y_by;

                        if(original_x < 0) original_x = w + original_x;
                        if(original_y < 0) original_y = h + original_y;

                        int   x0 = (int)original_x;
                        int   x1 = (int)original_x + 1;
                        int   y0 = (int)original_y;
                        int   y1 = (int)original_y + 1;

                        float dx = original_x - x0;
                        float dy = original_y - y0;

                        float p0 = Tools.smootherstep(noise[x0 % w + (y0 % h) * w], noise[x1 % w + (y0 % h) * w], dx);
                        float p1 = Tools.smootherstep(noise[x0 % w + (y1 % h) * w], noise[x1 % w + (y1 % h) * w], dx);

                        distorted[x + y * w] = Tools.smootherstep(p0, p1, dy);
                    }

                return distorted;
            }

            // Applies a random "natural looking" mask to the noise.
            // This results in parts of the noise being cut out.
            // The result is hard to explain, and it's easier to just try the function.

            public static float[] mask(Random rng, float[] noise, int repetitions, int w, int h) {
                float[] masking_noise = generate_noise(rng, 1.0f, w, h);

                for(int i = 0; i < repetitions; i++)
                    for(int x = 0; x < w; x++)
                        for(int y = 0; y < h; y++) {

                            int x0 =  x - 1;
                            int x1 = (x + 1) % w;
                            int y0 =  y - 1;
                            int y1 = (y + 1) % h;

                            if(x0 < 0) x0 = w + x0;
                            if(y0 < 0) y0 = h + y0;

                            float sum = masking_noise[x0 + y0 * w]
                                      + masking_noise[x  + y0 * w]
                                      + masking_noise[x1 + y0 * w]
                                      + masking_noise[x0 + y  * w]
                                      + masking_noise[x  + y  * w]
                                      + masking_noise[x1 + y  * w]
                                      + masking_noise[x0 + y1 * w]
                                      + masking_noise[x  + y1 * w]
                                      + masking_noise[x1 + y1 * w];

                            if(sum < 4.0f) masking_noise[x + y * w]  = 0.00f;
                            else masking_noise[x + y * w] += 0.25f;
                        }

                for(int x = 0; x < w; x++)
                    for(int y = 0; y < h; y++)
                        noise[x + y * w] *= masking_noise[x + y * w];

                return noise;
            }

            // Softens edges and shapes in a noise map.
            public static float[] soften(float[] noise, int repetitions, int w, int h) {
                for(int i = 0; i < repetitions; i++)
                    for(int x = 0; x < w; x++)
                        for(int y = 0; y < h; y++) {
                            int   x0             =  x;
                            int   x1             = (x + 1) % w;

                            int   y0             =  y;
                            int   y1             = (y + 1) % h;

                            float v0             = Tools.smootherstep(noise[x0 + y0 * w], noise[x1 + y0 * w], (noise[x0 + y0 * w] + noise[x1 + y0 * w]) * 0.5f);
                            float v1             = Tools.smootherstep(noise[x0 + y1 * w], noise[x1 + y1 * w], (noise[x0 + y1 * w] + noise[x1 + y1 * w]) * 0.5f);

                            noise[x + y * w] = Tools.smootherstep(v0, v1, (v0 + v1) * 0.5f);
                        }

                return noise;
            }

            // Applies a circular mask to the noise map.
            // This converts the square noise map to a circle.

            [Obsolete("The circular mask function has been deprecated. Use the compute shader in Shaders/SystemView/Compute/circular_mask.compute instead.")]
            public static float[] circular_mask(float[] noise, int w, int h) {
                float[] output = new float[w * h];

                for(int x = 0; x < w; x++)
                    for(int y = 0; y < h; y++) {
                        float half_w  = w  * 0.5f;
                        float half_h  = h * 0.5f;
                        float local_x = x - half_w;
                        float local_y = y - half_h;

                        float d       = local_x * local_x / (half_w * half_w)
                                      + local_y * local_y / (half_h * half_h);

                        if(d <= 0.15f)
                            output[x + y * w] = noise[x + y * w];
                        else if(d <= 0.5f)
                            output[x + y * w] = Tools.smootherstep(0.0f, noise[x + y * w], (0.5f - d) / 0.35f);
                    }

                return output;
            }

            // Clamps a float value between 0.0f and 1.0f

            public static float truncate(float f) {
                if(f < 0.0f) return 0.0f;
                if(f > 1.0f) return 1.0f;
                return f;
            }

            // Adjusts the contrast of a pixel

            public static void adjust_contrast(ref float r, ref float g, ref float b, float contrast) {
                r = truncate(contrast * (r - 0.5f) + 0.5f);
                g = truncate(contrast * (g - 0.5f) + 0.5f);
                b = truncate(contrast * (b - 0.5f) + 0.5f);
            }

            // Returns the brightness of a pixel

            public static float get_brightness(float r, float g, float b) {
                return Tools.magnitude(r, g, b);
            }

            // Adjusts the black level of a pixel.
            //
            // Black level = brightness at the darkest part of an image.

            public static void adjust_black_level(ref float r, ref float g, ref float b, ref float a, float black_level) {
                a = ProceduralImages.truncate((a - black_level) / (1.0f - black_level));

                float old_brightness =  get_brightness(r, g, b);

                if(old_brightness <= 0.0f) return;

                float new_brightness = (old_brightness - black_level) / (1.0f - black_level);
                float scale_factor   =  new_brightness / old_brightness;

                r *= scale_factor;
                g *= scale_factor;
                b *= scale_factor;
            }

            // Swirls noise around in a circular motion
            // Results in a spiral

            public static float[] swirl(float[] noise, float spin, int w, int h) {
                int half_width  = w / 2;
                int half_height = h / 2;

                int maxr        = (w + h) / 4;

                float[] alpha   = new float[w * h];

                for(int x = 0; x < w; x++)
                    for(int y = 0; y < h; y++) {
                        float r = Tools.get_distance(x, y, half_width, half_height) / maxr;

                        if(r > 1.0f || r == 0.0f) continue;

                        float a  = Tools.get_angle(half_width - x, half_height - y);
                              a += (spin > Tools.pi ? Tools.pi : spin) / r;

                        float original_x = half_width  * (1.0f + r * (float)Math.Cos(a));
                        float original_y = half_height * (1.0f + r * (float)Math.Sin(a));

                        int x0 = (int)original_x;
                        int x1 = (int)original_x + 1;
                        int y0 = (int)original_y;
                        int y1 = (int)original_y + 1;

                        if(x0 <  0) x0 =     0;
                        if(x0 >= w) x0 = w - 1;
                        if(y0 <  0) y0 =     0;
                        if(y0 >= h) y0 = h - 1;

                        if(x1 < 0 || x1 >= w) x1 = x0;
                        if(y1 < 0 || y1 >= h) y1 = y0;

                        float dx = original_x - x0;
                        float dy = original_y - y0;

                        float v0 = Tools.smootherstep(noise[x0 + y0 * w], noise[x1 + y0 * w], dx);
                        float v1 = Tools.smootherstep(noise[x0 + y1 * w], noise[x1 + y1 * w], dx);

                        alpha[x + y * w] = Tools.smootherstep(v0, v1, dy);
                    }

                return alpha;
            }
        }
    }
}
