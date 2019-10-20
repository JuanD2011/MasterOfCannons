public class Coin : Collectible
{
    protected override void Awake()
    {
        base.Awake();
        collectibleType = CollectibleType.Coin;
    }

    protected override void Collect()
    {
        base.Collect();
        gameObject.SetActive(false);
    }
}
