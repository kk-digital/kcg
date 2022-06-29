using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planet.Background
{
    public class PlanetBackgroundVisualEffects
    {
        PlanetBackgroundStarField starField;
        PlanetBackgroundParallaxLayer parallaxLayer;

        private bool Init;

        public void Initialize()
        {
            if(GameManager.BackgroundDraw)
            {
                starField = new Planet.Background.PlanetBackgroundStarField();
                parallaxLayer = new Planet.Background.PlanetBackgroundParallaxLayer();

                starField.Initialize();
                parallaxLayer.Initialize();

                Init = true;
            }
        }

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            if(Init)
            {
                if(GameManager.BackgroundDraw)
                {
                    starField.Draw(material, transform, drawOrder);
                    parallaxLayer.Draw(material, transform, drawOrder);
                }
            }
        }
    }
}
