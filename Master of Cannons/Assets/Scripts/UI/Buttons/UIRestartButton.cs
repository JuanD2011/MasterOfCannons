using UnityEngine.SceneManagement;

public class UIRestartButton : UIButtonBase
{
    public override void OnButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
