using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Owns the small amount of global game state: money, score, base health and win/lose state.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Starting Values")]
        [SerializeField] private int startingCurrency = 400;
        [SerializeField] private int startingLives = 20;

        [Header("Scene References")]
        [SerializeField] private GameUI gameUI;
        [SerializeField] private WaveManager waveManager;

        public int Currency { get; private set; }
        public int Lives { get; private set; }
        public int Score { get; private set; }
        public bool GameIsOver { get; private set; }
        public bool IsPaused { get; private set; }

        private void Awake()
        {
            Instance = this;
            Time.timeScale = 1f;
            Currency = startingCurrency;
            Lives = startingLives;
            Score = 0;
        }

        private void Start()
        {
            SaveSystem.RecordGameStarted();
            gameUI.RefreshStatus(Currency, Lives, Score);
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                TogglePause();
            }

            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            {
                return;
            }

            // UI buttons handle their own clicks. Only raycast into the world otherwise.
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject(Mouse.current.deviceId))
            {
                return;
            }

            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                return;
            }

            BuildSpot buildSpot = hit.collider.GetComponentInParent<BuildSpot>();
            if (buildSpot != null)
            {
                buildSpot.Select();
                return;
            }

            Tower tower = hit.collider.GetComponentInParent<Tower>();
            if (tower != null)
            {
                tower.SelectOwner();
            }
        }

        public bool TrySpend(int amount)
        {
            if (GameIsOver || amount < 0 || Currency < amount)
            {
                return false;
            }

            Currency -= amount;
            gameUI.RefreshStatus(Currency, Lives, Score);
            return true;
        }

        public void AddCurrency(int amount)
        {
            Currency += Mathf.Max(0, amount);
            gameUI.RefreshStatus(Currency, Lives, Score);
        }

        public void RewardEnemyDefeat(int creditReward)
        {
            int safeReward = Mathf.Max(0, creditReward);
            Currency += safeReward;
            Score += safeReward * 10;
            gameUI.RefreshStatus(Currency, Lives, Score);
        }

        public void DamageBase(int amount)
        {
            if (GameIsOver)
            {
                return;
            }

            Lives = Mathf.Max(0, Lives - amount);
            gameUI.RefreshStatus(Currency, Lives, Score);

            if (Lives == 0)
            {
                LoseGame();
            }
        }

        public void WinGame()
        {
            if (GameIsOver)
            {
                return;
            }

            GameIsOver = true;
            SaveSystem.RecordGameFinished(Score, true);
            AudioManager.Instance?.PlayVictory();
            gameUI.ShowEndScreen(true);
        }

        private void LoseGame()
        {
            GameIsOver = true;
            waveManager.StopAllCoroutines();
            SaveSystem.RecordGameFinished(Score, false);
            AudioManager.Instance?.PlayDefeat();
            gameUI.ShowEndScreen(false);
        }

        public void TogglePause()
        {
            if (GameIsOver)
            {
                return;
            }

            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0f : 1f;
            gameUI.ShowPause(IsPaused);
        }

        public void RestartLevel()
        {
            SaveSystem.RecordHighScore(Score);
            Time.timeScale = 1f;
            if (SceneFader.Instance != null)
            {
                SceneFader.Instance.ReloadCurrentScene();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        public void ReturnToMenu()
        {
            SaveSystem.RecordHighScore(Score);
            Time.timeScale = 1f;
            if (SceneFader.Instance != null)
            {
                SceneFader.Instance.LoadScene("MainMenu");
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void OnApplicationQuit()
        {
            SaveSystem.RecordHighScore(Score);
        }

        // Used only by the editor scene builder.
        public void Configure(GameUI ui, WaveManager waves)
        {
            gameUI = ui;
            waveManager = waves;
        }
    }
}
