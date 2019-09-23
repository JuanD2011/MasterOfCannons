using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class FirstTimeInGame : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] UnityEngine.UI.Button confirmNameButt;

    private void Start()
    {
        confirmNameButt.onClick.AddListener(async () => {
            await FirebaseDBManager.DB.UpdateUserName(FirebaseAuthManager.myUser.UserId, nameInput.text);
            DataManager.DM.settings.defaultScene = 1;
            Memento.SaveData(DataManager.DM.settings);
            SceneManager.LoadScene(1);
        });
    }

}
