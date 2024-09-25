using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Score Region

    private int score;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            OnScoreChanged(score);
        }
    }

    void UpdateScore(Component component, int newScore)
    {
        Score += newScore;
    }

    void OnScoreChanged(int newScore) // Update UI
    {
        EventManager.PlayerEvent.OnUIUpdate(this, newScore);
    }

    #endregion

    #region Subscribe To Events

    private void OnEnable()
    {
        EventManager.PlayerEvent.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        EventManager.PlayerEvent.OnScoreChanged -= UpdateScore;
    }

    #endregion
}