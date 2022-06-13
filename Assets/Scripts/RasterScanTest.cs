using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath;

public class RasterScanTest : MonoBehaviour
{
    // Start Point
    Cell start = new Cell
    {
        x = -2,
        y = 3
    };

    // End Point
    Cell end = new Cell
    {
        x = 5,
        y = -3
    };

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Log raster start and end point
        Debug.Log($"Raster Line from ({start.x}, {start.y}) to ({end.x}, {end.y}) :");

        // Log places drawed line go through
        foreach (var cell in start.LineTo(end))
        {
            Debug.Log($"({cell.x},{cell.y})");
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Draw Debug Line
        Debug.DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(end.x, end.y, 0.0f), Color.red);
    }
#endif
}
