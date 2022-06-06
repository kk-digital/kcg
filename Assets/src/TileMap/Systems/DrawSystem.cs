using Enums;
using UnityEngine;

namespace TileMap
{
    public class DrawSystem
    {
        public static readonly DrawSystem Instance;

        private Material material;
        private Transform parent;

        static DrawSystem()
        {
            Instance = new DrawSystem();
        }

        public void Initialize(Material material, Transform transform)
        {
            this.material = material;
            parent = transform;
        }

        public void DrawTiles(ref Component tileComponent)
        {
            foreach (var mr in parent.GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Object.Destroy(mr.gameObject);
                else
                    Object.DestroyImmediate(mr.gameObject);

            DrawLayer(ref tileComponent, PlanetLayer.Front, 10);
            DrawLayer(ref tileComponent, PlanetLayer.Ore, 11);
        }

        public void DrawLayer(ref Component tileComponent, PlanetLayer layer, int drawOrder)
        {
            var sprite = new Render.Sprite
            {
                Texture = tileComponent.LayerTextures[(int) layer],
                TextureCoords = new Vector4(0, 0, 1, -1)
            };

            Utility.RenderUtils.DrawSprite(0, 0, tileComponent.Chunks.Size.x, tileComponent.Chunks.Size.y, sprite, Object.Instantiate(material), parent, drawOrder);
        }
    }
}

