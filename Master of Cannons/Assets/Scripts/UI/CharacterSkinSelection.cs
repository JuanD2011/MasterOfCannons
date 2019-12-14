using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CharacterSkinSelection : MonoBehaviour
{
    [SerializeField] private Transform pagination = null;

    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private CharactersDatabase characterDatabase = null;

    [SerializeField] private GameObject skinTemplate = null;

    [SerializeField] private HorizontalScrollSnap horizontalScrollSnap = null;

    private int skinIndex = 0;
    private int characterIndex = 0;

    private GameObject skinTmp = null;

    //Horizontal scroll snap
    private GameObject[] horizontalScrollSnapChildren = null;
    private List<GameObject> scrollChildren = new List<GameObject>();

    /// <summary>
    /// Instantiate all the skins for the current selected character.
    /// </summary>
    public void Initialize()
    {
        horizontalScrollSnap.RemoveAllChildren(out horizontalScrollSnapChildren);

        for (int i = 0; i < pagination.childCount; i++)
        {
            pagination.GetChild(i).gameObject.SetActive(false);
        }

        //Verify if the game objects removed from the scroll does not exist in the list
        for (int i = 0; i < horizontalScrollSnapChildren.Length; i++)
        {
            if (!scrollChildren.Contains(horizontalScrollSnapChildren[i]))
            {
                scrollChildren.Add(horizontalScrollSnapChildren[i]); 
            } 
        }

        for (int i = 0; i < characterDatabase.characterDatas[characterIndex].Skins.Length; i++)
        {
            if (i < scrollChildren.Count)//If the list contains the game object, then it gives it to the scroll
            {
                horizontalScrollSnap.AddChild(scrollChildren[i]);
            }
            else //If the list does not contain the number of game objects, then instantiate it and add it to the list (pool functionality)
            {
                skinTmp = Instantiate(skinTemplate);
                horizontalScrollSnap.AddChild(skinTmp);
                scrollChildren.Add(skinTmp);
            }

            pagination.GetChild(i).gameObject.SetActive(true);
        }

        horizontalScrollSnap.UpdateLayout();
    }

    /// <summary>
    /// Set character and skin, this is executed by the button select when skin selection process has finished
    /// </summary>
    public void SetCharacter()
    {
        characterDatabase.currentCharacterData = characterDatabase.characterDatas[characterIndex];

        //Set player character
        playerData.currentCharacter.Name = characterDatabase.currentCharacterData.Name;
        playerData.currentCharacter.CharacterType = characterDatabase.currentCharacterData.CharacterType;
        playerData.currentCharacter.CurrentSkin = characterDatabase.currentCharacterData.Skins[skinIndex];
    }

    /// <summary>
    /// Set character index, this is executed by the images button
    /// </summary>
    /// <param name="_Index"></param>
    public void SetCharacterIndex(int _Index) { characterIndex = _Index; }

    /// <summary>
    /// Set skin index, this method is executed by scroll snap
    /// </summary>
    /// <param name="_Index"></param>
    public void SetSkinIndex(int _Index) { skinIndex = _Index; }
}
