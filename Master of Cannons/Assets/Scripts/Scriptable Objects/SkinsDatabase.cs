using UnityEngine;

[CreateAssetMenu(fileName = "Skins Database", menuName = "Skins Database")]
public class SkinsDatabase : ScriptableObject
{
    public Skin[] skins = new Skin[3];
}
