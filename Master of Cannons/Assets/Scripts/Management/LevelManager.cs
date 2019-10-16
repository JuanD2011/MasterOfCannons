using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] protected Slider slider = null;

    public static Delegates.Action<LoadLevelStatusType> OnLoadLevel = null;

    AsyncOperation operation = null;

    private void Awake()
    {
        OnLoadLevel = null;
    }

    private void Start()
    {
        Level.OnLevelSelected += IsLevelPlayable;
    }

    private void IsLevelPlayable(int _StarsRequired, int _LevelBuildIndex)
    {
        if (playerData.stars < _StarsRequired)
        {
            Debug.Log("Insufficient stars");
            OnLoadLevel(LoadLevelStatusType.InsufficientStars);
        }
        else
        {
            StartCoroutine(LoadAsynchronously(_LevelBuildIndex));
            OnLoadLevel(LoadLevelStatusType.Successful);
        }
    }

    protected IEnumerator LoadAsynchronously(int _LevelBuildIndex)
    {
        operation = SceneManager.LoadSceneAsync(_LevelBuildIndex);
        float progress = 0f;
        
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress * 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}