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
        
        public void PerformAction(object actionData)
        {
            // Cast the object to the expected type (in this case, a System.Action)
            var action = (System.Action)actionData;

            // Execute the passed functionality
            action.Invoke();
        }
    }

    public class NetworkEvents
    {
        public UnityAction onConnect;
        public UnityAction onDisconnect;
    }
}