using UnityEngine;

public class Coin : Collectible
{
    private void Awake()
    {
        collectibleType = CollectibleType.Coin;
    }

    protected override void Collect()
    {
        Debug.Log("Coinn");
    }
}
