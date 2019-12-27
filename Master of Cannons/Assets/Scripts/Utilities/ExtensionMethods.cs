using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 GetRandomPointInVolume(this Collider collider)
    {
        Vector3 result;
        result = new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x), Random.Range(collider.bounds.min.y, collider.bounds.max.y), collider.gameObject.transform.position.z);
        return result;
    }
}
