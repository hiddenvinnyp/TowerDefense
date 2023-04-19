using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    [System.Serializable]
    struct Spawners
    {
        public EnemySpawner Spawner;
        public int SpawnAmount;
    }

    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private float m_DelayBetweenWavesTime;
        [SerializeField] private Spawners[] m_Spawners;

        private int m_CurrentSpawner = 0;

        private void Awake()
        {
            foreach (var spawner in m_Spawners)
            {
                spawner.Spawner.enabled = false;
            }
        }

        private void Start()
        {
            

            m_Spawners[m_CurrentSpawner].Spawner.enabled = true;
        }

        private void Update()
        {
            if (m_CurrentSpawner >= m_Spawners.Length) return;
            if (m_Spawners[m_CurrentSpawner].Spawner.NumSpawns >= m_Spawners[m_CurrentSpawner].SpawnAmount)
            {
                m_Spawners[m_CurrentSpawner].Spawner.enabled = false;
                m_CurrentSpawner++;
                if (m_CurrentSpawner >= m_Spawners.Length) return;
                Invoke("ActivateSpawner", m_DelayBetweenWavesTime);                
            }
        }

        private void ActivateSpawner()
        {
            m_Spawners[m_CurrentSpawner].Spawner.enabled = true;
        }
    }
}
