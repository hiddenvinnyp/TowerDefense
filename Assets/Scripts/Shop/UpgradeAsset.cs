using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class UpgradeAsset : ScriptableObject
    {
        [Header("������� ���")]
        public Sprite Icon;

        public int[] CostByLevel = { 3};
    }
}
