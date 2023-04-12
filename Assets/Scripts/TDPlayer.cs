using SpaceShooter;
using UnityEngine;
using System;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance => Player.Instance as TDPlayer;
        public static event Action<int> OnGoldUpdate;
        public static event Action<int> OnLifeUpdate;

        [SerializeField] private int m_Gold;

        private void Start()
        {
            OnGoldUpdate(m_Gold);
            OnLifeUpdate(NumLives);
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
    }
}