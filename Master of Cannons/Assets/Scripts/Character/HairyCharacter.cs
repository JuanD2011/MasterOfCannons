
public class HairyCharacter : Character
{
    protected override void Start()
    {
        base.Start();
        GlobalMultipliers.IncreaseWickLenght(this, 0.2f);
    }
}
