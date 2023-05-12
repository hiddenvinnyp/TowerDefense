using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class TowerAsset: ScriptableObject
    {
        public int GoldCost = 15;
        public Sprite TowerSprite;
        public Sprite GUISprite;
        public TurretProperties Properties;
        [SerializeField] private UpgradeAsset RequiredUpgrade;
        [SerializeField] private int RequiredUpgradeLevel;
        public bool IsAvaliable() =>
            !RequiredUpgrade || RequiredUpgradeLevel <= Upgrades.GetUpdateLevel(RequiredUpgrade);
        public TowerAsset[] m_UpgradesTo;
    }
}