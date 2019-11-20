using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private GameObject character = null;

    [SerializeField]
    private PlayerData playerData = null;

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
