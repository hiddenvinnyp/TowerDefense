using System;
using UnityEditor.VersionControl;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public event Action OnAllWavesDead;

        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private Path[] m_Paths;
        [SerializeField] private EnemyWave m_CurrentWave;
        private int m_ActiveEnemyCount = 0;

        private void RecordEnemyDead()
        {
            if (--m_ActiveEnemyCount == 0)
            {
                    ForceNextWave();              
            }
        }

        private void Start()
        {
            m_CurrentWave.Prepare(SpawnEnemies);
        }

        private void SpawnEnemies()
        {
            foreach ((EnemyAsset asset, int count, int pathIndex) in m_CurrentWave.EnumerateSquads())
            {
                if (pathIndex < m_Paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var enemy = Instantiate(m_EnemyPrefab, m_Paths[pathIndex].StartArea.GetRandomInsideZone(), Quaternion.identity);
                        enemy.OnEnd += RecordEnemyDead;
                        enemy.Use(asset);
                        enemy.GetComponent<PathPatrol>().SetPath(m_Paths[pathIndex]);
                        m_ActiveEnemyCount++;
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid pathIndex in {name}");
                }
            }

            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnEnemies);
        }

        public void ForceNextWave()
        {
            if (m_CurrentWave)
            {
                TDPlayer.Instance.ChangeGold((int)m_CurrentWave.GetRemainingTime);
                SpawnEnemies();
            }
            else
            {
                if (m_ActiveEnemyCount == 0)
                    OnAllWavesDead?.Invoke();
            }
        }
    }
}
