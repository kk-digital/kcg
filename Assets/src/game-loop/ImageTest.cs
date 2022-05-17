using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehaviors should be in Asset/Script folder?
namespace ImageLoader
{
    public class ImageTest : MonoBehaviour
    {
        LoaderData ImageLoaderManager;
        LoaderData SpriteSheetLoaderManager;
        private void Awake() 
        {
            ImageLoaderManager = new TileSpriteImageLoaderManager();
            SpriteSheetLoaderManager = new SpriteSheetImageLoader();
        }
        private void Start() 
        {
            //image
            TileSpriteImageLoaderManager.Instance.GetImageID("cat.png");// not found id, will add to dictionary afterwards if file exists.
            //it will get its rgba each pixel of the image "PixelsArray"
            TileSpriteImageLoaderManager.Instance.GetImageID("basic-enemy.png");// not found id, will add to dictionary afterwards if file exists.
            TileSpriteImageLoaderManager.Instance.GetImageID("cat.png");// found id, returns an id


            //spritesheet or tilesheet
            SpriteSheetImageLoader.Instance.GetSpriteSheetID("spiderDrill_spritesheet.png");
            SpriteSheetImageLoader.Instance.GetSpriteSheetID("grassHopper.png");
            SpriteSheetImageLoader.Instance.GetSpriteSheetID("slime.png");
            
        }
        
    }
}

