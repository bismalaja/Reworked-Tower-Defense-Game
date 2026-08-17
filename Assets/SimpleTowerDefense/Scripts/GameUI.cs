using UnityEngine;
using UnityEngine.UI;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Keeps UI button methods in one obvious place. Buttons call these methods directly.
    /// </summary>
    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance { get; private set; }

        [SerializeField] private Text currencyText;
        [SerializeField] private Text livesText;
        [SerializeField] private Text waveText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text enemyCountText;
        [SerializeField] private Text selectionTitle;
        [SerializeField] private Text upgradeButtonText;
        [SerializeField] private Text endTitle;
        [SerializeField] private Text endScoreText;
        [SerializeField] private Text endHighScoreText;
        [SerializeField] private Button startWaveButton;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private GameObject buildPanel;
        [SerializeField] private GameObject towerPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject endPanel;

        private BuildSpot selectedSpot;

        private void Awake()
        {
            Instance = this;
            buildPanel.SetActive(false);
            towerPanel.SetActive(false);
            pausePanel.SetActive(false);
            endPanel.SetActive(false);
        }

        public void RefreshStatus(int currency, int lives, int score)
        {
            currencyText.text = $"Credits: {currency}";
            livesText.text = $"Base: {lives}";
            scoreText.text = $"Score: {score}";
            RefreshSelection();
        }

        public void SetWaveStatus(int currentWave, int waveCount, bool ready)
        {
            waveText.text = currentWave == 0
                ? $"Waves: 0 / {waveCount}"
                : $"Wave: {currentWave} / {waveCount}";
            startWaveButton.gameObject.SetActive(ready);
            startWaveButton.interactable = ready;
        }

        public void SetEnemyCount(int enemyCount)
        {
            enemyCountText.text = $"Enemies: {enemyCount}";
        }

        public void SelectBuildSpot(BuildSpot spot)
        {
            selectedSpot = spot;
            RefreshSelection();
        }

        private void RefreshSelection()
        {
            if (selectedSpot == null)
            {
                buildPanel.SetActive(false);
                towerPanel.SetActive(false);
                return;
            }

            bool empty = selectedSpot.IsEmpty;
            buildPanel.SetActive(empty);
            towerPanel.SetActive(!empty);

            if (empty)
            {
                selectionTitle.text = "Choose a tower";
            }
            else
            {
                Tower tower = selectedSpot.BuiltTower;
                selectionTitle.text = tower.IsUpgraded
                    ? $"{tower.DisplayName} - Level 2"
                    : $"{tower.DisplayName} - Level 1";
                upgradeButtonText.text = tower.IsUpgraded
                    ? "Fully Upgraded"
                    : $"Upgrade ({tower.UpgradeCost})";
                upgradeButton.interactable = !tower.IsUpgraded &&
                    GameManager.Instance.Currency >= tower.UpgradeCost;
            }
        }

        public void BuyMachineGun() => selectedSpot?.BuildTower(0);
        public void BuyLaser() => selectedSpot?.BuildTower(1);
        public void BuyRocket() => selectedSpot?.BuildTower(2);
        public void UpgradeSelected() => selectedSpot?.UpgradeTower();
        public void SellSelected() => selectedSpot?.SellTower();
        public void StartNextWave()
        {
            AudioManager.Instance?.PlayButton();
            FindFirstObjectByType<WaveManager>().StartNextWave();
        }

        public void TogglePause()
        {
            AudioManager.Instance?.PlayButton();
            GameManager.Instance.TogglePause();
        }

        public void Restart()
        {
            AudioManager.Instance?.PlayButton();
            GameManager.Instance.RestartLevel();
        }

        public void ReturnToMenu()
        {
            AudioManager.Instance?.PlayButton();
            GameManager.Instance.ReturnToMenu();
        }

        public void ShowPause(bool show)
        {
            pausePanel.SetActive(show);
        }

        public void ShowEndScreen(bool playerWon)
        {
            buildPanel.SetActive(false);
            towerPanel.SetActive(false);
            startWaveButton.gameObject.SetActive(false);
            endTitle.text = playerWon ? "You Win!" : "Base Destroyed";
            endScoreText.text = $"Score: {GameManager.Instance.Score}";
            endHighScoreText.text = $"High Score: {SaveSystem.Data.highScore}";
            endPanel.SetActive(true);
        }

        // Used only by the editor scene builder.
        public void Configure(
            Text credits, Text health, Text wave, Text score, Text selectedTitle, Text upgradeLabel,
            Text resultTitle, Button waveButton, Button upgrade,
            GameObject building, GameObject towerActions, GameObject pause, GameObject end)
        {
            currencyText = credits;
            livesText = health;
            waveText = wave;
            scoreText = score;
            selectionTitle = selectedTitle;
            upgradeButtonText = upgradeLabel;
            endTitle = resultTitle;
            startWaveButton = waveButton;
            upgradeButton = upgrade;
            buildPanel = building;
            towerPanel = towerActions;
            pausePanel = pause;
            endPanel = end;
        }
    }
}
