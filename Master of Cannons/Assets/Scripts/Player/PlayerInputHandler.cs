using UnityEngine;
using UnityEngine.EventSystems;
using Delegates;

public class PlayerInputHandler : MonoBehaviour
{
    //private Button specialButton = null;
    public static event Action OnShootAction = null;
    public static event Func<System.Collections.IEnumerator> OnSpecialFunc = null;
    public static Action canActivateSpecialHandler;

    private void Awake()
    {
        OnSpecialFunc = null;
        OnShootAction = null;
    }

    private void Start()
    {
        //TODO: Refactor this
        //specialButton = GetComponentInChildren<Button>();
        //specialButton.interactable = false;
        canActivateSpecialHandler = CanActivateSpecial;

        //specialButton.onClick.AddListener(() =>
        //{
        //    specialButton.interactable = false;
        //    specialButton.image.color = Color.grey;
        //    StartCoroutine(OnSpecialFunc());
        //});
    }

    void CanActivateSpecial()
    {
        //specialButton.image.color = Color.red;
        //specialButton.interactable = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO set this through raycast and verify if it does not hit UI
        //OnShootAction?.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("eee");
        }
    }
}
