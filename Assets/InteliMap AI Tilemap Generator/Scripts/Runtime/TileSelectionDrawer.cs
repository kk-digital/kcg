using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileSelectionDrawer
{
    public static void DrawBounds(Tilemap map, Vector3Int a, Vector3Int b, Action<Vector3, Vector3> drawLine)
    {
        Vector3 sizeX;
        Vector3 sizeY;
        if (map.cellLayout == GridLayout.CellLayout.Isometric || map.cellLayout == GridLayout.CellLayout.IsometricZAsY)
        {
            sizeX = map.transform.localToWorldMatrix.MultiplyVector(new Vector3(map.cellSize.x * 0.5f, map.cellSize.y * 0.5f));
            sizeY = map.transform.localToWorldMatrix.MultiplyVector(new Vector3(map.cellSize.x * -0.5f, map.cellSize.y * 0.5f));
        }
        else
        {
            sizeX = map.transform.localToWorldMatrix.MultiplyVector(new Vector3(map.cellSize.x, 0));
            sizeY = map.transform.localToWorldMatrix.MultiplyVector(new Vector3(0, map.cellSize.y));
        }

        DrawRect(map,
            a,
            new Vector3Int(a.x, b.y),
            b,
            new Vector3Int(b.x, a.y),
            sizeX,
            sizeY,
            drawLine);
    }

    public static void DrawBounds(Tilemap map, BoundsInt bounds, Action<Vector3, Vector3> drawLine)
    {
        Vector3 sizeX;
        Vector3 sizeY;
        if (map.cellLayout == GridLayout.CellLayout.Isometric || map.cellLayout == GridLayout.CellLayout.IsometricZAsY)
        {
            sizeX = map.transform.localToWorldMatrix.MultiplyVector(new Vector3(map.cellSize.x * 0.5f, map.cellSize.y * 0.5f));
            sizeY = map.transform.localToWorldMatrix.MultiplyVector(new Vector3(map.cellSize.x * -0.5f, map.cellSize.y * 0.5f));
        }
        else
        {
            sizeX = map.transform.localToWorldMatrix.MultiplyVector(new Vector3(map.cellSize.x, 0));
            sizeY = map.transform.localToWorldMatrix.MultiplyVector(new Vector3(0, map.cellSize.y));
        }

        DrawRect(map,
            bounds.position,
            bounds.position + new Vector3Int(0, bounds.size.y - 1),
            bounds.position + bounds.size - Vector3Int.one,
            bounds.position + new Vector3Int(bounds.size.x - 1, 0),
            sizeX,
            sizeY,
            drawLine);
    }

    private static void DrawRect(Tilemap map, Vector3Int a, Vector3Int b, Vector3Int c, Vector3Int d, Vector3 sizeX, Vector3 sizeY, Action<Vector3, Vector3> drawLine)
    {
        Vector3 aPos = map.layoutGrid.CellToWorld(a);
        Vector3 bPos = map.layoutGrid.CellToWorld(b);
        Vector3 cPos = map.layoutGrid.CellToWorld(c);
        Vector3 dPos = map.layoutGrid.CellToWorld(d);

        Vector3 size = sizeX + sizeY;

        if (a.x <= c.x)
        {
            if (a.y <= c.y)
            {
                drawLine(aPos, bPos + sizeY);
                drawLine(dPos + sizeX, cPos + size);
            }
            else
            {
                drawLine(bPos, aPos + sizeY);
                drawLine(cPos + sizeX, dPos + size);
            }
        }
        else
        {
            if (a.y <= c.y)
            {
                drawLine(aPos + sizeX, bPos + size);
                drawLine(dPos, cPos + sizeY);
            }
            else
            {
                drawLine(bPos + sizeX, aPos + size);
                drawLine(cPos, dPos + sizeY);
            }
        }

        if (a.y <= c.y)
        {
            if (a.x <= c.x)
            {
                drawLine(bPos + sizeY, cPos + size);
                drawLine(aPos, dPos + sizeX);
            }
            else
            {
                drawLine(cPos + sizeY, bPos + size);
                drawLine(dPos, aPos + sizeX);
            }
        }
        else
        {
            if (a.x <= c.x)
            {
                drawLine(bPos, cPos + sizeX);
                drawLine(aPos + sizeY, dPos + size);
            }
            else
            {
                drawLine(cPos, bPos + sizeX);
                drawLine(dPos + sizeY, aPos + size);
            }
        }
    }
}
