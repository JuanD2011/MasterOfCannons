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
            //other.isTrigger = true;
            //Physics.IgnoreLayerCollision(9, 10, true);
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
        //t = 0;
        //while (t < 0.5f)
        //{
        //    t += Time.deltaTime;
        //    character.transform.position += Vector3.down * Time.deltaTime * 40F;
        //    yield return null;
        //}

        character.SetFunctional(true);
        //other.isTrigger = false;
        //Physics.IgnoreLayerCollision(9, 10, false);
        //character.transform.parent = null;
    }

}
