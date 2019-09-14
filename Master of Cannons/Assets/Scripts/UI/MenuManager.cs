using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] panels = new GameObject[0];

    Animator[] panelAnimators = new Animator[0];

    int currentPanelIndex = 0;

    private readonly string panelFadeIn = "MP Fade-in";
    private readonly string panelFadeOut = "MP Fade-out";
    private readonly string panelFadeInStart = "MP Fade-in Start";

    private void Awake()
    {
        panelAnimators = new Animator[panels.Length];

        for (int i = 0; i < panelAnimators.Length; i++)
        {
            panelAnimators[i] = panels[i].GetComponent<Animator>();
        }
    }

    void Start()
    {
        panelAnimators[currentPanelIndex].Play(panelFadeInStart);
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
}
