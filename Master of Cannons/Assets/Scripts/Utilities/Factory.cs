using UnityEngine;

public class Factory : Singleton<Factory>
{
    protected override void OnAwake() { }

    public static GameObject CreateNewInstance(GameObject _prefab, Vector3 _position, Quaternion _rotation, Transform _parent)
    {
        return Instantiate(_prefab, _position, _rotation, _parent);
    }
}
