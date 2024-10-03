using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Space(10)] [SerializeField] private float timeToSpawnNewBalloon;



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