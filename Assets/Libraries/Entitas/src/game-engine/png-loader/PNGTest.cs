using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PNGLoader
{
    public class PNGTest : MonoBehaviour
    {
        private void Start() 
        {
            PngLoaderManager.InitializePNGTest();
            PngLoaderManager.DebugImageDatas();
        }
        
    }
}

