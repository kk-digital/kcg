using System;
using UnityEngine;

namespace Scripts {
    namespace SystemView {
        public class Background : MonoBehaviour {
            public int            seed;
            public int            stars;
            public int            width;
            public int            height;

            public Texture2D      texture;
            public SpriteRenderer renderer;

            private System.Random rng;

            private void generate() {
                rng = new(seed);

                Color[] pixels    = new Color[width * height];

                for(int i = 0; i < width * height; i++) pixels[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                for(int i = 0; i < stars; i++) {
                    int x = rng.Next(width);
                    int y = rng.Next(height);

                    pixels[x + y * width].r = 0.75f + (float)rng.NextDouble() * 0.25f;
                    pixels[x + y * width].g = 0.75f + (float)rng.NextDouble() * 0.25f;
                    pixels[x + y * width].b = 0.75f + (float)rng.NextDouble() * 0.25f;
                    pixels[x + y * width].a =         (float)rng.NextDouble();
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

                renderer.transform.Translate(new Vector3(0.0f, 0.0f, 10.0f));
            }
        }
    }
}
