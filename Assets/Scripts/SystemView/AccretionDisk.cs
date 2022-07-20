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
            public float          swirl;
            public float          spin;             // Angular velocity in degrees/second
            public float          brightness;
            public Vector3        center;           // Center point to spin around

            public Texture2D      texture;
            public SpriteRenderer renderer;

            private System.Random rng;

            private float         last_time;

            private void generate() {

                rng = new(seed);

                Color[] pixels    = new Color[width * height];

                for(int i = 0; i < width * height; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                float[] base_alpha = ProceduralImages.generate_noise(rng, 1.0f,             width / 64, height / 64);
                        base_alpha = ProceduralImages.smoothen_noise(base_alpha,            width / 64, height / 64, 64);
                        base_alpha = ProceduralImages.distort(rng,   base_alpha, 16.0f, 16, width,      height);
                        base_alpha = ProceduralImages.soften(        base_alpha, 4,         width,      height);
                        base_alpha = ProceduralImages.circular_mask( base_alpha,            width,      height);

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++)
                        pixels[x + y * width].a = base_alpha[x + y * width];

                float   halfw = width  / 2;
                float   halfh = height /  2;

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

                float rotation = swirl * Tools.deg;
                
                alpha = ProceduralImages.exponential_filter(alpha,            width, height);
                alpha = ProceduralImages.distort(rng,       alpha, 32.0f, 64, width, height);
                alpha = ProceduralImages.soften(            alpha,         8, width, height);
                alpha = ProceduralImages.swirl(             alpha, rotation,  width, height);

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        int i = x + y * width;

                        float a                 = alpha[i];

                        float d                 = 1.0f - Tools.magnitude(x / halfw - 1.0f, y / halfh - 1.0f);

                        float r, g, b;

                        if(d < 0.25f) {
                            float val           =  d          / 0.25f;

                            r                   = Tools.smoothstep(0.0f,               0.30f * brightness, val);
                            g                   = Tools.smoothstep(0.0f,               0.15f * brightness, val);
                            b                   = Tools.smoothstep(0.0f,               0.05f * brightness, val);
                            a                  *= Tools.smoothstep(0.0f,               0.70f,              val);
                        } else if(d < 0.5f) {
                            float val           = (d - 0.25f) / 0.25f;

                            r                   = Tools.smoothstep(0.30f * brightness, 0.80f * brightness, val);
                            g                   = Tools.smoothstep(0.15f * brightness, 0.40f * brightness, val);
                            b                   = Tools.smoothstep(0.05f * brightness, 0.20f * brightness, val);
                            a                  *= Tools.smoothstep(0.70f,              1.00f,              val);
                        } else if(d < 0.75f) {
                            float val           = (d - 0.50f) / 0.25f;

                            r                   = Tools.smoothstep(0.80f * brightness, 1.00f * brightness, val);
                            g                   = Tools.smoothstep(0.40f * brightness, 1.00f * brightness, val);
                            b                   = Tools.smoothstep(0.20f * brightness, 0.65f * brightness, val);
                            a                  *= Tools.smoothstep(1.00f,              1.80f,              val);
                        } else {
                            float val           = (d - 0.75f) / 0.25f;

                            r                   = Tools.smoothstep(1.00f * brightness, 1.00f * brightness, val);
                            g                   = Tools.smoothstep(1.00f * brightness, 1.00f * brightness, val);
                            b                   = Tools.smoothstep(0.65f * brightness, 0.90f * brightness, val);
                            a                  *= Tools.smoothstep(1.80f,              5.00f,              val);
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
                    }

                texture = new Texture2D(width, height);
                texture.filterMode = FilterMode.Trilinear;
                texture.SetPixels(pixels);
                texture.Apply();

            }

            private void Start() {
                generate();

                renderer        = gameObject.AddComponent<SpriteRenderer>();
                last_time       = Time.time;

                renderer.sprite = Sprite.Create(texture,
                                                new Rect(0, 0, width, height),
                                                new Vector2(0.5f, 0.5f));
            }

            private void Update() {
                renderer.transform.RotateAround(center, new Vector3(0.0f, 0.0f, 1.0f), -spin * (Time.time - last_time));

                last_time = Time.time;
            }
        }
    }
}
