
using UnityEngine;

public class SaveSystem
{
   private const string HighScoreKey = "Highscore";

   public static int HighScore
   {
      get => PlayerPrefs.GetInt(HighScoreKey, 0);
      set
      {
         PlayerPrefs.SetInt(HighScoreKey, value);
         PlayerPrefs.Save();
      }
   }

   public static void UpdateHighscore(int score)
   {
      if (score > HighScore)
      {
         HighScore = score;
      }
   }

   public static void ResetHighscore()
   {
      HighScore = 0;
   }
}
