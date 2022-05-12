using UnityEngine;

[System.Serializable]
public class Cube {

    /// <summary>
    /// x coordinate
    /// </summary>
    public int _x;

    /// <summary>
    /// y coordinate. y = -x - z
    /// </summary>
    public int _y;

    /// <summary>
    /// z coordinate
    /// </summary>
    public int _z;

    static Cube[] _directions =
    {
        new Cube(1, -1, 0), new Cube(1, 0, -1), new Cube(0, 1, -1),
        new Cube(-1, 1, 0), new Cube(-1, 0, 1), new Cube(0, -1, 1)
    };

    public Cube(int x, int y, int z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public Hex ToHex()
    {
        return new Hex(_x, _z);
    }

    public static Cube GetDirections(int direction)
    {
        return _directions[direction];
    }

    public Cube GetNeighbor(int direction)
    {
        return new Cube(_x + GetDirections(direction)._x, _y + GetDirections(direction)._y, _z + GetDirections(direction)._z);
    }

    public static float GetDistance(Cube a, Cube b)
    {
        return (Mathf.Abs(a._x - b._x) + Mathf.Abs(a._y - b._y) + Mathf.Abs(a._z - b._z)) / 2;
    }
}
