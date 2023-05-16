using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerDefence
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        [SerializeField] private UpgradeAsset m_RadiusUpgrade;
        [SerializeField] private float m_RadiusUpgradeFactor = 0.1f;
        [SerializeField] private Color m_GizmosColor;

        [SerializeField] private float m_Lead;
        private Turret[] m_Turrents;
        private Rigidbody2D m_Target = null;

        #region UnityEvents
        private void Start()
        {
            m_Turrents = GetComponentsInChildren<Turret>();
            if (m_RadiusUpgrade)
            {
                var level = Upgrades.GetUpdateLevel(m_RadiusUpgrade);
                m_Radius += level * m_RadiusUpgradeFactor;
            }
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
                        turret.transform.up = 
                            m_Target.transform.position - turret.transform.position + (Vector3)m_Target.velocity * m_Lead;
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
                    m_Target = enter.transform.root.GetComponent<Rigidbody2D>();                    
                }
            }
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = m_GizmosColor;
            Gizmos.DrawSphere(transform.position, m_Radius);
        }

        public void Use(TowerAsset towerAsset)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.TowerSprite;
            m_Turrents = GetComponentsInChildren<Turret>();

            foreach (var turret in m_Turrents)
            {
                turret.AssignLoadOut(towerAsset.Properties);
            }

            GetComponentInChildren<BuildPlace>().SetBuildableTowers(towerAsset.m_UpgradesTo);
        }
    }
}
