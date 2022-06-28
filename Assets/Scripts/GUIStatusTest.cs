using UnityEngine;

public class GUIStatusTest : MonoBehaviour
{
    // ATLAS
    [SerializeField]
    private Material material;

    KGUI.HealthBarUI healthBarUI;
    KGUI.FoodBarUI foodBarUI;

    //Init
    private bool Init;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Health Bar Initialize
        healthBarUI = new KGUI.HealthBarUI();
        healthBarUI.Initialize();

        // Food Bar Initialize
        foodBarUI = new KGUI.FoodBarUI();
        foodBarUI.Initialize(transform);

        Init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html
    private void OnGUI()
    {
        if (Init)
        {
            // Clear last frame
            foreach (var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            //Health Bar Draw
            healthBarUI.Draw();

            //Food Bar Draw
            foodBarUI.Draw();
        }
    }
}
