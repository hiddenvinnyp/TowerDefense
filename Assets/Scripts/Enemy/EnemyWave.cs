using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWave : MonoBehaviour
    {
        [Serializable]
        private class Squad
        {
            public EnemyAsset Asset;
            public int Count;
        }

        [Serializable]
        private class PathGroup
        {
            public Squad[] Squads;
        }

        [SerializeField] private PathGroup[] m_Groups;
        [SerializeField] private float m_PrepareTime = 10f;
        private event Action OnWaveReady;

        private void Awake()
        {
            enabled = false;
        }

        public void Prepare(Action spawnEnemies)
        {
            m_PrepareTime += Time.time;
            enabled = true;
            OnWaveReady += spawnEnemies;
        }

        private void Update()
        {
            if (Time.time >= m_PrepareTime)
            {
                enabled = false;
                OnWaveReady?.Invoke();
            }
        }

        public IEnumerable<(EnemyAsset asset, int count, int pathIndex)> EnumerateSquads()
        {
            for (int i = 0; i < m_Groups.Length; i++)
            {
                foreach (var squad in m_Groups[i].Squads)
                {
                    yield return (squad.Asset, squad.Count, i);
                }
            }
        }

        [SerializeField] private EnemyWave m_NextWave;
        public EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies;
            if (m_NextWave) 
                m_NextWave.Prepare(spawnEnemies);
            return m_NextWave;
        }
    }
}
