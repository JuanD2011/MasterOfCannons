using UnityEngine;

public class FakeCharacter : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData = null;

    private void Awake()
    {
        Instantiate(playerData.currentCharacter.CurrentSkin, Vector3.zero, Quaternion.identity, transform);
    }
}
