using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private GameData gameData = null;

    private void Start()
    {
        Referee.onGameOver += GiveStars;
    }

    private void GiveStars(LevelStatus _levelStatus)
    {
        if (_levelStatus == LevelStatus.Defeat) return;

        //TODO check points and give stars
    }
}
