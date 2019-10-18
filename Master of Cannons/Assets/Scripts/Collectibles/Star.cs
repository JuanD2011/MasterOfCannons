public class Star : Collectible
{
    protected override void Awake()
    {
        base.Awake();
        collectibleType = CollectibleType.Star;
    }

    protected override void Collect()
    {
        base.Collect();
    }
}
