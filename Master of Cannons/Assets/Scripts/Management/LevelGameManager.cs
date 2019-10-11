using UnityEngine.SceneManagement;

public class LevelGameManager : LevelManager
{
    private void Awake()
    {
        OnLoadLevel = null;
    }

    private void Start()
    {
        //This start is to override parent's start
        //We don't want to call it
    }

    public void RestartLevel()
    {
        OnLoadLevel(LoadLevelStatusType.Successful);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }
}