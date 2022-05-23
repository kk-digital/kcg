using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Tiles.Junk;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiles.Unity.Junk
{ 
    public class TilesTest : MonoBehaviour
    {
        public static string BaseDir => Application.streamingAssetsPath;
        public string TileMap;

        public void Build()
        {
            TileImporter.LoadTileTypes();
            SpriteImporter.LoadSprites();

            TmxImporter.LoadMap(Path.Combine(BaseDir, TileMap));

            //build atlas for sprites
            var sprites = Tiles.Junk.Assets.Sprites.Values.ToList();
            Tiles.Junk.Assets.SpriteAtlas0 = AtlasBuilder.Build(sprites, true);
            //replace sprite in Assets
            foreach (var sprite in sprites)
                Tiles.Junk.Assets.Sprites[sprite.Name] = sprite;
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