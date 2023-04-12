using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        [SerializeField] private int m_Gold;

        public void ChangeGold(int gold)
        {
            m_Gold += gold;
        }
    }
}