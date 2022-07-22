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

            public  Color            orbitColor = new Color(0.5f, 0.7f, 1.0f, 1.0f);

            public  CameraController Camera;
            private System.Random    rng;

            public  ComputeShader         blur_noise_shader;
            public  ComputeShader        scale_noise_shader;
            public  ComputeShader exponential_filter_shader;
            public  ComputeShader         distortion_shader;
            public  ComputeShader      circular_blur_shader;
            public  ComputeShader      circular_mask_shader;

            private bool initialized;

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

                Color[] pixels    = new Color[radius * radius];

                for(int i = 0; i < radius * radius; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                // Generate base noise
                ComputeBuffer base_buffer1 = new ComputeBuffer(radius * radius, sizeof(float));
                ComputeBuffer base_buffer2 = new ComputeBuffer(radius * radius / 4096, sizeof(float));

                base_buffer2.SetData(ProceduralImages.generate_noise(rng, 1.0f, radius / 64, radius / 64));
                    
                // Scale noise  
                scale_noise_shader.SetInt( width_id, radius);
                scale_noise_shader.SetInt(height_id, radius);
                scale_noise_shader.SetInt( scale_id, 64);

                scale_noise_shader.SetBuffer(0,  noise_id, base_buffer2);
                scale_noise_shader.SetBuffer(0, output_id, base_buffer1);

                scale_noise_shader.Dispatch(0, radius / 8, radius / 8, 1);

                // Apply circular blur
                circular_blur_shader.SetInt( width_id, radius);
                circular_blur_shader.SetInt(height_id, radius);

                circular_blur_shader.SetFloat(radius_id, 16.0f);

                circular_blur_shader.SetBuffer(0,  noise_id, base_buffer2);
                circular_blur_shader.SetBuffer(0, output_id, base_buffer1);

                circular_blur_shader.Dispatch(0, radius / 8, radius / 8, 1);

                base_buffer2.Release(); 

                // Apply circular mask
                circular_mask_shader.SetInt( width_id, radius);
                circular_mask_shader.SetInt(height_id, radius);

                circular_mask_shader.SetBuffer(0, noise_id, base_buffer1);

                circular_mask_shader.Dispatch(0, radius / 8, radius / 8, 1);

                float[] base_alpha = new float[radius * radius];
                base_buffer1.GetData(base_alpha);
                base_buffer1.Release();

                for(int x = 0; x < radius; x++)
                    for(int y = 0; y < radius; y++) {
                        pixels[x + y * radius].r = basecolor.r;
                        pixels[x + y * radius].g = basecolor.g;
                        pixels[x + y * radius].b = basecolor.b;
                        pixels[x + y * radius].a = base_alpha[x + y * radius];
                    }

                if(colors != null)
                    foreach(Color color in colors) {

                    }

                texture = new Texture2D(radius, radius);
                texture.filterMode = FilterMode.Trilinear;
                texture.SetPixels(pixels);
                texture.Apply();
            }

            public void init() {
                or = gameObject.AddComponent<OrbitRenderer>();
                sr = gameObject.AddComponent<SpriteRenderer>();

                Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

                or.descriptor = planet.descriptor;

                generate();

                sr.sprite = Sprite.Create(texture,
                                          new Rect(0, 0, radius, radius),
                                          new Vector2(0.5f, 0.5f));

                initialized = true;
            }
             
            void LateUpdate() {
                if(!initialized) return;

                float[] pos = planet.descriptor.get_position();

                sr.transform.position   = new Vector3(pos[0], pos[1], -0.1f);
                sr.transform.localScale = new Vector3(3.0f / Camera.scale, 3.0f / Camera.scale, 1.0f);

                or.color = orbitColor;

                or.update_renderer(128);
            }

            void OnDestroy() {
                if(sr != null) GameObject.Destroy(sr);
                if(or != null) GameObject.Destroy(or);
            }
        }
    }
}
