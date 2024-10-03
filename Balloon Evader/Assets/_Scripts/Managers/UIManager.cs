using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Text Mesh Pro")] [SerializeField]
    private TMP_Text scoreText;

    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private string highScoreBeginText = "HighScore: ";
    [SerializeField] private TMP_Text maxBalloonsFlyAway;

    [Header("DoTween score shake")] [SerializeField]
    private float shakeDuration = 0.4f;

    [SerializeField] private float shakeStrength = 40f;

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
    }

    private void UpdateUI( int newScore)
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

    private void UpdateFlyBalloons( int flyBalloons)
    {
        FlownBalloons(flyBalloons);
    }

    private void FlownBalloons(int flyBalloons)
    {
        maxBalloonsFlyAway.text = $"Flown balloons: {flyBalloons}";
    }

    private void OnEnable()
    {
       // EventManager.PlayerEvent.OnUIUpdate += UpdateUI;
       EventManager.UIEvent.OnUIUpdate += UpdateUI;
       EventManager.UIEvent.OnUIMaxLivesUpdate += UpdateFlyBalloons;
        //EventManager.PlayerEvent.OnUIMaxLivesUpdate += UpdateFlyBalloons;
    }

    private void OnDisable()
    {
        //EventManager.PlayerEvent.OnUIUpdate -= UpdateUI;
        EventManager.UIEvent.OnUIUpdate -= UpdateUI;
        EventManager.UIEvent.OnUIMaxLivesUpdate -= UpdateFlyBalloons;
       // EventManager.PlayerEvent.OnUIMaxLivesUpdate -= UpdateFlyBalloons;
    }
}