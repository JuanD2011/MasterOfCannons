using UnityEngine;

public class RotatingBehaviour : MovingBehaviour
{
    [Tooltip("Will find the shortest path to reach the angle")]
    [SerializeField] Vector3[] angles = new Vector3[2];

    [SerializeField] private float pathRendererRadius = 1.2f;

    private Vector3 initialRotation = Vector3.zero;

    int anglesCounter = 0;

    protected override void Awake() => base.Awake();

    private void Start()
    {
        initialRotation = transform.eulerAngles;
        repeatMethod += Move;
        cannon.OnCharacterInCannon += OnCharacterInCannon;
        trailRenderer.positionCount = angles.Length + 1;
        RenderPath();
        if (startMoving) Move();
    }

    protected override void Move()
    {
        LeanTween.rotate(gameObject, angles[anglesCounter] + initialRotation, 1f).setEase(tweenType).setOnComplete(repeatMethod).setSpeed(speed);
        anglesCounter = (anglesCounter + 1) % angles.Length;
    }

    protected override void RenderPath()
    {
        bool graphMiddleValue = (trailRenderer.positionCount - 1) % 2 == 0 ? true : false;
        int middleValue = graphMiddleValue ? (trailRenderer.positionCount - 1) / 2 : 0;
        for (int i = 0; i < trailRenderer.positionCount; i++)
        {
            if (i < middleValue)
            {
                Vector3 trailPosition = new Vector3(pathRendererRadius * (float)System.Math.Sin(angles[i].z * Mathf.Deg2Rad),
                    pathRendererRadius * (float)System.Math.Cos(angles[i].z * Mathf.Deg2Rad), 0);
                trailRenderer.SetPosition(i, trailPosition + transform.position);
            }
            else if (i == middleValue)
            {
                Vector3 trailPosition = new Vector3(pathRendererRadius * (float)System.Math.Sin(transform.eulerAngles.z * Mathf.Deg2Rad),
                    pathRendererRadius * (float)System.Math.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), 0);
                trailRenderer.SetPosition(i, trailPosition + transform.position);
            }
            else if (i > middleValue)
            {
                Vector3 trailPosition = new Vector3(pathRendererRadius * (float)System.Math.Sin(angles[i - 1].z * Mathf.Deg2Rad),
                    pathRendererRadius * (float)System.Math.Cos(angles[i - 1].z * Mathf.Deg2Rad), 0);
                trailRenderer.SetPosition(i, trailPosition + transform.position);
            }
        }
    }

    protected override void OnCharacterInCannon(bool _value) => base.OnCharacterInCannon(_value);
}
