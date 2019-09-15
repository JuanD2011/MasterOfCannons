using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using Delegates;

public class FirebaseDBManager : MonoBehaviour
{
    public static FirebaseDBManager DB = null;
    public DatabaseReference dataBaseRef;  

    /// <summary>
    /// Save user data in Database (userID, name, email)
    /// </summary>    
    public Action<string, string, string> SaveData;

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

        SaveData = (userId, name, email) => { WriteNewUser(userId, name, email); };
        UpdateUserName = UpdateUsername;

        InitializeDB();
    }

    void InitializeDB()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                print("Ready to use FIREBASE...");

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }


    private void WriteNewUser(string userId, string name, string email)
    {
        Debug.Log("Writing new user in database bitch...");
        User user = new User { username = name, email = email };
        string json = JsonUtility.ToJson(user);
        dataBaseRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void UpdateUsername(string userId, string newUserName)
    {
        Debug.Log(string.Format("Ouh yeah updating name to: {0} ", newUserName));
        dataBaseRef.Child("users").Child(userId).Child("username").SetValueAsync(newUserName);
    }
}
