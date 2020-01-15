public class UISaveCoinsButton : UIButtonBase
{
    public override void OnButtonClicked()
    {
        CollectibleManager.Instance.UpdateCoins();
    }
}
