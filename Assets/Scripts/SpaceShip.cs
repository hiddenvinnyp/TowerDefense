using TowerDefence;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;

        /// <summary>
        /// Automatically set up rigidbody mass.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Pushing forward force.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Spin force.
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// Max linear velocity.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        public float Speed => m_MaxLinearVelocity;

        /// <summary>
        /// Max angular velocity in grad per sec.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;
        public float Agility => m_MaxAngularVelocity;

        [SerializeField] private Turret[] m_Turrets;

        [Space(10)]
        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        [Space(10)]
        [SerializeField] private GameObject m_ShieldPrefab;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;
        private Timer m_SpeedupTimer;
        private float m_MultipliarSpeed;

        /// <summary>
        /// Rigidbody link.
        /// </summary>
        private Rigidbody2D m_Rigidbody;

        #region Public Api
        /// <summary>
        /// Linear thrust control from -1.0 to +1.0.
        /// </summary>
        public float ThrustControl { get; set; }
        /// <summary>
        /// Rotational thrust control from -1.0 to +1.0.
        /// </summary>
        public float TorqueControl { get; set; }

        /// <summary>
        /// TODO: используется в AIController
        /// </summary>
        /// <param name="mode"></param>
        public void Fire(TurretMode mode)
        {
            /*foreach (Turret turret in m_Turrets)
            {
                if (turret.Mode == mode)
                {
                    turret.Fire();
                }
            }*/
        }

        public void AddSpeed(float multipliarSpeed, float time)
        {
            if (m_SpeedupTimer == null)
            {
                m_MultipliarSpeed = multipliarSpeed;
                m_SpeedupTimer = new Timer(time);
                m_MaxLinearVelocity = m_MaxLinearVelocity * multipliarSpeed;
            } else
            {
                m_SpeedupTimer.Start(time);
            }

        }

        /*public void MakeIndestructible(float time)
        {
            GameObject shieldFX =  Instantiate(m_ShieldPrefab,transform.position, Quaternion.identity);
            shieldFX.GetComponent<ImpactEffect>().LifeTime = time;
            shieldFX.transform.parent = transform;

            var invulnerability = gameObject.GetComponent<Invulnerability>();
            if (invulnerability == null)
            {
                gameObject.AddComponent<Invulnerability>();
                invulnerability = gameObject.GetComponent<Invulnerability>();
            }

            invulnerability.enabled = true;
            invulnerability.SetTimer(time);
        }*/

        /*public void AddEnergy(int energy)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + energy, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        

        public void AssignWeapon(TurretProperties properties)
        {
            foreach (Turret turret in m_Turrets)
            {
                turret.AssignLoadOut(properties);
            }
        }*/
        
        /// <summary>
        /// TODO: заглушка. Используется турелями
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DrawAmmo(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: заглушка. Используется турелями
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DrawEnergy(int count)
        {
            return true;
        }
        #endregion

        #region Unity Events
        protected override void Start()
        {
            base.Start();

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Rigidbody.mass = m_Mass;

            m_Rigidbody.inertia = 1;

           // InitOffensive();
        }

        private void Update()
        {
            if(m_SpeedupTimer != null)
            {
                m_SpeedupTimer.RemoveTime(Time.deltaTime);

                if (m_SpeedupTimer.IsFinished)
                {
                    m_MaxLinearVelocity = m_MaxLinearVelocity / m_MultipliarSpeed;
                    m_SpeedupTimer = null;
                }
            }            
        }

        // Просчёт физики только в 
        private void FixedUpdate()
        {
            UpdateRigidBody();
            //UpdateEnergyRegen();
        }
        #endregion

        /// <summary>
        /// Add forces for ship movement.
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigidbody.AddForce(m_Thrust * ThrustControl * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
            m_Rigidbody.AddForce(-m_Rigidbody.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
            
            m_Rigidbody.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);
            m_Rigidbody.AddTorque(-m_Rigidbody.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        /*private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }*/

        public new void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.MoveSpeed;
            base.Use(asset);
        }
    }
}
