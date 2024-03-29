using SpaceShooter;
using TowerDefence;
#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
#endif
using UnityEngine;

namespace SpaceShooter
{
    public class Projectile : Entity
    {
        [SerializeField] protected float m_Velocity;
        [SerializeField] protected float m_Lifetime;
        [SerializeField] protected int m_Damage;
        [SerializeField] private GameObject m_ImpactEffectPrefab;

        [SerializeField] private bool m_IsAOE;
        [SerializeField] private GameObject m_AOEPrefab;

        [SerializeField] private UpgradeAsset m_VelocityUpgrade;
        [SerializeField] private float m_VelocityUpgradeFactor = 1;

        protected float m_Timer;
        protected Destructible m_Parent;

        private bool m_IsPlayer;

        public void SetFromOtherProjectile(Projectile other)
        {
            other.GetData(out m_Velocity, out m_Lifetime, out m_Damage, out m_ImpactEffectPrefab, 
                            out m_IsAOE, out m_AOEPrefab, out m_VelocityUpgrade, out m_VelocityUpgradeFactor);
        }
        private void GetData(out float m_Velocity, out float m_Lifetime, out int m_Damage, out GameObject m_ImpactEffectPrefab, out bool m_IsAOE, out GameObject m_AOEPrefab, out UpgradeAsset m_VelocityUpgrade, out float m_VelocityUpgradeFactor)
        {
            m_Velocity = this.m_Velocity;
            m_Lifetime = this.m_Lifetime;
            m_Damage = this.m_Damage;
            m_ImpactEffectPrefab = this.m_ImpactEffectPrefab;
            m_IsAOE = this.m_IsAOE;
            m_AOEPrefab = this.m_AOEPrefab;
            m_VelocityUpgrade = this.m_VelocityUpgrade;
            m_VelocityUpgradeFactor = this.m_VelocityUpgradeFactor;
        }

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
            if (Player.Instance != null && m_Parent == Player.Instance.ActiveShip)
                m_IsPlayer = true;
        }

        public void SetTarget(Destructible target)
        {

        }

        private void Start()
        {
            if (m_VelocityUpgrade)
            {
                var level = Upgrades.GetUpdateLevel(m_VelocityUpgrade);
                m_Velocity += level * m_VelocityUpgradeFactor;
            }
        }

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);
            // ��������� � ��������� ������� => ������� Physics2D:
            // - queries hit triggers
            // - queries start in collider

            if (hit)
            {
                OnHit(hit);
                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.deltaTime;
            if (m_Timer > m_Lifetime)
            {
                SpawnAreaDamage(new Vector2(transform.position.x + step.x, transform.position.y + step.y));
                Destroy(gameObject);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }

        protected virtual void OnHit(RaycastHit2D hit)
        {
            Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();
            if (destructible != null && destructible != m_Parent)
            {
                destructible.ApplyDamage(m_Damage);

                if (m_IsPlayer)
                {
                    Player.Instance.AddScore(destructible.ScoreValue);

                    if (destructible.transform.root.GetComponent<SpaceShip>() && destructible.HitPoints <= 0)
                    {
                        Player.Instance.AddKill();
                    }
                }
            }
        }

        protected void OnProjectileLifeEnd(Collider2D collider, Vector2 position)
        {
            // ��� �������� �����
            if (m_ImpactEffectPrefab != null)
            {
                GameObject effect = Instantiate(m_ImpactEffectPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
                if (effect.GetComponent<Freeze>())
                {
                    effect.GetComponent<Freeze>().SetTarget(collider.transform.root.GetComponent<Destructible>());
                }
            }
            if (m_IsAOE)
                SpawnAreaDamage(position);

            Destroy(gameObject);
        }

        private void SpawnAreaDamage(Vector2 position)
        {
            if (m_AOEPrefab == null) return;
            
            Instantiate(m_AOEPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
            
        }
    }
}
#if UNITY_EDITOR
namespace TowerDefence
{
    [CustomEditor(typeof(Projectile))]
    public class ProjectileInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create TDProjectile"))
            {
                var target = this.target as Projectile;
                var tdproj = target.AddComponent<TDProjectile>();
                tdproj.SetFromOtherProjectile(target);
            }
        }
    }
}
#endif