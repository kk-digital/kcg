using UnityEngine;

[CreateAssetMenu(menuName = "Game Config")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
    [SerializeField] Vector2Int _boardSize; public Vector2Int boardSize => _boardSize;
  
}
