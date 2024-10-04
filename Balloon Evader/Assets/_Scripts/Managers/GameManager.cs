using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeToSpawnNewBalloon;

    #region Score Region

    private int maxBalloonsFlyAway = 3;

    public int MaxBalloonsFlyAway
    {
        get { return maxBalloonsFlyAway; }
        set
        {
            maxBalloonsFlyAway = value;
            OnBalloonsFlyAway();
            if (MaxBalloonsFlyAway <= 0)
            {
                GameOver();
            }
        }
        
    }

    private int score;

    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            SaveSystem.UpdateHighScore(score);
            EventManager.UIEvent.OnUIUpdate(score);
        }
    }

    private void Start()
    {
        StartCoroutine(IncreaseBalloonNumber());
        OnBalloonsFlyAway();
        print(Score);
    }

    void UpdateScore(int scoreChanged)
    {
        Score += scoreChanged;
    }

    IEnumerator IncreaseBalloonNumber()
    {
        yield return new WaitForSeconds(timeToSpawnNewBalloon);
        RequestNewBalloon();
        StartCoroutine(IncreaseBalloonNumber());
    }

    private void RequestNewBalloon()
    {
        EventManager.GameManagerEvent.OnSpawnNewBalloon?.Invoke();
    }

    private void OnBalloonsFlyAway()
    {
        // send event to update max lives
        EventManager.UIEvent.OnUIMaxLivesUpdate?.Invoke(MaxBalloonsFlyAway);
    }

    private void UpdateFlyBalloons(int flyBalloons)
    {
        MaxBalloonsFlyAway -= flyBalloons;
    }

    private void GameOver()
    {
        StopAllCoroutines();
        EventManager.GameManagerEvent.OnGameOver?.Invoke();
    }

    #endregion

    #region Subscribe To Events

    private void OnEnable()
    {
        EventManager.GameManagerEvent.OnScoreChanged += UpdateScore;
        EventManager.GameManagerEvent.OnFlyBalloonUpdate += UpdateFlyBalloons;
    }

    private void OnDisable()
    {
        EventManager.GameManagerEvent.OnScoreChanged -= UpdateScore;
        EventManager.GameManagerEvent.OnFlyBalloonUpdate -= UpdateFlyBalloons;
    }

    #endregion
}