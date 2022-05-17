using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehaviors should be in Asset/Script folder?
namespace PNGLoader
{
    public class PNGTest : MonoBehaviour
    {
        void Awake()
        {
            SceneManager.Instance.Register(this.GetType().Name);
        }
        
        private void Start() 
        {
            PngLoaderManager.InitStage1();
            PngLoaderManager.DebugImageDatas();
        }
        
    }
}

