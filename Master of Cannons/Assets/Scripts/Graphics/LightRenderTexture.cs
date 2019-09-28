using UnityEngine;

public class LightRenderTexture : MonoBehaviour
{
    [SerializeField] Transform character = null;
    [SerializeField] float horizontalAngle = 0f;
    [SerializeField] float verticalAngle = 0f;

    float maxHorizontalAngle = 0f;

    float playerXDirection = 0f, playerYDirection = 0f, playerDirectionMagnitude = 0f;

    float sin = 0f, alpha = 0f;
    float cos = 0f, theta = 0f;

    private void Start()
    {
        maxHorizontalAngle = 180f - horizontalAngle;
    }

    void Update()
    {
        playerXDirection = -character.transform.up.x;
        playerYDirection = character.transform.up.y;
        playerDirectionMagnitude = character.transform.up.magnitude;

        sin = playerYDirection / playerDirectionMagnitude;
        alpha = Mathf.Asin(sin) * Mathf.Rad2Deg;
        alpha = Mathf.Clamp(alpha, -verticalAngle, verticalAngle);

        cos = playerXDirection / playerDirectionMagnitude;
        theta = Mathf.Acos(cos) * Mathf.Rad2Deg;
        theta = Mathf.Clamp(theta, horizontalAngle, maxHorizontalAngle);
        transform.localRotation = Quaternion.Euler(alpha, theta, 0);

        //Debug.LogFormat("X:        {0:N10} \nMAGNITUD:  {1:N10}", (double)character.transform.up.x, (double)character.transform.up.magnitude);
        //Debug.LogFormat(" Theta: {0} \n Alpha: {1}", theta, alpha);
    }
}
