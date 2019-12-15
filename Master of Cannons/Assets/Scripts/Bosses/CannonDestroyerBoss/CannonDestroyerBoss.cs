using UnityEngine;

public class CannonDestroyerBoss : Boss
{
    [SerializeField]
    private GameObject cannonsParent = null;

    [SerializeField]
    private float bulletLifeTime = 0f;

    private Cannon[] cannons;

    private void Start()
    {
        cannons = cannonsParent.GetComponentsInChildren<Cannon>();
    }

    protected override void SetDifficulty()
    {

    }
}
