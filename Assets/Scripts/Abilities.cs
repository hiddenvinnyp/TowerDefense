using SpaceShooter;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class Abilities : SingletonBase<Abilities>
    {
        [Serializable]
        public class FireAbility
        {
            [SerializeField] private int m_Cost = 5;
            public int Cost => m_Cost;
            [SerializeField] private int m_Damage = 2;
            public void SetDamage(int factor) { m_Damage += m_Damage * factor; }
            [SerializeField] private Color m_TargetingColor;

            public void TryUse()
            {
                if (TDPlayer.Instance.Mana >= m_Cost)
                {
                    TDPlayer.Instance.ChangeMana(-m_Cost);

                    ClickProtection.Instance.Activate((Vector2 v) =>
                    {
                        Vector3 position = v;
                        position.z = Camera.main.transform.position.z;
                        position = Camera.main.ScreenToWorldPoint(position);
                        foreach (var collider in Physics2D.OverlapCircleAll(position, 5))
                        {
                            if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                            {
                                enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Magic);
                            }
                        }
                    });
                }
            }
        }

        [Serializable]
        public class TimeAbility
        {
            [SerializeField] private int m_Cost = 10;
            public int Cost => m_Cost;
            [SerializeField] private float m_Cooldown = 10f;
            [SerializeField] private float m_Duration = 5f;
            public void SetDuration(float duration) { m_Duration += duration; }
            public bool IsCooldown;
            public void TryUse()
            {
                void Slow(Enemy enemy)
                {
                    enemy.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
                }

                IEnumerator Restore()
                {
                    yield return new WaitForSeconds(m_Duration);
                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                        ship.RestoreMaxLinearVelocity();
                    EnemyWaveManager.OnEnemySpawn -= Slow;
                }

                IEnumerator TimeAbilityButton()
                {
                    Instance.m_TimeButton.interactable = false;
                    IsCooldown = true;
                    yield return new WaitForSeconds(m_Cooldown);
                    Instance.m_TimeButton.interactable = true;
                    IsCooldown = false;
                }

                if (TDPlayer.Instance.Mana >= m_Cost)
                {
                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                    {
                        ship.HalfMaxLinearVelocity();
                    }
                    EnemyWaveManager.OnEnemySpawn += Slow;

                    Instance.StartCoroutine(Restore());

                    Instance.StartCoroutine(TimeAbilityButton());
                }
            }
        }
        //TODO: rewrite ALL
        [SerializeField] private Image m_TargetCircle;
        [SerializeField] private Button m_FireButton;
        [SerializeField] private Button m_TimeButton;
        [SerializeField] private TextMeshProUGUI m_FireCostText;
        [SerializeField] private TextMeshProUGUI m_TimeCostText;
        [SerializeField] private UpgradeAsset m_FireUpgradeAsset;
        [SerializeField] private UpgradeAsset m_TimeUpgradeAsset;
        [SerializeField] private FireAbility m_FireAbility;
        [SerializeField] private TimeAbility m_TimeAbility;

        public void UseFireAbility() => m_FireAbility.TryUse();
        public void UseTimeAbility() => m_TimeAbility.TryUse();

        private void Start()
        {
            var fireLevel = Upgrades.GetUpdateLevel(m_FireUpgradeAsset);
            if (fireLevel <= 0)
                m_FireButton.gameObject.SetActive(false);
            else
                m_FireAbility.SetDamage(fireLevel);

            var freezeLevel = Upgrades.GetUpdateLevel(m_TimeUpgradeAsset);
            if (freezeLevel <= 0)
                m_TimeButton.gameObject.SetActive(false);
            else
                m_TimeAbility.SetDuration(freezeLevel);

            TDPlayer.Instance.ManaUpdateSubscribe(ManaStatusCheck);
            m_FireCostText.text = m_FireAbility.Cost.ToString();
            m_TimeCostText.text = m_TimeAbility.Cost.ToString();
        }

        private void ManaStatusCheck(int mana)
        {
            if (mana >= m_FireAbility.Cost != m_FireButton.interactable)
            {
                m_FireButton.interactable = !m_FireButton.interactable;
                m_FireCostText.color = m_FireButton.interactable ? Color.white : Color.red;
            }

            if (((mana >= m_TimeAbility.Cost) && !m_TimeAbility.IsCooldown) != m_TimeButton.interactable)
            {
                m_TimeButton.interactable = !m_TimeButton.interactable;
                m_TimeCostText.color = m_TimeButton.interactable ? Color.white : Color.red;
            }
        }
    }
}
