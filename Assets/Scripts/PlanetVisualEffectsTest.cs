using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Planet.VisualEffects;

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
            // Clear last frame
            foreach (var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            // Draw The Visual Effects
            planetVisualEffects.Draw(Instantiate(Material), transform, 1);
        }
    }
}
