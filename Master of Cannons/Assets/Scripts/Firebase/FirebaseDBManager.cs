using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using Delegates;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;


public class FirebaseDBManager : MonoBehaviour
{
    public static FirebaseDBManager DB = null;
    public DatabaseReference dataBaseRef;
    public FirebaseApp app;

    /// <summary>
    /// Save user data in Database (userID, name, email)
    /// </summary>    
    public Action<User, PlayerInfo> WriteNewUserHandler;

    public Action<string, string> WriteFacebookUserHandler;

    public Action<float, float, int> WriteNewInfo;
    /// <summary>
    /// Update user name (userID, new username)
    /// </summary>
    public Func<string, string, Task> UpdateUserName;

    private void Awake()
    {
        if (DB == null) DB = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {        
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://master-of-cannons.firebaseio.com/");

        // Get the root reference location of the database.
        dataBaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        WriteNewUserHandler = WriteNewUser;
        WriteFacebookUserHandler = WriteFacebookUserData;
        WriteNewInfo = WriteNewPlayerInfo;

        UpdateUserName = UpdateUsername;        
        InitializeDB();
    }

    void InitializeDB()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                print("Ready to use FIREBASE...");
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    private void WriteNewUser(User user, PlayerInfo playerInfo)
    {
        Debug.Log("Writing new user in database bitch...");
        string userJson = JsonUtility.ToJson(user);
        string playerInfoJson = JsonUtility.ToJson(playerInfo);
        dataBaseRef.Child("users").Child(user.userId).SetRawJsonValueAsync(userJson);
        dataBaseRef.Child("player info").Child(user.userId).SetRawJsonValueAsync(playerInfoJson);
    }

    public async Task UpdateUsername(string userId, string newUserName)
    {        
        Debug.LogFormat("Ouh yeah updating name to: {0} ", newUserName);
        await FirebaseAuthManager.UpdateUserProfile(newUserName);
        await dataBaseRef.Child("users").Child(userId).Child("username").SetValueAsync(newUserName);
    }

    public async void GetPlayerData(Action<string, string, string> ShowPlayerData)
    {
        if (FirebaseAuthManager.myUser == null) return;

        string userId = FirebaseAuthManager.myUser.UserId;        
        Dictionary<string, string> iDictUser = new Dictionary<string, string>();
        await dataBaseRef.Child("player info").Child(userId).GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                Debug.LogError("Get Player data error" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    iDictUser.Add(data.Key, data.Value.ToString());
                    print(data);
                }

                Debug.Log("Get Player Data Succesful");
            }

        });
      
        ShowPlayerData(FirebaseAuthManager.myUser.DisplayName, iDictUser["coins"], iDictUser["xp"]);
    }

    public async Task<Dictionary<string, string>> GetFacebookUserData(string userID)
    {
        if (FirebaseAuthManager.myUser == null) return null;

        string userId = FirebaseAuthManager.myUser.UserId;
        Dictionary<string, string> iDictUser = new Dictionary<string, string>();
        await dataBaseRef.Child("facebook users").Child(userID).GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                Debug.LogError("Get Player data error" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    iDictUser.Add(data.Key, data.Value.ToString());
                    print(data);
                }

                Debug.Log("Get Player Data Succesful");
            }

        });

        UISocial.showFriendDataHandler(iDictUser["fbName"], iDictUser["username"], iDictUser["coins"]);

        return iDictUser;        
    }

    private async void WriteFacebookUserData(string facebookID, string name)
    {
        string username = string.Empty;
        string coins = string.Empty;
        await dataBaseRef.Child("player info").Child(FirebaseAuthManager.myUser.UserId).Child("coins").GetValueAsync().ContinueWith(task=> {

            if(task.IsCompleted)
            {
                DataSnapshot data = task.Result;
                coins = data.Value.ToString();
            }

        });
        await dataBaseRef.Child("users").Child(FirebaseAuthManager.myUser.UserId).Child("username").GetValueAsync().ContinueWith(task=> {
            if(task.IsCompleted)
            {
                DataSnapshot data = task.Result;
                username = data.Value.ToString();
            }

        });
        
        FacebookUser fbUser = new FacebookUser { fbName = name, username = username, coins = coins };
        string json = JsonUtility.ToJson(fbUser);
        await dataBaseRef.Child("facebook users").Child(facebookID).SetRawJsonValueAsync(json);        
    }

    //private async void WriteFacebookUserData(string facebookID, string name)
    //{
    //    string json = await GetPlayerDataAsJSON();
    //    await dataBaseRef.Child("facebook users").Child(facebookID).SetRawJsonValueAsync(json);
    //    await dataBaseRef.Child("facebook users").Child(facebookID).Child("name").SetValueAsync(name);
    //}

    private void WriteNewPlayerInfo(float _coins, float _xp, int _skinAvailability)
    {
        string key = dataBaseRef.Child("player info").Child(FirebaseAuthManager.myUser.UserId).Key;
        PlayerInfo playerInfo = new PlayerInfo { coins = _coins, xp = _xp, skinAvailability = _skinAvailability };
        Dictionary<string, System.Object> entryValues = playerInfo.ToDictionary();

        Dictionary<string, System.Object> childUpdates = new Dictionary<string, object>();
        childUpdates[string.Format("/users/{0}/{1}", FirebaseAuthManager.myUser.UserId, key)] = entryValues;

        dataBaseRef.UpdateChildrenAsync(childUpdates);
        Debug.LogFormat("Writing New Data Succesful, Coins: {0}, Xp: {1}, SkinAvailability: {2} ", _coins, _xp, _skinAvailability);
        UIPlayerData.showPlayerData(FirebaseAuthManager.myUser.DisplayName, _coins.ToString(), _xp.ToString());
    }

    public void WriteNewCoins(float _coins)
    {
        //dataBaseRef.Child("player info").Child(FirebaseAuthManager.myUser.UserId).Child("coins").ValueChanged += HandleValueChanged;
        dataBaseRef.Child("player info").Child(FirebaseAuthManager.myUser.UserId).Child("coins").SetValueAsync(_coins);
        if (DataManager.DM.settings.hasFacebookLinked) dataBaseRef.Child("facebook users").Child(Facebook.Unity.AccessToken.CurrentAccessToken.UserId).Child("coins").SetValueAsync(_coins);
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs e)
    {

    }

    public async Task<string> GetPlayerDataAsJSON()
    {
        string json = string.Empty;
        await dataBaseRef.Child("player info").Child(FirebaseAuthManager.myUser.UserId).GetValueAsync().ContinueWith(task => {

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                json = snapshot.GetRawJsonValue();
                Debug.Log("Get Player Data As JSON Succesful");
            }

        });

        return json;
    }
}


