using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planet.VisualEffects
{
    public class PlanetBackgroundVisualEffects
    {
        PlanetBackgroundStarField starField;

        private bool Init;

        public void Initialize()
        {
            if(GameLoop.BackgroundDraw)
            {
                starField = new Planet.VisualEffects.PlanetBackgroundStarField();

                starField.Initialize();

                Init = true;
            }
        }

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            if(Init)
            {
                if(GameLoop.BackgroundDraw)
                {
                    starField.Draw(material, transform, drawOrder);
                }
            }
        }
    }
}
