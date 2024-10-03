using System;

public static class EventManager
{
    public class GameManagerEvent
    {
        public static Action<int> OnScoreChanged;
        public static Action<int> OnFlyBalloonUpdate;
        public static Action OnSpawnNewBalloon;
        public static Action OnGameOver;
    }

    public class UIEvent
    {
        public static Action<int> OnUIUpdate;
        public static Action<int> OnUIMaxLivesUpdate;
    }
}