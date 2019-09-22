﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Facebook.Unity;
using Delegates;

public class FirebaseAuthManager : MonoBehaviour
{
    FirebaseAuth auth;
    public static FirebaseUser myUser;
    public static Task updateProfileTask;
    public static Action facebookLogHandler;
    public static Action signOutHandler;
    public static Func<bool> CheckDependenciesHandler = () => { return FirebaseApp.CheckDependencies() == DependencyStatus.Available; };

    private bool isInitialized = false;
    private bool userExist = false;

    private IEnumerator Start()
    {
        //Memento.ClearData(DataManager.DM.settings);
        yield return new WaitUntil(()=> CheckDependenciesHandler.Invoke());
        auth = FirebaseAuth.DefaultInstance;
        AnonymousSignIn();  
        signOutHandler = delegate () { FB.LogOut(); };
        facebookLogHandler = FacebookSignIn;
        auth.StateChanged += AuthStateChanged;        
        AuthStateChanged(this, null);
        FB.Init(InitCallBack, OnHideUnity);        
    }

    void InitCallBack()
    {
        if(FB.IsInitialized)
        {
            FB.ActivateApp();
            isInitialized = true;
            Debug.Log("Facebook Initialized");
            UISocial.buttonStatusHandler?.Invoke();
            if (FB.IsLoggedIn)
            {
                print("Already has logged in");
                return;
            }
        }
        else
        {
            Debug.LogError("Failed to Initialize Facebook");
        }
    }

    void OnHideUnity(bool isUnityShown)
    {
        if (isUnityShown) FB.ActivateApp();
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != myUser)
        {
            bool signedIn = myUser != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && myUser != null)
            {
                Debug.Log("Signed out " + myUser.UserId);
            }
            myUser = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + myUser.UserId);
            }
        }
    }

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

            if (!userExist)
            {
                Debug.Log("Add New Player To Database booy...");
                User mUser = new User { username = myUser.DisplayName, userId = myUser.UserId};                
                PlayerInfo playerInfo = new PlayerInfo { coins = 30, skinAvailability = 0, xp = 10 };
                FirebaseDBManager.DB.WriteNewUserHandler(mUser, playerInfo);
            }

            Debug.LogFormat("User signed in successfully: {0} ({1})", myUser.DisplayName, myUser.UserId);
        });
        
    }

    public void FacebookSignIn()
    {
        if (FB.IsLoggedIn)
        {
            print("Already has logged in");
            return;
        }
        Debug.Log("Sign In Into Facebook Account...");
        List<string> permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions, (result) => {
            if(FB.IsLoggedIn)
            {
                AccessToken accesToken = AccessToken.CurrentAccessToken;
                if (!DataManager.DM.settings.hasFacebookLinked) LinkFacebookAccount(accesToken);
                //else FacebookAuthentication(accesToken);
                print("Login Facebook Succesfully");
                UISocial.buttonStatusHandler.Invoke();

            }
            else
            {
                Debug.Log("Login was cancelled");
            }               
        });
        
    }

    async void LinkFacebookAccount(AccessToken accesToken)
    {
        print("link facebook ");
        Credential credential = FacebookAuthProvider.GetCredential(accesToken.TokenString);
        Task auxTask = null;
        await auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task => {

            if (task.IsCanceled)
            {
                auxTask = task;
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            else if (task.IsFaulted)
            {
                auxTask = task;
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            auxTask = task;
            DataManager.DM.settings.hasFacebookLinked = true;          
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Facebook User LINKED to FIREBASE Succesfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

        UISocial.buttonStatusHandler.Invoke();
        if (auxTask.IsCompleted)
        {
            FirebaseDBManager.DB.WriteFacebookUserHandler.Invoke(AccessToken.CurrentAccessToken.UserId);
            Memento.SaveData(DataManager.DM.settings);
        }

    }

    async void FacebookAuthentication(AccessToken accesToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accesToken.TokenString);
        await auth.SignInWithCredentialAsync(credential).ContinueWith(task => {

            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;         
            Debug.LogFormat("User FACEBOOK signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

        UISocial.buttonStatusHandler.Invoke();
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

    public async static void UpdateUserProfile(string displayName)
    {
       
        UserProfile userProfile = new UserProfile { DisplayName = displayName };
        await myUser.UpdateUserProfileAsync(userProfile).ContinueWith(task => {
            updateProfileTask = task;

            if (task.IsCanceled)
            {
                Debug.LogError("UpdateUserProfileAsync Was Canceled");
            }
            else if(task.IsFaulted)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
            }

            
            Debug.Log("User profile updated successfully.");

        });
    }

}
