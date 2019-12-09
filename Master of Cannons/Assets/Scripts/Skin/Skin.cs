using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [SerializeField]
    private string name = "";

    [SerializeField]
    private CharacterType characterType = CharacterType.None;

    [SerializeField]
    private GameObject[] skins = new GameObject[0];

    public GameObject[] Skins { get => skins; private set => skins = value; }
    public string Name { get => name; set => name = value; }
    public CharacterType CharacterType { get => characterType; private set => characterType = value; }
}

[System.Serializable]
public class PlayerSkin
{
    [SerializeField]
    private string name = "";

    [SerializeField]
    private GameObject Skin = null;

    public GameObject CurrentSkin { get => Skin; set => Skin = value; }
    public string Name { get => name; set => name = value; }
    public CharacterType CharacterType { get; set; } = CharacterType.None;
}