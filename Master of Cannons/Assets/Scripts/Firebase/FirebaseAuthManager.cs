using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;

public class FirebaseAuthManager : MonoBehaviour
{
    FirebaseAuth auth;
    public static FirebaseUser myUser;
    //FirebaseApp app;

    private IEnumerator Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        AnonymousSignIn();
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        yield return null;
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        //if (auth.CurrentUser != myUser)
        //{
        //    bool signedIn = myUser != auth.CurrentUser && auth.CurrentUser != null;
        //    if (!signedIn && myUser != null)
        //    {
        //        Debug.Log("Signed out " + myUser.UserId);
        //    }
        //    myUser = auth.CurrentUser;
        //    if (signedIn)
        //    {
        //        Debug.Log("Signed in " + myUser.UserId);
        //    }
        //}
    }

    public void AnonymousSignIn()
    {
        Debug.Log("Creating an Anonymous Account...");

        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
         
            myUser = task.Result;
            User mUser = new User { username = myUser.DisplayName, userId = myUser.UserId };
            //Memento.SaveData(mUser);
            FirebaseDBManager.DB.SaveData(mUser.userId, mUser.username, mUser.email);
            
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                myUser.DisplayName, myUser.UserId);
        });
        
    }

    public void UpdateUserProfile(string displayName)
    {
        UserProfile userProfile = new UserProfile { DisplayName = displayName };
        myUser.UpdateUserProfileAsync(userProfile).ContinueWith(task => {
            if(task.IsCanceled)
            {
                Debug.LogError("UpdateUserProfileAsync Was Canceled");
            }
            if(task.IsFaulted)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
            }
            Debug.Log("User profile updated successfully.");

        });
    }

}
