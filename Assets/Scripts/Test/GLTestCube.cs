using UnityEngine;

/* 
    GLSL CUBE RENDERING TEST SCRIPT
*/

class GLTestCube : MonoBehaviour
{

    // Documentation: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    private void Awake()
    {
        //Check if SceneManager even exists
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, Enums.SceneObjectType.SceneObjectTypeUtilityScript);
        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html
    void LateUpdate()
    {
        // Rotating cube around Y axis
        transform.Rotate(new Vector3(25f * Time.deltaTime, 25.0f * Time.deltaTime, 0f));
    }
}
