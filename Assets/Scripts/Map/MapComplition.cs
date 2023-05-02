using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace TowerDefence
{
    public class MapComplition : SingletonBase<MapComplition>
    {
        public const string filename = "complition.dat";

        [Serializable]
        private class EpisodeScore
        {
            public Episode Episode;
            public int Score;
        }
        
        public static void SaveEpisodeResult(int levelScore)
        {
            if (Instance)
                Instance.SaveResult(LevelSequenceController.Instance.CurrentEpisode, levelScore);
            else
                Debug.Log($"Episode Final Score: {levelScore}");
        }

        [SerializeField] private EpisodeScore[] m_ComplitionData;
        private int m_TotalScore;
        public int TotalScore => m_TotalScore;

        private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(filename, ref m_ComplitionData);
            foreach (var episodeScore in m_ComplitionData)
            {
                m_TotalScore += episodeScore.Score;
            }
        }

        private void SaveResult(Episode currentEpisode, int levelScore)
        {
            foreach (var item in m_ComplitionData)
            {
                if (item.Episode == currentEpisode)
                {
                    if (levelScore > item.Score)
                    {
                        item.Score = levelScore;
                        Saver<EpisodeScore[]>.Save(filename, m_ComplitionData);
                    }
                }
            }
        }

        public int GetEpisodeScore(Episode m_Episode)
        {
            foreach (var data in m_ComplitionData)
            {
                if (data.Episode == m_Episode)
                {
                    return data.Score;
                }
            }
            return 0;
        }
    }
}
