using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class PlayerInput : IExecuteSystem
{
    public KeyCode turnLeft = KeyCode.A;
    public KeyCode turnRight = KeyCode.D;
    public KeyCode accelerate = KeyCode.W;
    public KeyCode decelerate = KeyCode.S;
    public KeyCode strafeLeft = KeyCode.Q;
    public KeyCode strafeRight = KeyCode.E;
    public KeyCode fire = KeyCode.Space;

    readonly GameContext _contexts;

    public PlayerInput(Contexts contexts)
    {
        _contexts = contexts.game;
    }

    public void Execute()
    {
        var leftIsDown = Input.GetKey(turnLeft);
        var rightIsDown = Input.GetKey(turnRight);
        var accelerateIsDown = Input.GetKey(accelerate);
        var decelerateIsDown = Input.GetKey(decelerate);
        var strafeLeftIsDown = Input.GetKey(strafeLeft);
        var strafeRightIsDown = Input.GetKey(strafeRight);
        var fireIsDown = Input.GetKey(fire);

        if (leftIsDown || rightIsDown || accelerateIsDown|| decelerateIsDown || fireIsDown || strafeLeftIsDown|| strafeRightIsDown)
        {
            var e =_contexts.CreateEntity();
            e.AddInput(accelerateIsDown, decelerateIsDown, leftIsDown, rightIsDown, strafeLeftIsDown, strafeRightIsDown, fireIsDown);
        }
    }

   
}
