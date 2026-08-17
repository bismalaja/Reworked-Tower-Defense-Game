using UnityEngine;

namespace SimpleTowerDefense
{
    /// <summary>
    /// A fixed building pad. Click it to buy a tower, or click its tower to upgrade/sell.
    /// </summary>
    public class BuildSpot : MonoBehaviour
    {
        [SerializeField] private Transform towerAnchor;
        [SerializeField] private Tower[] towerPrefabs;
        [SerializeField] private int[] towerCosts = { 100, 140, 180 };

        private Tower builtTower;

        public bool IsEmpty => builtTower == null;
        public Tower BuiltTower => builtTower;

        public void Select()
        {
            GameUI.Instance.SelectBuildSpot(this);
        }

        public int GetTowerCost(int index)
        {
            return towerCosts[index];
        }

        public void BuildTower(int index)
        {
            if (!IsEmpty || index < 0 || index >= towerPrefabs.Length)
            {
                return;
            }

            if (!GameManager.Instance.TrySpend(towerCosts[index]))
            {
                return;
            }

            builtTower = Instantiate(towerPrefabs[index], towerAnchor.position, towerAnchor.rotation);
            builtTower.SetOwner(this);
            AudioManager.Instance?.PlayBuild();
            EffectsManager.Instance?.PlayBuildEffect(towerAnchor.position);
            GameUI.Instance.SelectBuildSpot(this);
        }

        public void UpgradeTower()
        {
            if (builtTower == null || builtTower.IsUpgraded)
            {
                return;
            }

            if (GameManager.Instance.TrySpend(builtTower.UpgradeCost))
            {
                builtTower.Upgrade();
                AudioManager.Instance?.PlayUpgrade();
                GameUI.Instance.SelectBuildSpot(this);
            }
        }

        public void SellTower()
        {
            if (builtTower == null)
            {
                return;
            }

            GameManager.Instance.AddCurrency(builtTower.SellValue);
            AudioManager.Instance?.PlaySell();
            Destroy(builtTower.gameObject);
            builtTower = null;
            GameUI.Instance.SelectBuildSpot(this);
        }

        // Used only by the editor scene builder.
        public void Configure(Transform anchor, Tower[] options, int[] costs)
        {
            towerAnchor = anchor;
            towerPrefabs = options;
            towerCosts = costs;
        }
    }
}
