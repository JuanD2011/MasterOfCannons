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

    public Action<string> WriteFacebookUserHandler;

    /// <summary>
    /// Update user name (userID, new username)
    /// </summary>
    public Action<string, string> UpdateUserName;

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
        //if (DataManager.DM.settings.hasFacebookLinked) WriteFacebookUserData();

    }

    public void UpdateUsername(string userId, string newUserName)
    {        
        Debug.LogFormat("Ouh yeah updating name to: {0} ", newUserName);
        FirebaseAuthManager.UpdateUserProfile(newUserName);
        dataBaseRef.Child("users").Child(userId).Child("username").SetValueAsync(newUserName);
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

    private async void WriteFacebookUserData(string facebookID)
    {
        string json = await GetPlayerDataAsJSON();
        await dataBaseRef.Child("facebook users").Child(facebookID).SetRawJsonValueAsync(json);
    }

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


    // For testing Database
    private float fumarato = 20;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
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


