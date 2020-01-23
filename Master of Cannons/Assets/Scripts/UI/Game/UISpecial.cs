using UnityEngine;
using UnityEngine.UI;

public class UISpecial : MonoBehaviour
{
    private Image specialFill = null;

    private void Awake()
    {
        specialFill = transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        Character.OnChargeSpecial += UpdateSpecialFill;
    }

    private void OnDestroy()
    {
        Character.OnChargeSpecial -= UpdateSpecialFill;
    } 

    private void UpdateSpecialFill(float amount)
    {
        specialFill.fillAmount = amount;
    }
}
