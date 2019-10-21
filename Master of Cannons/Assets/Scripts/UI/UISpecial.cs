using UnityEngine;
using UnityEngine.UI;

public class UISpecial : MonoBehaviour
{
    Image specialFill = null;

    private void Start()
    {
        Character.OnChargeSpecial += UpdateSpecialFill;
        specialFill = transform.GetChild(0).GetComponent<Image>();
    }

    void UpdateSpecialFill(float amount) => specialFill.fillAmount = amount;

}
