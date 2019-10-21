using System.Collections;

public class HairyCharacter : Character
{

    protected override void Start()
    {
        base.Start();
        specialTime = 15f;
    }
    protected override IEnumerator OnSpecial()
    {
        GlobalMultipliers.IncreaseWickLenght(this, 0.2f);
        yield return (base.OnSpecial());
        GlobalMultipliers.IncreaseWickLenght(this, -0.2f);
    }
}
