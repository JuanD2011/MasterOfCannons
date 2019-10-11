using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] float bounceForce = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            RejectCharacter(character, collision.contacts[0]);
        }
    }

    private void RejectCharacter(Character character, ContactPoint contact)
    {
        Rigidbody characterRigid = character.Rigidbody;
        Vector3 direction = Vector3.Reflect(character.velocityUpdated.normalized, contact.normal);
        characterRigid.AddForce(direction * bounceForce, ForceMode.Impulse);

        //Look At Direction  
        //characterRigid.freezeRotation = true;
        //float rotZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        //characterRigid.rotation = (Quaternion.Euler(0, 0, -rotZ));                
    }
}
