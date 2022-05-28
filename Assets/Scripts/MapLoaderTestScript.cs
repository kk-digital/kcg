using System;
using UnityEngine;
using System.Collections.Generic;
using Physics;
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

    class MapLoaderTestScript : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;

        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> verticies = new List<Vector3>();

        PlanetTileMap TileMap;
        RectangleBoundingBoxCollision Player;
        Vector2 PlayerPosition;

        int PlayerSprite2ID;
        const float TileSize = 1.0f;

        Vector2 MapOffset = new Vector2(-3.0f, 4.0f);

        static bool InitTiles = false;
        

        public void Start()
        {
            // inittialize the sprites/ tiles ect..
            if (!InitTiles)
            {
                // loads the sprites into memory
                LoadSprites();

                // creates the tiles properties
                CreateDefaultTiles();

                // creates collision stuff
                CreateTestPlayer();

                InitTiles = true;
            }
            // TODO(Mahdi): does not make sense to put them here
            // move them out ! 
            LateUpdate();
        }  


        
        public void Update()
        {
            // in here we will draw everything to the screen


            //remove all children MeshRenderer
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            float speed = 1.0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                PlayerPosition.y += speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                PlayerPosition.y -= speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                PlayerPosition.x += speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                PlayerPosition.x -= speed * Time.deltaTime;
            }


            // drawing the player to the screen
            PlayerCollidersTest();

            // drawing the tile map to the screen
            DrawMapTest();
        }

        // this function is used to draw a tile map to the screen
        // it will only draw the tiles that are visible
        // only works for orthographic mode
        void DrawMapTest()
        {

            var visibleRect = CalcVisibleRect();

            byte[] bytes = new byte[32 * 32* 4];

            for(int layerIndex = 0; layerIndex < 1; layerIndex++)
            {
                for(int j = 0; j < TileMap.Ysize; j++)
                {
                    for(int i = 0; i < TileMap.Xsize; i++)
                    {
                        ref PlanetTile tile = ref TileMap.getTile(i, j);
                        int tilePropertiesIndex = tile.TileIdPerLayer[layerIndex];

                        if (tilePropertiesIndex >= 0)
                        {
                            TilePropertiesData tileProperties =
                                GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);

                            GameState.TileSpriteAtlasManager.GetSpriteBytes(tileProperties.SpriteId, bytes);

                            var x = MapOffset.x + (i * TileSize);
                            var y = MapOffset.y + (j * TileSize);

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

        // loading the sprite sheets and filling up the sprite atlas
        public void LoadSprites()
        {

            // load all the sprite sheets
            int MetalSlabsTileSheet = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\Tiles_metal_slabs\\Tiles_metal_slabs.png");
            int StoneBulkheads = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tile_wallbase\\Tiles_stone_bulkheads.png");
            int TilesMoon = 
                        GameState.TileSpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png");
            int PlayerTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png",
                         32, 48);



          // blit the player sprite sheet to the atlas.
          // returns an id, we can use that for drawing
           PlayerSprite2ID = GameState.SpriteAtlasManager.Blit(PlayerTileSheet, 0, 0);;

            //


            // creating the tiles
            // we can add more properties to the tiles
            GameState.TileCreationApi.CreateTile(0);
            GameState.TileCreationApi.SetTileName("slab1");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 0, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(1);
            GameState.TileCreationApi.SetTileName("slab2");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 1, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(2);
            GameState.TileCreationApi.SetTileName("slab3");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 4, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(3);
            GameState.TileCreationApi.SetTileName("slab4");
            GameState.TileCreationApi.SetTileTexture16(MetalSlabsTileSheet, 5, 0);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(4);
            GameState.TileCreationApi.SetTileName("tile5");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 5, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(5);
            GameState.TileCreationApi.SetTileName("tile6");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 4, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(6);
            GameState.TileCreationApi.SetTileName("tile7");
            GameState.TileCreationApi.SetTileTexture16(StoneBulkheads, 7, 1);
            GameState.TileCreationApi.EndTile();

            GameState.TileCreationApi.CreateTile(7);
            GameState.TileCreationApi.SetTileName("tile_moon_1");
            GameState.TileCreationApi.SetTileTexture(TilesMoon, 0, 0);
            GameState.TileCreationApi.EndTile();



        }

        //NOTE(Mahdi): this is used to create some test tiles
        // to make sure the system is working
        public void CreateDefaultTiles()
        {
            
            // Generating the map
            int mapWidth = 128;
            int mapHeight = 128;

            TileMap = new PlanetTileMap(mapWidth, mapHeight);

            for(int j = 0; j < mapHeight; j++)
            {
                for(int i = 0; i < mapWidth; i++)
                {
                    PlanetTile tile = PlanetTile.EmptyTile();
                    tile.TileIdPerLayer[0] = 0;
                    if (i % 10 == 0)
                    {
                        tile.TileIdPerLayer[0] = 7;
                    }
                    if (j % 2 == 0)
                    {
                        tile.TileIdPerLayer[0] = 2;
                    }
                    if (j % 3 == 0)
                    {
                        tile.TileIdPerLayer[0] = 1;
                    }

                    if ((j > 1 && j < 6) || j > 10)
                    {
                       tile.TileIdPerLayer[0] = -1; 
                    }

                    
                    TileMap.SetTile(i, j, tile);
                }
            }

        }
        
        void CreateTestPlayer()
        {
            PlayerPosition = new Vector2(6, 6);

            // 1f - considered to be 32 pixel
            Player = new RectangleBoundingBoxCollision(PlayerPosition - MapOffset, new Vector2(1f, 48.0f / 32.0f));
        }


        // draws the player to the screen
        void PlayerCollidersTest()
        {
            Player = new RectangleBoundingBoxCollision(PlayerPosition - MapOffset, new Vector2(1f, 48.0f / 32.0f));
            bool isCollidingBottom = Player.IsCollidingBottom(ref TileMap);

            //Debug.Log($"Player Bottom Collided: {isCollidingBottom}");


            byte[] spriteBytes = new Byte[32 * 48 * 4];

            if (isCollidingBottom)
            {
                GameState.SpriteAtlasManager.GetSpriteBytes(PlayerSprite2ID, spriteBytes);
            }
            else
            {
                 GameState.SpriteAtlasManager.GetSpriteBytes(PlayerSprite2ID, spriteBytes);
            }

            DrawSprite(PlayerPosition.x, PlayerPosition.y, 1.0f, 48.0f / 32.0f, spriteBytes, 32, 48);

        }


        public void LoadMap()
        {
            LateUpdate();
        }


         // draws 1 tile into the screen
        // Note(Mahdi): this code is for testing purpose
        void DrawSprite(float x, float y, float w, float h, byte[] spriteBytes,
             int spriteW, int spriteH)
        {
            var tex = CreateTextureFromRGBA(spriteBytes, spriteW, spriteH);
            var mat = Instantiate(Material);
            mat.SetTexture("_MainTex", tex);
            var mesh = CreateMesh(transform, "abc", 0, mat);

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


        private Mesh CreateMesh(Transform parent, string name, int sortingOrder, Material material)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent);

            var mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            var mf = go.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.sortingOrder = sortingOrder;

            return mesh;
        }

        private void LateUpdate()
        {

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


        // converts pixel to a unity Texture2D
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
        
            
            float WorldPositionX(float pos) => (pos * TileSize);
            float WorldPositionY(float pos) =>(pos * TileSize);

            var playerBound = Player.Bounds(PlayerPosition);
            
            void DrawBottomCollision()
            {
                var y = WorldPositionY(PlayerPosition.y);
                var LeftTilePos = new Vector3(WorldPositionX(playerBound.LeftTile), y, 0);
                var RightTilePos = new Vector3(WorldPositionX(playerBound.RightTile) + TileSize, y, 0);
                Gizmos.color = Player.IsCollidingBottom(ref TileMap) ? Color.red : Color.green;
                
                Gizmos.DrawLine(LeftTilePos, RightTilePos);
            }

          /*  void DrawPlayerBottomCorners(int length)
            {
                // Centralized on player
                var y = WorldPositionY(PlayerPosition.y);

                var localPosStartX = (int) PlayerPosition.x - (length / 2);
                if (localPosStartX < 0) localPosStartX = 0;
                
                for (; localPosStartX < (int)PlayerPosition.x + (length / 2) + length % 2; localPosStartX++)
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
            }
            
            DrawPlayerBottomCorners(9);*/
            DrawBottomCollision();
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MapLoaderTestScript))]
    public class TerrainGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (MapLoaderTestScript)target;

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
