using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace InteliMap
{
    [CustomEditor(typeof(InteliMapGenerator))]
    public class InteliMapGeneratorEditor : Editor
    {
        private InteliMapGenerator mg;

        private void OnEnable()
        {
            mg = (InteliMapGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10.0f);

            if (GUILayout.Button("Clear Bounds"))
            {
                RecordMapUndo();

                mg.ClearBounds();
            }
            if (GUILayout.Button("Generate"))
            {
                RecordMapUndo();

                mg.StartGeneration();
            }
            if (GUILayout.Button("Clear and Generate"))
            {
                RecordMapUndo();

                mg.ClearBounds();
                mg.StartGeneration();
            }

            GUILayout.Space(10.0f);

            if (mg.mapToFill == null)
            {
                EditorGUILayout.HelpBox($"WARNING: Empty mapToFill. You must specific the map to fill for generation.", MessageType.Warning);
            }
            else if (mg.mapToFill.Count != mg.layerCount)
            {
                EditorGUILayout.HelpBox($"WARNING: Invalid mapToFill. This generator is built for {mg.layerCount} layers, but the mapToFill includes {mg.mapToFill.Count} layers.", MessageType.Warning);
            }

            GUILayout.Label($"Generator Info:");
            GUILayout.Label($"      {mg.layerCount} layers.");
            GUILayout.Label($"      {mg.NumUniqueTiles()} unique tiles.");
            GUILayout.Label($"      Neighborhood radius of {mg.GetNeighborhoodRadius()}.");
            GUILayout.Label($"      {mg.GetWeightCount()} total weights.");
            GUILayout.Label($"      {mg.weights.epochsTrained} total epochs trained.");
            if (mg.connectivity.con == Connectivity.EightWay)
            {
                GUILayout.Label($"      Connectivity: Eight way");
            }
            else
            {
                GUILayout.Label($"      Connectivity: Four way");
            }
            GUILayout.Label($"      Acknowledges bounds: {mg.weights.acknowledgeBounds}.");
            GUILayout.Label($"      Enforces border connectivity: {mg.connectivity.enforceBorderConnectivity}.");
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