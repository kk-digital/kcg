using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Agent
{
    public class AgentList
    {
        
        // array for storing entities
        public AgentEntity[] List;

        public int Size;
        // used for tracking down an available 
        // index that we can use to insert
        public int LastFreeIndex;

        // the capacity is just the length of the list
        public int Capacity
        {
            get
            {
                return List.Length;
            }
        }

        public AgentList()
        {
            List = new AgentEntity[1024];
        }


        public ref AgentEntity Add()
        {
            // if we dont have enough space we expand
            // the capacity
            if (Size + 1 >= Capacity)
            {
                Expand(ExpandFunction(Capacity));
            }


            // trying to find an empty index
            // we use LastFreeIndex for a faster insertion
            int Found = -1;
            for(int index = LastFreeIndex; index < Capacity; index++)
            {
                ref AgentEntity thisEntity = ref List[index];

                if (!thisEntity.IsInitialized)
                {
                    Found = index;
                    break;
                }
            }
            if (Found == -1)
            {
                for(int index = 0; index < LastFreeIndex; index++)
                {
                    ref AgentEntity thisEntity = ref List[index];

                    if (!thisEntity.IsInitialized)
                    {
                        Found = index;
                        break;
                    }
                }
            }

            // increment the LastFreeIndex
            LastFreeIndex = (LastFreeIndex + 1) % Capacity;


            // creating the Entity and initializing it
            AgentEntity NewEntity = new AgentEntity();
            NewEntity.AgentId = Found;
            NewEntity.IsInitialized = true;

            List[Found] = NewEntity;
            Size++;

             return ref List[Found];
        }


        public ref AgentEntity Get(int Index)
        {
            return ref List[Index];
        }


        // to remove an entity we just 
        // set the IsInitialized field to false
        public void Remove(AgentEntity entity)
        {
            entity.IsInitialized = false;
            Size--;
        }




        // used to grow the list
        private void Expand(int NewCapacity)
        {
            // make sure the new capacity is more than 1
            if (NewCapacity == 0)
            {
                NewCapacity = 1;
            }

            // make sure the new capacity 
            // is bigget than the old one
            if (NewCapacity > Capacity)
            {
                System.Array.Resize(ref List, Capacity);
            }
        }
        

        // We use this to determine 
        // the new size based off the old one.
        // The new size should allways be bigger 
        private int ExpandFunction(int oldSize)
        {
            return oldSize * 2;
        }
    }
}