using UnityEngine;

public class Referee : MonoBehaviour
{
    [SerializeField] private GameObject losingColliderObj = null;
    [SerializeField] private float colliderDistanceFromCannon = 0;
    private Vector3 posDifferenceFromCannon;

    private void Awake()
    {
        if (colliderDistanceFromCannon < 0) colliderDistanceFromCannon *= -1;
        posDifferenceFromCannon = new Vector3(0, colliderDistanceFromCannon, 0);
        FlagObj.OnVictory += GiveVictory;
        LosingVolume.OnDefeat += GiveDefeat;
        Cannon[] cannons = FindObjectsOfType<Cannon>();
        foreach (Cannon cannon in cannons) { cannon.OnRegisterLastCannon += UpdateLosingVolume; }
    }

    private void GiveVictory() => Time.timeScale = 0;
    private void GiveDefeat() => Time.timeScale = 0;
    private void UpdateLosingVolume(Cannon cannonEntry) => losingColliderObj.transform.position 
        = cannonEntry.transform.position - posDifferenceFromCannon;
}
