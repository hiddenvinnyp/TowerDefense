using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class EnemySpawner : Spawner
    {
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private Path m_Path;
        [SerializeField] private EnemyAsset[] m_EnemyAssets;

        protected override GameObject m_GenerateSpawnedEntity()
        {
            var enemy = Instantiate(m_EnemyPrefab);
            enemy.Use(m_EnemyAssets[Random.Range(0, m_EnemyAssets.Length)]);
            enemy.GetComponent<PathPatrol>().SetPath(m_Path);            
            
            return enemy.gameObject;
        }
    }
}
