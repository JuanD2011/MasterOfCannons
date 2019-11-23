using UnityEngine;

public class StickyCharacter : Character
{
    float minDistanceToStick = 0.8f;
    float maxDistanceToStick = 1.8f;
    float lerpTime = 0.4f;

    protected override void Start()
    {
        base.Start();
        specialTime = 20f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasSpecial && collision.gameObject.layer == 10)
        {
            //Transform cannonReference = collision.transform.root.GetChild(0);  
     
            Transform[] cannonChilds = collision.transform.root.GetComponentsInChildren<Transform>();
            Transform cannonReference = null;

            foreach (Transform t in cannonChilds)
                if (t.name == "Reference")
                    cannonReference = t;
                    
            ContactPoint contactPoint = collision.contacts[0];
            float contactPointToReference = Mathf.Abs(Vector3.Distance(contactPoint.point, cannonReference.position));
            if (contactPointToReference >= minDistanceToStick && contactPointToReference <= maxDistanceToStick)
                StartCoroutine(StickToCannon(cannonReference));
        }
    }

    System.Collections.IEnumerator StickToCannon(Transform to)
    {
        Vector3 from = transform.position;
        for (float t = 0; t <= lerpTime; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(from, to.position, t / lerpTime);
            yield return null;
        }
    }
}
