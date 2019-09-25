using System;
using UnityEngine;

public class FlagObj : MonoBehaviour
{
    public static event Action OnVictory = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            OnVictory?.Invoke();
        }
    }
}
