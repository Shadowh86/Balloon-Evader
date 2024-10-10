using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class is manager that controls all UI operations and elements
/// Author: @Tomislav Marković
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Text mesh Pro")] [SerializeField]
    private TMP_Text scoreText;

    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text maxBalloonsFlyAway;
    [SerializeField] private TMP_Text percentText;
    [Header("String")] [SerializeField] private string highScoreBeginText = "HighScore: ";

    [Header("Game Objects")] [SerializeField]
    private GameObject mainPanel;

    [SerializeField] private GameObject loadingPanel;

    [Header("Floats")] [SerializeField] private float shakeDuration = 0.4f;
    [SerializeField] private float shakeStrength = 40f;

    [Space(5)] [Header("Slider")] [SerializeField]
    private Slider percentSlider;

    private Coroutine loadCoroutine;

    private void Awake()
    {
        if (scoreText == null || highScoreText == null)
        {
            Debug.LogError("Score Text is not assigned in the UIManager.");
        }
    }

    private void Start()
    {
        UpdateScoreDisplay(0);
        UpdateHighScoreDisplay();

        mainPanel.Activate();
        loadingPanel.Deactivate();
    }

    private void UIGameOver()
    {
        loadCoroutine = StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        mainPanel.Deactivate();
        loadingPanel.Activate();

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

    private void UpdateUI(int newScore)
    {
        UpdateScoreDisplay(newScore);
        UpdateHighScoreDisplay();
    }

    private void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
            scoreText.rectTransform.DOScale(shakeStrength, shakeDuration).OnComplete(ResetTween);
        }
    }

    void ResetTween()
    {
        scoreText.rectTransform.DOScale(1, shakeDuration);
    }

    private void UpdateHighScoreDisplay()
    {
        highScoreText.text = highScoreBeginText + SaveSystem.HighScore;
    }

    private void UpdateFlyBalloons(int flyBalloons)
    {
        FlownBalloons(flyBalloons);
    }

    private void FlownBalloons(int flyBalloons)
    {
        maxBalloonsFlyAway.text = $"Flown balloons: {flyBalloons}";
    }

    private void OnEnable()
    {
        EventManager.UIEvent.OnUIUpdate += UpdateUI;
        EventManager.UIEvent.OnUIMaxLivesUpdate += UpdateFlyBalloons;
        EventManager.GameManagerEvent.OnGameOver += UIGameOver;
    }

    private void OnDisable()
    {
        EventManager.UIEvent.OnUIUpdate -= UpdateUI;
        EventManager.UIEvent.OnUIMaxLivesUpdate -= UpdateFlyBalloons;
        EventManager.GameManagerEvent.OnGameOver -= UIGameOver;

        if (loadCoroutine != null)
        {
            StopCoroutine(loadCoroutine);
            loadCoroutine = null;
        }
    }
}