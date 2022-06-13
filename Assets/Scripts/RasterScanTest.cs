using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath;
using Planet;

public class RasterScanTest : MonoBehaviour
{
    Planet.TileMap testTiles;

    // Start Point
    Cell start = new Cell
    {
        x = -1,
        y = 13
    };

    // End Point
    Cell end = new Cell
    {
        x = 8,
        y = 6
    };

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Assign Test Tiles
        testTiles = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;
        // Log raster start and end point
        Debug.Log($"Raster Line from ({start.x}, {start.y}) to ({end.x}, {end.y}) :");

        // Log places drawed line go through
        foreach (var cell in start.LineTo(end))
        {
            Debug.Log($"({cell.x},{cell.y})");
            testTiles.RemoveTile(cell.x, cell.y, Enums.Tile.MapLayerType.Front);
            testTiles.Layers.BuildLayerTexture(testTiles, Enums.Tile.MapLayerType.Front);
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
