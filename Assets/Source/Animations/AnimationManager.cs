using System;
using System.Collections.Generic;

namespace Animation
{
    public class AnimationManager
    {
        // Start is called before the first frame update

        private int CurrentIndex;
        private AnimationProperties[] TypeArray;

        private  Dictionary<string, int> NameToID;

        public AnimationManager()
        {
            NameToID = new Dictionary<string, int>();
            TypeArray = new AnimationProperties[1024];
            for(int i = 0; i < TypeArray.Length; i++)
            {
                TypeArray[i] = new AnimationProperties();
            }
            CurrentIndex = -1;
        }

        public AnimationProperties Get(int index)
        {
            if (index >= 0 && index < TypeArray.Length)
            {
                return TypeArray[index];
            }

            return new AnimationProperties();
        }

        public AnimationProperties Get(string name)
        {
            int value;
            bool exists = NameToID.TryGetValue(name, out value);
            if (exists)
            {
                return Get(value);
            }

            return new AnimationProperties();
        }

        public void CreateAnimation(int index)
        {
            while (index >= TypeArray.Length)
            {
                Array.Resize(ref TypeArray, TypeArray.Length * 2);
            }

            CurrentIndex = index;
            if (CurrentIndex != -1)
            {
                TypeArray[CurrentIndex].Index = CurrentIndex;
            }
        }

        public void SetName(string name)
        {
            if (CurrentIndex == -1) return;
            
            if (!NameToID.ContainsKey(name))
            {
                NameToID.Add(name, CurrentIndex);
            }

            TypeArray[CurrentIndex].Name = name;
        }

        public void SetTimePerFrame(float timePerFrame)
        {
            if (CurrentIndex == -1) return;

            TypeArray[CurrentIndex].TimePerFrame = timePerFrame;
        }
        
        public void SetFrameCount(int frameCount)
        {
            if (CurrentIndex == -1) return;

            TypeArray[CurrentIndex].FrameCount = frameCount;
        }

        public void SetBaseSpriteID(int baseSpriteId)
        {
            if (CurrentIndex == -1) return;

            TypeArray[CurrentIndex].BaseSpriteId = baseSpriteId;
        }

        public void EndAnimation()
        {
            CurrentIndex = -1;
        }

    }

}
