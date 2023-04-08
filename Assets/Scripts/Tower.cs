using SpaceShooter;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerDefence
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        [SerializeField] private Color m_GizmosColor;
        private Turret[] m_Turrents;
        private Destructible m_Target = null;

        #region UnityEvents
        private void Start()
        {
            m_Turrents = GetComponentsInChildren<Turret>();
        }

        private void Update()
        {
            if (m_Target)
            {
                Vector2 targetVector = m_Target.transform.position - transform.position;
                if (targetVector.magnitude <= m_Radius)
                {
                    foreach (var turret in m_Turrents)
                    {
                        turret.transform.up = targetVector;
                        turret.Fire();
                    }
                } else
                {
                    m_Target = null;
                }
            }
            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);
                if (enter)
                {
                    m_Target = enter.transform.root.GetComponent<Destructible>();                    
                }
            }
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = m_GizmosColor;
            Gizmos.DrawSphere(transform.position, m_Radius);
        }
    }
}
