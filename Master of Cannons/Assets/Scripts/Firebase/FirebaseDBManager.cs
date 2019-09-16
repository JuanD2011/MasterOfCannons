using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using Delegates;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;


public class FirebaseDBManager : MonoBehaviour
{
    public static FirebaseDBManager DB = null;
    public DatabaseReference dataBaseRef;
    public FirebaseApp app;

    /// <summary>
    /// Save user data in Database (userID, name, email)
    /// </summary>    
    public Action<User> SaveData;

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

        SaveData = (user) => { WriteNewUser(user); };
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

    private void WriteNewUser(User user)
    {
        Debug.Log("Writing new user in database bitch...");
        string json = JsonUtility.ToJson(user);
        dataBaseRef.Child("users").Child(user.userId).SetRawJsonValueAsync(json);
    }

    public void UpdateUsername(string userId, string newUserName)
    {        
        Debug.LogFormat("Ouh yeah updating name to: {0} ", newUserName);
        dataBaseRef.Child("users").Child(userId).Child("username").SetValueAsync(newUserName);
    }

    public async void GetPlayerData(Action<string, string, string> ShowPlayerData)
    {
        if (FirebaseAuthManager.myUser == null) return;

        string userId = FirebaseAuthManager.myUser.UserId;        
        Dictionary<string, string> iDictUser = new Dictionary<string, string>();
        //Dictionary<string, string> iDictUser = new Dictionary<string, string>();
        await dataBaseRef.Child("users").Child(userId).GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                Debug.LogError("Get user data error" + task.Exception);
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    iDictUser.Add(data.Key, data.Value.ToString());
                    print(data);
                }

                Debug.Log("Get User Data Succesful");
            }

        });

        ShowPlayerData(iDictUser["username"], iDictUser["coins"], iDictUser["xp"]);
    }

    public async void SetPlayerData(Action<string, string, string> ShowPlayerData)
    {

        if (FirebaseAuthManager.myUser == null) return;
        string userId = FirebaseAuthManager.myUser.UserId;
        Dictionary<string, string> iDictUser = new Dictionary<string, string>();
        //Dictionary<string, string> iDictUser = new Dictionary<string, string>();
        await dataBaseRef.Child("users").Child(userId).GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                Debug.LogError("Get user data error" + task.Exception);
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    iDictUser.Add(data.Key, data.Value.ToString());
                    print(data);
                }

                Debug.Log("Get User Data Succesful");
            }

        });

        ShowPlayerData(iDictUser["username"], iDictUser["coins"], iDictUser["xp"]);
    }

}
