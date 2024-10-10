using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BalloonPopper
{
    /// <summary>
    /// Class that manages score and sends data to SpawnManager.cs and to UIManager.cs
    /// 
    /// @Tomislav Markovic
    /// </summary>
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

        // score property 
        public int Score
        {
            get { return score; }
            private set
            {
                score = value;
                SaveSystem.UpdateHighScore(score); // after score is changed, update highscore
                EventManager.UIEvent.OnUIUpdate(score); // send event to UI to update score UI
            }
        }
        
        private void Start()
        {
            StartCoroutine(IncreaseBalloonNumber());
            OnBalloonsFlyAway();
        }

        // Event that is called from Balloon.cs to increase score
        void UpdateScore(int scoreChanged)
        {
            Score += scoreChanged;
        }

        // after certain time, increase balloon number on scene by sending event
        // to SpawnManager to spawn new balloon
        IEnumerator IncreaseBalloonNumber()
        {
            yield return new WaitForSeconds(timeToSpawnNewBalloon);
            RequestNewBalloon();
            StartCoroutine(IncreaseBalloonNumber());
        }

        
        private void RequestNewBalloon()
        {
            EventManager.SpawnEvent.OnSpawnNewBalloon?.Invoke();
        }

        // send event to UIManager.cs to update max lives on UI
        private void OnBalloonsFlyAway()
        {
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
}