using System;
using System.Collections.Generic;
using UnityEngine;
using KMath;


namespace Particle
{
    public class ParticleCreationApi
    {
        // Start is called before the first frame update

        private int CurrentIndex;
        private ParticleProperties[] PropertiesArray;

        private Dictionary<string, int> NameToID;

        public ParticleCreationApi()
        {
            NameToID = new Dictionary<string, int>();
            PropertiesArray = new ParticleProperties[1024];
            for(int i = 0; i < PropertiesArray.Length; i++)
            {
                PropertiesArray[i] = new ParticleProperties();
            }
            CurrentIndex = -1;
        }

        public ParticleProperties Get(int Id)
        {
            if (Id >= 0 && Id < PropertiesArray.Length)
            {
                return PropertiesArray[Id];
            }

            return new ParticleProperties();
        }

        public ref ParticleProperties GetRef(int Id)
        {      
            return ref PropertiesArray[Id];
        }

        public ParticleProperties Get(string name)
        {
            int value;
            bool exists = NameToID.TryGetValue(name, out value);
            if (exists)
            {
                return Get(value);
            }

            return new ParticleProperties();
        }

        public void Create(int Id)
        {
            while (Id >= PropertiesArray.Length)
            {
                Array.Resize(ref PropertiesArray, PropertiesArray.Length * 2);
            }

            CurrentIndex = Id;
            if (CurrentIndex != -1)
            {
                PropertiesArray[CurrentIndex].PropertiesId = CurrentIndex;
            }
        }

        public void SetName(string name)
        {
            if (CurrentIndex == -1) return;
            
            if (!NameToID.ContainsKey(name))
            {
                NameToID.Add(name, CurrentIndex);
            }

            PropertiesArray[CurrentIndex].Name = name;
        }

        public void SetDecayRate(float decayRate)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].DecayRate = decayRate;
            }
        }

        public void SetAcceleration(Vector2 acceleration)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].Acceleration = acceleration;
            }
        }

        public void SetDeltaRotation(float deltaRotation)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].DeltaRotation = deltaRotation;
            }
        }

        public void SetDeltaScale(float deltaScale)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].DeltaScale = deltaScale;
            }
        }

        public void SetSpriteId(int spriteId)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].SpriteId = spriteId;
            }
        }

        public void SetSize(Vec2f size)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].Size = size;
            }
        }

        public void SetStartingVelocity(Vector2 startingVelocity)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].StartingVelocity = startingVelocity;
            }
        }

        public void SetStartingRotation(float startingRotation)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].StartingRotation = startingRotation;
            }
        }

        public void SetStartingScale(float startingScale)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].StartingScale = startingScale;
            }
        }

        public void SetStartingColor(Color startingColor)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].StartingColor = startingColor;
            }
        }

        public void SetAnimationSpeed(float animationSpeed)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].AnimationSpeed = animationSpeed;
            }
        }


        public void End()
        {
            CurrentIndex = -1;
        }
    }

}
