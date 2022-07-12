using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

public class AnimancerTestScript : MonoBehaviour
{
    AnimationClip IdleAnimationClip ;
    AnimationClip RunAnimationClip ;
    AnimationClip WalkAnimationClip ;


    AnimancerComponent AnimancerComponent;

    void Start()
    {
        // get the 3d model from the scene
        GameObject humanoid = GameObject.Find("DefaultHumanoid");

        // get the animator component from the game object
        // this component is used by animancer
        Animator _Animator = humanoid.GetComponent<Animator>();


        // create an animancer object and give it a reference to the Animator component
        GameObject obj = new GameObject("AnimancerComponent", typeof(AnimancerComponent));
        AnimancerComponent = obj.GetComponent<AnimancerComponent>();
        AnimancerComponent.Animator = _Animator;

        
        // load some animation clips from disk
         IdleAnimationClip = (AnimationClip)Resources.Load("Animation/" + "Humanoid-Idle");
         RunAnimationClip = (AnimationClip)Resources.Load("Animation/" + "Humanoid-Run");
         WalkAnimationClip = (AnimationClip)Resources.Load("Animation/" + "Humanoid-Walk");


        // play the idle animation
        AnimancerComponent.Play(IdleAnimationClip);
    }

    // Update is called once per frame
    void Update()
    {
        bool run = Input.GetKeyDown(KeyCode.R);
        bool walk = Input.GetKeyDown(KeyCode.W);
        bool idle = Input.GetKeyDown(KeyCode.I);

        if (run)
        {
            AnimancerComponent.Play(RunAnimationClip);
        }
        else if (walk)
        {
            AnimancerComponent.Play(WalkAnimationClip);
        }
        else if (idle)
        {
            AnimancerComponent.Play(IdleAnimationClip);
        }
    }
}
