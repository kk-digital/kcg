using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
    public LayerMask walkableLayerMask;

    RaycastHit2D hit;

    // Update is called once per frame
    void Update () {
        if(Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 20.0f, walkableLayerMask);
            if (hit.collider != null)
                Pools.pool.ReplaceInput(InputIntent.SelectTile, new object[] { hit.collider.gameObject });
            else
                Pools.pool.ReplaceInput(InputIntent.UnselectTile, null);
        }
        
    }
}
