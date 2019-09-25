using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Delegates;
using System.Threading.Tasks;

public class FacebookData : MonoBehaviour
{
    public List<Dictionary<string, string>> friendsDataList = new List<Dictionary<string, string>>();

    public void GetFriendsPlayingThisGame()
    {              
        if (!CheckLogIn()) {
            Debug.Log("You are not LOGGED IN Facebook...");
            return; }
        friendsDataList.RemoveRange(0, friendsDataList.Count);
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {          
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            foreach (var dict in friendsList) {

                FirebaseDBManager.DB.GetFacebookUserData(((Dictionary<string, object>)dict)["id"].ToString());
                Debug.Log(((Dictionary<string, object>)dict)["id"].ToString());
            }

            //await Task.Delay(4000);
            //UISocial.showFriendDataHandler(friendsDataList);
        });
       
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

