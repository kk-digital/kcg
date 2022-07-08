using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace InteliMap
{
    public static class TileSelectionHandles
    {
        public static void DrawBounds(Tilemap map, Vector3Int a, Vector3Int b)
        {
            TileSelectionDrawer.DrawBounds(map, a, b, DrawLine);
        }

        public static void DrawBounds(Tilemap map, BoundsInt bounds)
        {
            TileSelectionDrawer.DrawBounds(map, bounds, DrawLine);
        }

        private static void DrawLine(Vector3 a, Vector3 b) 
        {
            Handles.DrawLine(a, b);
        }
    }
}