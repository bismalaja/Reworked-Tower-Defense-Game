using System;
using System.IO;
using UnityEngine;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Loads and saves one small JSON file in Unity's persistent data folder.
    /// </summary>
    public static class SaveSystem
    {
        private const string FileName = "save.json";

        private static SaveData currentData;

        public static string SavePath =>
            Path.Combine(Application.persistentDataPath, FileName);

        public static SaveData Data
        {
            get
            {
                if (currentData == null)
                {
                    Load();
                }

                return currentData;
            }
        }

        public static void Load()
        {
            if (!File.Exists(SavePath))
            {
                currentData = new SaveData();
                return;
            }

            try
            {
                string json = File.ReadAllText(SavePath);
                currentData = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
                SanitizeData();
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Could not load save data. Defaults will be used. {exception.Message}");
                currentData = new SaveData();
            }
        }

        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                string json = JsonUtility.ToJson(Data, true);
                File.WriteAllText(SavePath, json);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Could not save progress. {exception.Message}");
            }
        }

        public static void ResetProgress()
        {
            currentData = new SaveData();
            Save();
        }

        public static void RecordGameStarted()
        {
            Data.gamesPlayed++;
            Save();
        }

        public static void RecordWaveReached(int waveNumber)
        {
            if (waveNumber <= Data.highestWaveReached)
            {
                return;
            }

            Data.highestWaveReached = waveNumber;
            Save();
        }

        public static void RecordHighScore(int score)
        {
            if (score <= Data.highScore)
            {
                return;
            }

            Data.highScore = score;
            Save();
        }

        public static void RecordGameFinished(int score, bool playerWon)
        {
            Data.highScore = Mathf.Max(Data.highScore, score);
            if (playerWon)
            {
                Data.gamesWon++;
            }

            Save();
        }

        public static void SetVolumes(float musicVolume, float sfxVolume)
        {
            Data.musicVolume = Mathf.Clamp01(musicVolume);
            Data.sfxVolume = Mathf.Clamp01(sfxVolume);
            Save();
        }

        private static void SanitizeData()
        {
            currentData.highScore = Mathf.Max(0, currentData.highScore);
            currentData.highestWaveReached = Mathf.Max(0, currentData.highestWaveReached);
            currentData.gamesWon = Mathf.Max(0, currentData.gamesWon);
            currentData.gamesPlayed = Mathf.Max(0, currentData.gamesPlayed);
            currentData.musicVolume = Mathf.Clamp01(currentData.musicVolume);
            currentData.sfxVolume = Mathf.Clamp01(currentData.sfxVolume);
        }
    }
}
