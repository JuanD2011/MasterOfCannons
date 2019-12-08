using UnityEngine;
using UnityEngine.UI;

public class UISpecial : MonoBehaviour
{
    Image specialFill = null;

    private void Awake()
    {
        Character.OnChargeSpecial += UpdateSpecialFill;
        specialFill = transform.GetChild(0).GetComponent<Image>();
    }

    private void OnDisable() => Character.OnChargeSpecial -= UpdateSpecialFill;   

    void UpdateSpecialFill(float amount) { specialFill.fillAmount = amount; }

}
