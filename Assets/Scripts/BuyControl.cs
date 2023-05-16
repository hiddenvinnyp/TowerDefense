using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControl m_TowerBuyPrefab;
        [SerializeField] private TowerAsset[] m_TowerAssets;
        [SerializeField] private UpgradeAsset m_MageTowerUpgrade;
        private List<TowerBuyControl> m_ActiveControls;
        private RectTransform m_RectTransform;

        #region Unity events
        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            BuildPlace.OnClickEvent += MoveToBuildPlace;   

            
        }

        private void Start()
        {
            /*m_ActiveControls = new List<TowerBuyControl>();
            foreach (var asset in m_TowerAssets) if (asset.IsAvaliable())
            {              
                var newControl = Instantiate(m_TowerBuyPrefab, transform);
                m_ActiveControls.Add(newControl);
                newControl.SetTowerAsset(asset);                
            }

            var angle = 360 / m_ActiveControls.Count;
            for (int i = 0; i < m_ActiveControls.Count; i++)
            {
                var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.up * 120);
                m_ActiveControls[i].transform.position += offset;
            }*/

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            BuildPlace.OnClickEvent -= MoveToBuildPlace;
        }
        #endregion

        private void MoveToBuildPlace(BuildPlace buildPlace)
        {
            if (buildPlace)
            {
                Vector2 position = Camera.main.WorldToScreenPoint(buildPlace.transform.root.position);
                m_RectTransform.position = position;

                m_ActiveControls = new List<TowerBuyControl>();
                foreach (var asset in buildPlace.BuildableTowers) if (asset.IsAvaliable())
                {              
                    var newControl = Instantiate(m_TowerBuyPrefab, transform);
                    m_ActiveControls.Add(newControl);
                    newControl.SetTowerAsset(asset);                
                }

                if (m_ActiveControls.Count > 0)
                {
                    gameObject.SetActive(true);
                    var angle = 360 / m_ActiveControls.Count;
                    for (int i = 0; i < m_ActiveControls.Count; i++)
                    {
                        var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.up * 120);
                       // m_ActiveControls[i].transform.position += offset;
                        m_ActiveControls[i].GetComponent<RectTransform>().localPosition = offset;
                    }

                    foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
                    {
                        tbc.SetBuildPlace(buildPlace.transform.root);
                    }
                } 
            }
            else
            {
                if (m_ActiveControls == null) return;
                foreach (var control in m_ActiveControls)
                    Destroy(control.gameObject);
                m_ActiveControls?.Clear();
                gameObject.SetActive(false);
            }           
        }
    }
}
