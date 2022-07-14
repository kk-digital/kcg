using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

public class AnimancerTestScript : MonoBehaviour
{

    public static int HumanoidCount = 3;
    GameObject[] HumanoidArray;

    AnimationClip IdleAnimationClip ;
    AnimationClip RunAnimationClip ;
    AnimationClip WalkAnimationClip ;
    AnimationClip GolfSwingClip;


    AnimancerComponent[] AnimancerComponentArray;

    void Start()
    {
        // get the 3d model from the scene
        //GameObject humanoid = GameObject.Find("DefaultHumanoid");

        // load the 3d model from file
        GameObject prefab = (GameObject)Resources.Load("DefaultHumanoid");

        HumanoidArray = new GameObject[HumanoidCount];
        AnimancerComponentArray = new AnimancerComponent[HumanoidCount];

        for(int i = 0; i < HumanoidCount; i++)
        {
            HumanoidArray[i] = Instantiate(prefab);
            HumanoidArray[i].transform.position += new Vector3(i, 0.0f, 0.0f);
        }


        


        // create an animancer object and give it a reference to the Animator component
        for(int i = 0; i < HumanoidCount; i++)
        {
            GameObject animancerComponent = new GameObject("AnimancerComponent", typeof(AnimancerComponent));
            // get the animator component from the game object
            // this component is used by animancer
            AnimancerComponentArray[i] = animancerComponent.GetComponent<AnimancerComponent>();
            AnimancerComponentArray[i].Animator = HumanoidArray[i].GetComponent<Animator>();
        }

        
        // load some animation clips from disk
         IdleAnimationClip = (AnimationClip)Resources.Load("Animation/" + "Humanoid-Idle");
         RunAnimationClip = (AnimationClip)Resources.Load("Animation/" + "Humanoid-Run");
         WalkAnimationClip = (AnimationClip)Resources.Load("Animation/" + "Humanoid-Walk");
         GolfSwingClip = (AnimationClip)Resources.Load("Animation/" + "Humanoid-GolfSwing");


        // play the idle animation
        for(int i = 0; i < HumanoidCount; i++)
        {
            AnimancerComponentArray[i].Play(IdleAnimationClip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool run = Input.GetKeyDown(KeyCode.R);
        bool walk = Input.GetKeyDown(KeyCode.W);
        bool idle = Input.GetKeyDown(KeyCode.I);
        bool golf = Input.GetKeyDown(KeyCode.G);

        for(int i = 0; i < HumanoidCount; i++)
        {
            if (run)
            {
                AnimancerComponentArray[i].Play(RunAnimationClip, 0.25f);
            }
            else if (walk)
            {
                AnimancerComponentArray[i].Play(WalkAnimationClip, 0.25f);
            }
            else if (idle)
            {
                AnimancerComponentArray[i].Play(IdleAnimationClip, 0.25f);
            }
            else if (golf)
            {
                AnimancerComponentArray[i].Play(GolfSwingClip, 0.25f);
            }
        }
    }
}
