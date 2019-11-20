using System.Collections;
using UnityEngine;

public class CannonGodCharacter : Character
{
    enum CannonType { Aiming, Moving }
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
        CreateCanvas();
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

    protected void CreateCanvas()
    {
        GameObject canvas = UIUtilities.CreateCanvas("CannonSelectionCanvas");
        GameObject horLayout = UIUtilities.CreateHorizontalLayout("Buttons", canvas);

        for (int i = 0; i < System.Enum.GetNames(typeof(CannonType)).Length; i++)
        {            
            CannonType cannonType = (CannonType)i;
            string cannonTypeName = string.Format("{0} Cannon", cannonType.ToString());            
            Delegates.Action buttonAction = () => OnClickCannonType(cannonType, canvas);
            Font font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            GameObject button = UIUtilities.CreateButton(cannonTypeName, cannonTypeName, horLayout, buttonAction, Color.black, font);
        }
    }

    private void OnClickCannonType(CannonType cannonType, GameObject canvas)
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
        Destroy(canvas);
    }

   
}

