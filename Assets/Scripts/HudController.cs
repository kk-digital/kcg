using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entitas;
using TMPro;

public class HudController : MonoBehaviour
{
    public TMP_Text scoreTxt;

    readonly Contexts _contexts;
    void Start()
    {
        var _contexts = Contexts.sharedInstance;
        var score = _contexts.game.GetGroup(GameMatcher.Score);
        // e.AddCamera(cam);
        Debug.Log(score);
        scoreTxt.text = "Ore: " + score.GetSingleEntity().score.score + "";

        score.OnEntityAdded += OnScoreChanged;
    }

    private void OnScoreChanged(IGroup @group, GameEntity entity, int index, IComponent component)
    {
        scoreTxt.text = "Ore: " + entity.score.score;
    }

}
