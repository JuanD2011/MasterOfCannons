using UnityEngine;

public abstract class Flying : MonoBehaviour
{
    public new Rigidbody rigidbody = null;
    float t = 0;
    public bool isVisible { get; private set; }

    protected virtual void Awake()
    {
        isVisible = false;
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        t += Time.deltaTime;
        if (t >= 1.5f)
        {
            OnVisible();
            t = 0;
        }
    }

    private void OnVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        isVisible = screenPoint.x < -2 || screenPoint.x > 2 ? false : true;
    }

    protected abstract void OnTriggerEnter(Collider other);

}
