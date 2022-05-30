using Tiles.PlanetMap;
using Planets.Systems;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private Material tileAtlas;
    
    private const int FPS = 60;


    // Method for setting everything up, for like init GameManager for example
    private void InitStage1()
    {
        Application.targetFrameRate = FPS; // Cap at 60 FPS
        TPMCreator.Instance.InitStage1();
    }
    
    private void InitStage2()
    {
    }

    private void Awake()
    {
        InitStage1();
        InitStage2();
    }
    
    // Method to update physics
    private void FixedUpdate()
    {
        
    }

    // Method for Drawing
    public void Update()
    {
        //remove all children MeshRenderer
        foreach(var mr in tileParent.GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);

        PSUpdate.Instance.UpdateTileMap(ref TPMCreator.Instance.PlanetTilesMap, ref tileParent, ref tileAtlas);
    }
}
