using UnityEngine;

[CreateAssetMenu(fileName = "Characters Database", menuName = "Characters Database")]
public class CharactersDatabase : ScriptableObject
{
    public CharacterData[] skins = new CharacterData[3];

    public CharacterData currentSkinData = new CharacterData();
}
