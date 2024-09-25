using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   [SerializeField] private TMP_Text scoreText;

   private void Start()
   {
      scoreText.text = $"Score: 0";
   }

   public void UpdateUI(Component component, int newScore)
   {
      scoreText.text = $"Score: {newScore}";
   }
   private void OnEnable()
   {
      EventManager.PlayerEvent.OnUIUpdate += UpdateUI;
   }

   private void OnDisable()
   {
      EventManager.PlayerEvent.OnUIUpdate -= UpdateUI;
   }
}
