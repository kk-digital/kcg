using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class HandleInputSystem : IReactiveSystem, ISetPool
{
    Pool _pool;

    public void SetPool(Pool pool)
    {
        _pool = pool;
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.Input.OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            switch(e.input.intent)
            {
                case InputIntent.SelectTile:
                    if(_pool.tileSelectedEntity != null)
                    {
                        _pool.tileSelectedEntity.IsTileSelected(false);
                    }

                    foreach (var tile in _pool.GetGroup(Matcher.AllOf(Matcher.Tile, Matcher.TilePosition, Matcher.TileView)).GetEntities()) {
                        if(((GameObject)e.input.data[0] == tile.tileView.model))
                        {
                            tile.IsTileSelected(true);
                        }
                    }

                    break;

                case InputIntent.UnselectTile:
                    _pool.tileSelectedEntity.IsTileSelected(false);

                    break;
            }
        }
    }
}
