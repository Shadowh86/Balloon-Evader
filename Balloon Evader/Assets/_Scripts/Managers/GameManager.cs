using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Loading")] [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider percentSlider;
    [SerializeField] private TMP_Text percentText;

    [Space(10)] [SerializeField] private float timeToSpawnNewBalloon;

    private Coroutine loadCoroutine;

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
            SaveSystem.UpdateHighscore(score);
            OnScoreChanged();
        }
    }

    private void Start()
    {
        mainPanel.SetActive(true);
        loadingPanel.SetActive(false);
        StartCoroutine(IncreaseBalloonNumber());
        OnBalloonsFlyAway();
        EventManager.PlayerEvent.OnHighScoreUpdate?.Invoke(this, SaveSystem.HighScore);
    }

    void UpdateScore(Component component, int scoreChanged)
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
        EventManager.PlayerEvent.OnMethodActivate?.Invoke(this,
            EventArgs.Empty); // spawn another balloon
    }

    void OnScoreChanged() // Update UI
    {
        EventManager.PlayerEvent.OnUIUpdate?.Invoke(this, Score);
        EventManager.PlayerEvent.OnHighScoreUpdate?.Invoke(this, SaveSystem.HighScore);
    }

    private void OnBalloonsFlyAway()
    {
        // send event to update max lives
        EventManager.PlayerEvent.OnUIMaxLivesUpdate?.Invoke(this, MaxBalloonsFlyAway);
    }

    private void UpdateFlyBalloons(object _object, int flyBalloons)
    {
        MaxBalloonsFlyAway -= flyBalloons;
    }

    private void GameOver()
    {
        StopAllCoroutines();
        loadCoroutine = StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        mainPanel.SetActive(false);
        loadingPanel.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            percentSlider.value = progress;
            percentText.text = $"{progress * 100:F0}%";

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    #endregion

        #region Subscribe To Events

        private void OnEnable()
        {
            EventManager.PlayerEvent.OnScoreChanged += UpdateScore;
            EventManager.PlayerEvent.OnFlyBalloonUpdate += UpdateFlyBalloons;
        }

        private void OnDisable()
        {
            EventManager.PlayerEvent.OnScoreChanged -= UpdateScore;
            EventManager.PlayerEvent.OnFlyBalloonUpdate -= UpdateFlyBalloons;
        }

        #endregion
    }