using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class SystemPlanetRenderer : MonoBehaviour {
            public  SystemPlanet     planet;

            public  SpriteRenderer   sr;
            public  OrbitRenderer    or;

            public  int              seed;
            public  Texture2D        texture;
            public  Color            basecolor;
            public  Color[]          colors;
            public  int              radius;
            public  int              layers;

            public  Color            orbitColor = new Color(0.5f, 0.7f, 1.0f, 1.0f);

            public  CameraController Camera;
            private System.Random    rng;

            public  ComputeShader    blur_noise_shader;
            public  ComputeShader    scale_noise_shader;
            public  ComputeShader    exponential_filter_shader;
            public  ComputeShader    distortion_shader;
            public  ComputeShader    circular_blur_shader;
            public  ComputeShader    circular_mask_shader;

            public  bool             autoinit; // For testing

            private bool             initialized;
            private int              texture_size = 512;

            private void generate() {
                // Shaders properties
                int      width_id = Shader.PropertyToID("radius");
                int     height_id = Shader.PropertyToID("radius");
                int      noise_id = Shader.PropertyToID("noise");
                int      scale_id = Shader.PropertyToID("scale");
                int   strength_id = Shader.PropertyToID("strength");
                int     radius_id = Shader.PropertyToID("radius");
                int distortion_id = Shader.PropertyToID("distortionnoise");
                int     output_id = Shader.PropertyToID("outputnoise");
                int    sharpen_id = Shader.PropertyToID("sharpen");

                rng = new(seed);

                Color[] pixels    = new Color[texture_size * texture_size];

                for(int i = 0; i < texture_size * texture_size; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                // Generate base noise
                float[] base_alpha = new float[texture_size * texture_size];
                for(int i = 0; i < texture_size * texture_size; i++) base_alpha[i] = 1.0f;

                ComputeBuffer base_buffer = new ComputeBuffer(texture_size * texture_size, sizeof(float));

                base_buffer.SetData(base_alpha);

                // Apply circular mask
                circular_mask_shader.SetInt( width_id, texture_size);
                circular_mask_shader.SetInt(height_id, texture_size);

                circular_mask_shader.SetBool(sharpen_id, true);

                circular_mask_shader.SetBuffer(0, noise_id, base_buffer);

                circular_mask_shader.Dispatch(0, texture_size / 8, texture_size / 8, 1);

                base_buffer.GetData(base_alpha);
                base_buffer.Release();

                for(int x = 0; x < texture_size; x++)
                    for(int y = 0; y < texture_size; y++) {
                        pixels[x + y * texture_size].r = basecolor.r;
                        pixels[x + y * texture_size].g = basecolor.g;
                        pixels[x + y * texture_size].b = basecolor.b;
                        pixels[x + y * texture_size].a = base_alpha[x + y * texture_size];
                    }

                if(colors != null)
                    foreach(Color color in colors) {
                        float[] alpha = new float[texture_size * texture_size];
                        ComputeBuffer color_buffer1 = new ComputeBuffer(texture_size * texture_size, sizeof(float));

                        for(int layer = 0; layer < layers; layer++) {

                            int   scale         = 1 << layers - layer - 1;
                            float strength      = scale / layers;

                            ComputeBuffer layer_buffer = new ComputeBuffer(texture_size / scale * texture_size / scale, sizeof(float));

                            // Generate random noise
                            layer_buffer.SetData(ProceduralImages.generate_noise(rng, strength, texture_size / scale, texture_size / scale));

                            // Blur noise
                            blur_noise_shader.SetInt( width_id, texture_size / scale);
                            blur_noise_shader.SetInt(height_id, texture_size / scale);

                            blur_noise_shader.SetBuffer(0, noise_id, layer_buffer);

                            blur_noise_shader.Dispatch(0, texture_size / scale / 8, texture_size / scale / 8, 1);

                            // Scale noise
                            scale_noise_shader.SetInt( width_id, texture_size);
                            scale_noise_shader.SetInt(height_id, texture_size);
                            scale_noise_shader.SetInt( scale_id, scale);

                            scale_noise_shader.SetBuffer(0,  noise_id, layer_buffer);
                            scale_noise_shader.SetBuffer(0, output_id, color_buffer1);

                            scale_noise_shader.Dispatch(0, texture_size / 8, texture_size / 8, 1);

                            layer_buffer.Release();

                        }

                        // Apply exponential filter
                        exponential_filter_shader.SetInt( width_id, texture_size);
                        exponential_filter_shader.SetInt(height_id, texture_size);

                        exponential_filter_shader.SetBuffer(0, noise_id, color_buffer1);

                        exponential_filter_shader.Dispatch(0, texture_size / 8, texture_size / 8, 1);

                        // Apply mask
                        color_buffer1.GetData(alpha);
                        color_buffer1.SetData(ProceduralImages.mask(rng, alpha, 8, texture_size, texture_size));

                        // Generate distortion noise
                        ComputeBuffer color_buffer2 = new ComputeBuffer(texture_size * texture_size / 4096, sizeof(float));
                        color_buffer2.SetData(ProceduralImages.generate_noise(rng, 1.0f, texture_size / 64, texture_size / 64));

                        // Scale distortion noise
                        ComputeBuffer distortion_noise = new ComputeBuffer(texture_size * texture_size, sizeof(float));

                        scale_noise_shader.SetInt( width_id, texture_size);
                        scale_noise_shader.SetInt(height_id, texture_size);
                        scale_noise_shader.SetInt( scale_id, 64);

                        scale_noise_shader.SetBuffer(0,  noise_id, color_buffer2);
                        scale_noise_shader.SetBuffer(0, output_id, distortion_noise);

                        scale_noise_shader.Dispatch(0, texture_size / 8, texture_size / 8, 1);

                        color_buffer2.Release();

                        // Apply distortion
                        color_buffer2 = new ComputeBuffer(texture_size * texture_size, sizeof(float));

                        distortion_shader.SetInt( width_id, texture_size);
                        distortion_shader.SetInt(height_id, texture_size);

                        distortion_shader.SetFloat(strength_id, 8.0f);

                        distortion_shader.SetBuffer(0, distortion_id, distortion_noise);
                        distortion_shader.SetBuffer(0,      noise_id, color_buffer1);
                        distortion_shader.SetBuffer(0,     output_id, color_buffer2);

                        distortion_shader.Dispatch(0, texture_size / 8, texture_size / 8, 1);

                        distortion_noise.Release();

                        // Apply circular blur
                        circular_blur_shader.SetInt( width_id, texture_size);
                        circular_blur_shader.SetInt(height_id, texture_size);

                        circular_blur_shader.SetFloat(radius_id, 4.0f);

                        circular_blur_shader.SetBuffer(0, noise_id,  color_buffer2);
                        circular_blur_shader.SetBuffer(0, output_id, color_buffer1);

                        circular_blur_shader.Dispatch(0, texture_size / 8, texture_size / 8, 1);

                        color_buffer2.Release();

                        // Soften noise
                        color_buffer1.GetData(alpha);
                        color_buffer1.SetData(ProceduralImages.soften(alpha, 8, texture_size, texture_size));

                        // Apply circular mask
                        circular_mask_shader.SetInt( width_id, texture_size);
                        circular_mask_shader.SetInt(height_id, texture_size);

                        circular_mask_shader.SetBool(sharpen_id, true);

                        circular_mask_shader.SetBuffer(0, noise_id, color_buffer1);

                        circular_mask_shader.Dispatch(0, texture_size / 8, texture_size / 8, 1);

                        color_buffer1.GetData(alpha);
                        color_buffer1.Release();

                        for(int x = 0; x < texture_size; x++)
                            for(int y = 0; y < texture_size; y++) {
                                float a                  = color.a * Tools.smootherstep(alpha[x + y * texture_size] * 0.3f, 0.0f, 1.0f - pixels[x + y * texture_size].a);

                                float blended_alpha      =            a +                            pixels[x + y * texture_size].a * (1.0f - a);
                                pixels[x + y * texture_size].r = (color.r * a + pixels[x + y * texture_size].r * pixels[x + y * texture_size].a * (1.0f - a)) / blended_alpha;
                                pixels[x + y * texture_size].g = (color.g * a + pixels[x + y * texture_size].g * pixels[x + y * texture_size].a * (1.0f - a)) / blended_alpha;
                                pixels[x + y * texture_size].b = (color.b * a + pixels[x + y * texture_size].b * pixels[x + y * texture_size].a * (1.0f - a)) / blended_alpha;
                                pixels[x + y * texture_size].a = blended_alpha;
                            }
                    }

                texture = new Texture2D(texture_size, texture_size);
                texture.filterMode = FilterMode.Trilinear;
                texture.SetPixels(pixels);
                texture.Apply();
            }

            private void Start() {
                if(autoinit) init();
            }

            public void init() {
                or = gameObject.AddComponent<OrbitRenderer>();
                sr = gameObject.AddComponent<SpriteRenderer>();

                Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

                if(planet != null) or.descriptor = planet.descriptor;

                generate();

                sr.sprite = Sprite.Create(texture,
                                          new Rect(0, 0, texture_size, texture_size),
                                          new Vector2(0.5f, 0.5f));

                initialized = true;
            }
             
            void LateUpdate() {
                if(!initialized) return;

                sr.transform.localScale = new Vector3(50.0f * radius / texture_size, 50.0f * radius / texture_size, 1.0f);

                if(planet != null) {
                    float[] pos = planet.descriptor.get_position();

                    sr.transform.position   = new Vector3(pos[0], pos[1], -0.1f);

                    or.color = orbitColor;

                    or.update_renderer(128);
                }
            }

            void OnDestroy() {
                if(sr != null) GameObject.Destroy(sr);
                if(or != null) GameObject.Destroy(or);
            }
        }
    }
}
