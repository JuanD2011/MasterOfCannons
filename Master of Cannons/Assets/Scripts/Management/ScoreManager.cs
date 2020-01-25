using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private GameData gameData = null;

    private PlayerLevelsData playerLevelsData = null;
    private PlayerData playerData = null;

    private int points = 0;

    public event Delegates.Action<int> OnPointsUpdated = null;

    public byte Stars { get; private set; } = 0;

    protected override void OnAwake()
    {
        playerLevelsData = Resources.Load<PlayerLevelsData>("Scriptable Objects/Player Levels Data");//TODO Load this from firebase
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");//TODO Load this from firebase
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
            Debug.Log("one star");
        }
        else if (points >= gameData.currentLevelData.pointsTwoStars && points < gameData.currentLevelData.pointsThreeStars)
        {
            Stars = 2;
            Debug.Log("two stars");
        }
        else if (points >= gameData.currentLevelData.pointsThreeStars)
        {
            Stars = 3;
            Debug.Log("three stars");
        }

        if (playerLevelsData.levelsStars.Count > 0)
        {
            byte levelCurrentStars = playerLevelsData.levelsStars[gameData.currentLevelData.number - 1].stars;

            if (Stars > levelCurrentStars)
            {
                playerData.stars += Stars - levelCurrentStars;
                playerLevelsData.levelsStars[gameData.currentLevelData.number - 1].stars = Stars;
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
