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
    }
}