using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections;

public class FirstTimeInGame : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject enterNamePanel;
    [SerializeField] UnityEngine.UI.Button confirmNameButt;

    private void Awake()
    {
    }

    private void Start()
    {
        if (DataManager.DM.settings.defaultScene == 1)
        {
            enterNamePanel.SetActive(false);
            SceneManager.LoadSceneAsync(1);            
        }
        else
        {
            confirmNameButt.onClick.AddListener(async () =>
            {
                await FirebaseDBManager.DB.UpdateUserName(FirebaseAuthManager.myUser.UserId, nameInput.text);
                DataManager.DM.settings.defaultScene = 1;
                Memento.SaveData(DataManager.DM.settings);
                SceneManager.LoadScene(1);
            });
        }
    }

}
