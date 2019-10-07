using UnityEngine;
using Delegates;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] panels = new GameObject[0];

    [SerializeField]
    private Settings settings = null;

    protected Animator[] panelAnimators = new Animator[0];

    private int currentPanelIndex = 0;

    public static bool canSelectLevel = false;

    private readonly string panelFadeIn = "MP Fade-in";
    private readonly string panelFadeOut = "MP Fade-out";
    private readonly string panelFadeInStart = "MP Fade-in Start";

    private readonly string panelModalIn = "MP Modal In";
    private readonly string panelModalOut = "MP Modal Out";

    [SerializeField] Transform popUpWindow;
    [SerializeField] UnityEngine.UI.Button confirmButton;
    [SerializeField] UnityEngine.UI.Button cancelButton;

    public static Action<string, Action> popUpHandler;

    protected virtual void Awake()
    {
        Memento.LoadData(settings);

        InitializePanelAnimators();
        SetLanguage();
    }

    protected virtual void Start()
    {
        panelAnimators[currentPanelIndex].Play(panelFadeInStart);
        LevelManager.OnLoadLevel += ManageLevelPlayerAction;
        popUpHandler = ConfirmationPopUp;
        popUpWindow.gameObject.SetActive(false);
    }

    private void ManageLevelPlayerAction(bool _CanPlay)
    {
        if (_CanPlay)
        {
            PanelAnim(1);
        }
        else
        {
            //TODO show message that the player has not enough stars to play
        }
    }

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
    /// Set current panel animation depending on the bool
    /// </summary>
    /// <param name="_IsOn"></param>
    public void ModalAnim(bool _IsOn)
    {
        if (_IsOn == true)
        {
            panelAnimators[currentPanelIndex].Play(panelModalOut);
        }
        else
        {
            panelAnimators[currentPanelIndex].Play(panelModalIn);
        }
    }

    /// <summary>
    /// Equals can select level to the _value
    /// </summary>
    /// <param name="_Value"></param>
    public void SelectingLevels(bool _Value) => canSelectLevel = _Value;

    /// <summary>
    /// Save settings
    /// </summary>
    public void SaveSettings()
    {
        Memento.SaveData(settings);
    }

    private void ConfirmationPopUp(string _text, Delegates.Action confirmAction)
    {
        popUpWindow.gameObject.SetActive(true);
        confirmButton.onClick.RemoveAllListeners();
        popUpWindow.GetComponent<TMPro.TextMeshProUGUI>().text = _text;
        confirmButton.onClick.AddListener(() => {
            confirmAction.Invoke();
            popUpWindow.gameObject.SetActive(false);
        });
            
        cancelButton.onClick.AddListener(()=> popUpWindow.gameObject.SetActive(false));
    }

    private void OnApplicationQuit()
    {
        if (settings == null) return;

        Memento.SaveData(settings);
    }
}
