using UnityEngine;
using System.Collections.Generic;
using TileProperties;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PlanetTileMap.Unity
{
    //Note: TileMap should be mostly controlled by GameManager


    //Note(Mahdi): we are just testing and making sure everything is working
    // before we move things out of here
    // there will be things like rendering, collision, TileMap
    // that are not supposed to be here.

    class MapLoaderTestScript : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        PlanetTileMap TileMap;
        static bool InitTiles;

        public void Start()
        {
            if (!InitTiles)
            {
                InitializeSystems();
                
                InitTiles = true;
            }
        }

        public void InitializeSystems()
        {
            Planet.MemorySystem.Instance.InitializeTiles();
            Planet.DrawSystem.Instance.Initialize(Material, transform);
            TileMap = Planet.GenerateSystem.Instance.GenerateTileMap();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                Debug.Log(x + " " + y);
                TileMap.RemoveTile(x, y, Layer.Front);
                TileMap.BuildLayerTexture(Layer.Front);
            }
            
            Planet.DrawSystem.Instance.DrawTiles(ref TileMap);
        }
    }
}
