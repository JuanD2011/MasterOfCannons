using UnityEngine;

[System.Serializable]
public class Skin
{
    [SerializeField]
    private string name = "";

    [SerializeField]
    private GameObject[] skins = new GameObject[0];

    [SerializeField]
    private GameObject currentSkin = null;

    public GameObject[] Skins { get => skins; private set => skins = value; }
    public GameObject CurrentSkin { get => currentSkin; set => currentSkin = value; }
}