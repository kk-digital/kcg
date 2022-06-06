using UnityEngine;

namespace Planet
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

        public void DrawTiles(ref PlanetTileMap.PlanetTileMap tileMap)
        {
            foreach(var mr in parent.GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Object.Destroy(mr.gameObject);
                else
                    Object.DestroyImmediate(mr.gameObject);
            
            tileMap.DrawLayer(PlanetTileMap.Layer.Front, Object.Instantiate(material), parent, 10);
            tileMap.DrawLayer(PlanetTileMap.Layer.Ore, Object.Instantiate(material), parent, 11);
        }
    }
}

