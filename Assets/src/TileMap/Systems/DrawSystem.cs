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

        private DrawSystem()
        {
            
        }

        public void Initialize(Material material, Transform transform)
        {
            this.material = material;
            parent = transform;
        }

        public void DrawTiles(ref GameEntity tileMap)
        {
            foreach (var mr in parent.GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Object.Destroy(mr.gameObject);
                else
                    Object.DestroyImmediate(mr.gameObject);

            DrawLayer(ref tileMap, PlanetLayer.Front, 10);
            DrawLayer(ref tileMap, PlanetLayer.Ore, 11);
        }

        public void DrawLayer(ref GameEntity entity, PlanetLayer layer, int drawOrder)
        {
            var sprite = new Render.Sprite
            {
                Texture = entity.tileMapData.LayerTextures[(int) layer],
                TextureCoords = new Vector4(0, 0, 1, -1)
            };

            Utility.RenderUtils.DrawSprite(0, 0, entity.tileMapData.Chunks.Size.x, entity.tileMapData.Chunks.Size.y, sprite, Object.Instantiate(material), parent, drawOrder);
        }
    }
}

