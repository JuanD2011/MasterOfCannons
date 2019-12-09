using UnityEngine;
using UnityEngine.UI.Extensions;

public class CharacterSkinSelection : MonoBehaviour
{
    [SerializeField]
    private Transform pagination = null, content = null;

    [SerializeField]
    private PlayerData playerData = null;

    [SerializeField]
    private CharactersDatabase skinsDatabase = null;

    [SerializeField]
    private GameObject skinTemplate = null;

    [SerializeField]
    private HorizontalScrollSnap horizontalScrollSnap = null;

    private int skinIndex = 0;

    private GameObject skinTmp = null;

    /// <summary>
    /// Instantiate all the skins for the current selected character.
    /// </summary>
    public void Initialize()
    {
        for (int i = 0; i < content.childCount; i++) Destroy(content.GetChild(i).gameObject);
        for (int i = 0; i < pagination.childCount; i++) pagination.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < skinsDatabase.currentSkinData.Skins.Length; i++)
        {
            //skinTmp = Instantiate(skinTemplate);
            //horizontalScrollSnap.AddChild(skinTmp);

            pagination.GetChild(i).gameObject.SetActive(true);
        }

        horizontalScrollSnap.UpdateLayout();
    }

    /// <summary>
    /// Set character
    /// </summary>
    public void SetCharacter(int _Index)
    {
        skinsDatabase.currentSkinData = skinsDatabase.skins[_Index];
        playerData.currentCharacter.Name = skinsDatabase.currentSkinData.Name;
        playerData.currentCharacter.CharacterType = skinsDatabase.currentSkinData.CharacterType;
    }

    /// <summary>
    /// Set current skin 
    /// </summary>
    /// <param name="_Index"></param>
    public void SetSkin() => playerData.currentCharacter.CurrentSkin = skinsDatabase.currentSkinData.Skins[skinIndex];

    /// <summary>
    /// Set skin index
    /// </summary>
    /// <param name="_Index"></param>
    public void SetSkinIndex(int _Index) { skinIndex = _Index; }
}
