using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class FirstTimeInGame : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] UnityEngine.UI.Button confirmNameButt;

    private void Start()
    {
        confirmNameButt.onClick.AddListener(() => {FirebaseDBManager.DB.UpdateUserName(FirebaseAuthManager.myUser.UserId, nameInput.text); SceneManager.LoadScene(0); });
    }

}
