using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planet.Background
{
    public class PlanetBackgroundVisualEffects
    {
        PlanetBackgroundParallaxLayer parallaxLayer;

        private bool Init;

        public void Initialize(Material material, Transform transform, int drawOrder)
        {
            if(GameManager.BackgroundDraw)
            {
                parallaxLayer = new Planet.Background.PlanetBackgroundParallaxLayer();
                parallaxLayer.Initialize(material, transform);

                Init = true;
            }
        }

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            if(Init)
            {
                if(GameManager.BackgroundDraw)
                {
                    parallaxLayer.Draw();
                }
            }
        }
    }
}
