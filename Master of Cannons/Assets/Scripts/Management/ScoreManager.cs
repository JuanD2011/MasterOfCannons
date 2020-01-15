using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private GameData gameData = null;

    private PlayerLevelsData playerLevelsData = null;

    private int points = 0;

    public event Delegates.Action<int> OnPointsUpdated = null;

    protected override void OnAwake()
    {
        playerLevelsData = Resources.Load<PlayerLevelsData>("Scriptable Objects/Player Levels Data");//TODO Load this from firebase
    }

    private void Start()
    {
        Referee.OnGameOver += GiveStars;
    }

    private void GiveStars(LevelStatus _levelStatus)
    {
        if (_levelStatus == LevelStatus.Defeat) return;

        byte stars = 0;

        if (points >= gameData.currentLevelData.pointsOneStar && points < gameData.currentLevelData.pointsTwoStars)
        {
            stars = 1;
        }
        else if (points >= gameData.currentLevelData.pointsTwoStars && points < gameData.currentLevelData.pointsThreeStars)
        {
            stars = 2;
        }
        else if (points >= gameData.currentLevelData.pointsThreeStars)
        {
            stars = 3;
        }

        if (stars > playerLevelsData.levelsStars[gameData.currentLevelData.number - 1])
        {
            playerLevelsData.levelsStars[gameData.currentLevelData.number - 1] = stars;
            //TODO save json in firebase
        }
    }

    /// <summary>
    /// Add points
    /// </summary>
    /// <param name="_points"></param>
    public void AddPoints(int _points)
    {
        points += _points;
        OnPointsUpdated(points);
    }
}
