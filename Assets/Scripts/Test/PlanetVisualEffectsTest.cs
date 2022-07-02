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

        planetVisualEffects.Initialize();

        Init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    void Update()
    {
        if(Init)
        {
            // Draw The Visual Effects
            planetVisualEffects.Draw(Instantiate(Material), transform, 1);
        }
    }
}
