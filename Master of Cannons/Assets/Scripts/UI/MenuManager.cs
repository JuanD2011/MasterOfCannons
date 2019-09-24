﻿using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] panels = new GameObject[0];
    [SerializeField] Settings settings = null;
    Animator[] panelAnimators = new Animator[0];

    int currentPanelIndex = 0;

    private readonly string panelFadeIn = "MP Fade-in";
    private readonly string panelFadeOut = "MP Fade-out";
    private readonly string panelFadeInStart = "MP Fade-in Start";

    public static bool IsPaused { get; private set; } = false;

    public static event System.Action<bool> OnPause = null;

    private void Awake()
    {
        OnPause = null;

        Memento.LoadData(settings);

        panelAnimators = new Animator[panels.Length];

        for (int i = 0; i < panelAnimators.Length; i++) panelAnimators[i] = panels[i].GetComponent<Animator>();

        SetLanguage();
    }

    void Start()
    {
        panelAnimators[currentPanelIndex].Play(panelFadeInStart);
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
    /// Save settings
    /// </summary>
    public void SaveSettings()
    {
        Memento.SaveData(settings);
    }

    /// <summary>
    /// Set if the game is paused by the _Value given
    /// </summary>
    /// <param name="_Value"></param>
    public void SetGamePause(bool _Value)
    {
        IsPaused = _Value;
        OnPause?.Invoke(_Value);
    }
}
