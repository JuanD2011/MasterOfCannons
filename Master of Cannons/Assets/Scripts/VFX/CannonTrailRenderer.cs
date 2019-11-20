using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTrailRenderer : MonoBehaviour
{
    private CannonBehaviour[] cannonBehaviours = new CannonBehaviour[0];

    // Start is called before the first frame update
    void Start()
    {
        cannonBehaviours = GetComponents<CannonBehaviour>();
        RenderTrails();
    }

    private void RenderTrails()
    {
        foreach(CannonBehaviour behaviour in cannonBehaviours)
        {
            if(behaviour is MovingBehaviour)
            {
                MovingBehaviour moveBehaviour  = behaviour as MovingBehaviour;
                LineRendererPath.RenderLines(GetComponent<LineRenderer>(), moveBehaviour.Targets.ToArray());
            }
            if(behaviour is RotatingBehaviour)
            {
                Debug.Log("Rendering");
                RotatingBehaviour rotateBehaviour = behaviour as RotatingBehaviour;
                LineRendererPath.RenderBezierCurve(GetComponent<LineRenderer>(), 1, rotateBehaviour.Angles, transform.position);
            }
        }
    }
}
