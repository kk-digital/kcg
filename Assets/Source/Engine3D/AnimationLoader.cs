using UnityEngine;
using System;
using System.Collections.Generic;
using Enums.Tile;
using KMath;


namespace Engine3D
{
    public class AnimationLoader
    {
        public AnimationClip[] ClipArray;
        public Dictionary<string, int> AnimationID;

        public AnimationLoader()
        {
            ClipArray = new AnimationClip[1024];
            AnimationID = new Dictionary<string, int>();
        }

        public void Load(string filename, AnimationType animationType)
        {
            int index = (int)animationType;
            if (index < ClipArray.Length)
            {
                
            }
            else
            {
                Array.Resize(ref ClipArray, index * 2);
            }

            AnimationID.Add(filename, index);
            AnimationClip animation = (AnimationClip)Resources.Load(filename, typeof(AnimationClip));
            ClipArray[index] = animation;
        }

        public ref AnimationClip GetAnimationClip(AnimationType animationType)
        {
            return ref ClipArray[(int)animationType];
        }
    }
}
