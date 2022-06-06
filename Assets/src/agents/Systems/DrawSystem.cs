using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public class DrawSystem
    {

        Contexts EntitasContext;
        
        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();

        public DrawSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public void Draw(Transform transform)
        {
            var AgentsWithSprite = EntitasContext.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentSprite2D));
            foreach (var entity in AgentsWithSprite)
            {
                //Transform newTr = Transform.Instantiate(transform);
               // transform.Translate(new Vector3(entity.agentPosition2D.Value.x, entity.agentPosition2D.Value.y, 0.0f));
                SetMesh(entity/*, newTr*/);
            }
        }

        private void SetMesh(GameEntity entity/*, Transform transform*/)
        {
            triangles.Clear();
            uvs.Clear();
            verticies.Clear();
            
            var x = entity.agentPosition2D.Value.x;
            var y = entity.agentPosition2D.Value.y;
            var width = entity.agentSprite2D.Size.x;
            var height = entity.agentSprite2D.Size.y;
            
            var p0 = new Vector3(x, y, 0);
            var p1 = new Vector3((x + width), (y + height), 0);
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
    

            entity.agentSprite2D.Mesh.SetVertices(verticies);
            entity.agentSprite2D.Mesh.SetUVs(0, uvs);
            entity.agentSprite2D.Mesh.SetTriangles(triangles, 0);

            //entity.agentSprite2D.Mesh
        }
    }
}

