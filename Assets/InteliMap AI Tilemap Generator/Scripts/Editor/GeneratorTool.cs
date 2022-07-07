using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InteliMap
{
    [EditorTool("Tilemap Generator Tool", typeof(InteliMapGenerator))]
    [Tooltip("Generator Tool")]
    public class SchematicTool : TilemapTool
    {
        private bool areaSelected = false;
        private Vector3Int areaStart = Vector3Int.zero;
        private Vector3Int areaEnd = Vector3Int.zero;
        private BoundsInt areaBounds;
        private TileBase[][] original;

        private bool clear = false;
        private InteliMapGenerator mg;

        public override void OnEnable()
        {
            base.OnEnable();

            mg = target as InteliMapGenerator;
            map = mg.mapToFill[0];
            areaSelected = false;
        }

        public override Color handleColor
        {
            get { return areaSelected ? new Color(1.0f, 0.0f, 1.0f, 1.0f) : Color.cyan; }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            base.OnToolGUI(window);

            if (Event.current.type == EventType.Repaint)
            {
                ignoreClick = false;
            }

            Handles.BeginGUI();

            GUILayout.BeginHorizontal();
            GUILayout.Space(60);
            clear = GUILayout.Toggle(clear, "Clear on Generate");

            if (areaSelected)
            {
                if (GUILayout.Button("Set as Generation Bounds"))
                {
                    Undo.RecordObject(mg, mg.name);

                    mg.boundsToFill = areaBounds;
                }
                if (GUILayout.Button("Revert to Original"))
                {
                    RecordMapUndo();

                    for (int layer = 0; layer < mg.layerCount; layer++)
                    {
                        mg.mapToFill[layer].SetTilesBlock(areaBounds, original[layer]);
                    }
                }
                if (GUILayout.Button("Clear Area"))
                {
                    RecordMapUndo();

                    BoundsInt previousBounds = mg.boundsToFill;

                    mg.boundsToFill = areaBounds;
                    mg.ClearBounds();

                    mg.boundsToFill = previousBounds;
                }
                if (GUILayout.Button("Retry Generation"))
                {
                    RecordMapUndo();

                    BoundsInt previousBounds = mg.boundsToFill;

                    mg.boundsToFill = areaBounds;

                    if (clear)
                    {
                        mg.ClearBounds();
                    }
                    else
                    {
                        for (int layer = 0; layer < mg.layerCount; layer++)
                        {
                            mg.mapToFill[layer].SetTilesBlock(areaBounds, original[layer]);
                        }
                    }

                    mg.StartGeneration();

                    mg.boundsToFill = previousBounds;
                }
            }

            GUILayout.EndHorizontal();
            if (Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                ignoreClick = true;
            }

            Handles.EndGUI();
        }

        public override void OnFinish()
        {
            BoundsInt previousBounds = mg.boundsToFill;

            Vector3Int mins = new Vector3Int(Mathf.Min(start.x, stop.x), Mathf.Min(start.y, stop.y), 0);
            Vector3Int maxs = new Vector3Int(Mathf.Max(start.x, stop.x), Mathf.Max(start.y, stop.y), 0);

            areaStart = start;
            areaEnd = stop;

            areaBounds = new BoundsInt(mins, maxs - mins + new Vector3Int(1, 1, 1));
            mg.boundsToFill = areaBounds;
            areaSelected = true;

            for (int layer = 0; layer < mg.layerCount; layer++)
            {
                Undo.RecordObject(mg.mapToFill[layer], mg.mapToFill[layer].name);
            }

            original = new TileBase[mg.layerCount][];
            for (int layer = 0; layer < mg.layerCount; layer++)
            {
                original[layer] = mg.mapToFill[layer].GetTilesBlock(areaBounds);
            }

            if (clear)
            {
                mg.ClearBounds();
            }
            mg.StartGeneration();

            mg.boundsToFill = previousBounds;

            forceDraw = true;
        }

        private void RecordMapUndo()
        {
            for (int layer = 0; layer < mg.layerCount; layer++)
            {
                Undo.RecordObject(mg.mapToFill[layer], mg.mapToFill[layer].name);
            }
        }
    }
}