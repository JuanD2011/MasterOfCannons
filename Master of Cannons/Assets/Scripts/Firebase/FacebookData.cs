using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Threading.Tasks;

public class FacebookData : MonoBehaviour
{
    public List<Dictionary<string, string>> friendsDataList = new List<Dictionary<string, string>>();
    public void GetFriendsPlayingThisGame()
    {
        friendsDataList.RemoveRange(0, friendsDataList.Count);
        UISocial.hideFBFriends.Invoke();
        if (!CheckLogIn()) {
            Debug.Log("You are not LOGGED IN Facebook...");
            return; }

        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, async result =>
        {          
            Dictionary<string, object> dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            List<object> friendsList = (List<object>)dictionary["data"];

            foreach (var dict in friendsList) {

                Dictionary<string, string> friendData = await FirebaseDBManager.DB.GetFacebookUserData(((Dictionary<string, object>)dict)["id"].ToString());
                friendsDataList.Add(friendData);
                //Debug.Log(((Dictionary<string, object>)dict)["id"].ToString());
            }

            //await Task.Delay(1000);
            FriendsOrderedByValue();
            //UISocial.showFriendDataHandler(friendsDataList);
        });
       
    }

    public void FriendsOrderedByValue()
    {
        int friendsCount = friendsDataList.Count;

        for (int i = 0; i < friendsCount - 1; i++)
        {
            for (int j = 0; j < friendsCount - 1 - i; j++)
            {
                int actualFriendCoins = int.Parse(friendsDataList[j]["coins"]);
                int nextFriendCoins = int.Parse(friendsDataList[j + 1]["coins"]);
                if (actualFriendCoins < nextFriendCoins)
                {
                    friendsDataList[j]["coins"] = nextFriendCoins.ToString();
                    friendsDataList[j + 1]["coins"] = actualFriendCoins.ToString();
                }
            }
        }

        UISocial.showFriendDataHandler(friendsDataList);
    }

    public static async Task<string> GetMyName()
    {
        string name = string.Empty;
        FB.API("me?fields=name", HttpMethod.GET, result => {
            name = result.ResultDictionary["name"].ToString();
            Debug.Log("My Facebook Name Is : " + name);
        });

        while(name == string.Empty)
            await Task.Delay(1000);

        return name;
    }


    public static bool CheckLogIn()
    {        
        return (FB.IsLoggedIn == true) ? true: false; 
    }
}

