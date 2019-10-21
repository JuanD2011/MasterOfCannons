using UnityEngine;
using UnityEngine.EventSystems;
using Delegates;
using UnityEngine.UI;

public class PlayerInputHandler : MonoBehaviour, IPointerClickHandler
{
    Button specialButton = null;
    public static event Action OnShootAction = null;
    public static event Func<System.Collections.IEnumerator> OnSpecialFunc;
    public static Action canActivateSpecialHandler;

    void Start()
    {
        specialButton = GetComponentInChildren<Button>();
        specialButton.interactable = false;
        canActivateSpecialHandler = CanActivateSpecial;

        specialButton.onClick.AddListener(() =>
        {
            specialButton.interactable = false;
            specialButton.image.color = Color.grey;
            StartCoroutine(OnSpecialFunc());
        });
    }

    void CanActivateSpecial()
    {
        specialButton.image.color = Color.red;
        specialButton.interactable = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnShootAction?.Invoke();
    }
}
