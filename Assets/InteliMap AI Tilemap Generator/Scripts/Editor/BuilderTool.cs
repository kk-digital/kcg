using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InteliMap
{
    [EditorTool("Schematic Generator Tool", typeof(InteliMapBuilder))]
    public class BuilderTool : TilemapTool
    {
        public override void OnEnable()
        {
            base.OnEnable();

            map = FindObjectOfType<Tilemap>();
        }

        public override Color handleColor
        {
            get { return Color.magenta; }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            base.OnToolGUI(window);
        }

        public override void OnFinish()
        {
            InteliMapBuilder builder = target as InteliMapBuilder;

            Vector3Int mins = new Vector3Int(Mathf.Min(start.x, stop.x), Mathf.Min(start.y, stop.y), 0);
            Vector3Int maxs = new Vector3Int(Mathf.Max(start.x, stop.x), Mathf.Max(start.y, stop.y), 0);

            if (mins == maxs)
            {
                return; // dont bother with a 1x1
            }

            Undo.RecordObject(builder, builder.name);

            if (builder.buildMaps == null)
            {
                builder.buildMaps = new List<GeneratorMap>();
            }

            builder.buildMaps.Add(new GeneratorMap(new List<Tilemap>(FindObjectsOfType<Tilemap>()), new BoundsInt(mins, maxs - mins + new Vector3Int(1, 1, 1))));
        }
    }
}