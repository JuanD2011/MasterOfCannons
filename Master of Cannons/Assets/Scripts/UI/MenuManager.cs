using UnityEngine;
using Delegates;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Settings settings = null;

    public static bool canSelectLevel = false;

    protected SettingsTabManager settingsTabManager = null;

    private readonly string panelModalIn = "MP Modal In";
    private readonly string panelModalOut = "MP Modal Out";

    [SerializeField] Transform popUpWindow = null;
    [SerializeField] TMPro.TextMeshProUGUI popUpText = null;
    [SerializeField] UnityEngine.UI.Button confirmButton = null;
    [SerializeField] UnityEngine.UI.Button cancelButton = null;

    public static Action<string, Action> popUpHandler;

    [SerializeField] Transform loadingCircle;
    public static Action<bool> loadingCircleHandler;

    private void Awake()
    {
        settingsTabManager = GetComponent<SettingsTabManager>();

        Memento.LoadData(settings);

        SetLanguage();

        CheckIfIsInLevelSelection();
    }

    private void Start()
    {
        LevelManager.OnLoadLevel += ManageLevelPlayerAction;
        popUpHandler = ConfirmationPopUp;
        popUpWindow.gameObject.SetActive(false);
        loadingCircleHandler = (activate) => loadingCircle.gameObject.SetActive(activate);
    }

    private void CheckIfIsInLevelSelection()
    {
        if (!LevelGameManager.LevelSelection) return;

        settingsTabManager.PanelAnim(4);//Initialize level selection panel
        SelectingLevels(true);
    }

    private void ManageLevelPlayerAction(LoadLevelStatusType _LoadLevelStatusType)
    {
        if (_LoadLevelStatusType == LoadLevelStatusType.Successful)
        {
            settingsTabManager.PanelAnim(1);
        }
        else if (_LoadLevelStatusType == LoadLevelStatusType.InsufficientStars)
        {
            //TODO show message that the player has not enough stars to play
        }
    }

    private void SetLanguage()
    {
        Translation.currentLanguageId = settings.languageID;
        Translation.LoadLanguage(Translation.idToLanguage[Translation.currentLanguageId]);
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
        popUpText.text = _text;
        confirmButton.onClick.AddListener(() => {
            confirmAction.Invoke();
            popUpWindow.gameObject.SetActive(false);
        });

        cancelButton.onClick.AddListener(() => {
            popUpWindow.gameObject.SetActive(false);
            FirebaseAuthManager.signOutFBHandler.Invoke();
            UISocial.fbButtonStatus.Invoke();
        });
    }

    private void OnLoadingCircle(bool activate) => loadingCircle.gameObject.SetActive(activate);
    
    private void OnApplicationQuit()
    {
        if (settings == null) return;

        Memento.SaveData(settings);
    }
}
