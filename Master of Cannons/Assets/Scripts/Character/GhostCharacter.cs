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
        if (specialProgress >= 0.8f && otherGameObj.layer == 10)
            lastCannon = otherGameObj.gameObject;
    }

    protected override IEnumerator OnSpecial()
    {        
        transform.position = lastCannon.transform.Find("Reference").position;
        yield return (base.OnSpecial());
        float t = 0;
        while(t <= 3)
        {
            SetFunctional(false);
            yield return null;
        }
    }
}
