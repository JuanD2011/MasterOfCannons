using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using Delegates;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;


public class FirebaseDBManager : MonoBehaviour
{
    public static FirebaseDBManager DB = null;
    public DatabaseReference dataBaseRef;
    public FirebaseApp app;

    /// <summary>
    /// Save user data in Database (userID, name, email)
    /// </summary>    
    public Action<User, PlayerInfo, DailyGifts> WriteNewUserHandler;

    //public Action<string, string> WriteFacebookUserHandler;
    public Func<string, string, Task> WriteFacebookUserHandler;

    public Action<int, float, int> WriteNewInfo;
    /// <summary>
    /// Update user name (userID, new username)
    /// </summary>
    public Func<string, string, Task> UpdateUserName;

    public float TimeStamp { get => float.Parse(ServerValue.Timestamp.ToString()); set => TimeStamp = value; }

    private void Awake()
    {        
        if (DB == null) DB = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://master-of-cannons-ef77a.firebaseio.com/");
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://master-of-cannons.firebaseio.com/");

        // Get the root reference location of the database.
        dataBaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        print(ServerValue.Timestamp);
        WriteNewUserHandler = WriteNewUser;
        WriteFacebookUserHandler = WriteNewFacebookUserData;
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

    private void WriteNewUser(User user, PlayerInfo playerInfo, DailyGifts giftInfo)
    {
        Debug.Log("Writing new user in database bitch...");
        string userJson = JsonUtility.ToJson(user);
        string playerInfoJson = JsonUtility.ToJson(playerInfo);
        //string dailyGiftsJson = JsonUtility.ToJson(giftInfo);

        dataBaseRef.Child("users").Child(user.userID).SetRawJsonValueAsync(userJson);
        dataBaseRef.Child("player info").Child(user.userID).SetRawJsonValueAsync(playerInfoJson);
        dataBaseRef.Child("gifts info").Child(user.userID).Child(DataManager.timeChestWasOpenedStr).SetValueAsync(ServerValue.Timestamp);
        //dataBaseRef.Child("gifts info").Child(user.userID).SetRawJsonValueAsync(dailyGiftsJson);
    }

    public async Task UpdateUsername(string userId, string newUserName)
    {        
        Debug.LogFormat("Ouh yeah updating name to: {0} ", newUserName);
        await FirebaseAuthManager.UpdateUserProfile(newUserName);
        await dataBaseRef.Child("users").Child(userId).Child("username").SetValueAsync(newUserName);
    }

    public async Task GetPlayerData(Action<string, string, string> ShowPlayerData)
    {
        if (FirebaseAuthManager.myUser == null) return;
        string username = string.Empty;
        string userID = FirebaseAuthManager.myUser.UserId;        
        Dictionary<string, string> iDictUser = new Dictionary<string, string>();
        await dataBaseRef.Child("player info").Child(userID).GetValueAsync().ContinueWith(task => {

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

                Debug.Log("Get Player Data Successful");
            }

        });

        username = await GetUsername(userID);
        ShowPlayerData?.Invoke(username, iDictUser[DataManager.coinsStr], iDictUser[DataManager.prestigeStr]);
        //ShowPlayerData?.Invoke(FirebaseAuthManager.myUser.DisplayName, iDictUser[DataManager.coinsStr], iDictUser[DataManager.prestigeStr]);
        DataManager.DM.InitializePlayerData(iDictUser);
    }

    public async Task<string> GetUsername(string userID)
    {
        string username = string.Empty;
        await dataBaseRef.Child("users").Child(userID).Child("username").GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                Debug.LogError("Get USERNAME data error" + task.Exception);
                return;
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Get Player USERNAME Successful" + task.Result.Value.ToString());
                username = task.Result.Value.ToString();
            }
        });

        return username;
    }


    public async Task<Dictionary<string, string>> GetFacebookUserData(string userID)
    {
        if (FirebaseAuthManager.myUser == null) return null;

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
                    //Debug.Log(data);
                }

                Debug.Log("Get Player Data Successful");
            }

        });

        //UISocial.showFriendDataHandler(iDictUser["fbName"], iDictUser["username"], iDictUser["coins"]);
        return iDictUser;        
    }

    private async Task WriteNewFacebookUserData(string facebookID, string name)
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
        
        FacebookUser fbUser = new FacebookUser { fbName = name, username = username, coins = coins, userID = FirebaseAuthManager.myUser.UserId };
        string json = JsonUtility.ToJson(fbUser);
        await dataBaseRef.Child("facebook users").Child(facebookID).SetRawJsonValueAsync(json);        
    }

    private void WriteNewPlayerInfo(int _coins, float prestige, int _skinAvailability)
    {
        string key = dataBaseRef.Child("player info").Child(FirebaseAuthManager.myUser.UserId).Key;
        PlayerInfo playerInfo = new PlayerInfo { coins = _coins, prestige = prestige, skinAvailability = _skinAvailability };
        Dictionary<string, System.Object> entryValues = playerInfo.ToDictionary();

        Dictionary<string, System.Object> childUpdates = new Dictionary<string, object>();
        childUpdates[string.Format("/users/{0}/{1}", FirebaseAuthManager.myUser.UserId, key)] = entryValues;

        dataBaseRef.UpdateChildrenAsync(childUpdates);
        Debug.LogFormat("Writing New Data Successful, Coins: {0}, Xp: {1}, SkinAvailability: {2} ", _coins, prestige, _skinAvailability);
        UIPlayerData.showPlayerData(FirebaseAuthManager.myUser.DisplayName, _coins.ToString(), prestige.ToString());
    }

    public void WriteNewCoins(int _coins)
    {
        //dataBaseRef.Child("player info").Child(FirebaseAuthManager.myUser.UserId).Child("coins").ValueChanged += HandleValueChanged;
        dataBaseRef.Child("player info").Child(FirebaseAuthManager.myUser.UserId).Child("coins").SetValueAsync(_coins);
        if (DataManager.DM.settings.hasFacebookLinked && Facebook.Unity.AccessToken.CurrentAccessToken != null)
            dataBaseRef.Child("facebook users").Child(Facebook.Unity.AccessToken.CurrentAccessToken.UserId).Child("coins").SetValueAsync(_coins);
    }

    public async Task<Dictionary<string,string>> GetGiftsInfo(string userID)
    {
        string serverTime = string.Empty;
        Dictionary<string, string> giftsInfoDict = new Dictionary<string, string>();
        await dataBaseRef.Child("gifts info").Child(userID).Child("current time").SetValueAsync(ServerValue.Timestamp);
        await dataBaseRef.Child("gifts info").Child(userID).Child("current time").GetValueAsync().ContinueWith(task=>{

            if(task.IsCompleted)
                serverTime = task.Result.Value.ToString();

        });
        
        await dataBaseRef.Child("gifts info").Child(userID).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach(DataSnapshot data in snapshot.Children)
                    giftsInfoDict.Add(data.Key, data.Value.ToString());
            }
        });
      
        return giftsInfoDict;
    }

    public async void UpdateCoinsGiftInfo(Action updateUI)
    {
        await dataBaseRef.Child("gifts info").Child(FirebaseAuthManager.myUser.UserId).Child(DataManager.timeChestWasOpenedStr).SetValueAsync(ServerValue.Timestamp);
        updateUI.Invoke();
    }

    public async Task<string> GetDataAsJSON(string databaseBranch ,string _userID)
    {
        string json = string.Empty;
        await dataBaseRef.Child(databaseBranch).Child(_userID).GetValueAsync().ContinueWith(task => {

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


