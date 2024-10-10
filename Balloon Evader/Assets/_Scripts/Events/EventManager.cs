using System;

/// <summary>
/// Class that is used as a "relay" for communication between classes
/// @Tomislav Marković
/// </summary>
public static class EventManager
{
    /// <summary>
    /// Events that are send to Game Manager
    /// </summary>
    public class GameManagerEvent
    {
        public static Action<int> OnScoreChanged;
        public static Action<int> OnFlyBalloonUpdate;
        public static Action OnGameOver;
    }

    /// <summary>
    /// Events that are send to Spawn Manager
    /// </summary>
    public class SpawnEvent
    {
        public static Action OnSpawnNewBalloon;
    }

    /// <summary>
    /// Events that are send to UI manager
    /// </summary>
    public class UIEvent
    {
        public static Action<int> OnUIUpdate;
        public static Action<int> OnUIMaxLivesUpdate;
    }
}