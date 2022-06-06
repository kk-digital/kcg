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
            foreach(var mr in parent.GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Object.Destroy(mr.gameObject);
                else
                    Object.DestroyImmediate(mr.gameObject);
            
            tileComponent.DrawLayer(PlanetLayer.Front, Object.Instantiate(material), parent, 10);
            tileComponent.DrawLayer(PlanetLayer.Ore, Object.Instantiate(material), parent, 11);
        }
    }
}

