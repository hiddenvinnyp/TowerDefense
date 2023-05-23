using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TowerDefence;

namespace SpaceShooter
{
    /// <summary>
    /// Destructible object at a scene. It has hitpoints.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// Object ignores the damage.
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible { get { return m_Indestructible; } set { m_Indestructible = value; } }

        /// <summary>
        /// Starting amount of hitpoints.
        /// </summary>
        [SerializeField] private int m_HitPoint;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        /// <summary>
        /// Current hitpoints.
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        [SerializeField] private GameObject m_ExplosionPrefab;

        private float totalDuration;
        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoint;
        }
        #endregion

        #region Public API
        /// <summary>
        /// Apply damage to the object.
        /// </summary>
        /// <param name="damage">Damage amount</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;
            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
                OnDeath();
        }
        #endregion

        /// <summary>
        /// Overridable object destruction event. Happens when hitpoints below 0.
        /// </summary>
        protected virtual void OnDeath()
        {
            if (m_ExplosionPrefab == null)
            {
                EventOnDeathInvoke();
                return;
            }

            Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
            /*ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
            totalDuration = particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier;*/
            //Destroy(explosion, totalDuration);
            
            //Invoke("EventOnDeathInvoke", totalDuration);
            EventOnDeathInvoke();
        }

        private void EventOnDeathInvoke()
        {
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();
        }

        #region Destructibles List
        private static HashSet<Destructible> m_AllDestructibles;
        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructible>();
            }
            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }
        #endregion

        #region Teams
        public const int TeamIdNeutral = 0;
        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;
        #endregion

        #region Score
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
        #endregion

        protected void Use(EnemyAsset asset)
        {
            m_HitPoint = asset.Hitpoints;
            m_ScoreValue = asset.Score;
        }
    }
}
