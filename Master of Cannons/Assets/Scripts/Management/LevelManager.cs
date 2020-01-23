using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    protected Slider slider = null;

    private AsyncOperation operation = null;

    private MenuManager menuManager = null;

    protected virtual void Awake()
    {
        menuManager = GetComponent<MenuManager>();
    }

    protected virtual void Start()
    {
         menuManager.OnLoadLevel += LoadLevel;
    }

    private void LoadLevel(int _levelBuildIndex)
    {
        StartCoroutine(LoadAsynchronously(_levelBuildIndex));
    }

    protected IEnumerator LoadAsynchronously(int _levelBuildIndex)
    {
        if (_levelBuildIndex <= SceneManager.sceneCount)
        {
            operation = SceneManager.LoadSceneAsync(_levelBuildIndex);
        }
        else
        {
            operation = SceneManager.LoadSceneAsync(0);//If the level that is trying to load does not exist, then load menu scene
        }

        float progress = 0f;
        
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress * 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}