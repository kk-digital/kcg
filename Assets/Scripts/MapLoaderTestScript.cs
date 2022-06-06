using Enums;
using UnityEngine;

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

        TileMap.Component tileMap;
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
            TileMap.MemorySystem.Instance.InitializeTiles();
            TileMap.DrawSystem.Instance.Initialize(Material, transform);
            tileMap = TileMap.GenerateSystem.Instance.GenerateTileMap();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)worldPosition.x;
                int y = (int)worldPosition.y;
                Debug.Log(x + " " + y);
                TileMap.ManagerSystem.Instance.RemoveTile(ref tileMap, x, y, PlanetLayer.Front);
                TileMap.GenerateSystem.Instance.BuildLayerTexture(ref tileMap, PlanetLayer.Front);
            }
            
            TileMap.DrawSystem.Instance.DrawTiles(ref tileMap);
        }
    }
}
