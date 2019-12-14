using UnityEngine;

[CreateAssetMenu(fileName = "Characters Database", menuName = "Characters Database")]
public class CharactersDatabase : ScriptableObject
{
    public CharacterData[] characterDatas = new CharacterData[3];

    public CharacterData currentCharacterData = new CharacterData();
}
