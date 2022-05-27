using UnityEngine;
using System.Collections.Generic;
using Agents.Entities;
using TileProperties;

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

    class TilesRenderer : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;

        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();

        Entity Player;
        World World;

        int PlayerSpriteID;
        int PlayerSprite2ID;
        const float TileSize = 1.0f;

        readonly Vector2 MapOffset = new(-3.0f, 4.0f);

        static bool InitTiles;
        

        public void Start()
        {
            if (!InitTiles)
            {
                World = new World(new Vector2Int(128, 128));
                World.CreateDefaultTiles();
                InitTiles = true;
            }
            // TODO(Mahdi): does not make sense to put them here
            // move them out ! 
            InitStage1();
            InitStage2();
        }

        //All memory allocations/setups go here
        //File loading should not occur at this stage
        public void InitStage1()
        {
            // todo: commented out the tmx stuff for now
            /*FileLoader = new TmxImporter(Path.Combine(BaseDir, TileMap));
            FileLoader.LoadStage1();*/
        }

        //Load settings from files and other init, that requires systems to be intialized
        public void InitStage2()
        {
 

            //TestDrawTiles();
            LateUpdate();


        }      

        public void Update()
        {
            //remove all children MeshRenderer
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            //TODO: Move DrawMapTest to DrawMap()
            DrawMapTest();
        }

        void DrawMapTest()
        {
            var visibleRect = CalcVisibleRect();

            byte[] bytes = new byte[32 * 32* 4];

            for(int layerIndex = 0; layerIndex < 1; layerIndex++)
            {
                for(int j = 0; j < World.Planet.Size.y; j++)
                {
                    for(int i = 0; i < World.Planet.Size.x; i++)
                    {
                        ref PlanetTile tile = ref World.Planet.getTile(i, j);
                        int tilePropertiesIndex = tile.TileIdPerLayer[layerIndex];

                        if (tilePropertiesIndex >= 0)
                        {
                            TilePropertiesData tileProperties =
                                GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);

                            GameState.TileSpriteAtlasManager.GetSpriteBytes(tileProperties.SpriteId, bytes);

                            var x = (i * TileSize);
                            var y = (j * TileSize);

                            if (x + TileSize >= visibleRect.X && x <= visibleRect.X + visibleRect.W &&
                            y + TileSize >= visibleRect.Y && y <= visibleRect.Y + visibleRect.H)
                            {
                                DrawTile(x, y, TileSize, TileSize, bytes);
                            }
                        }

                    }
                }
            }
        }
        
        //NOTE(Mahdi): this is used to create some test tiles
        // to make sure the system is working

        public void LoadMap()
        {
            InitStage1();
            InitStage2();
        }


         // draws 1 tile into the screen
        // Note(Mahdi): this code is for testing purpose
        void DrawSprite(float x, float y, float w, float h, byte[] spriteBytes,
             int spriteW, int spriteH)
        {
            var tex = CreateTextureFromRGBA(spriteBytes, spriteW, spriteH);
            var mat = Instantiate(Material);
            mat.SetTexture("_MainTex", tex);
            var mesh = World.CreateMesh(transform, "abc", 0, mat);

            triangles.Clear();
            uvs.Clear();
            verticies.Clear();


            var p0 = new Vector3(x, y, 0);
            var p1 = new Vector3((x + w), (y + h), 0);
            var p2 = p0; p2.y = p1.y;
            var p3 = p1; p3.y = p0.y;

            verticies.Add(p0);
            verticies.Add(p1);
            verticies.Add(p2);
            verticies.Add(p3);

            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(1);
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(3);

            var u0 = 0;
            var u1 = 1;
            var v1 = -1;
            var v0 = 0;

            var uv0 = new Vector2(u0, v0);
            var uv1 = new Vector2(u1, v1);
            var uv2 = uv0; uv2.y = uv1.y;
            var uv3 = uv1; uv3.y = uv0.y;


            uvs.Add(uv0);
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
    

            mesh.SetVertices(verticies);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
        }
        
        void DrawTile(float x, float y, float w, float h, byte[] spriteBytes)
        {
            DrawSprite(x, y, w, h, spriteBytes, 32, 32);
        }

        private void LateUpdate()
        {
         /*   var visibleRect = CalcVisibleRect();

            //rebuild all layers for visible rect
            foreach (var mb in meshBuildersByLayers)
                mb.BuildMesh(visibleRect);
        */
        }

        public struct R
        {
            public float X;
            public float Y;
            public float W;
            public float H;

            public R(float x, float y, float w, float h)
            {
                X = x;
                Y = y;
                W = w;
                H = h;
            }
        }
        private static R CalcVisibleRect()
        {
            var cam = Camera.main;
            var pos = cam.transform.position;
            var height = 2f * cam.orthographicSize;
            var width = height * cam.aspect;
            var visibleRect = new R(pos.x - width / 2, pos.y - height / 2, width, height);
            return visibleRect;
        }

        private Texture2D CreateTextureFromRGBA(byte[] rgba, int w, int h)
        {

            var res = new Texture2D(w, h, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };

            var pixels = new Color32[w * h];
            for (int x = 0 ; x < w; x++)
            for (int y = 0 ; y < h; y++)
            { 
                int index = (x + y * w) * 4;
                var r = rgba[index];
                var g = rgba[index + 1];
                var b = rgba[index + 2];
                var a = rgba[index + 3];

                pixels[x + y * w] = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
            }
            res.SetPixels32(pixels);
            res.Apply();

            return res;
        }
        
#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            float WorldPositionX(float pos) => pos * TileSize;
            float WorldPositionY(float pos) => pos * TileSize;

            //var playerBound = AgentBoxColliderComponent.Bounds(Player.Pos, Player.Size);
            
            /*void DrawBottomCollision()
            {
                if (!Player.IsCollidingBottom(ref TileMap)) return;
                
                var y = WorldPositionY(playerBound.BottomTile + 1f);
                var leftTilePos = new Vector3(WorldPositionX(playerBound.LeftTile), y, 0);
                var rightTilePos = new Vector3(WorldPositionX(playerBound.RightTile) + TileSize, y, 0);
                Gizmos.color = Color.red;
                
                Gizmos.DrawLine(leftTilePos, rightTilePos);
            }*/

            /*void DrawPlayerBottomCorners(int length)
            {
                // Centralized on player
                var y = WorldPositionY(playerBound.BottomTile + 1f);

                var localPosStartX = (int) Player.Pos.x - (length / 2);
                if (localPosStartX < 0) localPosStartX = 0;
                
                for (; localPosStartX < (int)Player.Pos.x + (length / 2) + length % 2; localPosStartX++)
                {
                    var tile = TileMap.getTile(localPosStartX, playerBound.BottomTile);

                    if (tile.TileIdPerLayer[0] >= 0)
                    {
                        var startPos = new Vector3(WorldPositionX(localPosStartX), y, 0);
                        var endPos = new Vector3(WorldPositionX(localPosStartX) + TileSize, y, 0);;
                        
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(startPos, endPos);
                    }
                }
            }*/
            
            //DrawPlayerBottomCorners(9);
            //DrawBottomCollision();
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TilesRenderer))]
    public class TerrainGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (TilesRenderer)target;

            // Show default inspector property editor
            DrawDefaultInspector();

            if (GUILayout.Button("Load Map"))
            {
                myTarget.LoadMap();
            }

        }
    }
#endif
}
