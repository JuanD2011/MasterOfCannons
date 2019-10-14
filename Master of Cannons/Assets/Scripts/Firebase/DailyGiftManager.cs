using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Delegates;
public class DailyGiftManager : MonoBehaviour
{
    float minutesToWaitGift;
    public static Delegates.Action getGiftTimesHandler;
    public static Delegates.Action giveRewardHandler;

    private IEnumerator Start()
    {
        HoursToMinutes(0.02f, ref minutesToWaitGift);
        giveRewardHandler = GiveReward;
        getGiftTimesHandler = GetGiftTime;
        yield return new WaitUntil(() => FirebaseAuthManager.myUser != null);
        getGiftTimesHandler.Invoke();
    }

    async void GetGiftTime()
    {
        Dictionary<string, string> giftInfo = new Dictionary<string, string>();
        giftInfo =  await FirebaseDBManager.DB.GetGiftsInfo(FirebaseAuthManager.myUser.UserId);
        double currentTime = double.Parse(giftInfo["current time"]);
        double timeChestWasOpened = double.Parse(giftInfo[DataManager.timeChestWasOpenedStr]);
        StartCoroutine(ChestCountdown(currentTime, timeChestWasOpened));
    }

    IEnumerator ChestCountdown(double currentTime, double timeChestWasOpened)
    {
        //From milliseconds to seconds
        currentTime /= 1000;
        timeChestWasOpened /= 1000;

        for (; ; )
        {
            currentTime += Time.deltaTime;

            double chestRemainingTime = currentTime - timeChestWasOpened;
            double chestMinutesDifference = chestRemainingTime / 60D;
            double chestSubstractTime = minutesToWaitGift - chestMinutesDifference;
            TimeSpan chestSpan = TimeSpan.FromMinutes(chestSubstractTime);
            string chestLabel = chestSpan.ToString(@"h\:mm\:ss");

            if(chestSubstractTime <= 0)
            {
                UIPlayerData.updateChestsInfo();
                yield break;
            }

            UIPlayerData.showChestsInfo(chestLabel);
            yield return null;
        }
    }

    public void GiveReward()
    {
        UIPlayerData.updateCoins(UnityEngine.Random.Range(10, 30));
    }
   
    void HoursToMinutes(float hours, ref float minutes) => minutes = hours * 60;


}
