using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath;
using Planet;

public class RasterScanTest : MonoBehaviour
{
    Planet.TileMap testTiles;
    public GameObject CollisionBlock;

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

    // Local Variables
    private int random;
    private int TempRandom;

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

            if(testTiles.GetTileRef(cell.x, cell.y, Enums.Tile.MapLayerType.Front).Type >= 0)
            {
                Instantiate(CollisionBlock);
                CollisionBlock.transform.position = new Vector2(cell.x, cell.y);

                RandomColor(CollisionBlock);
            }
        }
    }

    private void RandomColor(GameObject target)
    {
        random = Random.Range(0, 10);

        if(TempRandom == random) 
        { 
            RandomColor(CollisionBlock);
            return;
        }

        switch(random)
        {
            case 0:
                target.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 1:
                target.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 2:
                target.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 3:
                target.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case 4:
                target.GetComponent<SpriteRenderer>().color = new Color(75, 13, 46);
                break;
            case 5:
                target.GetComponent<SpriteRenderer>().color = Color.magenta;
                break;
            case 6:
                target.GetComponent<SpriteRenderer>().color = Color.grey;
                break;
            case 7:
                target.GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            case 8:
                target.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case 9:
                target.GetComponent<SpriteRenderer>().color = Color.gray;
                break;
            case 10:
                target.GetComponent<SpriteRenderer>().color = Color.black;
                break;
        }

        TempRandom = random;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Draw Debug Line
        Debug.DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(end.x, end.y, 0.0f), Color.red);
    }
#endif
}
