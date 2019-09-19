using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Facebook.Unity;

public class FirebaseAuthManager : MonoBehaviour
{
    [SerializeField] Settings settings = null;
    FirebaseAuth auth;
    public static FirebaseUser myUser;
    public static Task updateProfileTask;
    private bool isInitialized = false;
    public bool MarikiSI;

    private IEnumerator Start()
    {        
        yield return new WaitUntil(()=>FirebaseApp.CheckDependencies() == DependencyStatus.Available);
        auth = FirebaseAuth.DefaultInstance;
        AnonymousSignIn();
        //Memento.LoadData(settings);        
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
            if (settings.hasFacebookLinked) FacebookSignIn();
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

            if (!userExist)
            {
                Debug.Log("Add New Player To Database booy...");
                User mUser = new User { username = myUser.DisplayName, userId = myUser.UserId};                
                PlayerInfo playerInfo = new PlayerInfo { coins = 30, skinAvailability = 0, xp = 10 };
                FirebaseDBManager.DB.SaveData(mUser, playerInfo);
            }

            //Memento.SaveData(mUser);
            Debug.LogFormat("User signed in successfully: {0} ({1})", myUser.DisplayName, myUser.UserId);
        });
        
    }

    public void FacebookSignIn()
    {
        Debug.Log("Sign In Into Facebook Account...");
        List<string> permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions, (result) => {
            if(FB.IsLoggedIn)
            {
                AccessToken accesToken = AccessToken.CurrentAccessToken;
                if (!settings.hasFacebookLinked) LinkFacebookAccount(accesToken);
                else FacebookAuthentication(accesToken);
            }
            else
            {
                Debug.Log("Login was cancelled");
            }

        });
        
    }

    async void LinkFacebookAccount(AccessToken accesToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accesToken.TokenString);
        Task auxTask = null;
        await auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task => {

            if (task.IsCanceled)
            {
                auxTask = task;
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                auxTask = task;
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            auxTask = task;
            settings.hasFacebookLinked = true;
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Facebook User LINKED Succesfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

        if(auxTask.IsCompleted) Memento.SaveData(settings);

    }

    void FacebookAuthentication(AccessToken accesToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accesToken.TokenString);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {

            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User FACEBOOK signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
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

    public async static void UpdateUserProfile(string displayName)
    {
       
        UserProfile userProfile = new UserProfile { DisplayName = displayName };
        await myUser.UpdateUserProfileAsync(userProfile).ContinueWith(task => {
            updateProfileTask = task;

            if (task.IsCanceled)
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
