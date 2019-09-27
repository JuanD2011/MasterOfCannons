using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRenderTexture : MonoBehaviour
{
    [SerializeField] Transform character;
    [SerializeField] float horizontalAngle;
    [SerializeField] float verticalAngle;

    float maxHorizontalAngle;

    private void Start()
    {
        maxHorizontalAngle = 180 - horizontalAngle;
    }


    void Update()
    {
        float playerXDirection = -character.transform.up.x;
        float playerHypotenuse = character.transform.up.magnitude;
        float playerYDirection = character.transform.up.y;

        float sin = playerYDirection / playerHypotenuse;
        float alpha = Mathf.Asin(sin) * Mathf.Rad2Deg;
        alpha = Mathf.Clamp(alpha, -verticalAngle, verticalAngle);

        float cos = playerXDirection / playerHypotenuse;
        float theta = Mathf.Acos(cos) * Mathf.Rad2Deg;
        theta = Mathf.Clamp(theta, horizontalAngle, maxHorizontalAngle);

        transform.localRotation = Quaternion.Euler(alpha, theta, 0);

        Debug.LogFormat(" Theta: {0} \n Alpha: {1}", theta, alpha);
    }
}
