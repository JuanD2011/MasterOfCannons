using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Delegates;

public class UISocial : MonoBehaviour
{
    [SerializeField] Button facebookButt;
    TextMeshProUGUI facebookButtText;
    public static Action buttonStatusHandler;
    public static Action<string, string, string> showFriendDataHandler;

    private Action multiDel;
    [SerializeField] Transform friendsContainer;


    float coins;
    int index = 0;

    IEnumerator Start()
    {
        showFriendDataHandler = ShowFriendData;
        buttonStatusHandler = ChangeButtonStatus;  
        facebookButtText = facebookButt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        yield return new WaitUntil(() => FirebaseAuthManager.CheckDependenciesHandler() && FirebaseAuthManager.facebookLogHandler != null);

        multiDel = buttonStatusHandler +  FirebaseAuthManager.facebookLogHandler;
        facebookButt.onClick.AddListener(()=> multiDel.Invoke());

        yield return new WaitWhile(() => Facebook.Unity.FB.IsInitialized);
        //yield return new WaitForSeconds(2f); //ESTO ES UN MACHETAZO TEMPORAL...
        buttonStatusHandler();
    }
    
    private void ChangeButtonStatus()
    {
        print("Change button status....");
        if(FacebookData.CheckLogIn())
        {
            print("Has logged... change button");
            facebookButtText.text = "Sign Out";
            facebookButt.onClick.RemoveAllListeners();
            facebookButt.onClick.AddListener(SignOutFacebook);
        }
        else
        {            
            print("Isnt Logged");
        }
    }
    private void ShowFriendData(string name, string nickname, string coins)
    {        
        friendsContainer.GetChild(index).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        friendsContainer.GetChild(index).GetChild(1).GetComponent<TextMeshProUGUI>().text = nickname;
        friendsContainer.GetChild(index).GetChild(2).GetComponent<TextMeshProUGUI>().text = coins;
        index++;
    }

    public void CloseSocialPanel() => index = 0;

    public void UpdateCoins(float _coins)
    {
        coins += _coins;
        UIPlayerData.showCoins(coins.ToString());
        FirebaseDBManager.DB.WriteNewCoins(coins);
    }

    private void SignOutFacebook()
    {
        facebookButtText.text = "FB Sign In";
        FirebaseAuthManager.signOutHandler.Invoke();
        facebookButt.onClick.AddListener(()=> multiDel());
    }

}
