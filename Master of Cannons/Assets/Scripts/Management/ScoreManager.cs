using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private GameData gameData = null;

    private PlayerLevelsData playerLevelsData = null;

    private int points = 0;

    public event Delegates.Action<int> OnPointsUpdated = null;

    public byte Stars { get; private set; } = 0;

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

        Stars = 0;

        if (points >= gameData.currentLevelData.pointsOneStar && points < gameData.currentLevelData.pointsTwoStars)
        {
            Stars = 1;
        }
        else if (points >= gameData.currentLevelData.pointsTwoStars && points < gameData.currentLevelData.pointsThreeStars)
        {
            Stars = 2;
        }
        else if (points >= gameData.currentLevelData.pointsThreeStars)
        {
            Stars = 3;
        }

        if (gameData.currentLevelData.number <= playerLevelsData.levelsStars.Count && playerLevelsData.levelsStars.Count > 0)
        {
            if (Stars > playerLevelsData.levelsStars[gameData.currentLevelData.number - 1])
            {
                playerLevelsData.levelsStars[gameData.currentLevelData.number - 1] = Stars;
                //TODO save json in firebase
            } 
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
