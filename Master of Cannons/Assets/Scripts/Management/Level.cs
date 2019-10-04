using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Level : MonoBehaviour
{
    [SerializeField]
    private int levelBuildIndex = 0;

    [SerializeField]
    private int starsNedeed = 0;

    public static event Delegates.Action<int> OnLoadLevel = null;

    private void OnMouseDown()
    {
        if (starsNedeed < 0) return;//TODO compare player stars and stars needed

        OnLoadLevel(levelBuildIndex);
    }
}
