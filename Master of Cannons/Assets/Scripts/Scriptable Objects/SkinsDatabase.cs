using UnityEngine;

[CreateAssetMenu(fileName = "Skins Database", menuName = "Skins Database")]
public class SkinsDatabase : ScriptableObject
{
    public SkinData[] skins = new SkinData[3];

    public SkinData currentSkinData = new SkinData();
}
