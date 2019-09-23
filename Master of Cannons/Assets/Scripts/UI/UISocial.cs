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
    public static Action<List<Dictionary<string, string>>> showFriendDataHandler;

    private Action multiDel;
    [SerializeField] Transform friendsContainer;

    IEnumerator Start()
    {
        showFriendDataHandler = ShowFriendData;
        buttonStatusHandler = ChangeButtonStatus;  
        facebookButtText = facebookButt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        yield return new WaitUntil(() => FirebaseAuthManager.CheckDependenciesHandler() && FirebaseAuthManager.facebookLogHandler != null);
        multiDel = buttonStatusHandler +  FirebaseAuthManager.facebookLogHandler;
        facebookButt.onClick.AddListener(()=> multiDel.Invoke());
        //yield return new WaitWhile(() => Facebook.Unity.FB.IsInitialized);
        yield return new WaitForSeconds(2f); //ESTO ES UN MACHETAZO TEMPORAL...
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
    private void ShowFriendData(List<Dictionary<string, string>> friendsData)
    {
        friendsContainer.gameObject.SetActive(true);
        for (int i = 0; i < friendsData.Count; i++)
        {
            friendsContainer.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = friendsData[i]["fbName"];
            friendsContainer.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = friendsData[i]["username"];
            friendsContainer.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = friendsData[i]["coins"];
        }
    }

    public void CloseSocialPanel()
    {
        friendsContainer.gameObject.SetActive(false);
    }

    float coins;
    public void UpdateCoins(float _coins)
    {
        coins += _coins;
        FirebaseDBManager.DB.WriteNewCoins(coins);
        UIPlayerData.showCoins(coins.ToString());
    }

    private void SignOutFacebook()
    {
        facebookButtText.text = "FB Sign In";
        FirebaseAuthManager.signOutHandler.Invoke();
        facebookButt.onClick.AddListener(()=> multiDel());
    }

}
