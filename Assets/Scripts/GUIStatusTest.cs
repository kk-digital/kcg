using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIStatusTest : MonoBehaviour
{
    // ATLAS
    [SerializeField]
    private Material material;

    KGUI.HealthBarUI healthBarUI;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        healthBarUI = new KGUI.HealthBarUI();

        healthBarUI.Initialize(material, transform);
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    void Update()
    {
        // Clear last frame
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);

        healthBarUI.Draw(material, transform, 5000);
    }
}
