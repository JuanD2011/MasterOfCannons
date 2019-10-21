using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CannonGodCharacter : Character
{
    [SerializeField] GameObject movingCannon;
    protected override void Start()
    {
        base.Start();
        specialTime = 10f;
    }

    protected override IEnumerator OnSpecial()
    {
        StartCoroutine(base.OnSpecial());
        Time.timeScale = 0;
        GameObject instantiatedCannon = null;
        int index = 0;
        bool canPutTarget = false;
        int pointsToSet = 2;

        Vector3 lastTargetPos = Vector3.zero;
        Vector3[] targets = new Vector3[pointsToSet];
        float mForwardPos = transform.position.z;

        while (index < pointsToSet)
        {
            if (Input.GetMouseButtonDown(0))
            {                 
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Vector3.Distance(transform.position, Camera.main.transform.position);
                Vector3 currentTargetPos = Camera.main.ScreenToWorldPoint(mousePos);
                currentTargetPos.z = mForwardPos;
                                
                if (index == 0)
                    canPutTarget = !Physics.CheckSphere(currentTargetPos, 1.5f, LayerMask.GetMask("Cannon"));
                else
                    canPutTarget = !Physics.CheckCapsule(lastTargetPos, currentTargetPos, 1f, LayerMask.GetMask("Cannon"));

                if (canPutTarget)
                {
                    targets[index] = currentTargetPos;
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = currentTargetPos;
                    index++;
                    lastTargetPos = targets[index - 1];
                    Debug.Log("TARGET WAS PUT");                    
                }
            }
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        instantiatedCannon = Instantiate(movingCannon, targets[0], Quaternion.identity);        

        for (int i = 0; i < pointsToSet; i++)
            instantiatedCannon.transform.Find(string.Format("Target{0}", i + 1)).position = targets[i];

        transform.position = targets[0];
        Time.timeScale = 1;
    }

}
