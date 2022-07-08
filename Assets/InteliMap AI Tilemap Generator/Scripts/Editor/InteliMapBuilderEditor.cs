using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace InteliMap
{
    [CustomEditor(typeof(InteliMapBuilder))]
    public class InteliMapBuilderEditor : Editor
    {
        private InteliMapBuilder sb;

        private void OnEnable()
        {
            sb = (InteliMapBuilder)target;
        }

        public override void OnInspectorGUI()
        {
            if (sb.buildResult == GeneratorBuildResult.InProgress)
            {
                if (GUILayout.Button("Cancel Build"))
                {
                    sb.CancelBuild();
                }
                if (GUILayout.Button("Save and Quit Build"))
                {
                    sb.SaveAndQuitBuild();
                }
            }
            else
            {
                base.OnInspectorGUI();

                GUILayout.Space(20.0f);

                if (GUILayout.Button("Build Generator"))
                {
                    sb.Build();
                }
            }

            GUILayout.Space(20.0f);

            switch (sb.buildResult)
            {
                case GeneratorBuildResult.None:
                    EditorGUILayout.HelpBox("Generator not yet built. Input some build maps then click 'Build Generator' to build the generator.", MessageType.None);
                    break;
                case GeneratorBuildResult.InProgress:
                    EditorGUILayout.HelpBox(
                        "Build Stats:" +
                        "\nBuild in progress. " + sb.epoch + " / " + sb.epochs + " epochs." +
                        "\nTotal Epochs: " + sb.totalEpochs + " epochs." +
                        "\nLast 20 Epoch AVG Loss: " + sb.avgLossLast20Epochs +
                        "\nLast Epoch Loss: " + sb.lossLastEpoch +
                        "\nTime Started: " + sb.startTime.ToString("dddd MMMM dd, HH:mm:ss") +
                        "\nTime Elapsed: " + DateTime.Now.Subtract(sb.startTime).ToString("hh\\:mm\\:ss") +
                        "\nCurrent Learning Rate: " + sb.currentLearningRate +
                        "\n\nSettings:" +
                        "\nConnectivity: " + (sb.advanced.connectivity == Connectivity.EightWay ? "Eight way" : "Four way") +
                        "\nNeighborhood Radius: " + sb.neighborhoodRadius + " [" + (sb.neighborhoodRadius * 2 + 1) + "x" + (sb.neighborhoodRadius * 2 + 1) + "]" +
                        "\nAcknowledge Bounds: " + sb.advanced.acknowledgeBounds +
                        "\nEnforce Border Connectivity: " + sb.advanced.enforceBorderConnectivity, MessageType.None);
                    break;
                case GeneratorBuildResult.Cancelled:
                    EditorGUILayout.HelpBox("WARNING. Build cancelled.", MessageType.Warning);
                    break;
                case GeneratorBuildResult.Success:
                    EditorGUILayout.HelpBox("Successfully built schematic.\n" +
                        "Time taken: " + sb.endTime.Subtract(sb.startTime).ToString("hh\\:mm\\:ss") +
                        "\nEpochs trained: " + sb.epoch + " epochs.", MessageType.Info);
                    break;
                case GeneratorBuildResult.NanError:
                    EditorGUILayout.HelpBox("ERROR. BUILD TERMINATED. One of the internal learning mechanisms encountered a NaN value. This is likely a result of the learning rate being too high, lower it and retry.", MessageType.Error);
                    break;
                case GeneratorBuildResult.MismatchedLayers:
                    EditorGUILayout.HelpBox("ERROR. Not all build maps have the same amount of layers. All build maps must have the same amount of layers.", MessageType.Error);
                    break;
                case GeneratorBuildResult.NullMaps:
                    EditorGUILayout.HelpBox("ERROR. Some of the maps in the build maps are null, these must not be null.", MessageType.Error);
                    break;
                case GeneratorBuildResult.ZeroMaps:
                    EditorGUILayout.HelpBox("ERROR. Zero maps inputted, there must be at least one map.", MessageType.Error);
                    break;
                case GeneratorBuildResult.InvalidCommonality:
                    EditorGUILayout.HelpBox("ERROR. One or more of the build maps has an invalid commonality. No commonalities can be negative and at least one must be positive.", MessageType.Error);
                    break;
            }
        }
    }
}