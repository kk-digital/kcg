using UnityEngine;

public class TestSquare : MonoBehaviour {
    // Renderer
    public LineRenderer renderer;
    public Shader       shader;
    public Material     material;

    // Camera
    public Camera       camera;

    // Current color of square
    public Color color;

    // Color of square if square is colliding
    public Color    collision_color;

    // Color of square if square is not colliding with anything
    public Color no_collision_color;

    // Set to true if square should be draggable with mouse
    public bool draggable;

    // Position of center
    public float xpos;
    public float ypos;
    public float zpos;

    // Half of size
    public float half_width;
    public float half_height;

    // Velocity
    public float vx;
    public float vy;

    // Position of bottom left corner
    public float c1_x { get { return xpos - half_width;  } }
    public float c1_y { get { return ypos + half_height; } }

    // Position of top right corner
    public float c3_x { get { return xpos + half_width;  } }
    public float c3_y { get { return ypos - half_height; } }

    // Helper functions
    private int min(float f, float v) {
        if(v < 0.0f) f += v;

        if(f < 0.0f) {
            if(f == (int)f) return (int)f;
            else            return (int)f - 1;
        }

        return (int)f;
    }

    private int max(float f, float v) {
        if(v > 0.0f) f += v;

        if(f > 0.0f) {
            if(f == (int)f) return (int)f;
            else return (int)f + 1;
        }

        return (int)f;
    }

    private float abs(float f) {
        return f >= 0.0f ? f : -f;
    }

    // Min/max positions rounded to ints
    public int   xmin { get { return min(c1_x, vx); } }
    public int   ymin { get { return min(c3_y, vy); } }

    public int   xmax { get { return max(c3_x, vx); } }
    public int   ymax { get { return max(c1_y, vy); } }

    // Used by renderer
    public Vector3[] corners   = new Vector3[4];

    // Position relative to square where square was dragged from. Hard to explain, but needed to make dragging less janky.
    private float xdrag;
    private float ydrag;
    private bool  being_dragged;

    // Checks if square is colliding with another square
    public bool colliding(TestSquare square) {
        return abs(xpos - square.xpos) < half_width  + square.half_width
            && abs(ypos - square.ypos) < half_height + square.half_height;
    }
    
    // Checks if square is colliding with mouse
    public bool colliding(float x, float y) {
        return abs(xpos - x) < half_width  * 2.0f
            && abs(ypos - y) < half_height * 2.0f;
    }

    void Start() {
        // Initialize test shader, material, and renderer
        shader                 = Shader.Find("Hidden/Internal-Colored");
        material               = new Material(shader);
        material.hideFlags     = HideFlags.HideAndDontSave;
        renderer               = gameObject.AddComponent<LineRenderer>();
        renderer.material      = material;
        renderer.useWorldSpace = true;
        renderer.loop          = true;

        renderer.startWidth    =
        renderer.endWidth      = 1.0f;

        corners[0]             = new Vector3();
        corners[1]             = new Vector3();
        corners[2]             = new Vector3();
        corners[3]             = new Vector3();

        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        material.SetInt("_ZWrite", 0);
        material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
    }

    void Update() {
        if(draggable) {
            Vector3 mouse = camera.ScreenToWorldPoint(Input.mousePosition);

            if(Input.GetMouseButton(0) && colliding(mouse.x, mouse.y)) {
                if(!being_dragged) {
                    xdrag = xpos - mouse.x;
                    ydrag = ypos - mouse.y;
                    being_dragged = true;
                }

                xpos = mouse.x + xdrag;
                ypos = mouse.y + ydrag;
            } else being_dragged = false;
        }

        xpos += vx;
        ypos += vy;

        corners[0].x = c1_x;
        corners[1].x = c1_x;
        corners[2].x = c3_x;
        corners[3].x = c3_x;

        corners[0].y = c1_y;
        corners[1].y = c3_y;
        corners[2].y = c3_y;
        corners[3].y = c1_y;

        corners[0].z = zpos;
        corners[1].z = zpos;
        corners[2].z = zpos;
        corners[3].z = zpos;

        renderer.SetPositions(corners);
        renderer.positionCount = 4;

        renderer.startColor    =
        renderer.endColor      = color;
    }
}

public class RectangleTileCollisionTest : MonoBehaviour {
    // Camera
    public Camera camera;

    // 16x16 background grid
    public TestSquare[,] grid;

    // Two primary test squares
    public TestSquare square1;
    public TestSquare square2;

    // Colors for primary test squares
    public Color    square1_collision_color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    public Color    square2_collision_color = new Color(0.0f, 0.0f, 1.0f, 1.0f);

    public Color square1_no_collision_color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color square2_no_collision_color = new Color(0.8f, 0.8f, 0.8f, 1.0f);

    // Size of squares in grid
    public const int square_width  = 12;
    public const int square_height = 12;

    // Amount of squares in grid
    public const int grid_width    = 12;
    public const int grid_height   = 12;

    // Size of square 1
    public int square1_width       = 30;
    public int square1_height      = 18;

    // Size of square 2
    public int square2_width       = 12;
    public int square2_height      = 24;
    void Start() {
        grid = new TestSquare[grid_width, grid_height];

        // Alternate colors for odd and even tiles to make grid more pleasurable to look at
        Color     odd_collision_color       = new Color(1.00f, 0.50f, 0.50f, 1.0f);
        Color    even_collision_color       = new Color(0.75f, 0.25f, 0.25f, 1.0f);

        Color  odd_no_collision_color       = new Color(0.50f, 0.50f, 0.50f, 1.0f);
        Color even_no_collision_color       = new Color(0.25f, 0.25f, 0.25f, 1.0f);

        int start_x = -grid_width  * (square_width  + 1) / 2;
        int start_y = -grid_height * (square_height + 1) / 2;

        for(int x = 0; x < grid_width; x++)
            for(int y = 0; y < grid_height; y++) {
                GameObject obj = new GameObject();
                obj.name       = "Background grid square " + (x + 1) + " | " + (y + 1);

                grid[x, y]     = obj.AddComponent<TestSquare>();

                bool even      = x % 2 != y % 2;

                if(even) {
                    grid[x, y].   collision_color =    even_collision_color;
                    grid[x, y].no_collision_color = even_no_collision_color;
                } else {
                    grid[x, y].   collision_color =     odd_collision_color;
                    grid[x, y].no_collision_color =  odd_no_collision_color;
                }

                grid[x, y].color       = grid[x, y].no_collision_color;
                grid[x, y].half_width  = 0.5f * square_width;
                grid[x, y].half_height = 0.5f * square_height;
                grid[x, y].xpos        = start_x + grid[x, y].half_width  + x * (square_width  + 1);
                grid[x, y].ypos        = start_y + grid[x, y].half_height + y * (square_height + 1);
            }

        GameObject square1_obj     = new GameObject();
        GameObject square2_obj     = new GameObject();

        square1_obj.name           = "Test square 1";
        square2_obj.name           = "Test square 2";

        square1                    = square1_obj.AddComponent<TestSquare>();
        square2                    = square2_obj.AddComponent<TestSquare>();

        square1.   collision_color =    square1_collision_color;
        square1.no_collision_color = square1_no_collision_color;
        square1.color              = square1_no_collision_color;

        square2.collision_color    =    square2_collision_color;
        square2.no_collision_color = square2_no_collision_color;
        square2.color              = square2_no_collision_color;

        square1.half_width         = 0.5f * square1_width;
        square1.half_height        = 0.5f * square1_height;

        square2.half_width         = 0.5f * square2_width;
        square2.half_height        = 0.5f * square2_height;

        square1.xpos               =  start_x + (square1_width  + square_width ) * 0.5f + 0.5f;
        square1.ypos               =  start_y + (square1_height + square_height) * 0.5f + 0.5f;
        square1.zpos               = -0.1f;

        square2.xpos               = -start_x - (square2_width  + square_width ) * 0.5f - 1.5f;
        square2.ypos               = -start_y - (square2_height + square_height) * 0.5f - 1.5f;
        square2.zpos               = -0.1f;

        square1.camera             = camera;
        square2.camera             = camera;

        square1.draggable          = true;
        square2.draggable          = true;
    }

    void Update() {
        // Reset colors for all squares
        for(int x = 0; x < grid_width; x++)
            for(int y = 0; y < grid_height; y++) 
                grid[x, y].color = grid[x, y].no_collision_color;

        square1.color = square1.no_collision_color;
        square2.color = square2.no_collision_color;

        // Check collisions for square 1
        for(int x = 0; x < grid_width; x++)
            for(int y = 0; y < grid_height; y++)
                if(square1.colliding(grid[x, y])) {
                    square1   .color = square1   .collision_color;
                    grid[x, y].color = grid[x, y].collision_color;
                }
        
        // Check collisions for square 2
        for(int x = 0; x < grid_width; x++)
            for(int y = 0; y < grid_height; y++)
                if(square2.colliding(grid[x, y])) {
                    square2   .color = square2   .collision_color;
                    grid[x, y].color = grid[x, y].collision_color;
                }
    }
}
