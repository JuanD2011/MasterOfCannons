using UnityEngine;
using System.Collections;

public class GhostCharacter : Character
{
    GameObject lastCannon;

    protected override void Start()
    {
        base.Start();
        specialTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObj = other.gameObject;
        if (canActivateSpecial && otherGameObj.layer == 10)
            lastCannon = otherGameObj.gameObject;
    }

    protected override IEnumerator OnSpecial()
    {
        lastCannon.SetActive(true);
        transform.position = lastCannon.transform.Find("Reference").position;
        yield return (base.OnSpecial());
    }
}
