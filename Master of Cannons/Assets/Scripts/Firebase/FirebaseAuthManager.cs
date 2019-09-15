using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;

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
    bool userExist = false;

    private async void AnonymousSignIn()
    {
        Debug.Log("Sign in into anonymous account...");

        await auth.SignInAnonymouslyAsync().ContinueWith(async task =>
        {
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

            await CheckUserExistance();
            // FirebaseDatabase.DefaultInstance.GetReference("users").Child(myUser.UserId).GetValueAsync().ContinueWith(dbTask => {
            //    if (dbTask.IsFaulted)
            //    {
            //        Debug.LogError("Searching UserID in data base encountered an error: " + dbTask.Exception);
            //    }
            //    else if (dbTask.IsCompleted)
            //    {
            //        DataSnapshot snapshot = dbTask.Result;
            //        Debug.Log("Searching UserID in data base COMPLETED...");
            //    }
            //    bool playerExists = dbTask.Result.Exists;

            //    Debug.LogFormat("Player exists? {0} ", playerExist);
            //});

            if (!userExist)
            {
                Debug.Log("Add New Player To Database booy...");
                User mUser = new User { username = myUser.DisplayName, userId = myUser.UserId, coins = 20, skinAvailability = 0, xp = 0 };
                FirebaseDBManager.DB.SaveData(mUser);
            }

            //Memento.SaveData(mUser);
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                myUser.DisplayName, myUser.UserId);
        });
        
    }

    public async Task CheckUserExistance()
    {
        await FirebaseDatabase.DefaultInstance.GetReference("users").Child(myUser.UserId).GetValueAsync().ContinueWith(dbTask =>
        {
            if (dbTask.IsFaulted)
            {
                Debug.LogError("Searching UserID in data base encountered an error: " + dbTask.Exception);
            }
            else if (dbTask.IsCompleted)
            {
                DataSnapshot snapshot = dbTask.Result;
                Debug.Log("Searching UserID in data base COMPLETED...");
            }
            userExist = dbTask.Result.Exists;
            Debug.LogFormat("Player exists? {0} ", userExist);
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
