using UnityEngine;

public class GUIStatusTest : MonoBehaviour
{
    // ATLAS
    [SerializeField]
    private Material material;

    // Health Bar
    KGUI.HealthBarUI healthBarUI;

    // Food Bar
    KGUI.FoodBarUI foodBarUI;

    // Water Bar
    KGUI.WaterBarUI waterBarUI;

    // Oxygen Bar
    KGUI.OxygenBarUI oxygenBarUI;

    // Fuel Bar
    KGUI.FuelBarUI fuelBarUI;

    //Init
    private bool Init;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        Contexts entitasContext = Contexts.sharedInstance;
        // Health Bar Initialize
        healthBarUI = new KGUI.HealthBarUI();
        healthBarUI.Initialize();

        // Food Bar Initialize
        foodBarUI = new KGUI.FoodBarUI();
        foodBarUI.Initialize(entitasContext);

        // Water Bar Initialize
        waterBarUI = new KGUI.WaterBarUI();
        waterBarUI.Initialize(entitasContext);

        // Oxygen Bar Initialize
        oxygenBarUI = new KGUI.OxygenBarUI();
        oxygenBarUI.Initialize(entitasContext);

        // Oxygen Bar Initialize
        fuelBarUI = new KGUI.FuelBarUI();
        fuelBarUI.Initialize(entitasContext);

        Init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html
    private void OnGUI()
    {
        Contexts entitasContext = Contexts.sharedInstance;
        if (Init)
        {
            //Health Bar Draw
            healthBarUI.Draw(entitasContext);

            // Food Bar Update
            foodBarUI.Update();

            // Water Bar Update
            waterBarUI.Update();

            // Fuel Bar Update
            fuelBarUI.Update(null);

            // OxygenBar Update
            oxygenBarUI.Update();
        }
    }
}
