using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Main Manager class that it manages main menu scene.
/// Button control and loading screen
/// @Tomislav Marković
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Text mesh pro")] [SerializeField]
    private TMP_Text highScoreText;
    [SerializeField] private TMP_Text percentText;

    [Header("Game Objects")] [SerializeField]
    private GameObject mainPanel;
    [SerializeField] private GameObject loadingPanel;

    [Header("String")] [SerializeField] string sceneName = "Game";
    [Header("Slider")] [SerializeField] private Slider percentSlider;
    private Coroutine loadingCoroutine;

    private void Start()
    {
        mainPanel.Activate();
        loadingPanel.Deactivate();
        UpdateHighScoreDisplay();
    }

    public void PlayGame()
    {
        loadingCoroutine = StartCoroutine(LoadLevel());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void UpdateHighScoreDisplay()
    {
        highScoreText.text = $"HighScore: {SaveSystem.HighScore}";
    }

    IEnumerator LoadLevel()
    {
        mainPanel.Deactivate();
        loadingPanel.Activate();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
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

    private void OnDisable()
    {
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }
    }
}