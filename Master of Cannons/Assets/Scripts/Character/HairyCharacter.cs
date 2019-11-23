using System.Collections;

public class HairyCharacter : Character
{
    private float increasePercentage = 0.2f;
    protected override void Start()
    {
        base.Start();
        specialTime = 15f;
    }
    protected override IEnumerator OnSpecial()
    {
        GlobalMultipliers.SetWickLenght(this, increasePercentage);
        yield return (base.OnSpecial());
        GlobalMultipliers.SetWickLenght(this, -increasePercentage);
    }
}
