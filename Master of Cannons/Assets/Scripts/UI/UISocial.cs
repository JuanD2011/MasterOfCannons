//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using Delegates;

//public class UISocial : MonoBehaviour
//{
//    [SerializeField] Button facebookButt;
//    TextMeshProUGUI facebookButtText;
//    public static Action buttonStatusHandler;
//    private Action multiDel;

//    IEnumerator Start()
//    {
//        buttonStatusHandler = ChangeButtonStatus;
//        facebookButtText = facebookButt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
//        yield return new WaitUntil(() => FirebaseAuthManager.CheckDependenciesHandler() && FirebaseAuthManager.facebookLogHandler != null);
//        multiDel = buttonStatusHandler +  FirebaseAuthManager.facebookLogHandler;
//        facebookButt.onClick.AddListener(()=> multiDel.Invoke());
//    }
    

//    private void ChangeButtonStatus()
//    {
//        print("Change button status....");
//        if(FacebookData.CheckLogIn())
//        {
//            print("Has logged... change button");
//            facebookButtText.text = "Sign Out";
//            facebookButt.onClick.RemoveAllListeners();
//            facebookButt.onClick.AddListener(SignOutFacebook);
//        }
//        else
//        {            
//            print("Isnt Logged");
//        }
//    }

//    private void SignOutFacebook()
//    {
//        facebookButtText.text = "FB Sign In";
//        FirebaseAuthManager.signOutHandler.Invoke();
//        facebookButt.onClick.AddListener(()=> multiDel());
//    }

//}
