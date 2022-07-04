using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class CreateGameSystem : IInitializeSystem
{
    readonly Contexts _contexts;


    public CreateGameSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        var e = _contexts.game.CreateEntity();
        var bounds = GetBounds();

        e.isGame = true;
        e.AddBounds(bounds);
        e.AddLevel(5);
        e.AddLives(3);
        e.AddScore(0);
        e.isPlaying = true;
    }

    private Bounds GetBounds()
    {
        var size = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        return new Bounds(Vector3.zero, new Vector3(size.x * 2, size.y * 2));
    }
    // Start is called before the first frame update

}
