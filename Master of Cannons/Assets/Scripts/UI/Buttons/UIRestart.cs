using UnityEngine.SceneManagement;

public class UIRestart : UIButtonBase
{
    public override void OnButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
