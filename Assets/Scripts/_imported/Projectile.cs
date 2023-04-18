using TowerDefence;
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

        protected float m_Timer;
        protected Destructible m_Parent;

        private bool m_IsPlayer;

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
            if (Player.Instance != null && m_Parent == Player.Instance.ActiveShip)
                m_IsPlayer = true;
        }

        public void SetTarget(Destructible target)
        {

        }

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);
            // Выключить в свойствах проекта => вкладка Physics2D:
            // - queries hit triggers
            // - queries start in collider

            if (hit)
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

        protected void OnProjectileLifeEnd(Collider2D collider, Vector2 position)
        {
            // тут спавнить взрыв
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