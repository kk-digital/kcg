using System;
using System.Collections.Generic;
using KMath;


namespace Particle
{
    public class ParticleEmitterCreationApi
    {
        // Start is called before the first frame update

        private int CurrentIndex;
        private ParticleEmitterProperties[] PropertiesArray;

        private Dictionary<string, int> NameToID;

        public ParticleEmitterCreationApi()
        {
            NameToID = new Dictionary<string, int>();
            PropertiesArray = new ParticleEmitterProperties[1024];
            for(int i = 0; i < PropertiesArray.Length; i++)
            {
                PropertiesArray[i] = new ParticleEmitterProperties();
            }
            CurrentIndex = -1;
        }

        public ParticleEmitterProperties Get(int Id)
        {
            if (Id >= 0 && Id < PropertiesArray.Length)
            {
                return PropertiesArray[Id];
            }

            return new ParticleEmitterProperties();
        }

        public ref ParticleEmitterProperties GetRef(int Id)
        {      
            return ref PropertiesArray[Id];
        }

        public ParticleEmitterProperties Get(string name)
        {
            int value;
            bool exists = NameToID.TryGetValue(name, out value);
            if (exists)
            {
                return Get(value);
            }

            return new ParticleEmitterProperties();
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

        public void SetParticleType(ParticleType particleType)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].ParticleType = particleType;
            }
        }

        public void SetVelocityInterval(Vec2f begin, Vec2f end)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].VelocityIntervalBegin = begin;
                PropertiesArray[CurrentIndex].VelocityIntervalEnd = end;
            }
        }

        public void SetSpawnRadius(float spawnRadius)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].SpawnRadius = spawnRadius;
            }
        }

        public void SetDuration(float duration)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].Duration = duration;
            }
        }

        public void SetLoop(bool loop)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].Loop = loop;
            }
        }

        public void SetParticleCount(int particleCount)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].ParticleCount = particleCount;
            }
        }

        public void SetTimeBetweenEmissions(float timeBetweenEmissions)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].TimeBetweenEmissions = timeBetweenEmissions;
            }
        }


        public void End()
        {
            CurrentIndex = -1;
        }
    }

}
