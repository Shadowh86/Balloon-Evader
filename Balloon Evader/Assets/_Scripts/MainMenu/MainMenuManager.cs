using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
   [SerializeField] private TMP_Text highScoreText;
   [SerializeField] private GameObject mainPanel;
   [SerializeField] private GameObject loadingPanel;
   [SerializeField] private Slider percentSlider;
   [SerializeField] private TMP_Text percentText;
   [SerializeField] string  sceneName = "Game";

   private Coroutine loadingCoroutine;
   private void Start()
   {
      mainPanel.SetActive(true);
      loadingPanel.SetActive(false);
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
      mainPanel.SetActive(false);
      loadingPanel.SetActive(true);
      
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
