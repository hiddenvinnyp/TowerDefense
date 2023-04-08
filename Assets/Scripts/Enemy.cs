using SpaceShooter;
using UnityEngine;
using UnityEditor;

namespace TowerDefence
{
    [RequireComponent (typeof(PathPatrol))]
    public class Enemy : MonoBehaviour
    {
        public void Use(EnemyAsset asset)
        {
            var spriterenderer = transform.Find("ViewModel").GetComponent<SpriteRenderer>();
            spriterenderer.color = asset.Color;
            spriterenderer.transform.localScale = new Vector3(asset.SpriteScale.x, asset.SpriteScale.y, 1);

            spriterenderer.GetComponent<Animator>().runtimeAnimatorController = asset.AnimatorController;

            GetComponent<SpaceShip>().Use(asset);

            GetComponentInChildren<CircleCollider2D>().radius = asset.ColliderRadius;

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