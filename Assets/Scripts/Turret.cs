using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {
        #region Properties
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;
        //public TurretProperties TurretProperties { set { m_TurretProperties = value; } }

        [SerializeField] private UpgradeAsset m_RateofFireUpgrade;
        [SerializeField] private float m_RateofFireUpgradeFactor = 0.01f;
        [SerializeField] private float m_MinUpgradeFactor = 0.01f;
        private float m_RateUpgradeFactor = 1;

        /// <summary>
        /// Cooldown
        /// </summary>
        private float m_RefireTimer;
        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;
        private AudioClip m_FireSound;
        #endregion

        #region Unity Events
        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();

            m_FireSound = m_TurretProperties.LaunchSFX;      
            
            if (m_RateofFireUpgrade)
            {
                var level = Upgrades.GetUpdateLevel(m_RateofFireUpgrade);
                m_RateUpgradeFactor -= level * m_RateofFireUpgradeFactor;
                if (m_RateUpgradeFactor <= 0)
                    m_RateUpgradeFactor = m_MinUpgradeFactor;
            }
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;
            else if (m_Mode == TurretMode.Auto)           
                Fire();            
        }
        #endregion

        #region Public API
        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship)
            {
                if (!m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage)) return;
                if (!m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage)) return;
            }

            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            if (m_Ship)
                projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire * m_RateUpgradeFactor;

            // тут звук
            if (m_FireSound != null && m_FireSound.loadState == AudioDataLoadState.Loaded)
            {                
                AudioSource.PlayClipAtPoint(m_FireSound, transform.position, 0.8f);
            }
        }

        public void AssignLoadOut(TurretProperties properties)
        {
            if (m_Mode != properties.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = properties;
        }
        #endregion
    }
}
