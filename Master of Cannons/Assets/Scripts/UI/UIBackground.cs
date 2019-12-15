using UnityEngine;

public class UIBackground : MonoBehaviour
{
    /// <summary>
    /// Intercalate between active and inactive
    /// </summary>
    public void IntercalateActiveGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Set background game object active state
    /// </summary>
    /// <param name="_Value"></param>
    public void SetActive(bool _Value)
    {
        gameObject.SetActive(_Value);
    }
}
