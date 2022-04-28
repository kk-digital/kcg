using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

            //build atlas for sprites
            Assets.SpriteAtlas0 = AtlasBuilder.Build(Assets.Sprites.Values.ToList(), true, true);
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