using UnityEngine.SceneManagement;

public class LevelGameManager : LevelManager
{
    public static bool LevelSelection { get; private set; } = false;

    private void Awake()
    {
        OnLoadLevel = null;
        LevelSelection = false;
    }

    private void Start()
    {
        //This start is to override parent's start
        //We don't want to call it
    }

    /// <summary>
    /// Restart current level by buildIndex
    /// </summary>
    public void RestartLevel()
    {
        OnLoadLevel(LoadLevelStatusType.Successful);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }

    /// <summary>
    /// Load menu and set level selection panel
    /// </summary>
    public void LoadLevelSelection()
    {
        LevelSelection = true;
        OnLoadLevel(LoadLevelStatusType.Successful);
        StartCoroutine(LoadAsynchronously(0));
    }

    /// <summary>
    /// Load next level 
    /// </summary>
    public void NextLevel()
    {
        OnLoadLevel(LoadLevelStatusType.Successful);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
    }
}