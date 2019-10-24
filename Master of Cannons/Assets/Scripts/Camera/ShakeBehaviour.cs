using UnityEngine;

public class ShakeBehaviour : MonoBehaviour
{
    private void Start()
    {
        Boss.OnBossHit += OnBossHit;
    }

    private void OnBossHit(int _bossLife)
    {
        
    }
}
