using UnityEngine;
//using Firebase.Functions;
using Firebase.Extensions;
using System.Collections.Generic;
using Firebase;
using System.Collections;

public class FirebaseCloudFuncs : MonoBehaviour
{
    //FirebaseFunctions functions;
    //DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected virtual void Start()
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        //    dependencyStatus = task.Result;
        //    if (dependencyStatus == DependencyStatus.Available)
        //    {
        //        InitializeFirebase();
        //    }
        //    else
        //    {
        //        Debug.LogError(
        //          "Could not resolve all Firebase dependencies: " + dependencyStatus);
        //    }
        //});
    }

    protected virtual void InitializeFirebase()
    {
        //functions = FirebaseFunctions.DefaultInstance;

        // To use a local emulator, uncomment this line:
        //   functions.UseFunctionsEmulator("http://localhost:5005");
        // Or from an Android emulator:
        //   functions.UseFunctionsEmulator("http://10.0.2.2:5005");
    }

    //public void Llame()
    //{
    //    StartCoroutine(AccountMigration());
    //}

    //public IEnumerator AccountMigration()
    //{
    //    HttpsCallableReference func = functions.GetHttpsCallable("accountMigrate");
    //    Dictionary<string, object> data = new Dictionary<string, object>();
    //    data["userID"] = FirebaseAuthManager.myUser.UserId;
    //    var task = func.CallAsync(data).ContinueWithOnMainThread(calltask =>
    //    {

    //        if (calltask.IsFaulted)
    //        {
    //            Debug.LogWarning("Cloud Function That migrated account Failed" + calltask.Exception);
    //            return;
    //        }

    //        if (calltask.IsCanceled)
    //        {
    //            Debug.LogWarning("Cloud Function That migrated account was Canceled");
    //            return;
    //        }

    //        Debug.Log("Task was completed");
    //    });
    //    yield return new WaitUntil(() => task.IsCompleted);
    //}

}
