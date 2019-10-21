using UnityEngine;

public class GlobalMultipliers : MonoBehaviour
{
    private static float wickLenght = 1;
    public static float WickLenght { get => wickLenght; private set => wickLenght = value; }

    public static float coins = 1;
    public static float prestige = 1;

    /// <summary>
    /// Increase wick multiplier value
    /// </summary>
    /// <param name="sender"> Object that called the method </param>
    /// <param name="percentageIncrease"> How much do you want it that increase with respect to the current value? </param>
    public static void IncreaseWickLenght(object sender, float percentageIncrease)
    {
        bool canSet = CanSetWickMultiplier(sender);
        if (canSet)
            WickLenght += percentageIncrease;
    }

    static bool CanSetWickMultiplier(object obj)
    {
        switch (obj)
        {
            case HairyCharacter h:
                return true;
            default:
                return false;
        }
    }

}
