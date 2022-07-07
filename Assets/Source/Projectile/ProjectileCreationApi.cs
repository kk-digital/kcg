using System;
using System.Collections.Generic;
using UnityEngine;
using KMath;


namespace Projectile
{
    public class ProjectileCreationApi
    {
        // Start is called before the first frame update

        private int CurrentIndex;
        private ProjectileProperties[] PropertiesArray;

        private Dictionary<string, int> NameToID;

        public ProjectileCreationApi()
        {
            NameToID = new Dictionary<string, int>();
            PropertiesArray = new ProjectileProperties[1024];
            for(int i = 0; i < PropertiesArray.Length; i++)
            {
                PropertiesArray[i] = new ProjectileProperties();
            }
            CurrentIndex = -1;
        }

        public ProjectileProperties Get(int Id)
        {
            if (Id >= 0 && Id < PropertiesArray.Length)
            {
                return PropertiesArray[Id];
            }

            return new ProjectileProperties();
        }

        public ref ProjectileProperties GetRef(int Id)
        {      
            return ref PropertiesArray[Id];
        }

        public ProjectileProperties Get(string name)
        {
            int value;
            bool exists = NameToID.TryGetValue(name, out value);
            if (exists)
            {
                return Get(value);
            }

            return new ProjectileProperties();
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

        public void SetSpriteId(int SpriteId)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].SpriteId = SpriteId;
            }
        }

        public void SetSize(Vec2f size)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].Size = size;
            }
        }

        public void SetAnimation(Animation.AnimationType animationType)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].HasAnimation = true;
                PropertiesArray[CurrentIndex].AnimationType = animationType;
            }
        }

        public void SetSpeed(float speed)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].Speed = speed;
            }
        }

        public void SetRamp(bool canRamp, float startSpeed, float maxSpeed, 
            float rampTime)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].rampTime = rampTime;
                PropertiesArray[CurrentIndex].canRamp = canRamp;
                PropertiesArray[CurrentIndex].StartVelocity = startSpeed;
                PropertiesArray[CurrentIndex].MaxVelocity = maxSpeed;
            }
        }

        public void SetLinearDrag(bool canDrag, float linearDrag)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].canLinearDrag = canDrag;
                PropertiesArray[CurrentIndex].linearDrag = linearDrag;
            }
        }

        public void SetQuadraticDrag(bool canDrag, float quadraticDrag)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].canQuadraticDrag = canDrag;
                PropertiesArray[CurrentIndex].quadraticDrag = quadraticDrag;
            }
        }

        public void SetAcceleration(Vec2f acceleration)
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

        public void End()
        {
            CurrentIndex = -1;
        }
    }

}
