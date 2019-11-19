using System.Collections;
//using System;
using Delegates;
using UnityEngine;
using System.Collections.Generic;

public class PlaceCannonSystem : MonoBehaviour
{
    [SerializeField] GameObject movingCannon = null;
    [SerializeField] GameObject aimingCannon = null;
    public static Action<System.Type, Action<Vector3>> placeCannonHandler;
    public static Action destroyFakeColliders;

    int cannonLayer = 10;
    int cannonLayerMask = 0;

    private void Start()
    {
        placeCannonHandler = PlaceCannon;
        cannonLayerMask = 1 << cannonLayer;
    }

    public void PlaceCannon(System.Type type, Action<Vector3> onCannonPlaced)
    {
        switch (type)
        {
            case System.Type _ when type == typeof(MovingBehaviour):
                StartCoroutine(PlaceMovingCannon(onCannonPlaced));
                break;
            case System.Type _ when type == typeof(AimingBehaviour):
                StartCoroutine(PlaceAimingCannon(onCannonPlaced));
                break;
            default:
                break;
        }        
    }

    IEnumerator PlaceAimingCannon(Action<Vector3> onCannonPlaced)
    {
        List<GameObject> fakeColliders = MovingCannonsInCamView();
        destroyFakeColliders = () => { foreach (GameObject g in fakeColliders) Destroy(g); };
        GameObject instantiatedCannon = null;
        bool canPutTarget = false;
        Vector3 currentTargetPos = Vector3.zero;

        for(; ; )
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentTargetPos = Screen2WorldTap();
                canPutTarget = !Physics.CheckSphere(currentTargetPos, 1.5f, cannonLayerMask);
                if (canPutTarget)
                {
                    instantiatedCannon = Instantiate(aimingCannon, currentTargetPos, Quaternion.identity);
                    yield return new WaitForSecondsRealtime(0.3f);
                    onCannonPlaced?.Invoke(currentTargetPos);
                }
            }
            yield return null;
        }

    }

    IEnumerator PlaceMovingCannon(Action<Vector3> onCannonPlaced)
    {
        List<GameObject> fakeColliders = MovingCannonsInCamView();
        destroyFakeColliders = () => { foreach (GameObject g in fakeColliders) Destroy(g); };
        GameObject instantiatedCannon = null;
        int index = 0;
        bool canPutTarget = false;
        int pointsToSet = 3;

        Vector3 lastTargetPos = Vector3.zero;
        Vector3[] targets = new Vector3[pointsToSet];
        float mForwardPos = transform.position.z;

        while (index < pointsToSet)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 currentTargetPos = Screen2WorldTap();

                if (index == 0)
                    canPutTarget = !Physics.CheckSphere(currentTargetPos, 1.5f, cannonLayerMask);
                else
                    canPutTarget = !Physics.CheckCapsule(lastTargetPos, currentTargetPos, 1f, cannonLayerMask);

                if (canPutTarget)
                {
                    targets[index] = currentTargetPos;
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = currentTargetPos;
                    index++;
                    lastTargetPos = targets[index - 1];
                }
            }
            yield return null;
        }

        instantiatedCannon = Instantiate(movingCannon, targets[0], Quaternion.identity);        
        if(pointsToSet > 2)
        {
            for (int i = 3; i <= pointsToSet; i++)
            {
                GameObject target = new GameObject(string.Format("Target{0}", i));
                target.tag = "Target";
                target.transform.SetParent(instantiatedCannon.transform, true);
            }
        }
        for (int i = 0; i < pointsToSet; i++)
            instantiatedCannon.transform.Find(string.Format("Target{0}", i + 1)).position = targets[i];

        yield return new WaitForSecondsRealtime(0.3f);
        yield return StartCoroutine(SetCannonRotation(instantiatedCannon.transform));        
        onCannonPlaced?.Invoke(targets[0]);
    }

    private IEnumerator SetCannonRotation(Transform instantiatedCannon)
    {
        Vector3 direction = Vector3.zero;
        float targetRotation = 0f;
        Camera m_Camera = Camera.main;

        while (!Input.GetMouseButtonUp(0))
        {
            Vector2 _AimVector = Input.mousePosition;
            direction = new Vector3(_AimVector.x, _AimVector.y, 0) - m_Camera.WorldToScreenPoint(instantiatedCannon.transform.position);
            targetRotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            instantiatedCannon.transform.rotation = Quaternion.AngleAxis(targetRotation - 90, Vector3.forward);
            yield return null;
        }
    }

    private Vector3 Screen2WorldTap()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector3 currentTargetPos = Camera.main.ScreenToWorldPoint(mousePos);
        currentTargetPos.z = transform.position.z;
        return currentTargetPos;
    }

    protected List<GameObject> MovingCannonsInCamView()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 screenCenter2World = Camera.main.ScreenToWorldPoint(screenCenter);
        screenCenter2World.z = transform.position.z;
        Collider[] cannonsColliders = Physics.OverlapSphere(screenCenter2World, 8, cannonLayerMask);
        List<MovingBehaviour> movCannons = new List<MovingBehaviour>();

        foreach (Collider c in cannonsColliders)
        {
            MovingBehaviour m = c.GetComponent<MovingBehaviour>();
            System.Type type = m?.GetType();
            if (type == typeof(MovingBehaviour))
                movCannons.Add(m);
        }

        return CreateFakeColliders(movCannons);
    }

    protected List<GameObject> CreateFakeColliders(List<MovingBehaviour> movCannons)
    {
        List<GameObject> fakeColliders = new List<GameObject>();
        int movCannonsCount = movCannons.Count;

        for (int i = 0; i < movCannonsCount; i++)
        {
            MovingBehaviour movBehaviour = movCannons[i];
            int movBehaviourTargetsCount = movBehaviour.Targets.Count;

            for(int j = 0; j < movBehaviourTargetsCount - 1; j++)
            {
                GameObject auxCollider = new GameObject("fakeColl", typeof(CapsuleCollider));
                auxCollider.layer = cannonLayer;

                Vector3 middlePoint = (movBehaviour.Targets[j] + movBehaviour.Targets[j+1]) / 2;
                auxCollider.transform.position = middlePoint;

                Vector3 relativePos = movBehaviour.Targets[j] - movBehaviour.Targets[j+1];
                float zRot = Mathf.Atan2(relativePos.x, relativePos.y) * Mathf.Rad2Deg;
                auxCollider.transform.rotation = Quaternion.Euler(0, 0, -zRot);

                float distance = Vector3.Distance(movBehaviour.Targets[j], movBehaviour.Targets[j+1]);
                auxCollider.GetComponent<CapsuleCollider>().height = distance + 0.5f;

                fakeColliders.Add(auxCollider);
            }
        }

        return fakeColliders;
    }

}

