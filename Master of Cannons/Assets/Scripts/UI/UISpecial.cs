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

    void UpdateSpecialFill(float amount) { if(specialFill != null) specialFill.fillAmount = amount; }

}
