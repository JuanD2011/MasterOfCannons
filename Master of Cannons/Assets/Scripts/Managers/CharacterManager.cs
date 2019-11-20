using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private GameObject character = null;

    [SerializeField]
    private PlayerData playerData = null;

    [Header("Only if current character is Cannon God")]
    [SerializeField]
    private GameObject aimingCannon = null, movingCannon = null;

    private void Awake()
    {
        switch (playerData.currentCharacter.CharacterType)
        {
            case CharacterType.Ghost:
                character.AddComponent<GhostCharacter>();
                break;
            case CharacterType.Sticky:
                character.AddComponent<StickyCharacter>();
                break;
            case CharacterType.CannonGod:
                character.AddComponent<CannonGodCharacter>();
                character.AddComponent<PlaceCannonSystem>();
                PlaceCannonSystem placeCannonSystem = character.GetComponent<PlaceCannonSystem>();
                placeCannonSystem.AimingCannon = aimingCannon;
                placeCannonSystem.MovingCannon = movingCannon;
                break;
            case CharacterType.Hairy:
                character.AddComponent<HairyCharacter>();
                break;
            case CharacterType.SlowMo:
                character.AddComponent<SlowMoCharacter>();
                break;
            case CharacterType.None:
                character.AddComponent<Character>();
                break;
            default:
                break;
        }
    }
}
