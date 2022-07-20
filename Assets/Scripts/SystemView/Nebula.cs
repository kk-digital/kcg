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
            public float          spin;             // Angular velocity in degrees/second
            public Vector3        center;           // Center position to spin around

            public Texture2D      texture;
            public SpriteRenderer renderer;

            private System.Random rng;
            private float         last_time;

            public  ComputeShader         blur_noise_shader;
            public  ComputeShader        scale_noise_shader;
            public  ComputeShader exponential_filter_shader;
            public  ComputeShader         distortion_shader;
            public  ComputeShader      circular_blur_shader;
            public  ComputeShader      circular_mask_shader;

            private void generate() {
                // Shaders properties
                int      width_id = Shader.PropertyToID("width");
                int     height_id = Shader.PropertyToID("height");
                int      noise_id = Shader.PropertyToID("noise");
                int      scale_id = Shader.PropertyToID("scale");
                int   strength_id = Shader.PropertyToID("strength");
                int     radius_id = Shader.PropertyToID("radius");
                int distortion_id = Shader.PropertyToID("distortionnoise");
                int     output_id = Shader.PropertyToID("outputnoise");
                
                rng = new(seed);

                Color[] pixels    = new Color[width * height];

                for(int i = 0; i < width * height; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                float base_r = (float)rng.NextDouble() * 0.8f;
                float base_g = (float)rng.NextDouble() * 0.8f;
                float base_b = (float)rng.NextDouble() * 0.8f;

                // Generate base noise
                ComputeBuffer base_buffer1 = new ComputeBuffer(width * height, sizeof(float));
                ComputeBuffer base_buffer2 = new ComputeBuffer(width * height / 4096, sizeof(float));

                base_buffer2.SetData(ProceduralImages.generate_noise(rng, 1.0f, width / 64, height / 64));
                    
                // Scale noise  
                scale_noise_shader.SetInt( width_id, width);
                scale_noise_shader.SetInt(height_id, height);
                scale_noise_shader.SetInt( scale_id, 64);

                scale_noise_shader.SetBuffer(0,  noise_id, base_buffer2);
                scale_noise_shader.SetBuffer(0, output_id, base_buffer1);

                scale_noise_shader.Dispatch(0, width / 8, height / 8, 1);

                // Generate distortion noise
                base_buffer2.Release();
                base_buffer2 = new ComputeBuffer(width * height / 256, sizeof(float));
                base_buffer2.SetData(ProceduralImages.generate_noise(rng, 16.0f, width / 16, height / 16));

                ComputeBuffer distortion_buffer = new ComputeBuffer(width * height, sizeof(float));

                // Scale distortion noise
                scale_noise_shader.SetInt( width_id, width);
                scale_noise_shader.SetInt(height_id, height);
                scale_noise_shader.SetInt( scale_id, 16);

                scale_noise_shader.SetBuffer(0,  noise_id, base_buffer2);
                scale_noise_shader.SetBuffer(0, output_id, distortion_buffer);

                scale_noise_shader.Dispatch(0, width / 8, height / 8, 1);

                base_buffer2.Release();

                // Apply distortion
                base_buffer2 = new ComputeBuffer(width * height, sizeof(float));

                distortion_shader.SetInt( width_id, width);
                distortion_shader.SetInt(height_id, height);

                distortion_shader.SetFloat(strength_id, 16.0f);

                distortion_shader.SetBuffer(0, distortion_id, distortion_buffer);
                distortion_shader.SetBuffer(0,      noise_id, base_buffer1);
                distortion_shader.SetBuffer(0,     output_id, base_buffer2);

                distortion_shader.Dispatch(0, width / 8, height / 8, 1);

                distortion_buffer.Release();

                // Soften noise
                float[] base_alpha = new float[width * height];
                base_buffer1.GetData(base_alpha);
                base_buffer1.SetData(ProceduralImages.soften(base_alpha, 4, width, height));

                // Apply circular blur
                circular_blur_shader.SetInt( width_id, width);
                circular_blur_shader.SetInt(height_id, height);

                circular_blur_shader.SetFloat(radius_id, 16.0f);

                circular_blur_shader.SetBuffer(0,  noise_id, base_buffer1);
                circular_blur_shader.SetBuffer(0, output_id, base_buffer2);

                circular_blur_shader.Dispatch(0, width / 8, height / 8, 1);

                base_buffer1.Release();

                // Apply circular mask
                circular_mask_shader.SetInt( width_id, width);
                circular_mask_shader.SetInt(height_id, height);

                circular_mask_shader.SetBuffer(0, noise_id, base_buffer2);

                circular_mask_shader.Dispatch(0, width / 8, height / 8, 1);

                base_buffer2.GetData(base_alpha);

                base_buffer2.Release();

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        pixels[x + y * width].r = base_r;
                        pixels[x + y * width].g = base_g;
                        pixels[x + y * width].b = base_b;
                        pixels[x + y * width].a = base_alpha[x + y * width];
                    }

                float max_dist = Tools.get_distance(0, 0, width, height);

                for(int color = 0; color < colors; color++) {

                    float r0 = base_r * 0.6f + (float)rng.NextDouble() * base_r * 0.80f;
                    float g0 = base_g * 0.6f + (float)rng.NextDouble() * base_g * 0.80f;
                    float b0 = base_b * 0.6f + (float)rng.NextDouble() * base_b * 0.80f;

                    float r1 = base_r * 0.6f + (float)rng.NextDouble() * base_r * 0.80f;
                    float g1 = base_g * 0.6f + (float)rng.NextDouble() * base_g * 0.80f;
                    float b1 = base_b * 0.6f + (float)rng.NextDouble() * base_b * 0.80f;

                    float[] alpha = new float[width * height];
                    ComputeBuffer color_buffer1 = new ComputeBuffer(width * height, sizeof(float));

                    for(int layer = 0; layer < layers; layer++) {

                        int   scale         = 1 << layers - layer + 1;
                        float strength      = scale / layers;

                        ComputeBuffer layer_buffer = new ComputeBuffer(width / scale * height / scale, sizeof(float));

                        // Generate random noise
                        layer_buffer.SetData(ProceduralImages.generate_noise(rng, strength, width / scale, height / scale));

                        // Blur noise
                        blur_noise_shader.SetInt( width_id, width  / scale);
                        blur_noise_shader.SetInt(height_id, height / scale);

                        blur_noise_shader.SetBuffer(0, noise_id, layer_buffer);

                        blur_noise_shader.Dispatch(0, width / scale / 8, height / scale / 8, 1);

                        // Scale noise
                        scale_noise_shader.SetInt( width_id, width);
                        scale_noise_shader.SetInt(height_id, height);
                        scale_noise_shader.SetInt( scale_id, scale);

                        scale_noise_shader.SetBuffer(0,  noise_id, layer_buffer);
                        scale_noise_shader.SetBuffer(0, output_id, color_buffer1);

                        scale_noise_shader.Dispatch(0, width / 8, height / 8, 1);

                        layer_buffer.Release();
                    }

                    // Apply exponential filter
                    exponential_filter_shader.SetInt( width_id, width);
                    exponential_filter_shader.SetInt(height_id, height);

                    exponential_filter_shader.SetBuffer(0, noise_id, color_buffer1);

                    exponential_filter_shader.Dispatch(0, width / 8, height / 8, 1);

                    // Apply mask
                    color_buffer1.GetData(alpha);
                    color_buffer1.SetData(ProceduralImages.mask(rng, alpha, 8, width, height));

                    // Generate distortion noise
                    ComputeBuffer color_buffer2 = new ComputeBuffer(width * height / 4096, sizeof(float));
                    color_buffer2.SetData(ProceduralImages.generate_noise(rng, 16.0f, width / 64, height / 64));

                    // Scale distortion noise
                    ComputeBuffer distortion_noise = new ComputeBuffer(width * height, sizeof(float));

                    scale_noise_shader.SetInt( width_id, width);
                    scale_noise_shader.SetInt(height_id, height);
                    scale_noise_shader.SetInt( scale_id, 64);

                    scale_noise_shader.SetBuffer(0,  noise_id, color_buffer2);
                    scale_noise_shader.SetBuffer(0, output_id, distortion_noise);

                    scale_noise_shader.Dispatch(0, width / 8, height / 8, 1);

                    color_buffer2.Release();

                    // Apply distortion
                    color_buffer2 = new ComputeBuffer(width * height, sizeof(float));

                    distortion_shader.SetInt( width_id, width);
                    distortion_shader.SetInt(height_id, height);

                    distortion_shader.SetFloat(strength_id, 16.0f);

                    distortion_shader.SetBuffer(0, distortion_id, distortion_noise);
                    distortion_shader.SetBuffer(0,      noise_id, color_buffer1);
                    distortion_shader.SetBuffer(0,     output_id, color_buffer2);

                    distortion_shader.Dispatch(0, width / 8, height / 8, 1);

                    distortion_noise.Release();

                    // Soften noise
                    color_buffer2.GetData(alpha);
                    color_buffer1.SetData(ProceduralImages.soften(alpha, 8, width, height));
                    color_buffer2.Release();

                    // Apply circular mask
                    circular_mask_shader.SetInt( width_id, width);
                    circular_mask_shader.SetInt(height_id, height);

                    circular_mask_shader.SetBuffer(0, noise_id, color_buffer1);

                    circular_mask_shader.Dispatch(0, width / 8, height / 8, 1);

                    color_buffer1.GetData(alpha);
                    color_buffer1.Release();
                        
                    float target_x = rng.Next(width);
                    float target_y = rng.Next(height);

                    for(int x = 0; x < width; x++)
                        for(int y = 0; y < height; y++) {
                            float a                 = Tools.smootherstep(alpha[x + y * width] * 0.3f, 0.0f, 1.0f - pixels[x + y * width].a);

                            float d                 = Tools.get_distance(x, y, target_x, target_y) / max_dist;

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


                for(int i = 0; i < width * height; i++) {
                    // Adjust contrast
                    ProceduralImages.adjust_contrast(ref pixels[i].r, ref pixels[i].g, ref pixels[i].b, contrast);

                    // Adjust black level
                    ProceduralImages.adjust_black_level(ref pixels[i].r, ref pixels[i].g, ref pixels[i].b, ref pixels[i].a, cutoff);

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

                renderer.transform.Translate(new Vector3(0.0f, 0.0f, renderer.transform.position.z + 5.0f));

                last_time = Time.time;
            }

            private void Update() {
                float current_time = Time.time - last_time;
                      last_time    = Time.time;

                renderer.transform.RotateAround(center, new Vector3(0.0f, 0.0f, 1.0f), -current_time * spin);
            }
        }
    }
}
    