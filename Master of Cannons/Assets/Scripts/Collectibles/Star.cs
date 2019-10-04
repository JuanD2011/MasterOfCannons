using UnityEngine;

public class Star : Collectible
{
    private void Awake()
    {
        collectibleType = CollectibleType.Star;
    }

    protected override void Collect()
    {
        Debug.Log("Star");
    }
}
