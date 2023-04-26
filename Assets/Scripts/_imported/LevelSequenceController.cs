using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    /// <summary>
    /// Контроллер перехода между уровнями. Должен быть с пометкой DoNotDestroyOnLoad.
    /// </summary>
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneName = "LevelMap";
        public static SpaceShip PlayerShip { get; set; }
        public Episode CurrentEpisode { get; private set; }
        public int CurrentLevel { get; private set; }
        public bool LastLevelResult { get; private set; }
        public PlayerStatistics LevelStatistics { get; private set; }

        public void StartEpisode(Episode episode)
        {
            CurrentEpisode = episode;
            CurrentLevel = 0;

            // Тут сбросить статы перед началом эпизода
            LevelStatistics = new PlayerStatistics();
            LevelStatistics.ResetStats();

            SceneManager.LoadScene(episode.Levels[CurrentLevel]);
        }

        public void RestartLevel()
        {
            //SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            SceneManager.LoadScene(1);
        }

        public void FinishCurrentLevel(bool success)
        {
            LastLevelResult = success;
            //CalculateLevelStatistic();

            ResultPanelController.Instance.ShowResults(success);

            // Save stats
            //SaveSystem.Instance.Load();

            //SaveStatistics stats = SaveSystem.Instance.m_Stats;
            //stats.NumKills += LevelStatistics.NumKills;
            //stats.Score += LevelStatistics.Score;
            //stats.Time += LevelStatistics.Time;

            //SaveSystem.Instance.Save();

            //if (success)            
            //    AdvanceLevel();
            //if (!success)
            //{
            //    SceneManager.LoadScene(MainMenuSceneName);
            //}
        }

        public void AdvanceLevel()
        {
            //LevelStatistics.ResetStats();

            CurrentLevel++;

            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneName);
            } else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }

        private void CalculateLevelStatistic()
        {
            LevelStatistics.Time = LevelController.Instance.LevelTime;
            LevelStatistics.TimeBonus = CalculateTimeBonus(LevelController.Instance.LevelTime);
            LevelStatistics.Score = Player.Instance.Score + Player.Instance.Score * LevelStatistics.TimeBonus;
            LevelStatistics.NumKills = Player.Instance.NumKill;
        }

        private int CalculateTimeBonus(float levelTime)
        {
            int bonus = 5;
            if (levelTime > 30)
                bonus--;
            if (levelTime > 60)
                bonus--;
            if (levelTime > 120)
                bonus--;
            if (levelTime > 180)
                bonus--;
            if (levelTime > 300)
                bonus--;

            return bonus;
        }
    }
}
