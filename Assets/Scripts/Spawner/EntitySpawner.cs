using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class EntitySpawner : Spawner
    {
        [SerializeField] private Entity[] m_EntityPrefabs;

        protected override GameObject m_GenerateSpawnedEntity()
        {
            return Instantiate(m_EntityPrefabs[Random.Range(0, m_EntityPrefabs.Length)].gameObject);
        }
    }
}
