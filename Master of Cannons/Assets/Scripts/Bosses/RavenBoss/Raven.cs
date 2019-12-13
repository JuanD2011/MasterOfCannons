using System.Collections;
using UnityEngine;

public class Raven : Flying
{    
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character"))
        {
            Character character = other.GetComponent<Character>();
            character.CannonEnterReset(transform);
            character.transform.position = transform.GetChild(0).position;
            character.SetFunctional(false);
            StartCoroutine(DropOut(character, other));
        }
    }

    IEnumerator DropOut(Character character, Collider other)
    {
        float t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            yield return null;
        }

        Physics.IgnoreLayerCollision(9, 10, false);
        character.SetFunctional(true);
    }

}
