using SpaceShooter;
using System;
using UnityEngine;

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

        private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(filename, ref m_ComplitionData);
        }

        public bool TryIndex(int id, out Episode episode, out int score)
        {
            if (id >= 0 && id < m_ComplitionData.Length)
            {
                episode = m_ComplitionData[id].Episode;
                score = m_ComplitionData[id].Score;
                return true;
            }
            episode = null;
            score = 0;
            return false;
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
    }
}
