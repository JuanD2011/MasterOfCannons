using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Slider slider = null;

    private void Start()
    {
    }

    IEnumerator LoadAsynchronously(int _BuildIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_BuildIndex);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress * 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}