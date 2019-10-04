using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] panels = new GameObject[0];

    [SerializeField]
    private Settings settings = null;

    protected Animator[] panelAnimators = new Animator[0];

    protected LevelManager m_LevelManager = null;

    private int levelToLoad = 0;

    private int currentPanelIndex = 0;

    private readonly string panelFadeIn = "MP Fade-in";
    private readonly string panelFadeOut = "MP Fade-out";
    private readonly string panelFadeInStart = "MP Fade-in Start";

    protected virtual void Awake()
    {
        m_LevelManager = GetComponent<LevelManager>();

        Memento.LoadData(settings);

        InitializePanelAnimators();
        SetLanguage();
    }

    protected virtual void Start()
    {
        panelAnimators[currentPanelIndex].Play(panelFadeInStart);
        Level.OnLoadLevel += AssignLevelToLoad;
    }

    private void AssignLevelToLoad(int _LevelBuildIndex) => levelToLoad = _LevelBuildIndex;

    protected void InitializePanelAnimators()
    {
        panelAnimators = new Animator[panels.Length];
        for (int i = 0; i < panelAnimators.Length; i++) panelAnimators[i] = panels[i].GetComponent<Animator>();
    }

    private void SetLanguage()
    {
        Translation.currentLanguageId = settings.languageID;
        Translation.LoadLanguage(Translation.idToLanguage[Translation.currentLanguageId]);
    }

    /// <summary>
    /// Start loading the level.
    /// </summary>
    public void StartLoadingLevel()
    {
        StartCoroutine(m_LevelManager.LoadAsynchronously(levelToLoad));
    }

    /// <summary>
    /// Fade out the current panel and fade in the panel provided by _newPanelIndex
    /// </summary>
    /// <param name="_newPanelIndex"></param>
    public void PanelAnim(int _newPanelIndex)
    {
        if (_newPanelIndex != currentPanelIndex)
        {
            panelAnimators[currentPanelIndex].Play(panelFadeOut);
            currentPanelIndex = _newPanelIndex;
            panelAnimators[currentPanelIndex].Play(panelFadeIn);
        }
    }

    /// <summary>
    /// Save settings
    /// </summary>
    public void SaveSettings()
    {
        Memento.SaveData(settings);
    }

    private void OnApplicationQuit()
    {
        if (settings == null) return;

        Memento.SaveData(settings);
    }
}
