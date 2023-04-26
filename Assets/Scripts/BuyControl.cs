using UnityEngine;

namespace TowerDefence
{
    public class BuyControl : MonoBehaviour
    {
        private RectTransform m_RectTransform;

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            BuildPlace.OnClickEvent += MoveToBuildPlace;   
            gameObject.SetActive(false);
        }

        private void MoveToBuildPlace(Transform buildPlace)
        {
            if (buildPlace)
            {
                Vector2 position = Camera.main.WorldToScreenPoint(buildPlace.position);
                m_RectTransform.anchoredPosition = position;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }

            foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
            {
                tbc.SetBuildPlace(buildPlace);
            }
        }

        private void OnDestroy()
        {
            BuildPlace.OnClickEvent -= MoveToBuildPlace;
        }
    }
}
