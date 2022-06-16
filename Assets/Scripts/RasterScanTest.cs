using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath;
using Planet;
using Entitas;

public class RasterScanTest : MonoBehaviour
{
    // Tile Map
    Planet.TileMap testTiles;

    // Collision Block to test line
    public GameObject CollisionBlock;

    // Game Context
    private GameContext gameContext;

    // Player Position
    Vec2f playerPosition;

    // Mouse Position
    Vector2 pointerPos;

    // Blocks Array
    public GameObject[] blocks;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Assign Game Context to get players position
        gameContext = Contexts.sharedInstance.game;

        // Assign Test Tiles
        testTiles = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;

        // Creatae the blocks array
        blocks = new GameObject[100];
    }

    private void Update()
    {
        // Shoot Line when pressed right-click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Get Player Entity
            IGroup<GameEntity> entities =
            Contexts.sharedInstance.game.GetGroup(GameMatcher.AgentPlayer);
            foreach (var player in entities)
            {
                // Get Players Position
                playerPosition = player.physicsPosition2D.Value;
            }

            // Get Mouse Position
            Vector3 mousePos = Input.mousePosition;

            // Get NearZ
            mousePos.z = Camera.main.nearClipPlane;

            // Convert 3D mouse position to 2D Screen Space
            pointerPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Create new start and end position based on mouse position
            Cell start = new Cell
            {
                x = (int)playerPosition.X,
                y = (int)playerPosition.Y
            };

            Cell end = new Cell
            {
                x = (int)pointerPos.x,
                y = (int)pointerPos.y
            };

            // Log raster start and end point
            Debug.Log($"Raster Line from ({start.x}, {start.y}) to ({end.x}, {end.y}) :");

            // Log places drawed line go through
            foreach (var cell in start.LineTo(end))
            {
                Debug.Log($"({cell.x},{cell.y})");

                ref var tile = ref testTiles.GetTileRef(cell.x, cell.y, Enums.Tile.MapLayerType.Front);
                if (tile.Type >= 0)
                {
                    GameObject Temp = CollisionBlock;
                    Temp = Instantiate(Temp);
                    Temp.transform.position = new Vector2(cell.x, cell.y);

                    StartCoroutine(FadeOut(Temp));
                }
            }

#if UNITY_EDITOR
            // Draw Debug Line
            Debug.DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(end.x, end.y, 0.0f), Color.red);
#endif
        }
    }

    IEnumerator FadeOut(GameObject target)
    {
        //  yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null)
            {
                blocks[i] = target;
                break;
            }
        }

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != null)
            {
                float value = 0.5f;
                float elapsed = 0.0f;
                while (elapsed < 3.0f)
                {
                    value = Mathf.Lerp(0.5f, 0.0f, elapsed / 3.0f);
                    if (blocks[i])
                    {
                        blocks[i].GetComponent<SpriteRenderer>().color = new Color(blocks[i].GetComponent<SpriteRenderer>().color.r,
                            blocks[i].GetComponent<SpriteRenderer>().color.g, blocks[i].GetComponent<SpriteRenderer>().color.b,
                                value);

                    }
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                value = 0.0f;
                if (blocks[i])
                    Destroy(blocks[i]);
            }
        }

        yield return null;
    }
}