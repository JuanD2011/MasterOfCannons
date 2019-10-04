﻿using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Threading.Tasks;

public class FacebookData : MonoBehaviour
{
    public void GetFriendsPlayingThisGame()
    {              
        if (!CheckLogIn()) {
            Debug.Log("You are not LOGGED IN Facebook...");
            return; }

        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, async result =>
        {          
            Dictionary<string, object> dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            List<object> friendsList = (List<object>)dictionary["data"];

            foreach (var dict in friendsList) {

                await FirebaseDBManager.DB.GetFacebookUserData(((Dictionary<string, object>)dict)["id"].ToString());
                Debug.Log(((Dictionary<string, object>)dict)["id"].ToString());
            }

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

