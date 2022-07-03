using System;
using System.Collections.Generic;
using KMath;


namespace Agent
{
    public class AgentCreationApi
    {
        // Start is called before the first frame update

        private int CurrentIndex;
        private AgentProperties[] PropertiesArray;

        private Dictionary<string, int> NameToID;

        public AgentCreationApi()
        {
            NameToID = new Dictionary<string, int>();
            PropertiesArray = new AgentProperties[1024];
            for(int i = 0; i < PropertiesArray.Length; i++)
            {
                PropertiesArray[i] = new AgentProperties();
            }
            CurrentIndex = -1;
        }

        public AgentProperties Get(int Id)
        {
            if (Id >= 0 && Id < PropertiesArray.Length)
            {
                return PropertiesArray[Id];
            }

            return new AgentProperties();
        }

        public ref AgentProperties GetRef(int Id)
        {      
            return ref PropertiesArray[Id];
        }

        public AgentProperties Get(string name)
        {
            int value;
            bool exists = NameToID.TryGetValue(name, out value);
            if (exists)
            {
                return Get(value);
            }

            return new AgentProperties();
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

        public void SetSpriteSize(Vec2f size)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].SpriteSize = size;
            }
        }

        public void SetStartingAnimation(int startingAnimation)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].StartingAnimation = startingAnimation;
            }
        }

        public void SetEnemyBehaviour(int enemyBehaviour)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].EnemyBehaviour = enemyBehaviour;
            }
        }

        public void SetDetectionRadius(float detectionRadius)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].DetectionRadius = detectionRadius;
            }
        }

        public void SetHealth(float health)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].Health = health;
            }
        }

        public void SetAttackCooldown(float attackCooldown)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].AttackCooldown = attackCooldown;
            }
        }

        public void SetCollisionBox(Vec2f offset, Vec2f dimensions)
        {
            if (CurrentIndex >= 0 && CurrentIndex < PropertiesArray.Length)
            {
                PropertiesArray[CurrentIndex].CollisionOffset = offset;
                PropertiesArray[CurrentIndex].CollisionDimensions = dimensions;
            }
        }

        public void End()
        {
            CurrentIndex = -1;
        }
    }

}
