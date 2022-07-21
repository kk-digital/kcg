using UnityEngine;
using System;
using System.Collections.Generic;
using Enums.Tile;
using KMath;


namespace Engine3D
{
    public class ModelLoader
    {
        public GameObject[] ObjectArray;
        public Dictionary<string, int> ObjectID;

        public ModelLoader()
        {
            ObjectArray = new GameObject[1024];
            ObjectID = new Dictionary<string, int>();
        }

        public void Load(string filename, ModelType modelType)
        {
            int index = (int)modelType;
            if (index < ObjectArray.Length)
            {
 
            }
            else
            {
                Array.Resize(ref ObjectArray, index * 2);
            }

            ObjectID.Add(filename, index);
            GameObject prefab = (GameObject)Resources.Load(filename);
            ObjectArray[index] = prefab;
        }

        public ref GameObject GetModel(ModelType modelType)
        {
            return ref ObjectArray[(int)modelType];
        }
    }
}
