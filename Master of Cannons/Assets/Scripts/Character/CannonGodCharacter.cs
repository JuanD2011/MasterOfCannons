using System.Collections;
using UnityEngine;

public class CannonGodCharacter : Character
{
    enum CannonType { Aiming, Moving }
    [SerializeField] CannonType cannonType = CannonType.Aiming;
    Delegates.Action<Vector3> setPos2CannonPlaced = null;

    protected override void Start()
    {
        base.Start();
        specialTime = 10f;
        setPos2CannonPlaced = (cannonPlacedPos) =>
        {
            transform.position = cannonPlacedPos;
            OnDisableSpecial();
        };
    }

    protected override IEnumerator OnSpecial()
    {
        System.Type t = null;

        switch (cannonType)     
        {
            case CannonType.Aiming:
                t = typeof(AimingBehaviour);
                break;
            case CannonType.Moving:
                t = typeof(MovingBehaviour);
                break;
            default:
                break;
        }

        PlaceCannonSystem.placeCannonHandler(t, setPos2CannonPlaced);
        yield return new WaitForSecondsRealtime(0.3f);
        StartCoroutine(base.OnSpecial());            
        Time.timeScale = 0f;
    }

    protected override void OnDisableSpecial()
    {
        base.OnDisableSpecial();
        Time.timeScale = 1;
        GetComponent<PlaceCannonSystem>().StopAllCoroutines();
        PlaceCannonSystem.destroyFakeColliders?.Invoke();
    }    
}
