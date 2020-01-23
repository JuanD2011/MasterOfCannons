using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Level : MonoBehaviour
{
    public static event Delegates.Action<LevelData> OnLevelSelected = null;

    public LevelData LevelData { get; private set; } = null;

    /// <summary>
    /// Executed by level world manager, initialize data of the level
    /// </summary>
    /// <param name="_levelData"></param>
    public void InitializeLevelData(LevelData _levelData) => LevelData = _levelData;

    /// <summary>
    /// Level islannd clicked and try to load it
    /// </summary>
    private void OnMouseUp()
    {
        if (!MenuManager.canSelectLevel) return;
        OnLevelSelected(LevelData);
    }
}
