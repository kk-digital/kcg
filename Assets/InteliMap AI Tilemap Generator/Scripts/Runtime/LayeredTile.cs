using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct LayeredTile : IEqualityComparer<LayeredTile>
{
    // Creates an empty LayeredTile with the appropriate layer count
    public LayeredTile(int layerCount)
    {
        this.tiles = new TileBase[layerCount];
    }

    // Creates a LayeredTile with the corresponding tile array
    public LayeredTile(TileBase[] tiles)
    {
        this.tiles = tiles;
    }

    [SerializeField] public TileBase[] tiles;

    public bool IsEmpty()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    public bool Equals(LayeredTile x, LayeredTile y)
    {
        if (x.tiles.Length != y.tiles.Length)
        {
            return false;
        }

        for (int i = 0; i < x.tiles.Length; i++)
        {
            if (x.tiles[i] != y.tiles[i])
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(LayeredTile obj)
    {
        int hash = 0;
        for (int i = 0; i < obj.tiles.Length; i++)
        {
            hash ^= + (obj.tiles[i] != null ? obj.tiles[i].GetHashCode() : 0);
        }
        return hash;
    }
}


public class LayeredTileComparer : IEqualityComparer<LayeredTile>
{
    public bool Equals(LayeredTile x, LayeredTile y)
    {
        if (x.tiles.Length != y.tiles.Length)
        {
            return false;
        }

        for (int i = 0; i < x.tiles.Length; i++)
        {
            if (x.tiles[i] != y.tiles[i])
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(LayeredTile obj)
    {
        int hash = 0;
        for (int i = 0; i < obj.tiles.Length; i++)
        {
            hash ^= +(obj.tiles[i] != null ? obj.tiles[i].GetHashCode() : 0);
        }
        return hash;
    }
}