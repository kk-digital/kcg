using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PixelSnap : MonoBehaviour
{
    private Sprite sprite;
    private Vector3 actualPosition;
    private bool shouldRestorePosition;

    // Use this for initialization
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            sprite = renderer.sprite;
        }
        else
        {
            sprite = null;
        }
    }


    void OnWillRenderObject()
    {
        //Debug.Log("on will" + Camera.current);
        Camera cam = Camera.current;
        if (!cam)
            return;

        PixelPerfectCameraTestTool pixelPerfectCamera = cam.GetComponent<PixelPerfectCameraTestTool>();

        shouldRestorePosition = true;
        actualPosition = transform.position;

        float cameraPPU = (float)cam.pixelHeight / (2f * cam.orthographicSize);
        float cameraUPP = 1.0f / cameraPPU;

        Vector2 camPos = new Vector2(cam.transform.position.x, cam.transform.position.y);
        Vector2 pos = new Vector2(actualPosition.x, actualPosition.y);
        Vector2 relPos = pos - camPos;

        Vector2 offset = new Vector2(0, 0);
        // offset for screen pixel edge if screen size is odd
        offset.x = (cam.pixelWidth % 2 == 0) ? 0 : 0.5f;
        offset.y = (cam.pixelHeight % 2 == 0) ? 0 : 0.5f;
        // offset for pivot in Sprites
        Vector2 pivotOffset = new Vector2(0, 0);
        if (sprite != null)
        {
            pivotOffset = sprite.pivot - new Vector2(Mathf.Floor(sprite.pivot.x), Mathf.Floor(sprite.pivot.y)); // the fractional part in texture pixels           
            
            float camPixelsPerAssetPixel = cameraPPU / sprite.pixelsPerUnit;
            pivotOffset *= camPixelsPerAssetPixel; // convert to screen pixels
            
        }
        
        // Convert the units to pixels, round them, convert back to units. The offsets make sure that the distance we round is from screen pixel (fragment) edges totexel  edges.
        relPos.x = (Mathf.Round(relPos.x / cameraUPP - offset.x) + offset.x + pivotOffset.x) * cameraUPP;
        relPos.y = (Mathf.Round(relPos.y / cameraUPP - offset.y) + offset.y + pivotOffset.y) * cameraUPP;
        
        pos = relPos + camPos;

        transform.position = new Vector3(pos.x, pos.y, actualPosition.z);
    }

    // This scripts is based on the assumption that every camera that calls OnWillRenderObject(), will call OnRenderObject() before any other
    // camera calls any of these methods.
    void OnRenderObject()
    {
        //Debug.Log("on did" + Camera.current);
        if (shouldRestorePosition)
        {
            shouldRestorePosition = false;
            transform.position = actualPosition;
        }
    }

}
