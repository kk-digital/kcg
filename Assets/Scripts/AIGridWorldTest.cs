using System.Collections.Generic;
using UnityEngine;
using AI;

// Note: Unit Testing to test AI.
public class AIGridWorldTest : MonoBehaviour
{
    [SerializeField] Material Material;

    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
    List<Vector3> verticies = new List<Vector3>();

    enum SquareType
    {
        AgentSquare,        // Blue Square.
        GoalSquare,         // Green Square.
        Obstaclesquares,    // Grey Square.
        NormalSquare,       // White Square.
        AgentPathSquare     // Yelow Square.
    }

    static bool Init = false;

    Contexts context;
    
    GameEntity agent;

    // Systems.
    PlannerSystem           planner;
    ActionControllerSystem  ActionController;

    SquareType[,] map;
    Vector2Int CurrentAgentPos = new Vector2Int(3, 0);
    Vector2Int GoalPos = new Vector2Int(5, 7);

    public void Start()
    {
        context = Contexts.sharedInstance;
        planner = new PlannerSystem();
        ActionController = new ActionControllerSystem();

        if (!Init)
        {
            Initialize();
            Init = true;
        }

        planner.Initialize();
        //ActionController.Initialize();
    }

    private bool IsValidPosition(Vector2Int pos)
    {
        int x = map.GetLength(0);
        int y = map.GetLength(1);

        if (pos.x < 0  ||
            pos.x >= x ||
            pos.y < 0  ||
            pos.y >= y)
        {
            return false;
        }

        if (map[pos.x, pos.y] == SquareType.Obstaclesquares)
        {
            return false;
        }

        return true;
    }

    private void UpdateBord()
    {
        Vector2Int NewPos = agent.agentPositionDiscrete2D.Value;
        map[CurrentAgentPos.x, CurrentAgentPos.y] = SquareType.AgentPathSquare;

        CurrentAgentPos = NewPos;
        map[CurrentAgentPos.x, CurrentAgentPos.y] = SquareType.AgentSquare;
    }

    public void Update()
    {
        //remove all children MeshRenderer
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);

        planner.Update();
        ActionController.Update();

        UpdateBord();
        DrawBoard();
    }

    // Create GridWorld board.
    public void Initialize()
    {
        map = new SquareType[8, 8]
            {   { SquareType.NormalSquare, SquareType.NormalSquare,    SquareType.NormalSquare,     SquareType.NormalSquare,    SquareType.NormalSquare, SquareType.NormalSquare,    SquareType.NormalSquare, SquareType.NormalSquare},
                { SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare,     SquareType.NormalSquare,    SquareType.NormalSquare, SquareType.NormalSquare,    SquareType.NormalSquare, SquareType.NormalSquare},
                { SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.Obstaclesquares,  SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.NormalSquare},
                { SquareType.AgentSquare,  SquareType.NormalSquare,    SquareType.NormalSquare,     SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.NormalSquare},
                { SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare,     SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.NormalSquare,    SquareType.NormalSquare, SquareType.NormalSquare},
                { SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare,     SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.GoalSquare},
                { SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare,     SquareType.NormalSquare,    SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.NormalSquare},
                { SquareType.NormalSquare, SquareType.NormalSquare,    SquareType.NormalSquare,     SquareType.NormalSquare,    SquareType.NormalSquare, SquareType.Obstaclesquares, SquareType.NormalSquare, SquareType.NormalSquare} };

        InitEntities();
    }

    public void InitEntities()
    {

        GoapState GoalState = new GoapState(new Dictionary<string, object>());
        GoalState.states.Add("pos", GoalPos);
        int GoalID = 0;
        GameEntity Goal = context.game.CreateEntity();
        Goal.AddAIGoal(GoalID, GoalState, 1);

        GoapState initialWorldState = new GoapState(new Dictionary<string, object>());
        initialWorldState.states.Add("pos", CurrentAgentPos);

        agent = context.game.CreateEntity();
        agent.AddAgentPositionDiscrete2D(CurrentAgentPos, Vector2Int.zero);
        agent.AddAIAgentPlanner(0, new Queue<int>(), new List<ActionInfo>(), new List<int>() { GoalID }, initialWorldState);

        int numRows = map.GetLength(0);
        int numColls = map.GetLength(1);

        Vector2Int[] dir = new Vector2Int[4] {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1) };

        int ActionID = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numColls; j++)
            {
                // Find avialable neighbors,
                Vector2Int pos = new Vector2Int(i, j);

                if (!IsValidPosition(pos))
                {
                    continue;
                }

                GoapState PreConditions = new GoapState(new Dictionary<string, object>());
                PreConditions.states = new Dictionary<string, object>();
                PreConditions.states.Add("pos", pos);

                for (int k = 0; k < 4; k++)
                {
                    Vector2Int effect = pos + dir[k];
                    if (IsValidPosition(effect))
                    {
                        GoapState Effects = new GoapState(new Dictionary<string, object>());
                        Effects.states.Add("pos", effect);

                        GameEntity entityAction = context.game.CreateEntity();
                        ActionID++;
                        int DurationTime = 200; // Miliseconds
                        entityAction.AddAIAction(ActionID, PreConditions, Effects, DurationTime, 1);
                    }
                }
            }
        }


    }

    private void DrawBoard()
    {
        int numRows = map.GetLength(0);
        int numColls = map.GetLength(1);

        int LeftUpConnerSquareX =  numRows / 2 - 1;
        int LeftUpConnerSquareY =  -numColls / 2;

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numColls; j++)
            {
                DrawSquare(LeftUpConnerSquareX - i, LeftUpConnerSquareY + j, map[i, j]);
            }
        }
    }

    private void DrawSquare(int posX, int posY, SquareType SqrtType)
    {
        // Loop Through map and draw Squares.
        const float cornerSize = 0.95f;

        Color color = Color.white;
        switch(SqrtType)
        {
            case SquareType.AgentSquare:
                color = Color.blue;
                break;
            case SquareType.GoalSquare:
                color = Color.green;
                break;
            case SquareType.Obstaclesquares:
                color = Color.grey;
                break;
            case SquareType.NormalSquare:
                color = Color.white;
                break;
            case SquareType.AgentPathSquare:
                color = Color.yellow;
                break;
            default:
                Debug.Log("Not supported square type.");
                break;
        }
        Drawtile(posY, posX, cornerSize, cornerSize, color);
    }

    private void Drawtile (float x, float y, float w, float h, Color color)

    {
        var mat = Instantiate(Material);
        mat.color = color;
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

        mesh.SetVertices(verticies);
        mesh.SetTriangles(triangles, 0);
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
}
