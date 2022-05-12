using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMapSystem : IInitializeSystem, ISetPool {
    Pool _pool;

    int _sizeX, _sizeY;
    int _range;

    readonly GameObject _tileViewContainer = new GameObject("Tiles");

    public void SetPool(Pool pool)
    {
        _pool = pool;
    }

    public void Initialize()
    {
        _sizeX = Map01.sizeX;
        _sizeY = Map01.sizeY;

        _range = Map01.range;

        foreach (int[] position in GeneratePositions(_range))
        {
            _pool.CreateEntity()
                .IsTile(true)
                .AddTilePosition(new Hex(position[0], position[1]))
                .AddTileType(GenerateTileType());
        }

        GenerateTileView(_pool.GetEntities(Matcher.AllOf(Matcher.Tile, Matcher.TilePosition, Matcher.TileType)));
    }

    /// <summary>
    /// Generate an hexagonal map.
    /// </summary>
    /// <param name="range">Max range</param>
    /// <returns></returns>
    List<int[]> GeneratePositions(int range)
    {
        List<int[]> positions = new List<int[]>();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                if((-x -y) >= - range && (-x- y) <= range)
                    positions.Add(new int[] { x, y });
                
            }
        }

        return positions;
    }

    /// <summary>
    /// Generate a map including all points from [-sixeX;-sizeY] to [sizeX;sizeY]
    /// </summary>
    /// <param name="sizeX">Width of the map</param>
    /// <param name="sizeY">Length of the map</param>
    /// <returns></returns>
    List<int[]> GeneratePositions(int sizeX, int sizeY)
    {
        List<int[]> positions = new List<int[]>();

        for(int x = (sizeX/2); x >= -(sizeX/2); x--)
        {
            for (int y = (sizeY / 2); y >= -(sizeY / 2); y--)
            {
                positions.Add(new int[] { x , y });
            }
        }

        return positions;
    }

    TileType GenerateTileType()
    {
        int[] values = (int[])System.Enum.GetValues(typeof(TileType));

        int randomIndex = Random.Range(0, values.Length);

        return TileType.Sand;//(TileType)values.GetValue(values[randomIndex]);
    }

    void GenerateTileView(Entity[] entities)
    {
        float _size = 0.5f; 

        float _height = 2*_size;
        float _verticalSpacing = _height * 0.75f;
        float _width = Mathf.Sqrt(3)/2 * _height;
        float _horizontalSpacing = _width;

        Sprite[] tileSprites = Resources.LoadAll<Sprite>("Sprites/hexagon_tiles");

        foreach (Entity e in entities)
        {
            // Instantiate the game object, name it and put it in a container
            GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Tile"), new Vector3(e.tilePosition.position._q*_horizontalSpacing, -e.tilePosition.position._r*_verticalSpacing), Quaternion.identity) as GameObject;

            go.name = "Tile [" + e.tilePosition.position._q + ";" + e.tilePosition.position._r + "]";
            go.transform.SetParent(_tileViewContainer.transform);

            // DEBUG
            // Show tile position on screen
            go.transform.GetComponentInChildren<TextMesh>().text = "[" + e.tilePosition.position._q + ";" + e.tilePosition.position._r + "]";

            // Set the game object position with hex related offsets
            // odd y coordinates (impair)
            go.transform.position += (Vector3.right * e.tilePosition.position._r * _horizontalSpacing / 2);
            
            // Assign a view 
            string[] sprites;
            TileTypeResourceCombination.combinations.TryGetValue(e.tileType.type, out sprites);

            string randomSpriteName = sprites[Random.Range(0, sprites.Length)];

            foreach(Sprite sprite in tileSprites)
            {
                if(sprite.name == randomSpriteName)
                {
                    go.GetComponent<SpriteRenderer>().sprite = sprite;
                }
            }

            e.AddTileView(go);
        }
    }
}
