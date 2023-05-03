using SpaceShooter;
using UnityEngine;
using System;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance => Player.Instance as TDPlayer;
        private static event Action<int> OnGoldUpdate;
        public static void GoldUpdateSubscribe(Action<int> action)
        {
            OnGoldUpdate += action;
            action(Instance.m_Gold);
        }

        public static void GoldUpdateUnSubscribe(Action<int> action)
        {
            OnGoldUpdate -= action;
        }

        public static event Action<int> OnLifeUpdate;
        public static void LifeUpdateSubscribe(Action<int> action)
        {
            OnLifeUpdate += action;
            action(Instance.NumLives);
        }

        public static void LifeUpdateUnSubscribe(Action<int> action)
        {
            OnLifeUpdate -= action;
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
                tower.GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.TowerSprite;
                tower.GetComponentInChildren<Turret>().TurretProperties = towerAsset.Properties;
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