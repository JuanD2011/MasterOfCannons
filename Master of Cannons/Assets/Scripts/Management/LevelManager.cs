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
         menuManager.onLoadLevel += LoadLevel;
    }

    private void LoadLevel(int _LevelBuildIndex)
    {
        StartCoroutine(LoadAsynchronously(_LevelBuildIndex));
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