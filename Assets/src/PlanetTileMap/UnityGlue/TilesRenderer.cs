using System.Collections;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Enums;
using TileProperties;
using TmxMapFileLoader;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PlanetTileMap.Unity
{
    //TODO: Move to script folder
    //Note: MonoBehaviors/Scripts should go into Asset/Scripts folder
    //Note: TileMap should be mostly controlled by GameManager
    class TilesRenderer : MonoBehaviour
    {
        public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;
        MeshBuilder mb;
        Mesh mesh;
        PlanetMapInfo mapInfo;
        List<MeshBuilder> meshBuildersByLayers = new List<MeshBuilder>();
        public TilePropertiesManager TilePropertiesManager;

        public void Awake()
        {
            TilePropertiesManager = new TilePropertiesManager();
            LoadMap();
        }

        public void LoadMap()
        {
            //load map
            mapInfo = TmxImporter.LoadMap(Path.Combine(BaseDir, TileMap));

            //build sprite quads for each layer
            meshBuildersByLayers.Clear();
            var planetLayers = new [] { PlanetTileLayer.TileLayerBack, PlanetTileLayer.TileLayerMiddle, PlanetTileLayer.TileLayerFurniture, PlanetTileLayer.TileLayerFront };

            //remove all children MeshRenderer
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);
            
            var sortingOrder = 0;
            foreach (var layer in planetLayers)
            {
                //create material for atlas
                var atlas = mapInfo.GetAtlas(layer);
                if (atlas.Length == 0)
                    continue;
                var tex = TextureBuilder.Build(atlas);
                var mat = Instantiate(Material);
                mat.SetTexture("_MainTex", tex);
                var mesh = CreateMesh(transform, layer.ToString(), sortingOrder++, mat);
                var quads = new QuadsBuilder().BuildQuads(mapInfo, layer, 0f);
                mb = new MeshBuilder(quads, mesh);
                meshBuildersByLayers.Add(mb);
            }

            LateUpdate();
        }

        private Mesh CreateMesh(Transform parent, string name, int sortingOrder, Material material)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent);

            var mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            var mf = go.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.sortingOrder = sortingOrder;

            return mesh;
        }

        private void LateUpdate()
        {
            var visibleRect = CalcVisibleRect();

            //rebuild all layers for visible rect
            foreach (var mb in meshBuildersByLayers)
                mb.BuildMesh(visibleRect);
        }

        private static Rect CalcVisibleRect()
        {
            var cam = Camera.main;
            var pos = cam.transform.position;
            var height = 2f * cam.orthographicSize;
            var width = height * cam.aspect;
            var visibleRect = new Rect(pos.x - width / 2, pos.y - height / 2, width, height);
            return visibleRect;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TilesRenderer))]
    public class TerrainGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (TilesRenderer)target;

            // Show default inspector property editor
            DrawDefaultInspector();

            if (GUILayout.Button("Load Map"))
                myTarget.LoadMap();
        }
    }
#endif
}