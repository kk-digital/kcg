using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehaviors should be in Asset/Script folder?
namespace PNGLoader
{
    public class PNGTest : MonoBehaviour
    {
        private void Start() 
        {
            
            TileSpriteImageLoaderManager.GetImageID("cat.png");// not found id, will add to dictionary afterwards if file exists.
            //it will get its rgba each pixel of the image "PixelsArray"
            TileSpriteImageLoaderManager.GetImageID("basic-enemy.png");// not found id, will add to dictionary afterwards if file exists.
            
            TileSpriteImageLoaderManager.GetImageID("cat.png");// found id, returns an id
        }
        
    }
}

