using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Level : MonoBehaviour
{
    [SerializeField]
    private int levelBuildIndex = 0;

    [SerializeField]
    private int starsNedeed = 0;

    public int StarsNedeed { get => starsNedeed; private set => starsNedeed = value; }

    public static event Delegates.Action<int, int> OnLevelSelected = null;

    private void Awake()
    {
        OnLevelSelected = null;
    }

    private void OnMouseDown()
    {
        if (!MenuManager.canSelectLevel) return;
        OnLevelSelected(StarsNedeed, levelBuildIndex);
    }
}
