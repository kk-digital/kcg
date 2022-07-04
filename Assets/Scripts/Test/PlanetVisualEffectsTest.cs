using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Planet.Background;

public class PlanetVisualEffectsTest : MonoBehaviour
{
    // ATLAS
    [SerializeField]
    private Material Material;

    PlanetBackgroundVisualEffects planetVisualEffects;

    private bool Init;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        planetVisualEffects = new PlanetBackgroundVisualEffects();

        planetVisualEffects.Initialize(Material, transform, 1);

        Init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    void Update()
    {
        if(Init)
        {
            // check if the sprite atlas textures needs to be updated
            for (int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
            {
                GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
            }

            // check if the tile sprite atlas textures needs to be updated
            for (int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
            {
                GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
            }

            // Draw The Visual Effects
            planetVisualEffects.Draw(Material, transform, 1);
        }
    }
}
