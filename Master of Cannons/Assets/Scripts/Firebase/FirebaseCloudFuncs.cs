using UnityEngine;
using Firebase.Functions;
using Firebase.Extensions;
using System.Collections.Generic;
using Firebase;
using Newtonsoft.Json;
using Delegates;

public class FirebaseCloudFuncs : MonoBehaviour
{
    FirebaseFunctions functions;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    public static Delegates.Action<string, string, string, Action> accountMigrationHandler;
    protected virtual void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

        accountMigrationHandler = AccountMigration;
    }

    protected virtual void InitializeFirebase()
    {
        functions = FirebaseFunctions.DefaultInstance;
    }

    public async void AccountMigration(string oldID, string newUserID, string facebookID, Delegates.Action fbLinkCallback)
    {
        HttpsCallableReference func = functions.GetHttpsCallable("accountMigration");
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["oldID"] = oldID;
        data["newUserID"] = newUserID;
        data["facebookID"] = facebookID;

        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        await func.CallAsync(data).ContinueWithOnMainThread(calltask =>
        {
            if (calltask.IsFaulted)
            {
                Debug.LogWarning("Cloud Function That migrated account Failed" + calltask.Exception);
                return;
            }

            if (calltask.IsCanceled)
            {
                Debug.LogWarning("Cloud Function That migrated account was Canceled");
                return;
            }

            var resultJSON = calltask.Result.Data.ToString();
            print("The Data in JSON is:  " + resultJSON);
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultJSON);
            DataManager.DM.InitializePlayerData(dataDict);
            UIPlayerData.showPlayerData.Invoke(dataDict["username"], dataDict[DataManager.coinsStr], dataDict[DataManager.prestigeStr]);
            fbLinkCallback.Invoke();
            Debug.Log("Task was completed");
        });      
    }
}
