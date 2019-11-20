using UnityEngine;

public class CharacterSkinSelection : MonoBehaviour
{
    [SerializeField]
    private Transform content = null, pagination = null;

    [SerializeField]
    private PlayerData playerData = null;

    [SerializeField]
    private SkinsDatabase skinsDatabase = null;

    [SerializeField]
    private GameObject skinTemplate = null, paginationToggle = null;

    private int skinIndex = 0;

    /// <summary>
    /// Instantiate all the skins for the current selected character.
    /// </summary>
    public void Initialize()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i));
            Destroy(pagination.GetChild(i));
        }

        for (int i = 0; i < playerData.currentCharacter.Skins.Length; i++)
        {
            Instantiate(skinTemplate, content);
            Instantiate(paginationToggle, pagination);
        }
    }

    /// <summary>
    /// Set character
    /// </summary>
    public void SetCharacter(int _Index) { playerData.currentCharacter = skinsDatabase.skins[_Index]; }

    /// <summary>
    /// Set current skin 
    /// </summary>
    /// <param name="_Index"></param>
    public void SetSkin() { playerData.currentCharacter.CurrentSkin = playerData.currentCharacter.Skins[skinIndex]; }

    /// <summary>
    /// Set skin index
    /// </summary>
    /// <param name="_Index"></param>
    public void SetSkinIndex(int _Index) { skinIndex = _Index; }
}
