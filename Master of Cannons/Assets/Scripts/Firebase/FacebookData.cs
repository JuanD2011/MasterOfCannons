using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookData : MonoBehaviour
{
    public bool isLoggedIn;
    public void GetFriendsPlayingThisGame()
    {
        if (!CheckLogIn()) { Debug.Log("You are not LOGGED IN Facebook..."); return; }
        
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            foreach (var dict in friendsList) Debug.Log(((Dictionary<string, object>)dict)["name"]);
        });
    }

    public static bool CheckLogIn()
    {        
        return (FB.IsLoggedIn == true) ? true: false; 
    }
}

