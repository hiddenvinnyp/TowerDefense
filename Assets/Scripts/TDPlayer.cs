using SpaceShooter;
using UnityEngine;
using System;
using System.Collections;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        [SerializeField] private int m_Mana;
        public int Mana => m_Mana;
        [SerializeField] private float m_AccumulateTime = 1;
        [SerializeField] private int m_ManaPerTime = 1;
        public int ManaPerTime { set { m_ManaPerTime = value; } }
        [SerializeField] private int m_Gold;

        public static new TDPlayer Instance => Player.Instance as TDPlayer;
        private event Action<int> OnGoldUpdate;
        public void GoldUpdateSubscribe(Action<int> action)
        {
            OnGoldUpdate += action;
            action(Instance.m_Gold);
        }

        public event Action<int> OnLifeUpdate;
        public event Action<int> OnManaUpdate;
        public void LifeUpdateSubscribe(Action<int> action)
        {
            OnLifeUpdate += action;
            action(Instance.NumLives);
        }
        
        public void ManaUpdateSubscribe(Action<int> action)
        {
            OnManaUpdate += action;
            action(m_Mana);
        }

        private float manaTime;

        private void Start()
        {
            manaTime = m_AccumulateTime;
        }

        private void Update()
        {
            if (manaTime <= 0 && NumLives > 0)
            {
                ChangeMana(m_ManaPerTime);
                manaTime = m_AccumulateTime;
            }
            manaTime -= Time.deltaTime;
        }

        public void ChangeMana(int mana)
        {
            m_Mana += mana;
            if (m_Mana < 0) m_Mana = 0;
            OnManaUpdate(m_Mana);
        }

        public void ChangeGold(int gold)
        {
            m_Gold += gold;
            OnGoldUpdate(m_Gold);
        }
        public void ReduceLife(int hits)
        {
            TakeDamage(hits);
            OnLifeUpdate(NumLives);
        }

        [SerializeField] private Tower m_TowerPrefab;
        //TODO: верим, что золота на постройку достаточно
        public void TryBuild(TowerAsset towerAsset, Transform buildPlace)
        {
            if (m_Gold >= towerAsset.GoldCost)
            {
                ChangeGold(-towerAsset.GoldCost);
                var tower = Instantiate(m_TowerPrefab, buildPlace.position, Quaternion.identity);
                tower.Use(towerAsset);
                Destroy(buildPlace.gameObject);
            }
        }

        [SerializeField] private UpgradeAsset m_HealthUpgrade;
        private new void Awake()
        {
            base.Awake();
            var level = Upgrades.GetUpdateLevel(m_HealthUpgrade);
            TakeDamage(-level * 5);
        }
    }
}