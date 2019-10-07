using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Facebook.Unity;
using Delegates;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using Firebase.Extensions;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuth auth;
    public static FirebaseUser myUser;
    public static Task updateProfileTask;
    public static Action facebookLogHandler;
    public static Action playGamesLogHandler;
    public static Action signOutFBHandler;
    public static Action signOutPlayGamesHandler;

    public static Func<bool> CheckDependenciesHandler = () => { return FirebaseApp.CheckDependencies() == DependencyStatus.Available; };
    private bool userExist = false;

    public static Action<AccessToken> facebookAuthenticationNoLinked;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => CheckDependenciesHandler.Invoke());

        auth = FirebaseAuth.DefaultInstance;
        Memento.LoadData(DataManager.DM.settings);
        if (DataManager.DM.settings.defaultScene == 0)
        {

            AnonymousSignIn();
        }
        signOutFBHandler = delegate () { FB.LogOut(); };
        signOutPlayGamesHandler = delegate () { PlayGamesPlatform.Instance.SignOut(); };

        facebookLogHandler = FacebookSignIn;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        FB.Init(InitCallBack, OnHideUnity);
        facebookAuthenticationNoLinked = UnlinkAndDeleteFBAccount;

        //playGamesLogHandler = PlayGamesSignIn;
        //InitializePlayGames();

    }

    void InitializePlayGames()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .RequestServerAuthCode(false)//Force refresh
        .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Debug.LogFormat("Play Games Configuration initialized");
    }

    void InitCallBack()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("Facebook Initialized");
            if (FB.IsLoggedIn)
            {
                UISocial.fbButtonStatus?.Invoke();
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
                
                if (DataManager.DM.settings.hasFacebookLinked) //That means that the user changed his account
                {
                    Debug.Log("USER WAS UNLINKED BY ANOTHER ACCOUNT");
                    DataManager.DM.settings.defaultScene = 0;
                    DataManager.DM.settings.hasFacebookLinked = false;
                    Memento.SaveData(DataManager.DM.settings);
                }
            }
            myUser = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + myUser.UserId);
            }
        }
    }



    private async Task AnonymousSignIn()
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
            bool userExist = await CheckUserExistance();

            if (!userExist)
            {
                Debug.Log("Add New Player To Database booy...");
                User mUser = new User { username = myUser.DisplayName, userID = myUser.UserId };
                PlayerInfo playerInfo = new PlayerInfo { coins = 0, skinAvailability = 0, prestige = 10 };
                FirebaseDBManager.DB.WriteNewUserHandler(mUser, playerInfo);
            }

            Debug.LogFormat("User signed in successfully: {0} ({1})", myUser.DisplayName, myUser.UserId);
        });

    }

    #region Facebook Authentication

    public void FacebookSignIn()
    {
        if (FB.IsLoggedIn)
        {
            print("Already has logged in");
            return;
        }

        Debug.Log("Sign In Into Facebook Account...");
        List<string> permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions, async result => {
            if (FB.IsLoggedIn)
            {
                AccessToken accesToken = AccessToken.CurrentAccessToken;
                bool? fbUserExists = await CheckFBUserExistance(accesToken);
                //FacebookAuthentication(accesToken);
                if (!DataManager.DM.settings.hasFacebookLinked && fbUserExists == null) LinkFacebookAccount(accesToken);
                if (fbUserExists == true)
                {
                    print("fb exists papi");
                    MenuManager.popUpHandler.Invoke("This account is linked to another account, do you want to link it to this account?", ()=> UnlinkAndDeleteFBAccount(accesToken));
                };

                print("Login Facebook Succesfully");
                UISocial.fbButtonStatus.Invoke();

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
        await auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWithOnMainThread(task => {

            if (task.IsCanceled)
            {
                auxTask = task;
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            else if (task.IsFaulted)
            {
                auxTask = task;
                MenuManager.popUpHandler.Invoke("This account is linked to another account, do you want to link it to this account?", () => UnlinkAndDeleteFBAccount(accesToken));
                Debug.LogWarning("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            auxTask = task;
            DataManager.DM.settings.hasFacebookLinked = true;
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Facebook User LINKED to FIREBASE Succesfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
        if (auxTask.IsFaulted) return;

        UISocial.fbButtonStatus.Invoke();
        if (auxTask.IsCompleted)
        {
            Memento.SaveData(DataManager.DM.settings);

            string facebookName = await FacebookData.GetMyName();
            FirebaseDBManager.DB.WriteFacebookUserHandler.Invoke(AccessToken.CurrentAccessToken.UserId, facebookName);
        }

    }


    async void UnlinkAndDeleteFBAccount(AccessToken accesToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accesToken.TokenString);

        string currentUserID = auth.CurrentUser.UserId;
        await auth.CurrentUser.DeleteAsync().ContinueWith(task=> { //Delete Current Account
            
            if(task.IsCompleted)
            {
                FirebaseDBManager.DB.DeleteUser(currentUserID);
                Debug.Log("Account Deleted Succesful");
            }
        });

        await auth.SignInWithCredentialAsync(credential).ContinueWith(task => //Log in into account that has facebook linked
        {
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

        currentUserID = auth.CurrentUser.UserId;

        await auth.CurrentUser.DeleteAsync().ContinueWith(task => { //Delete account that has facebook linked

            if (task.IsFaulted)
            {
                Debug.LogError("DELETE user was failed...");
                return;
            }
            else if (task.IsCanceled)
            {
                Debug.Log("DELETE user was canceled...");
                return;
            }
            FirebaseDBManager.DB.DeleteUser(currentUserID);
            Debug.Log("DELETE user Succesful");

        });

        //Create new account and link to facebook
        await AnonymousSignIn();
        LinkFacebookAccount(accesToken);
    }
    #endregion

    #region Google Play Games

    void PlayGamesSignIn()
    {
        Social.localUser.Authenticate((bool success) => {

            if (!success)
            {
                Debug.LogError("Failed to Sign into Play Games Services.");
                return;
            }
            else
            {
                Debug.Log("Signed Succesful into Play Games");
            }

            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

            if (string.IsNullOrEmpty(authCode))
            {
                Debug.LogError("Signed into Play Games Services but failed to get auth code.");
                return;
            }


            Debug.LogFormat("Google Auth code: {0}", authCode);

            Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
            LinkPlayGamesAccount(credential);

        });
    }

    void LinkPlayGamesAccount(Credential credential)
    {
        auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task => {

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
            Debug.LogFormat("GOOGLE User LINKED to FIREBASE Succesfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });
    }

    #endregion

    public async Task<bool> CheckUserExistance()
    {
        bool userExist = true;
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
            return userExist;
        });

        return userExist;
    }

    public async Task<bool?> CheckFBUserExistance(AccessToken accesToken)
    {
        bool? fbUserExists = null;
        if (!DataManager.DM.settings.hasFacebookLinked) return fbUserExists;

        await FirebaseDatabase.DefaultInstance.GetReference("facebook users").Child(accesToken.UserId).Child("userID").GetValueAsync().ContinueWith(dbTask =>
        {
            if (dbTask.IsFaulted)
            {
                Debug.LogError("Searching UserID in data base encountered an error: " + dbTask.Exception);
                fbUserExists = null;
                return null;
            }
            else if (dbTask.IsCanceled)
            {
                Debug.Log("Searching UserID in data base was CANCELED");
            }

            string userID = dbTask.Result.Value?.ToString();
            Debug.LogFormat("Facebook USER exists? {0} ", dbTask.Result.Exists);
            if (userID != auth.CurrentUser.UserId && userID != string.Empty)
            {
                fbUserExists = true;
                return fbUserExists;
            }
            else
            {
                fbUserExists = false;
                return fbUserExists;
            }

        });

        return fbUserExists;
    }

    async void UnlinkAndLink(string facebookId)
    {
        string userID = string.Empty;

        await FirebaseDBManager.DB.dataBaseRef.Child("facebook users").Child(facebookId).Child("userID").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Unlink and Link Account Failed:  " + task.Exception);
                return;
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Unlink and Link Account was canceled.");
                return;
            }

            DataSnapshot data = task.Result;
            userID = data.Value.ToString();
            print("user ID is" + userID);
        });


    }

    public async static Task UpdateUserProfile(string displayName)
    {
        UserProfile userProfile = new UserProfile { DisplayName = displayName };
        await myUser.UpdateUserProfileAsync(userProfile).ContinueWith(task => {
            updateProfileTask = task;

            if (task.IsCanceled)
            {
                Debug.LogError("UpdateUserProfileAsync Was Canceled");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
            }


            Debug.Log("User profile updated successfully.");

        });
    }

}