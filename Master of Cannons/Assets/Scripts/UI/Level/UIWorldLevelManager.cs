using UnityEngine;

public class UIWorldLevelManager : MonoBehaviour
{
    private UILevel[] uILevels = null;

    private void Awake()
    {
        uILevels = GetComponentsInChildren<UILevel>();
    }

    private void Start()
    {
        //TODO check if downloading levels data from firebase dont affect this start
        //In start becuase we need level world manager to load asset and initialize levels data
        for (int i = 0; i < uILevels.Length; i++)
        {
            uILevels[i].Initialize(LevelWorldManager.Instance.Levels[i].LevelData);
        }
    }
}
