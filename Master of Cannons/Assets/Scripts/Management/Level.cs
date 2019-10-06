using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Level : MonoBehaviour
{
    [SerializeField]
    private int levelBuildIndex = 0;

    [SerializeField]
    private int starsNedeed = 0;

    public static event Delegates.Action<int, int> OnCanPlayLevel = null;

    private void OnMouseDown()
    {
        if (!MenuManager.canSelectLevel) return;
        OnCanPlayLevel(starsNedeed, levelBuildIndex);
    }
}
