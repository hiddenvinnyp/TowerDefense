using SpaceShooter;
using UnityEngine;
using UnityEditor;
using System;

namespace TowerDefence
{
    [RequireComponent (typeof(PathPatrol))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int m_Damage = 1;
        [SerializeField] private int m_Gold = 1;

        public event Action OnEnd;

        private void OnDestroy()
        {
            OnEnd?.Invoke();
        }

        public void Use(EnemyAsset asset)
        {
            var spriterenderer = transform.Find("ViewModel").GetComponent<SpriteRenderer>();
            spriterenderer.color = asset.Color;
            spriterenderer.transform.localScale = new Vector3(asset.SpriteScale.x, asset.SpriteScale.y, 1);

            spriterenderer.GetComponent<Animator>().runtimeAnimatorController = asset.AnimatorController;

            GetComponent<SpaceShip>().Use(asset);

            GetComponentInChildren<CircleCollider2D>().radius = asset.ColliderRadius;

            m_Damage = asset.Damage;
            m_Gold = asset.Gold;
        }
        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_Damage);
        }

        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(m_Gold);
        }
    }

    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnemyAsset asset = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
            if (asset)
                (target as Enemy).Use(asset);
        }
    }
}