using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InteliMap
{
    public class TilemapTool : EditorTool, IDrawSelectedHandles
    {
        public bool isDragging = false;
        public bool forceDraw = false;
        public bool ignoreClick = false;
        public Vector3Int start;
        public Vector3Int stop;

        protected Tilemap map;

        private GUIContent icon;
        public override GUIContent toolbarIcon
        {
            get { return icon; }
        }

        public virtual void OnEnable()
        {
            icon = new GUIContent(EditorGUIUtility.GetIconForObject(target));
        }

        public override void OnWillBeDeactivated()
        {
            forceDraw = false;
        }

        public override void OnToolGUI(EditorWindow window)
        {
            if (!(window is SceneView))
                return;

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Keyboard));

            Event e = Event.current;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && !ignoreClick) // left click
                    {
                        Vector3Int? cell = GetMouseCell(e);

                        if (cell.HasValue)
                        {
                            isDragging = true;
                            start = cell.Value;
                            stop = start;
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (isDragging)
                    {
                        Vector3Int? cell = GetMouseCell(e);

                        if (cell.HasValue)
                        {
                            stop = cell.Value;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (isDragging && e.button == 0)
                    {
                        isDragging = false;

                        OnFinish();
                    }
                    break;
            }
        }

        public virtual Color handleColor
        {
            get { return Color.white; }
        }

        public void OnDrawHandles()
        {
            if (isDragging || forceDraw)
            {
                Handles.color = handleColor;

                TileSelectionHandles.DrawBounds(map, start, stop);

                HandleUtility.Repaint();
            }
        }

        private Vector3Int? GetMouseCell(Event e)
        {
            Matrix4x4 worldMatrix = map.transform.localToWorldMatrix;

            Plane tilemapPlane = new Plane(
                worldMatrix.MultiplyPoint3x4(new Vector3(0, 0, 0)),
                worldMatrix.MultiplyPoint3x4(new Vector3(1, 1, 0)),
                worldMatrix.MultiplyPoint3x4(new Vector3(1, 0, 0)));

            float enter;
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (tilemapPlane.Raycast(mouseRay, out enter))
            {
                return map.layoutGrid.WorldToCell(mouseRay.GetPoint(enter));
            }

            return null;
        }

        public virtual void OnFinish()
        {

        }
    }
}