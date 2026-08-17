using System;
using System.Collections;
using UnityEngine;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Spawns five readable waves. Each wave is just a list of enemy prefab/count pairs.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        [Serializable]
        public class EnemyGroup
        {
            public Enemy prefab;
            public int count = 5;
            public float timeBetweenSpawns = 0.7f;
        }

        [Serializable]
        public class Wave
        {
            public string name = "Wave";
            public EnemyGroup[] groups;
        }

        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Wave[] waves;
        [SerializeField] private GameUI gameUI;

        private int currentWaveIndex = -1;
        private int enemiesAlive;
        private bool spawning;

        public int CurrentWaveNumber => currentWaveIndex + 1;
        public int WaveCount => waves.Length;
        public bool CanStartWave => !spawning && enemiesAlive == 0 && currentWaveIndex + 1 < waves.Length;

        private void Start()
        {
            gameUI.SetWaveStatus(0, waves.Length, true);
            gameUI.SetEnemyCount(0);
        }

        public void StartNextWave()
        {
            if (!CanStartWave || GameManager.Instance.GameIsOver)
            {
                return;
            }

            currentWaveIndex++;
            SaveSystem.RecordWaveReached(CurrentWaveNumber);
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }

        private IEnumerator SpawnWave(Wave wave)
        {
            spawning = true;
            gameUI.SetWaveStatus(CurrentWaveNumber, waves.Length, false);

            foreach (EnemyGroup group in wave.groups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    Enemy enemy = Instantiate(group.prefab, waypoints[0].position, Quaternion.identity);
                    enemiesAlive++;
                    gameUI.SetEnemyCount(enemiesAlive);
                    enemy.BeginPath(waypoints, this);
                    yield return new WaitForSeconds(group.timeBetweenSpawns);
                }
            }

            spawning = false;
            CheckForCompletedWave();
        }

        public void EnemyFinished()
        {
            enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
            gameUI.SetEnemyCount(enemiesAlive);
            CheckForCompletedWave();
        }

        private void CheckForCompletedWave()
        {
            if (spawning || enemiesAlive > 0)
            {
                return;
            }

            if (currentWaveIndex == waves.Length - 1)
            {
                GameManager.Instance.WinGame();
            }
            else
            {
                gameUI.SetWaveStatus(CurrentWaveNumber, waves.Length, true);
            }
        }

        // Used only by the editor scene builder.
        public void Configure(Transform[] path, Wave[] waveList, GameUI ui)
        {
            waypoints = path;
            waves = waveList;
            gameUI = ui;
        }
    }
}
