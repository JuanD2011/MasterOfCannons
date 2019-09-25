using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LosingVolume : MonoBehaviour
{
    public static event Action OnDefeat = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            OnDefeat?.Invoke();
        }
    }
}
