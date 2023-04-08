using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public sealed class EnemyAsset : ScriptableObject
    {
        [Header("Appearance")]
        public Color Color = Color.white;
        public Vector2 SpriteScale = new Vector2(3,3);
        public RuntimeAnimatorController AnimatorController;

        [Header("Stats")]
        public float MoveSpeed = 1;
        public int Hitpoints;
        public int Score;
        public float ColliderRadius;
    }
}