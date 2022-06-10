using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class DrawSystem
    {

        Contexts EntitasContext;

        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> verticies = new List<Vector3>();

        public DrawSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public void Draw(Material material, Transform transform)
        {
        }
    }
}
