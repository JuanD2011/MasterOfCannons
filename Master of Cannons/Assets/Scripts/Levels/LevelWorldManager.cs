using UnityEngine;

public class LevelWorldManager : Singleton<LevelWorldManager>
{
    [SerializeField] private TextAsset levelDatasText = null;

    private LevelDataCollection levelDataCollection = new LevelDataCollection();

    public Level[] Levels { get; private set; } = null;

    protected override void OnAwake()
    {
        Levels = GetComponentsInChildren<Level>();

        levelDataCollection = Memento.DeserializeData<LevelDataCollection>(levelDatasText.text);//TODO Get text asset from firebase

        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].InitializeLevelData(levelDataCollection.levelDatas[i]);
        }
    }
}
