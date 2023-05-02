using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class WaveLevelCondition : MonoBehaviour, ILevelCondition
    {
        private bool m_IsCompleted;

        private void Start()
        {
            FindObjectOfType<EnemyWaveManager>().OnAllWavesDead += () => 
            { 
                m_IsCompleted = true; 
            };
        }

        public bool IsCompleted { get {return m_IsCompleted; } }

        public string Task => "Kill them all!";
    }
}