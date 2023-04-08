using UnityEditor;
using UnityEngine;

namespace SpaceShooter
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float m_Lifetime;
        public float LifeTime { set { m_Lifetime = value; } }

        private float m_Timer;

        private void Update()
        {
             if (m_Timer < m_Lifetime)            
                m_Timer += Time.deltaTime;
             else
                Destroy(gameObject);
        }
    }
}
