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
}
