using UnityEngine;

public class ShakeBehaviour : MonoBehaviour
{
    private void Start()
    {
        Boss.OnBossDamage += OnBossHit;
    }

    private void OnBossHit(int _bossLife)
    {
        
    }
}
