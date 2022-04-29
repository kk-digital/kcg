using System.Collections;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiles.Unity
{
    class TilesTest : MonoBehaviour
    {
        public static string BaseDir => Application.streamingAssetsPath;
        public string TileMap = "Moonbunker/Moon Bunker.tmx";
        public Material Material;
        public MeshBuilder mb;
        Mesh mesh;

        public Mode RenderMode = Mode.Mesh;

        public enum Mode
        {
            Mesh, GL
        }

        public void Start()
        {
            Build();
        }

        public void Build()
        {
            //load map
            TmxImporter.LoadMap(Path.Combine(BaseDir,TileMap));

            //create material for atlas
            var tex = TextureBuilder.Build(Assets.AtlasTexture);
            Material.SetTexture("_MainTex", tex);

            mb = new MeshBuilder();
            mb.BuildQauds();
                
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            var mf = GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = GetComponent<MeshRenderer>();
            mr.sharedMaterial = Material;

            LateUpdate();
        }

        private void LateUpdate()
        {
            if (mb == null)
                return;

            if (RenderMode != Mode.Mesh)
            { 
                mesh.Clear();
                return;
            }

            var visibleRect = CalcVisibleRect();

            //var sw = System.Diagnostics.Stopwatch.StartNew();
            mb.BuildMesh(visibleRect);
            mesh.Clear(true);
            mesh.SetVertices(mb.verticies);
            mesh.SetUVs(0, mb.uvs);
            mesh.SetTriangles(mb.triangles, 0);
            //Debug.Log(sw.Elapsed);
        }

        private void OnRenderObject()
        {
            if (mb == null)
                return;

            if (RenderMode != Mode.GL)
                return;

            var visibleRect = CalcVisibleRect();

            //var sw = System.Diagnostics.Stopwatch.StartNew();
            GLRenderer.Render(visibleRect, mb.quads, Material);
            //Debug.Log(sw.Elapsed);
        }

        private static Rect CalcVisibleRect()
        {
            var cam = Camera.main;
            var pos = cam.transform.position;
            float height = 2f * cam.orthographicSize;
            float width = height * cam.aspect;
            var visibleRect = new Rect(pos.x - width / 2, pos.y - height / 2, width, height);
            //Debug.Log(visibleRect);
            return visibleRect;
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