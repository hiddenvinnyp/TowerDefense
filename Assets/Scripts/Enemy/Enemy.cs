using SpaceShooter;
using UnityEngine;
using UnityEditor;
using System;

namespace TowerDefence
{
    [RequireComponent(typeof(Destructible))]
    [RequireComponent(typeof(PathPatrol))]
    public class Enemy : MonoBehaviour
    {
        public enum ArmorType
        {
            None,
            Base,
            Magic,
            Ice
        }
        private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions =
        {
            //ArmorType.None
            (int power, TDProjectile.DamageType type, int armor) =>
            {
                return power;
            },

            //ArmorType.Base
            (int power, TDProjectile.DamageType type, int armor) =>
            {
                switch (type)
                {
                    case TDProjectile.DamageType.Magic: return power;
                    case TDProjectile.DamageType.Ice: return power;
                    default: return Mathf.Max(1, power - armor);
                }                
            },

            //ArmorType.Magic
            (int power, TDProjectile.DamageType type, int armor) =>
            {
                 switch (type)
                {
                    case TDProjectile.DamageType.Base: return Mathf.Max(1, power - armor/2);
                    case TDProjectile.DamageType.Ice: return power;
                    default: return Mathf.Max(1, power - armor);
                }
            },

            //ArmorType.Ice
            (int power, TDProjectile.DamageType type, int armor) =>
            {
                switch (type)
                {
                    case TDProjectile.DamageType.Magic: return Mathf.Max(1, power - armor/2);
                    case TDProjectile.DamageType.Base: return power;
                    default: return Mathf.Max(1, power - armor);
                }
            }
        };

        [SerializeField] private int m_Damage = 1;
        [SerializeField] private int m_Gold = 1;
        [SerializeField] private int m_Armor;
        [SerializeField] private ArmorType m_ArmorType;

        public event Action OnEnd;

        private Destructible m_Destructible;

        private void Awake()
        {
            m_Destructible = GetComponent<Destructible>();
        }

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
            m_Armor = asset.Armor;
            m_ArmorType = asset.ArmorType;
        }

        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_Damage);
        }

        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(m_Gold);
        }

        public void TakeDamage(int damage, TDProjectile.DamageType damageType)
        {
            m_Destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_Armor));
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