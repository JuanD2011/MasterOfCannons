using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Delegates;

public class UISocial : MonoBehaviour
{
    [SerializeField] Button facebookButt = null;
    [SerializeField] Button playGamesButt = null;
    [SerializeField] Button authSignOutButt = null;
    [SerializeField] Button deleteUserButt = null;

    TextMeshProUGUI facebookButtText;
    TextMeshProUGUI playGamesButtText;

    public static Action fbButtonStatus;
    public static Action playGamesButtonStatus;
    public static Action<List<Dictionary<string, string>>> showFriendDataHandler;

    private Action onClickFBButton, onClickPlayGamesButton, onClickSignOut, onDeleteUser;
    [SerializeField] Transform friendsContainer = null;
    int index = 0;

    public static Action hideFBFriends;


    IEnumerator Start()
    {
        onDeleteUser = DeleteUser;
        onClickSignOut = FirebaseSignOut;
        showFriendDataHandler = ShowFriendData;
        playGamesButtonStatus = ChangePlayGamesStatus;
        fbButtonStatus = ChangeFBButtonStatus;
        hideFBFriends = ()=> friendsContainer.gameObject.SetActive(false);

        facebookButtText = facebookButt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playGamesButtText = playGamesButt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        yield return new WaitUntil(() => FirebaseAuthManager.CheckDependenciesHandler() && FirebaseAuthManager.facebookLogHandler != null);

        onClickFBButton = fbButtonStatus +  FirebaseAuthManager.facebookLogHandler;
        facebookButt.onClick.AddListener(()=> onClickFBButton.Invoke());
        
        onClickPlayGamesButton = playGamesButtonStatus + FirebaseAuthManager.playGamesLogHandler;
        playGamesButt.onClick.AddListener(() => onClickPlayGamesButton.Invoke());

        authSignOutButt.onClick.AddListener(() => onClickSignOut.Invoke());
        deleteUserButt.onClick.AddListener(() => onDeleteUser.Invoke());

        yield return new WaitWhile(() => Facebook.Unity.FB.IsInitialized);        
        //yield return new WaitForSeconds(2f); //ESTO ES UN MACHETAZO TEMPORAL...
        fbButtonStatus.Invoke();

        //yield return new WaitWhile(() => GooglePlayGames.PlayGamesPlatform.Instance == null);        
        //playGamesButtonStatus.Invoke();
    }
    
    private void DeleteUser()
    {
        FirebaseAuthManager.auth.CurrentUser.DeleteAsync().ContinueWith(task=> {
            if (task.IsCompleted)
            {
                Debug.Log("USER DELETEADO PAPI");
            }
        });
    }
    private void FirebaseSignOut()
    {
        FirebaseAuthManager.auth.CurrentUser.DeleteAsync();
    }
    private void ChangePlayGamesStatus()
    {
        FirebaseAuthManager.playGamesLogHandler.Invoke();
        if(Social.localUser.authenticated)
        {
            playGamesButtText.text = "Sign Out";
            facebookButt.onClick.RemoveAllListeners();
            facebookButt.onClick.AddListener(SignOutPlayGames);
        } 
    }
    private void SignOutPlayGames()
    {
        playGamesButtText.text = "Play Games Sign IN";
        FirebaseAuthManager.signOutPlayGamesHandler.Invoke();
        playGamesButt.onClick.AddListener(() => onClickPlayGamesButton());
    }
    private void ChangeFBButtonStatus()
    {
        print("Change Facebook button status....");
        if(FacebookData.CheckLogIn())
        {
            print("Has logged... change Facebook button");
            facebookButtText.text = "Sign Out";
            facebookButt.onClick.RemoveAllListeners();
            facebookButt.onClick.AddListener(SignOutFacebook);
        }
        else
        {            
            print("Isnt Logged");
        }
    }
    private void SignOutFacebook()
    {
        facebookButtText.text = "FB Sign In";
        FirebaseAuthManager.signOutFBHandler.Invoke();
        facebookButt.onClick.AddListener(() => onClickFBButton());
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
    //private void ShowFriendData(string name, string nickname, string coins)
    //{        
    //    friendsContainer.GetChild(index).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    //    friendsContainer.GetChild(index).GetChild(1).GetComponent<TextMeshProUGUI>().text = nickname;
    //    friendsContainer.GetChild(index).GetChild(2).GetComponent<TextMeshProUGUI>().text = coins;
    //    index++;
    //}

    public void CloseSocialPanel()
    {
        hideFBFriends.Invoke();
    }
}
