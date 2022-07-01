using System.Collections.Generic;
using UnityEngine;
using AI;
using KMath;

// Note: Unit Testing to test AI.
/*
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
    Action.ActionSchedulerSystem   actionScheduler;

    SquareType[,] map;
    Vec2i currentAgentPos = new Vec2i(3, 0);
    Vec2i goalPos = new Vec2i(5, 7);

    public void Start()
    {
        context = Contexts.sharedInstance;
        planner = new PlannerSystem();
        actionScheduler = new Action.ActionSchedulerSystem();

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
        Vec2i newPos = agent.agentPositionDiscrete2D.Value;
        map[currentAgentPos.X, currentAgentPos.X] = SquareType.AgentPathSquare;

        currentAgentPos = newPos;
        map[currentAgentPos.Y, currentAgentPos.Y] = SquareType.AgentSquare;
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
        actionScheduler.Update(Time.deltaTime);

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
        GoalState.states.Add("pos", goalPos);
        int GoalID = 0;
        GameEntity Goal = context.game.CreateEntity();
        Goal.AddAIGoal(GoalID, GoalState, 1);

        GoapState initialWorldState = new GoapState(new Dictionary<string, object>());
        initialWorldState.states.Add("pos", currentAgentPos);

        agent = context.game.CreateEntity();
        agent.AddAgentPositionDiscrete2D(currentAgentPos, Vec2i.zero);
        agent.AddAgentActionScheduler(new List<int>(), new List<int>());
        agent.AddAgentAIController(0, new Queue<int>(), new List<int>() { GoalID }, initialWorldState);

        int numRows = map.GetLength(0);
        int numColls = map.GetLength(1);

        Vector2Int[] dir = new Vector2Int[4] {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1) };

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

                GoapState preCondition = new GoapState(new Dictionary<string, object>());
                preCondition.states = new Dictionary<string, object>();
                preCondition.states.Add("pos", pos);

                for (int k = 0; k < 4; k++)
                {
                    Vector2Int effect = pos + dir[k];
                    float durationTime = 200f; // Miliseconds
                    if (IsValidPosition(effect))
                    {
                        GoapState effects = new GoapState(new Dictionary<string, object>());
                        effects.states.Add("pos", effect);


                        GameState.ActionManager.CreateAction();
                        GameState.ActionManager.SetTime(durationTime);
                        GameState.ActionManager.SetGoap(preCondition, effects, 1);
                        GameState.ActionManager.EndAction();
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
        Utility.Render.DrawQuadColor(posY, posX, cornerSize, cornerSize, color, Instantiate(Material), transform, 0);
    }
}*/
