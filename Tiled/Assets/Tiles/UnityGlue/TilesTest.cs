using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiles.Unity
{
    public class TilesTest : MonoBehaviour
    {
        public static string BaseDir => Application.streamingAssetsPath;
        public string TileMap = "Moonbunker/Moon Bunker.tmx";
        public Material Material;

        public void Build()
        {
            //load map
            TmxImporter.LoadMap(Path.Combine(BaseDir,TileMap));

            //create material for atlas
            var tex = TextureBuilder.Build(Assets.AtlasTexture);
            Material.SetTexture("_MainTex", tex);

            var mb = new MeshBuilder();
            for (int i = 0 ; i < Assets.Map.Layers.Length; i++)
                mb.Build(i, -i / 100f);

            var mesh = new Mesh();
            if (mb.verticies.Count > 65000)
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.SetVertices(mb.verticies);
            mesh.SetUVs(0, mb.uvs);
            mesh.SetTriangles(mb.triangles, 0);

            var mf = GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = GetComponent<MeshRenderer>();
            mr.sharedMaterial = Material;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TilesTest))]
    public class TerrainGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (TilesTest)target;

            // Show default inspector property editor
            DrawDefaultInspector();

            if (GUILayout.Button("Build"))
                myTarget.Build();
        }
    }
#endif
}