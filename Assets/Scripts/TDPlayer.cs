using SpaceShooter;
using UnityEngine;
using System;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance => Player.Instance as TDPlayer;
        private event Action<int> OnGoldUpdate;
        public void GoldUpdateSubscribe(Action<int> action)
        {
            OnGoldUpdate += action;
            action(Instance.m_Gold);
        }

        public event Action<int> OnLifeUpdate;
        public void LifeUpdateSubscribe(Action<int> action)
        {
            OnLifeUpdate += action;
            action(Instance.NumLives);
        }

        [SerializeField] private int m_Gold;

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