using System.Collections;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiles.Unity
{
    class TilesRenderer : MonoBehaviour
    {
        public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;
        MeshBuilder mb;
        Mesh mesh;

        public Mode RenderMode = Mode.Mesh;

        public enum Mode
        {
            Mesh, GL
        }

        public void Start()
        {
            LoadMap();
        }

        public void LoadMap()
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

            mb.BuildMesh(visibleRect);
            mesh.Clear(true);
            mesh.SetVertices(mb.verticies);
            mesh.SetUVs(0, mb.uvs);
            mesh.SetTriangles(mb.triangles, 0);
        }

        private void OnRenderObject()
        {
            if (mb == null)
                return;

            if (RenderMode != Mode.GL)
                return;

            var visibleRect = CalcVisibleRect();

            GLRenderer.Render(visibleRect, mb.quads, Material);
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