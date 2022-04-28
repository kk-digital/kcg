using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiles.Unity
{ 
    public class TilesTest : MonoBehaviour
    {
        public string TileMap;

        public void Build()
        {
            TileImporter.LoadTileTypes();
            SpriteImporter.LoadSprites();
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