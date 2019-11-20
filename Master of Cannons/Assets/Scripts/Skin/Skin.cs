using UnityEngine;

[System.Serializable]
public class SkinData
{
    [SerializeField]
    private string name = "";

    [SerializeField]
    private GameObject[] skins = new GameObject[0];

    public GameObject[] Skins { get => skins; private set => skins = value; }
    public string Name { get => name; set => name = value; }
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
}