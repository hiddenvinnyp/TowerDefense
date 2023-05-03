using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class UpgradeAsset : ScriptableObject
    {
        [Header("Внешний вид")]
        public Sprite Icon;

        public int[] CostByLevel = { 3};
    }
}
