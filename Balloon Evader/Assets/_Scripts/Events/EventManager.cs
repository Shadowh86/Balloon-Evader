using System;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public class PlayerEvent
    {
        public static UnityAction<Component, int> OnScoreChanged;
        public static UnityAction<Component, int> OnUIUpdate;
        public static UnityAction<Component, int> OnUIMaxLivesUpdate;
        public static UnityAction<object, int> OnHighScoreUpdate;
        public static UnityAction<object, int> OnFlyBalloonUpdate;
        public static UnityAction<object, EventArgs> OnMethodActivate;
    }
}