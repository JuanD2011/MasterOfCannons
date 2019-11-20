using UnityEngine;
using UnityEngine.UI;

public static class UIUtilities
{
    public static GameObject CreateCanvas(string name)
    {
        GameObject canvas = new GameObject(name, typeof(GraphicRaycaster), typeof(CanvasScaler));     
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.matchWidthOrHeight = 0.5f;
        canvas.layer = 5;
        return canvas;
    }

    public static GameObject CreateButton(string name, string text, GameObject parent, Delegates.Action onClickAction, Color fontColor, Font fontType)
    {
        GameObject button = new GameObject(name, typeof(RectTransform), typeof(Button), typeof(Image));
        button.transform.SetParent(parent.transform);
        button.layer = 5;

        GameObject buttonTxt = new GameObject("Text", typeof(RectTransform), typeof(Text));        
        button.GetComponent<Button>().onClick.AddListener(() => onClickAction.Invoke());
        buttonTxt.transform.SetParent(button.transform);

        Text txt = buttonTxt.GetComponent<Text>();
        txt.color = fontColor;
        txt.font = fontType;
        txt.resizeTextForBestFit = true;
        txt.text = string.Format(text);

        return button;
    }

    public static GameObject CreateHorizontalLayout(string name, GameObject parent)
    {
        GameObject layoutObject = new GameObject(name, typeof(HorizontalLayoutGroup));
        layoutObject.transform.localPosition = Vector3.zero;
        layoutObject.transform.SetParent(parent.transform);

        HorizontalLayoutGroup layoutGroup = layoutObject.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.spacing = 20f;
        layoutGroup.childControlHeight = false;
        layoutGroup.childControlWidth = false;
        
        RectTransform rect = layoutObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(-50, 100);
        Vector2 centerBottom = new Vector2(0.5f, 0);
        rect.anchorMax = centerBottom;
        rect.anchorMin = centerBottom;

        return layoutObject;
    }
}

