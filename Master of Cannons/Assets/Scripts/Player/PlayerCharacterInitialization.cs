using UnityEngine;

public class PlayerCharacterInitialization : MonoBehaviour
{
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private CharactersDatabase charactersDatabase = null;

    [SerializeField] private bool loadData = false;

    private void Awake()
    {
        if (loadData)
        {
            Memento.LoadData(playerData);
        }

        InitializeCharacterFirstTime();
    }

    /// <summary>
    /// This only occurs the first time the user join the game, Set the default character
    /// </summary>
    private void InitializeCharacterFirstTime()
    {
        if (!playerData.defaultCharacterSet)
        {
            playerData.currentCharacter.Name = charactersDatabase.characterDatas[0].Name;
            playerData.currentCharacter.CurrentSkin = charactersDatabase.characterDatas[0].Skins[0];
            playerData.currentCharacter.CharacterType = charactersDatabase.characterDatas[0].CharacterType;

            playerData.defaultCharacterSet = true;

            Memento.SaveData(playerData);
        }
    }
}
