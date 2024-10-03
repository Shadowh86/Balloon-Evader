using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Text Mesh Pro")] [SerializeField]
    private TMP_Text scoreText;

    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private string highScoreBeginText = "HighScore: ";
    [SerializeField] private TMP_Text maxBalloonsFlyAway;
    
    
    [Header("Loading")] [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider percentSlider;
    [SerializeField] private TMP_Text percentText;

    [Header("DoTween score shake")] [SerializeField]
    private float shakeDuration = 0.4f;

    [SerializeField] private float shakeStrength = 40f;
    
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
        
        mainPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    private void UIGameOver()
    {
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